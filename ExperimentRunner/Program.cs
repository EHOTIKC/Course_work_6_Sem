using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Course_work_6_Sem;

namespace ExperimentRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   PARALLEL SORTING EXPERIMENT RUNNER   ");
            Console.WriteLine("========================================");

            int arraySize = GetValidInput(
                "Enter array size (100,000 to 100,000,000): ",
                100_000,
                100_000_000);

            int runs = GetValidInput(
                "Enter number of runs (1 to 20): ",
                1,
                20);

            WarmUp();

            Console.WriteLine($"\n[INFO] Generating base array of {arraySize:N0} products...");
            Product[] baseArray = GenerateRandomProducts(arraySize);
            Console.WriteLine("[INFO] Array generated successfully.\n");

            List<long> seqTimes = new List<long>();
            List<long> parTimes = new List<long>();


            for (int i = 1; i <= runs; i++)
            {
                Console.WriteLine($"--- Run {i} of {runs} ---");

                Product[] seqArray = (Product[])baseArray.Clone();
                Product[] parArray = (Product[])baseArray.Clone();
                Product[] builtInArray = (Product[])baseArray.Clone();

                Stopwatch swSeq = Stopwatch.StartNew();
                MergeSorter.Sort(seqArray);
                swSeq.Stop();

                if (!IsSorted(seqArray))
                    Console.WriteLine("[ERROR] Sequential sort result is incorrect!");

                seqTimes.Add(swSeq.ElapsedMilliseconds);
                Console.WriteLine($"Sequential Time: {swSeq.ElapsedMilliseconds} ms");

                Stopwatch swPar = Stopwatch.StartNew();
                ParallelMergeSorter.Sort(parArray);
                swPar.Stop();

                if (!IsSorted(parArray))
                    Console.WriteLine("[ERROR] Parallel sort result is incorrect!");

                parTimes.Add(swPar.ElapsedMilliseconds);
                Console.WriteLine($"Parallel Time:   {swPar.ElapsedMilliseconds} ms");

                Stopwatch swBuiltIn = Stopwatch.StartNew();
                Array.Sort(builtInArray);
                swBuiltIn.Stop();
                Console.WriteLine($"Array.Sort Time: {swBuiltIn.ElapsedMilliseconds} ms");

                double currentSpeedup = (double)swSeq.ElapsedMilliseconds / swPar.ElapsedMilliseconds;
                Console.WriteLine($"Current Speedup: {currentSpeedup:F2}x\n");

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            double avgSeqTime = seqTimes.Average();
            double avgParTime = parTimes.Average();
            double avgSpeedup = avgSeqTime / avgParTime;

            int logicalCores = Environment.ProcessorCount;
            double efficiency = avgSpeedup / logicalCores;

            Console.WriteLine("========================================");
            Console.WriteLine("           EXPERIMENT RESULTS           ");
            Console.WriteLine("========================================");
            Console.WriteLine($"Array Size:      {arraySize:N0} elements");
            Console.WriteLine($"Total Runs:      {runs}");
            Console.WriteLine($"Avg Seq Time:    {avgSeqTime:F2} ms");
            Console.WriteLine($"Avg Par Time:    {avgParTime:F2} ms");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"AVERAGE SPEEDUP: {avgSpeedup:F2}x");
            Console.WriteLine($"Logical Cores:   {logicalCores}");
            Console.WriteLine($"EFFICIENCY:      {efficiency:F4} ( {efficiency * 100:F2}%)");
            Console.WriteLine("========================================");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void WarmUp()
        {
            Console.WriteLine("\n[INFO] Warming up JIT compiler and ThreadPool...");

            Product[] warmUpArray = GenerateRandomProducts(500_000);
            Product[] copy1 = (Product[])warmUpArray.Clone();
            Product[] copy2 = (Product[])warmUpArray.Clone();

            MergeSorter.Sort(copy1);
            ParallelMergeSorter.Sort(copy2);

            Console.WriteLine("[INFO] Warm-up complete.");
        }

        static int GetValidInput(string prompt, int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (input != null)
                {
                    input = input.Replace(" ", "").Replace("_", "").Replace(",", "");
                }

                if (int.TryParse(input, out value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"[ERROR] Invalid input. Please enter a number between {min:N0} and {max:N0}.\n");
            }
        }

        static Product[] GenerateRandomProducts(int count)
        {
            Product[] array = new Product[count];
            Random rand = new Random(42);

            string[] manufacturers = { "TechCorp", "GigaByte", "MegaSystems", "NanoTech", "Quantum" };
            string[] suppliers = { "GlobalDelivery", "FastLogistics", "PrimeShip", "EcoTransport" };
            string[] countries = { "USA", "China", "Germany", "Japan", "Taiwan", "Ukraine" };

            for (int i = 0; i < count; i++)
            {
                string name = $"Product_{i}";
                double price = rand.NextDouble() * 10000;
                string manufacturer = manufacturers[rand.Next(manufacturers.Length)];
                string supplier = suppliers[rand.Next(suppliers.Length)];
                string country = countries[rand.Next(countries.Length)];

                DateTime productionDate = DateTime.Now.AddDays(-rand.Next(0, 1825));

                array[i] = new Product(i, name, price, manufacturer, supplier, country, productionDate);
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