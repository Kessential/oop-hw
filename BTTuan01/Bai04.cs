using System;

namespace BTTuan01
{
    // Exercise 4: Foreach statement - mang so ngau nhien va tinh trung binh
    public static class Bai04
    {
        public static void Chay()
        {
            Console.WriteLine("=== BAI 04: FOREACH STATEMENT - MANG NGAU NHIEN & TRUNG BINH ===");

            DateTime now = DateTime.Now;
            Random rand = new Random((int)now.Millisecond);
            int[] Arr = new int[10];
            for (int x = 0; x < Arr.Length; ++x)
            {
                Arr[x] = rand.Next() % 100;
            }
            int Total = 0;
            Console.Write("Array values are ");
            foreach (int val in Arr)
            {
                Total += val;
                Console.Write(val + ", ");
            }
            Console.WriteLine("\nAnd the average is {0:F1}", (double)Total / (double)Arr.Length);
        }
    }
}
