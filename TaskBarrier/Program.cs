using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBarrier
{
    class Program
    {
        static Barrier barrier = new Barrier(2, b => 
        {
            Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished");
        });

        public static void LongAction()
        {
            Console.WriteLine("Long action before");
            Thread.Sleep(2000);
            barrier.SignalAndWait();
            Console.WriteLine("Long action after");
        }

        public static void ShortAction()
        {
            Console.WriteLine("Short action before");
            barrier.SignalAndWait();
            Console.WriteLine("Short action after");
        }

        static void Main(string[] args)
        {
            var longTask = Task.Factory.StartNew(LongAction);
            var shortTask = Task.Factory.StartNew(ShortAction);
            var actionTask = Task.Factory.ContinueWhenAll(new[] { longTask, shortTask }, tasks =>
            {
                Console.WriteLine("I enjoy barrier ...");
            });
            actionTask.Wait();

            Console.ReadLine();
        }
    }
}
