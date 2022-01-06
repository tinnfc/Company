﻿using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// TextBox扩展数值选择控件
    /// </summary>
    public partial class TextBoxNumeric : TextBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(TextBoxNumeric), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TextBoxNumeric),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached(nameof(Icon), typeof(ImageSource), typeof(TextBoxNumeric));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconStretchProperty =
            DependencyProperty.RegisterAttached(nameof(IconStretch), typeof(Stretch), typeof(TextBoxNumeric),
            new PropertyMetadata(Stretch.None));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(TextBoxNumeric));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleMinWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TitleMinWidth), typeof(double), typeof(TextBoxNumeric));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.RegisterAttached(nameof(Interval), typeof(double), typeof(TextBoxNumeric), new PropertyMetadata(1d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.RegisterAttached(nameof(MinValue), typeof(double), typeof(TextBoxNumeric), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.RegisterAttached(nameof(MaxValue), typeof(double), typeof(TextBoxNumeric), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty DecimalProperty =
            DependencyProperty.RegisterAttached(nameof(Decimal), typeof(int), typeof(TextBoxNumeric), new PropertyMetadata(0));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
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
        /// </summary>
        [Category("扩展")]
        [Description("文本框的边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 图片
        /// </summary>
        [Category("扩展")]
        [Description("图片")]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// 图片的内容如何拉伸才适合其磁贴
        /// </summary>
        [Category("扩展")]
        [Description("图片的内容如何拉伸才适合其磁贴")]
        public Stretch IconStretch
        {
            get { return (Stretch)GetValue(IconStretchProperty); }
            set { SetValue(IconStretchProperty, value); }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [Category("扩展")]
        [Description("标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>
        /// 标题长度
        /// </summary>
        [Category("扩展")]
        [Description("标题长度")]
        public double TitleMinWidth
        {
            get { return (double)GetValue(TitleMinWidthProperty); }
            set { SetValue(TitleMinWidthProperty, value); }
        }
        /// <summary>
        /// 单击一下按钮时增加或减少的数量
        /// </summary>
        [Category("扩展")]
        [Description("单击一下按钮时增加或减少的数量")]
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        /// <summary>
        /// 最小值
        /// </summary>
        [Category("扩展")]
        [Description("最小值")]
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        /// <summary>
        /// 最大值
        /// </summary>
        [Category("扩展")]
        [Description("最大值")]
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        /// <summary>
        /// 要显示的小数位数
        /// </summary>
        [Category("扩展")]
        [Description("要显示的小数位数")]
        public int Decimal
        {
            get { return (int)GetValue(DecimalProperty); }
            set { SetValue(DecimalProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TextBoxNumeric()
        {
            DefaultStyleKey = typeof(TextBoxNumeric);
            OnLostFocus(null);
        }
        /// <summary>
        /// 回车时移动焦点到下一控件
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // MoveFocus takes a TraveralReqest as its argument.
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                // Gets the element with keyboard focus.
                // Change keyboard focus.
                if (Keyboard.FocusedElement is UIElement elementWithFocus)
                {
                    elementWithFocus.MoveFocus(request);
                }
                //e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        #region 数值控制
        /// <summary>
        /// 记录上一次的正常值
        /// </summary>
        private double Value;
        /// <summary>
        /// 操作按钮
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template.FindName("UpButton", this) is ButtonEXT btnUp)
            {
                btnUp.Click -= BtnUp_Click;
                btnUp.Click += BtnUp_Click;
            }
            if (Template.FindName("DownButton", this) is ButtonEXT btnDown)
            {
                btnDown.Click -= BtnDown_Click;
                btnDown.Click += BtnDown_Click;
            }
        }
        /// <summary>
        /// 记录值
        /// </summary>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (double.TryParse(this.Text, out var value))
            {
                if (this.Value != value)
                {
                    this.Value = value;
                    if (!IsLoaded) AddValue(0);
                }
            }
        }
        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            AddValue(Interval);
        }
        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            AddValue(-Interval);
        }
        private void AddValue(double interval)
        {
            if (!double.TryParse(this.Text, out var value))
            {
                value = Value;
            }
            value += interval;
            if (!MinValue.Equals(double.NaN) && value < MinValue) value = MinValue;
            else if (!MaxValue.Equals(double.NaN) && value > MaxValue) value = MaxValue;
            this.Value = value;
            this.Text = PMethod.Rounds(value, Decimal, Decimal);
            this.Select(this.Text.Length, 0);
        }
        /// <summary>
        /// 丢失焦点时判断
        /// </summary>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (e != null) base.OnLostFocus(e);
            AddValue(0);
        }
        /// <summary>
        /// 键盘加减
        /// </summary>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    AddValue(Interval);
                    break;
                case Key.Down:
                    AddValue(-Interval);
                    break;
            }
            base.OnPreviewKeyDown(e);
        }
        /// <summary>
        /// 滑动加减
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (/*this.IsFocused &&*/ !IsReadOnly)
            {
                if (e.Delta > 0) AddValue(Interval);
                else AddValue(-Interval);
            }
        }

        #endregion
    }
}
