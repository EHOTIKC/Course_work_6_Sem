using System;

namespace Course_work_6_Sem
{
    public static class MergeSortCore
    {
        public static void Merge<T>(T[] array, T[] temp, int left, int mid, int right) where T : IComparable<T>
        {
            for (int i = left; i <= right; i++) temp[i] = array[i];

            int iLeft = left;
            int iRight = mid + 1;
            int current = left;

            while (iLeft <= mid && iRight <= right)
            {
                if (temp[iLeft].CompareTo(temp[iRight]) <= 0)
                {
                    array[current] = temp[iLeft];
                    iLeft++;
                }
                else
                {
                    array[current] = temp[iRight];
                    iRight++;
                }
                current++;
            }

            int remaining = mid - iLeft;
            for (int i = 0; i <= remaining; i++)
            {
                array[current + i] = temp[iLeft + i];
            }
        }

        public static void SortSequential<T>(T[] array, T[] temp, int left, int right) where T : IComparable<T>
        {
            if (left >= right)
                return;

            int mid = left + (right - left) / 2;

            SortSequential(array, temp, left, mid);
            SortSequential(array, temp, mid + 1, right);
            Merge(array, temp, left, mid, right);
        }
    }
}