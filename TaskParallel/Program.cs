using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Threading;


namespace TaskParallel
{
    class Program
    {
        public static IEnumerable<int> Range (int start, int end, int step)
        {
            for (int i = start; i < end; i += step)
            {
                yield return i;
            } 
        }
        static void Main(string[] args)
        {   
            var a = new Action(() => Console.WriteLine($"First Task - {Task.CurrentId}"));
            var b = new Action(() => Console.WriteLine($"Second Task - {Task.CurrentId}"));
            var c = new Action(() => Console.WriteLine($"Third Task - {Task.CurrentId}"));
            Parallel.Invoke(a, b, c);

            Parallel.For(1, 11, (i) => 
            {
                Console.WriteLine(i * i);
            });

            string[] words = {"abc", "bssddddd", "c", "dqawfwegrfserfg"};
            Parallel.ForEach(words, word =>
            {
                Console.WriteLine($"word has length {word.Length}");
            });

            Parallel.ForEach(Range(1, 20, 3), Console.WriteLine);

            Console.ReadLine();
        }
    }
}
