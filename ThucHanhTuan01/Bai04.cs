using System;

namespace ThucHanhTuan01
{
    // Bai 04: Mang 1 chieu - Loc va xoa so am tan cung bang 6 tai cho
    // De bai: Tim phan tu am co chu so tan cung la 6, sau do xoa het cac so am ra khoi mang tai cho (O(1) bo nho phu).
    public static class Bai04
    {
        public static int? TimSoAmTanCungBang6(int[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] < 0 && Math.Abs(a[i]) % 10 == 6)
                {
                    return i;
                }
            }

            return null;
        }

        public static int XoaSoAmTaiCho(int[] a)
        {
            int writeIdx = 0;
            for (int readIdx = 0; readIdx < a.Length; readIdx++)
            {
                if (a[readIdx] >= 0)
                {
                    a[writeIdx++] = a[readIdx];
                }
            }

            return writeIdx;
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 04: MANG 1 CHIEU - LOC VA XOA SO AM TAN CUNG BANG 6 (K = 0) ===");

            int[] a = { 12, -16, -26, 45, -3, 8 };
            Console.WriteLine("Mang goc: " + string.Join(", ", a));

            int? viTri = TimSoAmTanCungBang6(a);
            if (viTri.HasValue)
            {
                Console.WriteLine($"Phan tu am tan cung 6 dau tien: {a[viTri.Value]} tai vi tri {viTri.Value}");
            }
            else
            {
                Console.WriteLine("Khong co phan tu am nao tan cung bang 6.");
            }

            int n = XoaSoAmTaiCho(a);
            Console.WriteLine("Mang sau khi xoa cac so am: " + string.Join(", ", a[0..n]));
        }
    }
}
