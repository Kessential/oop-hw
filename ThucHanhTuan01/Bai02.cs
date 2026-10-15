using System;
using System.Collections.Generic;
using System.Text;

namespace ThucHanhTuan01
{
    // Bai 02: Phan tich thua so nguyen to (dang luy thua chuan)
    // De bai: Voi so nguyen N >= 2, phan tich N thanh tich cac thua so nguyen to.
    public static class Bai02
    {
        public struct ThuaSo
        {
            public long SoNguyenTo;
            public int SoMu;
        }

        public static List<ThuaSo> PhanTich(long n)
        {
            List<ThuaSo> ketQua = new List<ThuaSo>();
            for (long p = 2; p * p <= n; p++)
            {
                if (n % p == 0)
                {
                    int soMu = 0;
                    while (n % p == 0)
                    {
                        soMu++;
                        n /= p;
                    }
                    ketQua.Add(new ThuaSo { SoNguyenTo = p, SoMu = soMu });
                }
            }

            if (n > 1)
            {
                ketQua.Add(new ThuaSo { SoNguyenTo = n, SoMu = 1 });
            }

            return ketQua;
        }

        public static void InPhanTich(long n)
        {
            if (n < 2)
            {
                Console.WriteLine($"{n} khong hop le (N phai >= 2).");
                return;
            }

            List<ThuaSo> factors = PhanTich(n);
            StringBuilder sb = new StringBuilder();
            sb.Append(n).Append(" = ");
            for (int i = 0; i < factors.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" * ");
                }
                sb.Append(factors[i].SoNguyenTo).Append('^').Append(factors[i].SoMu);
            }

            Console.WriteLine(sb.ToString());
            Console.WriteLine($"  -> So luong uoc nguyen to phan biet: {factors.Count}");
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 02: PHAN TICH THUA SO NGUYEN TO (K = 0) ===");

            InPhanTich(360);
            Console.WriteLine();

            InPhanTich(17);
            Console.WriteLine();

            InPhanTich(1);
        }
    }
}
