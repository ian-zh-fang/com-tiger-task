using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace COM.TIGER.TASK.Service
{
    public partial class Service1 : ServiceBase
    {
        private readonly Task.TaskHandler _handler = null;

        public Service1()
        {
            InitializeComponent();
            _handler = new Task.TaskHandler();
        }

        protected override void OnStart(string[] args)
        {
            _handler.Start();
        }

        protected override void OnStop()
        {
            _handler.Shutdown();
        }
    }
}
