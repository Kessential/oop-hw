using System;

namespace BTTuan01
{
    // Exercise 5 (variant): Using multi-dimensional array
    public static class Bai07
    {
        public static void Chay()
        {
            Console.WriteLine("=== BAI 07: MULTI-DIMENSIONAL ARRAY ===");

            const int rows = 4;
            const int columns = 3;

            // declare a 4x3 integer array
            int[,] rectangularArray = new int[rows, columns];

            // populate the array
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    rectangularArray[i, j] = i + j;
                }
            }

            // report the contents of the array
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.WriteLine("rectangularArray[{0},{1}] = {2}",
                        i, j, rectangularArray[i, j]);
                }
            }
        }
    }
}
