using System;
using System.Diagnostics;

namespace Course_work_6_Sem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                Console.Clear();

                int arraySize = 5_000_000;

                Console.WriteLine($"Generating array of {arraySize:N0} products...");
                Product[] originalArray = GenerateRandomProducts(arraySize);
                Console.WriteLine("Array generated successfully.\n");

                Console.WriteLine("Select sorting method:");
                Console.WriteLine("1 - Sequential Sort");
                Console.WriteLine("2 - Parallel Sort");
                Console.WriteLine("3 - Benchmark Both (Compare for Coursework)");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "1")
                {
                    Product[] arrayCopy = (Product[])originalArray.Clone();
                    RunSort("Sequential", () => MergeSorter.Sort(arrayCopy), arrayCopy);
                }
                else if (choice == "2")
                {
                    Product[] arrayCopy = (Product[])originalArray.Clone();
                    RunSort("Parallel", () => ParallelMergeSorter.Sort(arrayCopy), arrayCopy);
                }
                else if (choice == "3")
                {
                    Product[] arrayForSeq = (Product[])originalArray.Clone();
                    Product[] arrayForPar = (Product[])originalArray.Clone();

                    long seqTime = RunSort("Sequential", () => MergeSorter.Sort(arrayForSeq), arrayForSeq);
                    long parTime = RunSort("Parallel", () => ParallelMergeSorter.Sort(arrayForPar), arrayForPar);

                    double speedup = (double)seqTime / parTime;
                    Console.WriteLine("========================================");
                    Console.WriteLine($"Speedup Coefficient: {speedup:F2}x");
                    Console.WriteLine("========================================");
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }

                Console.WriteLine("\nPress ESC to exit, or any other key to continue...");

                keyInfo = Console.ReadKey(true);

            } while (keyInfo.Key != ConsoleKey.Escape);
        }


        static long RunSort(string name, Action sortMethod, Product[] array)
        {
            Console.WriteLine($"--- Starting {name} Sort ---");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            sortMethod.Invoke();

            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds;

            bool isSorted = IsSorted(array);

            Console.WriteLine($"Status: {(isSorted ? "SUCCESS" : "FAILED")}");
            Console.WriteLine($"Elapsed time: {time} ms\n");

            return time;
        }

        static Product[] GenerateRandomProducts(int count)
        {
            Product[] array = new Product[count];
            Random rand = new Random(42);

            for (int i = 0; i < count; i++)
            {
                string name = $"Product_{i}";
                double price = rand.NextDouble() * 10000;
                array[i] = new Product(i, name, price);
            }
            return array;
        }

        static bool IsSorted(Product[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i].CompareTo(array[i + 1]) > 0)
                    return false;
            }
            return true;
        }
    }
}