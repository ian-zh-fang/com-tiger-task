using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.TIGER.TASK.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new TASK.Task.TaskHandler();
            handler.OnShutdown += handler_OnShutdown;
            handler.OnStart += handler_OnStart;
            handler.OnExecute += handler_OnExecute;

            handler.Start();
            Console.WriteLine("please enter the key 'Q' shutdown taskhandler .");
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                if (ConsoleKey.Q == Console.ReadKey().Key)
                {
                    Console.WriteLine();
                    break;
                }
            }
            Console.WriteLine("please wait for any times, taskhandler is completing .");
            handler.Shutdown();
            
            Console.WriteLine("please enter the key 'enter' exit application .");
            Console.Read();
        }

        static void handler_OnExecute(object obj)
        {
            Console.WriteLine(obj);
        }

        static void handler_OnStart(object obj)
        {
            Console.WriteLine("taskhandler is starting ...");
        }

        static void handler_OnShutdown(object obj)
        {
            Console.WriteLine("\ntaskhandler is shutdown .");
        }
    }
}
