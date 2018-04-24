using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ManualResentEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            //var evt = new ManualResetEventSlim();
            var evt = new AutoResetEvent(false);

            var water = Task.Factory.StartNew(()=>
            {
                Console.WriteLine("Boiling water...");
                Thread.Sleep(1000);
                evt.Set();
            });

            var tea = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Waiting for water...");
                Thread.Sleep(500);
                //evt.Wait();
                evt.WaitOne();
                Console.WriteLine("Here your tea!");
                //evt.Wait();
                evt.WaitOne();

            });

            Console.ReadLine();
        }
    }
}
