﻿using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Paway.WPF
{
    /// <summary>
    /// 绑定自定义虚拟键盘
    /// </summary>
    public class KeyboardMonitor
    {
        private CustomAdorner keyboardAdorner;
        private Window keyboardWindow;
        private Rect boxRect;
        private Rect keyboardRect;
        private FrameworkElement element;
        private KeyboardType Keyboard;

        /// <summary>
        /// 绑定自定义虚拟键盘
        /// </summary>
        public KeyboardMonitor(FrameworkElement element, KeyboardType type = KeyboardType.All)
        {
            element.PreviewMouseDown -= Element_PreviewMouseDown;
            element.PreviewMouseDown += Element_PreviewMouseDown;
            element.GotFocus -= Element_GotFocus;
            element.GotFocus += Element_GotFocus;
            element.LostFocus -= Element_LostFocus;
            element.LostFocus += Element_LostFocus;
            this.element = element;
            this.Keyboard = type;
        }
        /// <summary>
        /// 鼠标点击时自动打开虚拟键盘
        /// </summary>
        private void Element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.OpenKeyboard(e.ClickCount);
        }
        /// <summary>
        /// 获取焦点时自动打开虚拟键盘
        /// </summary>
        private void Element_GotFocus(object sender, RoutedEventArgs e)
        {
            this.OpenKeyboard(0);
        }
        private void OpenKeyboard(int clickCount)
        {
            if (!element.IsLoaded || !element.IsEnabled) return;
            if (PConfig.Keyboard == EnableType.None) return;
            if (keyboardAdorner != null || keyboardWindow != null)
            {
                if (clickCount == 2) this.CloseKeyboard();
                return;
            }
            var keyboardType = Keyboard;
            if (element is TextBoxEXT textBoxEXT)
            {
                keyboardType = textBoxEXT.Keyboard;
                if (textBoxEXT.IsReadOnly) return;
            }
            if (element is TextBoxNumeric textBoxNumeric)
            {
                keyboardType = textBoxNumeric.Keyboard;
                if (textBoxNumeric.IsReadOnly) return;
            }
            if (keyboardType == KeyboardType.None) return;

            if (PMethod.Parent(element, out Window owner) && owner.Content is FrameworkElement content)
            {
                if (keyboardType == KeyboardType.Auto && element is TextBoxBase)
                {
                    Binding binding = BindingOperations.GetBinding(element, TextBox.TextProperty);
                    if (binding != null && element.DataContext != null)
                    {
                        var property = element.DataContext.Property(binding.Path.Path);
                        if (property != null && property.PropertyType == typeof(string)) keyboardType = KeyboardType.All;
                        else keyboardType = KeyboardType.Num;
                    }
                    else keyboardType = KeyboardType.All;
                }
                owner.PreviewMouseLeftButtonDown += Owner_PreviewMouseLeftButtonDown;
                FrameworkElement keyboardElement = null;
                switch (keyboardType)
                {
                    case KeyboardType.Num:
                        if (PConfig.IKeyboardWindow)
                        {
                            var keyboardNum = new KeyboardNumWindow(element); keyboardWindow = keyboardNum; keyboardNum.CloseEvent += CloseKeyboard;
                        }
                        else
                        {
                            var keyboardNum = new KeyboardNum(); keyboardElement = keyboardNum; keyboardNum.CloseEvent += CloseKeyboard;
                        }
                        break;
                    default:
                    case KeyboardType.All:
                        if (PConfig.IKeyboardWindow)
                        {
                            var keyboardAll = new KeyboardAllWindow(element); keyboardWindow = keyboardAll; keyboardAll.CloseEvent += CloseKeyboard;
                        }
                        else
                        {
                            var keyboardAll = new KeyboardAll(); keyboardElement = keyboardAll; keyboardAll.CloseEvent += CloseKeyboard;
                        }
                        break;
                }
                Point point;
                if (PMethod.Parent(element, out Adorner adorner))
                {
                    point = element.TransformToAncestor(adorner).Transform(new Point(0, 0));
                }
                else if (!PConfig.IKeyboardWindow)
                {
                    point = element.TransformToAncestor(content).Transform(new Point(0, 0));
                }
                else
                {
                    point = element.TransformToAncestor(owner).Transform(new Point(0, 0));
                }
                var ownerPoint = element.TransformToAncestor(owner).Transform(new Point(0, 0));
                this.boxRect = new Rect(ownerPoint.X, ownerPoint.Y, element.ActualWidth, element.ActualHeight);
                if (keyboardElement != null) keyboardAdorner = PMethod.CustomAdorner(owner, keyboardElement, true, false);
                if (keyboardAdorner != null)
                {
                    var x = point.X;
                    if (content.ActualWidth < keyboardElement.Width + x)
                    {
                        x = content.ActualWidth - keyboardElement.Width;
                    }
                    if (x < 0) x = 0;
                    var y = point.Y + element.ActualHeight;
                    if (content.ActualHeight < keyboardElement.Height + y)
                    {
                        y = point.Y - keyboardElement.Height;
                    }
                    Canvas.SetLeft(keyboardElement, x);
                    Canvas.SetTop(keyboardElement, y);
                    var ownerPoint2 = content.TransformToAncestor(owner).Transform(new Point(x, y));
                    this.keyboardRect = new Rect(ownerPoint2.X, ownerPoint2.Y, keyboardElement.Width, keyboardElement.Height);
                }
                if (keyboardWindow != null)
                {
                    keyboardWindow.Show();
                    if (owner.WindowState != WindowState.Maximized) point = new Point(point.X + owner.Left, point.Y + owner.Top);
                    var hwnd = owner.Handle();
                    var current = System.Windows.Forms.Screen.FromHandle(hwnd);
                    var top = !(owner is WindowEXT) && owner.WindowStyle != System.Windows.WindowStyle.None ? 30 : 0;

                    var x = point.X + element.Margin.Left;
                    if (current.WorkingArea.Width < keyboardWindow.Width + x)
                    {
                        x = current.WorkingArea.Width - keyboardWindow.Width;
                    }
                    if (x < 0) x = 0;
                    var y = point.Y + element.ActualHeight + element.Margin.Top + top;
                    if (current.WorkingArea.Height < keyboardWindow.Height + y)
                    {
                        y = point.Y + top - keyboardWindow.Height;
                    }
                    keyboardWindow.Left = x;
                    keyboardWindow.Top = y;
                    var ownerPoint2 = content.TransformToAncestor(owner).Transform(new Point(x, y));
                    this.keyboardRect = new Rect(ownerPoint2.X, ownerPoint2.Y, keyboardWindow.Width, keyboardWindow.Height);
                }
            }
        }

        /// <summary>
        /// 失去焦点时自动关闭虚拟键盘
        /// </summary>
        private void Element_LostFocus(object sender, RoutedEventArgs e)
        {
            this.CloseKeyboard();
        }
        private void Owner_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is IInputElement element)
            {
                var point = e.GetPosition(element);
                if (!boxRect.Contains(point) && !keyboardRect.Contains(point))
                {
                    this.CloseKeyboard();
                }
            }
        }
        private void CloseKeyboard()
        {
            if (keyboardAdorner != null && PMethod.Parent(element, out Window owner) && owner.Content is FrameworkElement content)
            {
                owner.PreviewMouseLeftButtonDown -= Owner_PreviewMouseLeftButtonDown;
                var myAdornerLayer = PMethod.ReloadAdorner(content);
                if (myAdornerLayer == null) return;

                lock (myAdornerLayer) myAdornerLayer.Remove(keyboardAdorner);
                keyboardAdorner = null;
                KeyboardHelper.StopHook();
            }
            if (keyboardWindow != null && PMethod.Parent(element, out owner))
            {
                owner.PreviewMouseLeftButtonDown -= Owner_PreviewMouseLeftButtonDown;
                keyboardWindow.Close();
                keyboardWindow = null;
                KeyboardHelper.StopHook();
            }
        }
    }
}
