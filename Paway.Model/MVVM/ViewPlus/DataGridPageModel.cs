﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Utils;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 数据管理基类，必须指定当前菜单、及初始化
    /// </summary>
    public abstract class DataGridPageModel<T> : OperateItemModel where T : class, IBaseInfo, ICompare<T>, new()
    {
        #region 属性
        protected IDataGridServer server;
        protected DataGridEXT DataGrid;
        /// <summary>
        /// 刷新时数据库过滤条件
        /// </summary>
        private string sqlFilter;
        /// <summary>
        /// 源数据列表
        /// </summary>
        public List<T> List;
        /// <summary>
        /// List为全局缓存，当前页可能需要过滤显示
        /// </summary>
        protected Func<T, bool> listFilter;
        protected List<T> FilterList()
        {
            if (listFilter == null) return List;
            return List.FindAll(c => listFilter(c));
        }
        /// <summary>
        /// 当前显示数据列表
        /// </summary>
        private List<T> showList;
        /// <summary>
        /// 界面绑定列表
        /// </summary>
        public ObservableCollection<T> ObList { get; private set; } = new ObservableCollection<T>();
        /// <summary>
        /// 分页标记
        /// </summary>
        private bool IPage;
        public PagedCollectionView PagedList { get; private set; }
        private T _selectedItem;
        public virtual T SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(); }
        }

        protected virtual AddWindowModel<T> ViewModel()
        {
            return new AddWindowModel<T>();
        }
        private AddWindowModel<T> addViewModel;
        protected AddWindowModel<T> AddViewModel
        {
            get
            {
                if (addViewModel == null) addViewModel = ViewModel();
                return addViewModel;
            }
        }

        /// <summary>
        /// 选择单元格时，SelectedItem为空
        /// </summary>
        protected T SelectedInfo()
        {
            if (SelectedItem != null) return SelectedItem;
            if (DataGrid.SelectionUnit != DataGridSelectionUnit.FullRow && DataGrid.SelectedCells.Count > 0)
            {
                if (DataGrid.SelectedCells.First().Item is T t) return t;
            }
            return default;
        }

        #endregion

        #region 命令
        protected virtual Window AddWindow()
        {
            return null;
        }
        protected virtual List<T> Find()
        {
            var list = server.Find<T>(this.sqlFilter);
            Method.Sorted(list);
            return list;
        }
        protected virtual void Insert(T info)
        {
            server.Insert(info);
            Method.Update(info);
            Method.Sorted(List);
            var index = this.FilterList().FindIndex(c => c.Id == info.Id);
            if (!this.SearchReset()) ObList.Insert(index, info);
            SelectIndex(info, index);
        }
        private void SelectIndex(T info, int index)
        {
            Method.BeginInvoke(DataGrid, temp =>
            {
                if (IPage)
                {
                    PagedList.MoveToPage(temp / PagedList.PageSize);
                    temp %= PagedList.PageSize;
                }
                DataGrid.ScrollIntoView(info);
                DataGrid.SelectedIndex = temp;
            }, index);
        }
        protected virtual void Updated(T info)
        {
            info.UpdateOn = DateTime.Now;
            server.Update(info);
            Method.Sorted(List);
            var index = this.FilterList().FindIndex(c => c.Id == info.Id);
            ObList.Remove(info);
            if (!this.SearchReset()) ObList.Insert(index, info);
            SelectIndex(info, index);
        }
        protected virtual void Deleted(T info)
        {
            var index = DataGrid.SelectedIndex;
            try
            {
                server.Delete(info);
                Method.Delete(info);
                ObList.Remove(info);
            }
            finally
            {
                if (index >= DataGrid.Items.Count) index = DataGrid.Items.Count - 1;
                DataGrid.SelectedIndex = index;
            }
        }
        protected override void Refresh()
        {
            Method.Progress(DataGrid, () =>
            {
                Init(Find());
            }, null, ex =>
            {
                Messenger.Default.Send(new StatuMessage(ex));
            });
        }
        protected virtual void ImportChecked(List<T> list)
        {
            if (typeof(IIndex).IsAssignableFrom(typeof(T)))
            {
                var tList = Cache.List<T>();
                var index = tList.Count == 0 ? 0 : tList.Max(c => ((IIndex)c).Index) + 1;
                for (var i = 0; i < list.Count; i++)
                {
                    ((IIndex)list[i]).Index = index + i;
                }
            }
        }
        protected virtual void Import(List<T> list)
        {
            var updateList = Method.Import(this.FilterList(), list);
            var timeNow = DateTime.Now;
            updateList.ForEach(c => c.UpdateOn = timeNow);
            server.Replace(updateList);
            Method.Update(updateList);
            Method.Sorted(List);
            this.Reload();
        }
        protected virtual void Export(string file)
        {
            ExcelHelper.ToExcel(this.FilterList(), null, file);
            Messenger.Default.Send(new StatuMessage("导出成功", DataGrid));
            if (Method.Ask(DataGrid, "导出成功,是否打开文件?"))
            {
                Process.Start(file);
            }
        }

        #endregion

        #region 操作命令
        public ICommand RowDoubleCommand => new RelayCommand<SelectItemEventArgs>(e =>
        {
            ActionInternal("编辑");
        });
        public ICommand SelectionCommand => new RelayCommand<ListViewEXT>(listView1 =>
        {
            if (listView1.SelectedItem is IListViewItem item)
            {
                Selectioned(listView1, item);
            }
        });
        protected virtual void Selectioned(ListViewEXT listView1, IListViewItem item)
        {
            ActionInternal(item.Text);
            listView1.SelectedIndex = -1;
        }
        internal override void ActionInternal(string item)
        {
            try
            {
                Action(item);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex));
            }
        }
        protected override void Action(KeyMessage msg)
        {
            if (Config.Menu != this.Menu) return;
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Method.Find(DataGrid, out TextBoxEXT tbSearch, "tbSearch"))
                {
                    if (tbSearch.IsKeyboardFocusWithin) return;
                }
            }
            base.Action(msg);
        }
        protected override void Action(string item)
        {
            switch (item)
            {
                default:
                    base.Action(item);
                    break;
                case "添加":
                    AddViewModel.Info = new T();
                    var add = AddWindow();
                    if (add != null && Method.Show(DataGrid, add) == true)
                    {
                        Insert(AddViewModel.Info);
                    }
                    break;
                case "编辑":
                    if (SelectedInfo() is T info)
                    {
                        AddViewModel.Info = info;
                        var edit = AddWindow();
                        if (edit != null && Method.Show(DataGrid, edit) == true)
                        {
                            Updated(info);
                        }
                    }
                    break;
                case "删除":
                    if (SelectedInfo() is T infoDel)
                    {
                        if (Method.Ask(DataGrid, $"确认删除：[{infoDel.GetType().Description()}]" + infoDel))
                        {
                            Deleted(infoDel);
                        }
                    }
                    break;
                case "导入":
                    var title = typeof(T).Description();
                    if (Method.Import($"选择要导入的 {title} 表", out string file))
                    {
                        Method.Progress(DataGrid, "正在导入..", adorner =>
                        {
                            var list = Method.FromExcel<T>(file).Result;
                            ImportChecked(list);
                            Import(list);
                        }, () =>
                        {
                            Messenger.Default.Send(new StatuMessage($"{title} 导入完成", DataGrid));
                        }, error: ex =>
                        {
                            Messenger.Default.Send(new StatuMessage("导入失败", ex, DataGrid));
                        });
                    }
                    break;
                case "导出":
                    title = $"{typeof(T).Description()}{DateTime.Now:yyyy-MM-dd}";
                    if (Method.Export(title, out file))
                    {
                        Export(file);
                    }
                    break;
            }
        }
        protected void Init(IDataGridServer server, List<T> list, DataGridEXT dataGrid, bool iPage = false)
        {
            this.Init(server, list, dataGrid, null, iPage);
        }
        protected void Init(IDataGridServer server, List<T> list, DataGridEXT dataGrid, string sqlFilter, bool iPage = false)
        {
            this.server = server;
            this.sqlFilter = sqlFilter;
            this.IPage = iPage;
            this.DataGrid = dataGrid;
            this.Init(list);
            if (this.List.Count == 0) this.Refresh();
        }

        #endregion
        #region 拖拽排序
        public ICommand DragCompletedCmd => new RelayCommand<DataGridDragEventArgs>(e =>
        {
            if (typeof(IIndex).IsAssignableFrom(typeof(T)))
            {
                var updateList = new List<T>();
                for (var i = 0; i < ObList.Count; i++)
                {
                    var item = List.Find(c => c.Id == ObList[i].Id);
                    if (item is IIndex index)
                    {
                        index.Index = i;
                        updateList.Add(item);
                    }
                }
                server.Update(updateList, null, nameof(IIndex.Index));
            }
        });

        #endregion

        #region 加载列表
        protected void Init(List<T> list)
        {
            if (this.List == null) this.List = list;
            else
            {
                this.List.Clear();
                this.List.AddRange(list);
            }
            this.Reload();
        }
        protected override void Search()
        {
            if (SearchText.IsEmpty())
            {
                this.showList = null;
            }
            else
            {
                var filter = typeof(T).Predicate<T>(SearchText.Trim());
                this.showList = this.FilterList().Where(filter).ToList();
            }
            this.ReloadObList();
        }
        protected void Reload()
        {
            if (!SearchReset()) ReloadObList();
        }
        private bool SearchReset()
        {
            if (!SearchText.IsEmpty())
            {
                base.ClearSearch();
                this.showList = null;
                this.ReloadObList();
                return true;
            }
            return false;
        }
        private void ReloadObList()
        {
            Method.BeginInvoke(DataGrid, () =>
            {
                ObList.Clear();
                var list = showList ?? this.FilterList();
                foreach (var item in list) ObList.Add(item);
            });
        }
        /// <summary>
        /// 默认权限
        /// <para>刷新、添加、编辑、删除、导入、导出、搜索</para>
        /// </summary>
        protected override void AuthNormal()
        {
            this.Auth = MenuAuthType.Refresh | MenuAuthType.Add | MenuAuthType.Edit | MenuAuthType.Delete | MenuAuthType.Import | MenuAuthType.Export | MenuAuthType.Search;
        }
        /// <summary>
        /// 只读权限
        /// <para>刷新、导出、搜索</para>
        /// </summary>
        protected void AuthView()
        {
            this.Auth = MenuAuthType.Refresh | MenuAuthType.Export | MenuAuthType.Search;
        }
        /// <summary>
        /// 无搜索
        /// </summary>
        protected void AuthNoSearch()
        {
            this.Auth ^= MenuAuthType.Search;
        }

        #endregion

        public DataGridPageModel()
        {
            this.PagedList = new PagedCollectionView(ObList) { PageSize = 20 };
        }
    }
}