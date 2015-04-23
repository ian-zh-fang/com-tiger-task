using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.Task
{
    public class TaskHandler:IDisposable
    {
        /// <summary>
        /// 日志信息处理模块
        /// </summary>
        protected readonly Common.Logging.ILog _Logger = null;

        /// <summary>
        /// 任务调度程序处理模块
        /// </summary>
        protected readonly Quartz.IScheduler _Scheduler = null;

        /// <summary>
        /// 任务调度程序开始执行
        /// </summary>
        public event Action<object> OnStart;

        /// <summary>
        /// 任务调度程序已经结束
        /// </summary>
        public event Action<object> OnShutdown;

        public event Action<object> OnExecute;

        public TaskHandler()
        {
            _Logger = Common.Logging.LogManager.GetLogger(GetType());

            //依据配置文件获取需要调度任务
            var fact = new Quartz.Impl.StdSchedulerFactory();
            _Scheduler = fact.GetScheduler();

            //在此注册任务处理模块时间处理
            JobBaseHandler.OnExecute += JobBaseHandler_OnExecute;
        }

        void JobBaseHandler_OnExecute(object obj)
        {
            //在此处理日志信息
            ExecuteLog(obj as SyncEventArg);
            //其他动作
            EventTrgger(OnExecute, obj);
            Console.WriteLine(obj);
        }

        private void ExecuteLog(SyncEventArg e)
        {
            if (e == null) return;

            switch (e.Level)
            {
                case SyncEventLevel.ERROR:
                    _Logger.Error(e.Message);
                    break;
                case SyncEventLevel.EXCEPTION:
                    _Logger.Fatal(e.ExceptionContext);
                    break;
                case SyncEventLevel.INFO:
                    _Logger.Info(e.Argument);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 任务调度模块处理程序开始执行
        /// </summary>
        public virtual void Start()
        {
            _Logger.Info("任务调度程序开始初始化 ...");

            _Scheduler.Start();

            _Logger.Info("任务调度程序开始执行 .");
            EventTrgger(OnStart);
        }

        /// <summary>
        /// 关闭任务调度处理程序
        /// </summary>
        public virtual void Shutdown()
        {
            _Logger.Info("任务调度程序开始关闭 ...");

            _Scheduler.Shutdown();
            
            _Logger.Info("任务调度程序已经关闭 .");
            EventTrgger(OnShutdown);
        }

        /// <summary>
        /// 在此统一触发事件
        /// </summary>
        /// <param name="eventInstance">事件对象</param>
        /// <param name="args">事件参数</param>
        protected virtual void EventTrgger(Action<object> eventInstance, object args = null)
        {
            args = args ?? this;

            if (eventInstance != null)
                eventInstance(args);
        }

        public void Dispose()
        {
            Disposed(false);
        }

        private void Disposed(bool disposing)
        {
            try { }
            finally
            {
                if (disposing)
                    GC.Collect();
            }
        }
    }
}
