using System;

namespace ThucHanhTuan01
{
    // Bai 08: De quy & Bai toan Thap Ha Noi co dien (Bien the K = 0)
    // De bai: Chuyen N dia tu coc nguon sang coc dich, dung coc trung gian, dem so buoc.
    public static class Bai08
    {
        public static void ThapHaNoi(int n, char nguon, char dich, char trungGian, ref int demBuoc)
        {
            if (n == 1)
            {
                demBuoc++;
                Console.WriteLine($"  Buoc {demBuoc}: Chuyen dia 1 tu coc {nguon} sang coc {dich}");
                return;
            }

            ThapHaNoi(n - 1, nguon, trungGian, dich, ref demBuoc);
            demBuoc++;
            Console.WriteLine($"  Buoc {demBuoc}: Chuyen dia {n} tu coc {nguon} sang coc {dich}");
            ThapHaNoi(n - 1, trungGian, dich, nguon, ref demBuoc);
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 08: DE QUY - THAP HA NOI CO DIEN (K = 0) ===");

            int n = 3;
            Console.WriteLine($"Mo phong voi N = {n} dia (coc A -> C, coc trung gian B):");

            int buoc = 0;
            ThapHaNoi(n, 'A', 'C', 'B', ref buoc);

            Console.WriteLine($"Tong so buoc thuc hien: {buoc} (Cong thuc: 2^{n} - 1 = {(1 << n) - 1})");
        }
    }
}
