using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.Task
{
    public abstract class JobBaseHandler : Quartz.IJob
    {
        public static event Action<object> OnExecute;
    
        public abstract object EventContext { get; }

        public void Execute(Quartz.IJobExecutionContext context)
        {
            ExecuteJob(context);
        }

        protected virtual void EventTigger()
        {
            if (OnExecute != null)
                OnExecute(EventContext);
        }

        protected abstract void ExecuteJob(Quartz.IJobExecutionContext context);
    }
}
