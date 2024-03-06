﻿using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// TextBox扩展
    /// </summary>
    public partial class TextBoxEXT : TextBox
    {
        #region 动画
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IAnimationProperty =
            DependencyProperty.RegisterAttached(nameof(IAnimation), typeof(double), typeof(TextBoxEXT));
        /// <summary>
        /// 动画
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("动画")]
        public double IAnimation
        {
            get { return (double)GetValue(IAnimationProperty); }
            set { SetValue(IAnimationProperty, value); }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty KeyboardProperty =
                DependencyProperty.RegisterAttached(nameof(Keyboard), typeof(KeyboardType), typeof(TextBoxEXT), new PropertyMetadata(KeyboardType.All));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterProperty =
            DependencyProperty.RegisterAttached(nameof(Water), typeof(string), typeof(TextBoxEXT), new PropertyMetadata());
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached(nameof(WaterSize), typeof(double), typeof(TextBoxEXT), new PropertyMetadata(0.85));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.RegisterAttached(nameof(Unit), typeof(string), typeof(TextBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(TextBoxEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TextBoxEXT),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached(nameof(Icon), typeof(ImageSource), typeof(TextBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.RegisterAttached(nameof(IconWidth), typeof(double), typeof(TextBoxEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.RegisterAttached(nameof(IconHeight), typeof(double), typeof(TextBoxEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconStretchProperty =
            DependencyProperty.RegisterAttached(nameof(IconStretch), typeof(Stretch), typeof(TextBoxEXT),
            new PropertyMetadata(Stretch.None));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(TextBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleMinWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TitleMinWidth), typeof(double), typeof(TextBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty InterceptProperty =
            DependencyProperty.RegisterAttached(nameof(Intercept), typeof(bool), typeof(TextBoxEXT), new PropertyMetadata(true));

        #endregion

        #region 扩展
        /// <summary>
        /// 虚拟键盘类型
        /// <para>默认值：全键盘</para>
        /// </summary>
        [Category("扩展")]
        [Description("虚拟键盘类型")]
        public KeyboardType Keyboard
        {
            get { return (KeyboardType)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }
        /// <summary>
        /// 水印内容
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("水印内容")]
        public string Water
        {
            get { return (string)GetValue(WaterProperty); }
            set { SetValue(WaterProperty, value); }
        }
        /// <summary>
        /// 水印字体大小系数
        /// <para>默认值：0.85</para>
        /// </summary>
        [Category("扩展")]
        [Description("水印字体大小系数")]
        public double WaterSize
        {
            get { return (double)GetValue(WaterSizeProperty); }
            set { SetValue(WaterSizeProperty, value); }
        }
        /// <summary>
        /// 单位
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("单位")]
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：3</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 文本框的边框颜色
        /// <para>默认值：默认</para>
        /// </summary>
        [Category("扩展")]
        [Description("文本框的边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 图标
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("图标")]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// 图标宽度
        /// <para>默认值：double.NaN</para>
        /// </summary>
        [Category("扩展")]
        [Description("图标宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        /// <summary>
        /// 图标高度
        /// <para>默认值：double.NaN</para>
        /// </summary>
        [Category("扩展")]
        [Description("图标高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }
        /// <summary>
        /// 图标的内容如何拉伸才适合其磁贴
        /// <para>默认值：None</para>
        /// </summary>
        [Category("扩展")]
        [Description("图标的内容如何拉伸才适合其磁贴")]
        public Stretch IconStretch
        {
            get { return (Stretch)GetValue(IconStretchProperty); }
            set { SetValue(IconStretchProperty, value); }
        }
        /// <summary>
        /// 标题
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>
        /// 标题最小长度
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题最小长度")]
        public double TitleMinWidth
        {
            get { return (double)GetValue(TitleMinWidthProperty); }
            set { SetValue(TitleMinWidthProperty, value); }
        }
        /// <summary>
        /// 拦截Tab与回车跳转
        /// <para>默认值：true</para>
        /// </summary>
        [Category("扩展")]
        [Description("拦截Tab与回车跳转")]
        public bool Intercept
        {
            get { return (bool)GetValue(InterceptProperty); }
            set { SetValue(InterceptProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TextBoxEXT()
        {
            DefaultStyleKey = typeof(TextBoxEXT);
            if (!iMouseEvent)
            {
                iMouseEvent = true;
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            }
        }
        /// <summary>
        /// Water未自定义设置时绑定多语言
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Water == null)
            {
                this.Water = PConfig.LanguageBase.PleaseInputWater;
                var waterBinding = new Binding
                {
                    Source = PConfig.LanguageBase,//设置要绑定源-语言类
                    Path = new PropertyPath(nameof(PConfig.LanguageBase.PleaseInputWater)),//绑定绑定源下的属性。
                    Mode = BindingMode.OneWay//绑定模式单向
                };
                this.SetBinding(WaterProperty, waterBinding);//设置绑定到要绑定的控件
            }
        }
        /// <summary>
        /// 回车时移动焦点到下一控件
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Intercept) PMethod.OnKeyDown(e);
            base.OnKeyDown(e);
        }

        #region 动画
        /// <summary>
        /// 鼠标进入时启动
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (IAnimation > 0) PMethod.Animation(this, true);
        }
        /// <summary>
        /// 鼠标离开时关闭
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IAnimation > 0) PMethod.Animation(this, false);
        }

        #endregion

        #region 自定义虚拟键盘
        private CustomAdorner desktopAdorner;
        /// <summary>
        /// 鼠标点击时自动打开虚拟键盘
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            this.OpenKeyboard();
        }
        /// <summary>
        /// 获取焦点时自动打开虚拟键盘
        /// </summary>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.OpenKeyboard();
        }
        private void OpenKeyboard()
        {
            if (!this.IsLoaded || !this.IsEnabled || this.IsReadOnly) return;
            if (PConfig.Keyboard == EnableType.None || Keyboard == KeyboardType.None) return;
            if (desktopAdorner == null && PMethod.Parent(this, out Window owner) && owner.Content is FrameworkElement content)
            {
                FrameworkElement keyboard;
                switch (Keyboard)
                {
                    case KeyboardType.Num: var keyboardNum = new KeyboardNum(); keyboard = keyboardNum; keyboardNum.CloseEvent += CloseKeyboard; break;
                    case KeyboardType.All: var keyboardAll = new KeyboardAll(); keyboard = keyboardAll; keyboardAll.CloseEvent += CloseKeyboard; break;
                    default: return;
                }
                var point = this.TransformToAncestor(content).Transform(new Point(0, 0));
                var screenPoint1 = this.PointToScreen(new Point(0, 0));
                var screenPoint2 = this.PointToScreen(new Point(this.ActualWidth, this.ActualHeight));
                this.boxRect = new Rect(screenPoint1.X, screenPoint1.Y, screenPoint2.X - screenPoint1.X, screenPoint2.Y - screenPoint1.Y);
                desktopAdorner = PMethod.CustomAdorner(owner, keyboard, true, iBingParentSize: false);
                if (desktopAdorner != null)
                {
                    var x = point.X;
                    if (content.ActualWidth < keyboard.Width + x)
                    {
                        x = content.ActualWidth - keyboard.Width;
                    }
                    var y = point.Y + this.ActualHeight;
                    if (content.ActualHeight < keyboard.Height + y)
                    {
                        y = point.Y - keyboard.Height;
                    }
                    Canvas.SetLeft(keyboard, x);
                    Canvas.SetTop(keyboard, y);
                    screenPoint1 = content.PointToScreen(new Point(x, y));
                    screenPoint2 = content.PointToScreen(new Point(x + keyboard.Width, y + keyboard.Height));
                    this.keyboardRect = new Rect(screenPoint1.X, screenPoint1.Y, screenPoint2.X - screenPoint1.X, screenPoint2.Y - screenPoint1.Y);
                }
                iMouseHook = true;
                if (mouseHook == null && PConfig.KeyboardAutoClose == EnableType.Enable)
                {
                    mouseHook = new MouseHook();
                    mouseHook.MouseDownEvent += MouseHook_MouseDownEvent;
                    mouseHook.Start();
                }
            }
        }

        /// <summary>
        /// 失去焦点时自动关闭虚拟键盘
        /// </summary>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            this.CloseKeyboard();
        }
        private void CloseKeyboard()
        {
            if (desktopAdorner != null && PMethod.Parent(this, out Window owner) && owner.Content is FrameworkElement content)
            {
                iMouseHook = false;
                var myAdornerLayer = PMethod.ReloadAdorner(content);
                if (myAdornerLayer == null) return;

                lock (myAdornerLayer) myAdornerLayer.Remove(desktopAdorner);
                desktopAdorner = null;
                KeyboardHelper.StopHook();
            }
        }

        #region 鼠标钩子
        private static MouseHook mouseHook;
        private static bool iMouseHook;
        private static bool iMouseEvent;
        private Rect boxRect;
        private Rect keyboardRect;
        private void MouseHook_MouseDownEvent(object sender, MouseEventExtArgs e)
        {
            if (iMouseHook && desktopAdorner != null && !boxRect.Contains(new Point(e.X, e.Y)) && !keyboardRect.Contains(new Point(e.X, e.Y)))
            {
                this.CloseKeyboard();
            }
        }
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            mouseHook?.Stop();
        }

        #endregion

        #endregion
    }
}
