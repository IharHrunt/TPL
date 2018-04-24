using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TaskReadWriteLock
{
    class Program
    {
        static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();
        static Random rand = new Random();

        static void Main(string[] args)
        {
            int x = 0;

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padLock.EnterReadLock();
                    Console.WriteLine($"Entered read lock, x = {x}");
                    Thread.Sleep(5000);
                    padLock.ExitReadLock();
                    Console.WriteLine($"Exit read lock, x = {x}");
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();
                padLock.EnterWriteLock();
                Console.WriteLine("Write lock acquired");
                int newValue = rand.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");
                padLock.ExitWriteLock();
                Console.WriteLine("Write lock realeased");
            }

        }
    }
}
