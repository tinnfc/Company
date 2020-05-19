﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 自定义默认、鼠标划过时、鼠标点击时的大小
    /// </summary>
    [TypeConverter(typeof(DoubleRoundConverter))]
    public class DoubleRound : IEquatable<DoubleRound>
    {
        /// <summary>
        /// 默认的大小
        /// </summary>
        public double Normal { get; set; }
        /// <summary>
        /// 鼠标划过时的大小
        /// </summary>
        public double Mouse { get; set; }
        /// <summary>
        /// 鼠标点击时的大小
        /// </summary>
        public double Pressed { get; set; }

        /// <summary>
        /// </summary>
        public DoubleRound()
        {
            if (Normal == 0) Normal = new ThemeMonitor().FontSize;
            if (Mouse == 0) Mouse = new ThemeMonitor().FontSize;
            if (Pressed == 0) Pressed = new ThemeMonitor().FontSize;
        }
        /// <summary>
        /// </summary>
        public DoubleRound(double value) : this(value, value, value) { }
        /// <summary>
        /// </summary>
        public DoubleRound(double? normal, double? mouse, double? pressed)
        {
            if (normal != null) Normal = normal.Value;
            if (mouse != null) Mouse = mouse.Value;
            else Mouse = Normal;
            if (pressed != null) Pressed = pressed.Value;
            else Pressed = Mouse;
            if (Normal == 0) Normal = new ThemeMonitor().FontSize;
        }
        /// <summary>
        /// </summary>
        public bool Equals(DoubleRound other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转DoubleRound
    /// </summary>
    public class DoubleRoundConverter : TypeConverter
    {
        /// <summary>
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        /// <summary>
        /// 自定义字符串转换，以分号(;)隔开
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw base.GetConvertFromException(value);
            }
            if (value is double)
            {
                return new DoubleRound((double)value);
            }
            if (value is string str)
            {
                var strs = str.Split(';');
                double? normal = null;
                double? mouse = null;
                double? pressed = null;
                if (strs.Length > 0) Parse(strs[0], out normal);
                if (strs.Length > 1) Parse(strs[1], out mouse);
                if (strs.Length > 2) Parse(strs[2], out pressed);
                return new DoubleRound(normal, mouse, pressed);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private void Parse(string str, out double? value)
        {
            if (double.TryParse(str, out double result))
            {
                value = result;
            }
            else value = null;
        }
        /// <summary>
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);
            return value.ToString();
        }
    }
}
