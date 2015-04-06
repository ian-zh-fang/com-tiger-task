using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COM.TIGER.TASK.ServiceConsoleForm
{
    public partial class Form1 : Form
    {
        private readonly static Common.Logging.ILog LOGGER = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly static string REGISTEREYPATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private Task.TaskHandler _Handler = null;

        public Form1()
        {
            InitializeComponent();

            LOGGER.Info("初始化服务程序 .");
        }

        Task.TaskHandler CreateHandler()
        {
            _Handler = new Task.TaskHandler();
            _Handler.OnExecute += _Handler_OnExecute;
            _Handler.OnShutdown += _Handler_OnShutdown;
            _Handler.OnStart += _Handler_OnStart;

            return _Handler;
        }

        void _Handler_OnStart(object obj)
        {
            SetNotifyIcon(Properties.Resources.enable_server, "任务调度程序已经打开 .");
        }

        void _Handler_OnShutdown(object obj)
        {
            SetNotifyIcon(Properties.Resources.remove_server, "任务调度程序已经关闭 .");
        }

        void _Handler_OnExecute(object obj)
        {
            notifyIcon1.Text = "任务调度程序正在执行 ...";
        }

        void SetNotifyIcon(Icon icon, string tip)
        {
            notifyIcon1.Icon = icon;
            notifyIcon1.Text = tip;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckRegiste();
            btnRegister.Enabled = !CheckResiterKey();

            this.Icon = Properties.Resources.server;
            notifyIcon1.Icon = Properties.Resources.server;

            btnStart.Image = imageList1.Images["play"];
            btnExit.Image = imageList1.Images["error"];
            btnStop.Image = imageList1.Images["stop"];

            this.Hide();
            //开始任务调度
            CreateHandler().Start();
        }

        private void CheckRegiste()
        {
            try
            {
                if (!CheckResiterKey())
                {
                    SetAutoRun(true);
                }
            }
            catch (Exception e)
            {
                LOGGER.Error("注册开机启动失败 .");
                LOGGER.Fatal(e);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            CreateHandler().Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SetNotifyIcon(Properties.Resources.remove_server, "任务调度程序正在关闭 ...");
            _Handler.Shutdown();
            _Handler.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SetNotifyIcon(Properties.Resources.remove_server, "任务调度程序正在退出 ...");
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            btnRegister.Enabled = false;

            _Handler.Shutdown();
            Application.Exit();
        }

        private void SetAutoRun(bool isAutoRun, string fileName = null)
        {
            fileName = fileName ?? Application.ExecutablePath;

            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(REGISTEREYPATH, true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(REGISTEREYPATH);
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
            }
            catch(Exception e) 
            {
                LOGGER.Error("注册开机启动失败 .");
                LOGGER.Fatal(e);
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }

        private bool CheckResiterKey(string fileName = null)
        {
            bool flag = false;
            try
            {
                fileName = fileName ?? Application.ExecutablePath;

                string name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                var reg = Registry.LocalMachine.OpenSubKey(REGISTEREYPATH, true);
                var obj = reg.GetValue(name);

                flag = obj != null;
            }
            catch (Exception e)
            {
                flag = false;
                LOGGER.Error("注册开机启动失败 .");
                LOGGER.Fatal(e);
            }

            return flag;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            CheckRegiste();
            btnRegister.Enabled = !CheckResiterKey();
        }
    }
}
