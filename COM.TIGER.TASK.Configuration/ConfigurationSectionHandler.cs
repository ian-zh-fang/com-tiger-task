using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.Configuration
{
    public abstract class ConfigurationSectionHandler:System.Configuration.IConfigurationSectionHandler
    {
        object System.Configuration.IConfigurationSectionHandler.Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return Create(parent, configContext, section);
        }

        protected abstract object Create(object parent, object configContext, System.Xml.XmlNode section);
    }

    public abstract class ConfigurationSectionHandler<T> : ConfigurationSectionHandler where T : class, new()
    {
        protected override object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return Build(parent, configContext, section);
        }

        protected virtual T Build(object parent, object configContext, System.Xml.XmlNode section)
        {
            return Build(section, typeof(T)) as T;
        }

        protected virtual object Build(System.Xml.XmlNode node, Type tp)
        {
            var instance = Activator.CreateInstance(tp);
            var properties = tp.GetProperties();
            foreach (var t in properties)
            {
                var attributes = t.GetCustomAttributes(typeof(COM.TIGER.TASK.Configuration.ConfigurationSectionAttribute), true);

                if (attributes.Length == 0)
                    continue;

                var attribute = attributes[0] as COM.TIGER.TASK.Configuration.ConfigurationSectionAttribute;
                object val = Build(node, attribute, t.PropertyType);
                t.SetValue(instance, val, null);
            }
            return instance;
        }

        protected virtual object Build(System.Xml.XmlNode node, COM.TIGER.TASK.Configuration.ConfigurationSectionAttribute attr, Type tp)
        {
            //获取集合
            if (attr.IsCollection)
            {
                var gtp = tp.GetGenericArguments()[0];
                var addMethod = tp.GetMethod("Add");
                var list = Activator.CreateInstance(tp);
                System.Xml.XmlNodeList nds = node.SelectNodes(attr.Name);
                for (var i = 0; i < nds.Count; i++)
                {
                    addMethod.Invoke(list, new object[] { Build(nds[i], gtp) });
                }
                return list;
            }

            //获取当前节点信息
            System.Xml.XmlNode nd = node.SelectSingleNode(attr.Name);
            if (nd == null)
                return null;

            if (nd.HasChildNodes)
            {
                return Build(nd, tp);
            }

            object obj = null;
            if (attr.IsAttribute)
            {
                obj = nd.Attributes[attr.AttributeName].Value;
            }

            return obj;
        }
    }
}
