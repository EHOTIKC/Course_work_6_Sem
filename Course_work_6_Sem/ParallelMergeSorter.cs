using System;
using System.Threading.Tasks;

namespace Course_work_6_Sem
{
    public class ParallelMergeSorter
    {
        private const int ParallelThreshold = 8192;

        public static void Sort<T>(T[] array) where T : IComparable<T>
        {
            if (array == null || array.Length <= 1)
                return;

            T[] temp = new T[array.Length];
            SortParallel(array, temp, 0, array.Length - 1);
        }

        private static void SortParallel<T>(T[] array, T[] temp, int left, int right) where T : IComparable<T>
        {
            if (left >= right)
                return;

            if ((right - left) < ParallelThreshold)
            {
                MergeSortCore.SortSequential(array, temp, left, right);
                return;
            }

            int mid = left + (right - left) / 2;

            Parallel.Invoke(
                () => SortParallel(array, temp, left, mid),
                () => SortParallel(array, temp, mid + 1, right)
            );

            MergeSortCore.Merge(array, temp, left, mid, right);
        }
    }
}