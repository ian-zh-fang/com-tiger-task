using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization
{
    public class SyncModel
    {
        /// <summary>
        /// 默认值
        /// </summary>
        [COM.TIGER.TASK.Configuration.ConfigurationSection(Name = "defaults")]
        public SyncContext DefaultContext { get; set; }

        /// <summary>
        /// 任务集
        /// </summary>
        [COM.TIGER.TASK.Configuration.ConfigurationSection(Name="tasks")]
        public TasksContext TaskList { get; set; }

        public TaskContext this[int index]
        {
            get { return TaskList[index]; }
        }

        public TaskContext this[string name]
        {
            get { return TaskList[name]; }
        }

        public int Count()
        {
            if (TaskList == null)
                return 0;

            return TaskList.Count();
        }
    }

    public class SyncContext
    {
        /// <summary>
        /// 目标数据库连接字符串
        /// </summary>
        [COM.TIGER.TASK.Configuration.ConfigurationSection(Name = "target", IsAttribute = true, AttributeName = "value")]
        public string DbTarget { get; set; }

        /// <summary>
        /// 数据来源数据库连接字符串
        /// </summary>
        [COM.TIGER.TASK.Configuration.ConfigurationSection(Name = "from", IsAttribute = true, AttributeName = "value")]
        public string DbFrom { get; set; }

        /// <summary>
        /// 数据来源 T-SQL 命令
        /// </summary>
        [COM.TIGER.TASK.Configuration.ConfigurationSection(Name = "command", IsAttribute = true, AttributeName = "value")]
        public string CommandText { get; set; }
    }

    public class TaskContext:SyncContext
    {
        /// <summary>
        /// 任务处理程序集名称
        /// <para>标识处理当前任务的程序集全称</para>
        /// </summary>
        [COM.TIGER.TASK.Configuration.ConfigurationSection(Name = "name", IsAttribute = true, AttributeName = "value")]
        public string Name { get; set; }
    }

    public class TasksContext
    {
        /// <summary>
        /// 具体任务集
        /// </summary>
        [COM.TIGER.TASK.Configuration.ConfigurationSection(Name = "task", IsCollection = true)]
        public List<TaskContext> Tasks { get; set; }

        public TaskContext this[int index]
        {
            get { return Tasks[index]; }
        }

        public TaskContext this[string name]
        {
            get { return Tasks.FirstOrDefault(t => t.Name == name); }
        }

        public int Count()
        {
            if (Tasks == null)
                return 0;

            return Tasks.Count;
        }
    }
}
