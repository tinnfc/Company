﻿using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 管理数据
    /// </summary>
    [Serializable]
    public class AdminInfo
    {
        /// <summary>
        /// 数据库版本
        /// </summary>
        public int Version { get; set; }
    }
    /// <summary>
    /// 管理数据结构
    /// </summary>
    [Serializable]
    [Table("Admins")]
    public class AdminBaseInfo : BaseInfo, IInfo
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public AdminBaseInfo()
        {
            this.CreateOn = DateTime.Now;
        }
    }
}
