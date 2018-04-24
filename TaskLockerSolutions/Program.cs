using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLockerSolutions
{
    class Program
    {
        static void Main(string[] args)
        {
            var taskParent = new Task(() =>
            {
                var taskChild = new Task(() =>
                {
                    Console.WriteLine("Child task started");
                    Thread.Sleep(2000);
                    Console.WriteLine("Child task finished");
                }, TaskCreationOptions.AttachedToParent);

                taskChild.Start();
            });
            taskParent.Start();

            try
            {
                taskParent.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle(e=> true);
            }

            //var task = Task.Factory.StartNew(() => "Task1");
            //var task2 = Task.Factory.StartNew(() => "Task2");
            //var task3 = Task.Factory.ContinueWhenAll(new[] { task, task2 }, 
            //    tasks =>
            //    {
            //        Console.WriteLine("Tasks completed:");
            //        foreach (var t in tasks)
            //        {
            //            Console.WriteLine(t.Result);
            //        }
            //        Console.WriteLine("All tasks done");
            //    });
            //task3.Wait();            

            //var task = Task.Factory.StartNew(()=>
            //{
            //    Console.WriteLine("Boiling watter");
            //});

            //var task2 = task.ContinueWith(t=> 
            //{
            //    Console.WriteLine($"Continue task with {t.Id}, pour watter into cup.");
            //});
            //task2.Wait();

            Console.WriteLine("Main thread finished");
            Console.ReadLine();
        }
    }
}
