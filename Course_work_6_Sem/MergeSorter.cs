using System;

namespace Course_work_6_Sem
{
    public class MergeSorter
    {
        public static void Sort<T>(T[] array) where T : IComparable<T>
        {
            if (array == null || array.Length <= 1)
                return;

            T[] temp = new T[array.Length];

            MergeSortCore.SortSequential(array, temp, 0, array.Length - 1);
        }
    }
}