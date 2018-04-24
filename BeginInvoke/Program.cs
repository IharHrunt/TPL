using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeginInvoke
{
    public class MyTest
    {
        public int MethodChild(int x, int y, int z)
        {
            Console.WriteLine($"Child thread start - #{Thread.CurrentThread.ManagedThreadId}");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("---");
                Thread.Sleep(500);
            }
            Console.WriteLine("Child thread done");
            return x * y * z;
        }

        public void MethodCallback(IAsyncResult iAsyncResult)
        {
            var asyncResult = iAsyncResult as AsyncResult;
            var del = (Func<int, int, int, int>)asyncResult.AsyncDelegate;
            var result = del.EndInvoke(iAsyncResult);

            Console.WriteLine($"Result form child thread is {result}");

            Console.WriteLine($"Callback method - " +
                $"#{Thread.CurrentThread.ManagedThreadId}, " +
                $"asyncResult = {iAsyncResult.AsyncState.ToString()}");
        }
    }

    public class Parameter
    {
        public int par { get; set; }

        public override string ToString()
        {
            return par.ToString();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Maint thread start - # {Thread.CurrentThread.ManagedThreadId}");

            var test = new MyTest();
            var del = new Func<int, int, int, int>(test.MethodChild);
            var callback = new AsyncCallback(test.MethodCallback);

            IAsyncResult asyncResult = del.BeginInvoke(7, 10, 3, callback, new Parameter() { par = 333 });           

            Console.WriteLine($"Maint thread continues to work...");            
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("+++");
                Thread.Sleep(300);
            }

            //asyncResult.AsyncWaitHandle.WaitOne();
            //var result = del.EndInvoke(asyncResult);
            //asyncResult.AsyncWaitHandle.Close();
            //Console.WriteLine($"Result form child thread is {result}");


            Console.WriteLine("Main thread done");
            Console.ReadLine();
        }
       
    }
}
