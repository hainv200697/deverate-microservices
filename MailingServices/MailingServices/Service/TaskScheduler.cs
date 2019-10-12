using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace MailingServices.Service
{
    public class TaskScheduler
    {
        public TaskScheduler()
        {
        }


        public static void OnStart()
        {
            // When Windows service starts, creates and starts separate thread
            ThreadStart tsTask = new ThreadStart(TaskLoop);
            Thread MyTask = new Thread(tsTask);
            MyTask.Start();

        }

        static void TaskLoop()
        {
            while (true)
            {
                Console.WriteLine("ahihi");
            }
        }

        static void ScheduledTask()
        {
        }
    }
}
