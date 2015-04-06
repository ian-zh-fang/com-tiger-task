using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.Task
{
    public class SyncEventArg : EventArgs
    {
        private readonly string _Message = null;
        private readonly object _Argument = null;
        private readonly Exception _Exception = null;
        private readonly SyncEventLevel _Level = SyncEventLevel.INFO;

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Message
        {
            get { return _Message; }
        }

        /// <summary>
        /// 附加对象
        /// </summary>
        public object Argument
        {
            get { return _Argument; }
        }

        /// <summary>
        /// 附加异常对象
        /// </summary>
        public Exception ExceptionContext
        {
            get { return _Exception; }
        }

        /// <summary>
        /// 消息等级
        /// </summary>
        public SyncEventLevel Level
        {
            get { return _Level; }
        }

        public SyncEventArg(object argument, SyncEventLevel level = SyncEventLevel.INFO, Exception ecpt = null, string message = null)
        {
            _Message = message;
            _Argument = argument;
            _Exception = ecpt;
            _Level = level;
        }
    }

    /// <summary>
    /// 事件信息等级
    /// </summary>
    public enum SyncEventLevel : byte
    {
        /// <summary>
        /// 普通信息
        /// </summary>
        INFO = 0x00,
        /// <summary>
        /// 错误信息
        /// </summary>
        ERROR = 0x01,
        /// <summary>
        /// 异常信息
        /// </summary>
        EXCEPTION = 0x10,
    }
}
