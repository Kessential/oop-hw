using System;

namespace BTTuan01
{
    // Exercise 5 (variant): Reverse this array
    public static class Bai06
    {
        public static void Chay()
        {
            Console.WriteLine("=== BAI 06: REVERSE ARRAY ===");

            int i, j;
            int[] nums1 = new int[10];
            int[] nums2 = new int[10];

            for (i = 0; i < nums1.Length; i++) nums1[i] = i;

            Console.Write("Original contents: ");
            for (i = 0; i < nums2.Length; i++)
                Console.Write(nums1[i] + " ");
            Console.WriteLine();

            // reverse copy nums1 to nums2
            if (nums2.Length >= nums1.Length) // make sure nums2 is long enough
                for (i = 0, j = nums1.Length - 1; i < nums1.Length; i++, j--)
                    nums2[j] = nums1[i];

            Console.Write("Reversed contents: ");
            for (i = 0; i < nums2.Length; i++)
                Console.Write(nums2[i] + " ");
            Console.WriteLine();
        }
    }
}
