﻿using Newtonsoft.Json;
using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Paway.Comm
{
    public partial class WebApiClient : IDataGridServer
    {
        private static WebApiClient intance;
        /// <summary>
        /// 封装HTTP通讯接口调用
        /// </summary>
        public static WebApiClient Default
        {
            get
            {
                if (intance == null) intance = new WebApiClient();
                return intance;
            }
        }
        /// <summary>
        /// 插入项
        /// </summary>
        public int Insert<T>(T obj, DbCommand cmd = null) where T : class
        {
            Insert(obj);
            return 0;
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public int Insert<T>(List<T> list, DbCommand cmd = null) where T : class
        {
            Insert(list);
            return 0;
        }
        /// <summary>
        /// 删除项
        /// </summary>
        public int Delete<T>(T t, DbCommand cmd = null) where T : class
        {
            Delete(t);
            return 0;
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        public int Delete<T>(List<T> list, DbCommand cmd = null) where T : class
        {
            Delete(list);
            return 0;
        }
        /// <summary>
        /// 更新项
        /// </summary>
        public int Update<T>(T t, DbCommand cmd = null, params string[] args) where T : class
        {
            Update(t, args);
            return 0;
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public int Update<T>(List<T> list, DbCommand cmd = null, params string[] args) where T : class
        {
            Update(list, args);
            return 0;
        }
        /// <summary>
        /// 更新或插入项
        /// </summary>
        public int Replace<T>(T t, params string[] args) where T : class
        {
            Update(t, args);
            return 0;
        }
        /// <summary>
        /// 更新或插入列表
        /// </summary>
        public int Replace<T>(List<T> list, params string[] args) where T : class
        {
            Update(list, args);
            return 0;
        }
        /// <summary>
        /// 查找指定类型数据库所有数据列表(按指定条件过滤)
        /// </summary>
        public List<T> Find<T>(string find = null, DbCommand cmd = null, params string[] args) where T : class, new()
        {
            return Find<T>(find, args);
        }
    }
}
