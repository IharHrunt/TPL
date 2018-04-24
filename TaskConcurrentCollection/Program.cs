using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskConcurrentCollection
{
    class Program
    {
        private static ConcurrentDictionary<string, string> capitals = new ConcurrentDictionary<string, string>();

        public static void AddParis()
        {
            bool success = capitals.TryAdd("France", "Paris");
            string who = Task.CurrentId.HasValue ? ("Task " + Task.CurrentId) : ("Main thread");
            Console.WriteLine($"{who} - {success}");
        }

        static void Main(string[] args)
        {
            Task.Factory.StartNew(AddParis).Wait();
            AddParis();
            Console.WriteLine("-------------------------------------");

            //AddOrUpdate
            capitals["Russia"] = "Leningrad";
            capitals.AddOrUpdate("Russia", "Moscow", (k, old) => old + " --> Moscow");
            Console.WriteLine(capitals["Russia"].ToString());

            //TryRemove            
            string removed;
            bool didRemove = capitals.TryRemove("Russia", out removed);
            Console.WriteLine(didRemove);

            //GetOrAdd
            capitals["Sweden"] = "Upsalla";
            var capitalSweden = capitals.GetOrAdd("Sweden", "Stockholm");
            Console.WriteLine(capitalSweden);
            Console.WriteLine("-------------------------------------");

            var queue = new ConcurrentQueue<int>();
            int resultQueue;
            queue.Enqueue(1);
            queue.Enqueue(2);

            if (queue.TryDequeue(out resultQueue))
            {
                Console.WriteLine($"DeQueue{resultQueue}");
            }

            if (queue.TryPeek(out resultQueue))
            {
                Console.WriteLine($"Peek {resultQueue}");
            }
            Console.WriteLine("-------------------------------------");

            var stack = new ConcurrentStack<int>();
            int reusltStack;
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);

            if (stack.TryPop(out reusltStack))
            {
                Console.WriteLine($"Pop {reusltStack}");
            }

            if (stack.TryPeek(out reusltStack))
            {
                Console.WriteLine($"Peek {reusltStack}");
            }

            var items = new int[5];
            if (stack.TryPopRange(items, 0, 5) > 0)
            {
                var text = string.Join(", ", items.Select(x => x.ToString()));
                Console.WriteLine($"TryPop {text}");

            }
            Console.WriteLine("-------------------------------------");

            var bag = new ConcurrentBag<int>();
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var i1 = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    bag.Add(i1);
                    Console.WriteLine($"Task {Task.CurrentId} has added {i1}");
                    int resultBag;
                    if (bag.TryPeek(out resultBag))
                    {
                        Console.WriteLine($"{Task.CurrentId} has peeked the value {resultBag}");
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.ReadLine();
        }
    }
}
