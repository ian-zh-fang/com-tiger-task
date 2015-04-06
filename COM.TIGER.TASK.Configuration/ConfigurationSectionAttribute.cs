using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.Configuration
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false, Inherited=false)]
    public class ConfigurationSectionAttribute:Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// 当前值是否是属性
        /// </summary>
        public bool IsAttribute { get; set; }

        /// <summary>
        /// 保存值的属性名称
        /// <para>与 IsAttribute 结合使用</para>
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// 标识当前节点是节点组
        /// </summary>
        public bool IsCollection { get; set; }
    }
}
