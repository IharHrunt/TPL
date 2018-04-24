using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TaskLocker
{
    public class BankAccount
    {
        public object locker = new object();

        private int balance;
        public int Balance
        {
            get { return balance; }
            private set { balance = value; }
        }

        public void Deposit(int amount)
        {
            lock (locker)
            {
                Balance += amount;
            }
        }
        public void Withdraw(int amount)
        {
            lock (locker)
            {
                Balance -= amount;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount();
            var ba2 = new BankAccount();
            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Deposit(100);
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Withdraw(100);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The balance ba = {ba.Balance}");
            Console.WriteLine("The Main done");
            Console.ReadKey();
        }
    }
}
