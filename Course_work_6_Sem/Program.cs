using System;
using System.Diagnostics;
using System.IO;

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
                Console.WriteLine("3 - Benchmark Both");
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
            Console.WriteLine($"Elapsed time: {time} ms");

            if (isSorted)
            {
                SaveArrayToFile(name, array);
            }
            else
            {
                Console.WriteLine($"[WARNING] Array is not sorted, skipping file save.\n");
            }

            return time;
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

        static void SaveArrayToFile(string algorithmName, Product[] array)
        {
            string fileName = $"{algorithmName}_Result.txt";

            Console.WriteLine($"[INFO] Saving results to {fileName}...");

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine($"--- Sorting Results for {algorithmName} ---");
                    writer.WriteLine($"Total Elements: {array.Length:N0}");
                    writer.WriteLine("------------------------------------------");

                    int maxElementsToSave = 1000;

                    if (array.Length <= maxElementsToSave)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            writer.WriteLine($"[{i}] {array[i].ToString()}");
                        }
                    }
                    else
                    {
                        int half = maxElementsToSave / 2;

                        writer.WriteLine("... Showing First 500 Elements ...");
                        for (int i = 0; i < half; i++)
                        {
                            writer.WriteLine($"[{i}] {array[i].ToString()}");
                        }

                        writer.WriteLine("\n... [SKIPPED MILLIONS OF ELEMENTS] ...\n");

                        writer.WriteLine("... Showing Last 500 Elements ...");
                        for (int i = array.Length - half; i < array.Length; i++)
                        {
                            writer.WriteLine($"[{i}] {array[i].ToString()}");
                        }
                    }
                }
                Console.WriteLine($"[INFO] Saved successfully.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to save file: {ex.Message}\n");
            }
        }
    }
}