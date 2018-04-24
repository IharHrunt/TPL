using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Mutex
{

    public class BankAccount
    {
        private int balance;
        public int Balance
        {
            get { return balance; }
            private set { balance = value; }
        }

        public void Deposit(int amount)
        {
            Balance += amount;
        }
        public void Withdraw(int amount)
        {
            Balance -= amount;
        }

        public void Transfer(BankAccount where, int amount)
        {
            Balance -= amount;
            where.Balance += amount;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {

            var ba = new BankAccount();
            var ba2 = new BankAccount();
            var tasks = new List<Task>();

            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock) mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock) mutex2.ReleaseMutex();
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = Mutex.WaitAll(new[] { mutex, mutex2 });
                        try
                        {
                            ba.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));


            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The balance ba = {ba.Balance}");
            Console.WriteLine($"The balance ba2 = {ba2.Balance}");
            Console.WriteLine("The Main done");
            Console.ReadKey();
        }
    }
}
