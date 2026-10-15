using System;
using System.Collections.Generic;

namespace ThucHanhTuan01
{
    // Bai 05: Ma tran 2 chieu & Bai toan sap xep Zigzag (Bien the K = 0)
    // De bai: Sap xep tat ca phan tu cua ma tran tang dan, sau do dien lai theo kieu zigzag:
    // hang chan dien tu trai sang phai, hang le dien tu phai sang trai.
    public static class Bai05
    {
        public static void SapXepZigzag(int[,] mat)
        {
            int m = mat.GetLength(0);
            int n = mat.GetLength(1);

            List<int> flat = new List<int>();
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    flat.Add(mat[i, j]);
            flat.Sort();

            int idx = 0;
            for (int i = 0; i < m; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < n; j++)
                        mat[i, j] = flat[idx++];
                }
                else
                {
                    for (int j = n - 1; j >= 0; j--)
                        mat[i, j] = flat[idx++];
                }
            }
        }

        public static void InMaTran(int[,] mat)
        {
            int m = mat.GetLength(0);
            int n = mat.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    Console.Write(mat[i, j].ToString().PadLeft(4));
                Console.WriteLine();
            }
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 05: MA TRAN 2 CHIEU - SAP XEP ZIGZAG (K = 0) ===");

            int[,] mat = { { 9, 2, 7 }, { 4, 6, 1 }, { 8, 3, 5 } };

            Console.WriteLine("Ma tran goc:");
            InMaTran(mat);

            SapXepZigzag(mat);

            Console.WriteLine("Ma tran sau khi sap xep Zigzag:");
            InMaTran(mat);
        }
    }
}
