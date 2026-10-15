using System;

namespace ThucHanhTuan01
{
    // Bai 03: Modul so nguyen to & Gia thuyet Goldbach
    // De bai: Kiem tra so nguyen to, liet ke so nguyen to <= M, kiem tra Goldbach cho so chan N > 2.
    public static class Bai03
    {
        public static bool IsPrime(int n)
        {
            if (n < 2)
            {
                return false;
            }

            for (int i = 2; (long)i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static void ListPrimes(int m)
        {
            Console.Write($"Cac so nguyen to <= {m}: ");
            for (int i = 2; i <= m; i++)
            {
                if (IsPrime(i))
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine();
        }

        public static bool Goldbach(int n, out int p, out int q)
        {
            p = q = 0;
            if (n <= 2 || n % 2 != 0)
            {
                return false;
            }

            for (int a = 2; a <= n / 2; a++)
            {
                if (IsPrime(a) && IsPrime(n - a))
                {
                    p = a;
                    q = n - a;
                    return true;
                }
            }

            return false;
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 03: MODUL SO NGUYEN TO & GIA THUYET GOLDBACH (K = 0) ===");

            ListPrimes(10);
            Console.WriteLine();

            if (Goldbach(20, out int p, out int q))
            {
                Console.WriteLine($"Kiem tra Goldbach cho 20: 20 = {p} + {q}");
            }
            else
            {
                Console.WriteLine("Khong tim duoc cap so nguyen to thoa Goldbach cho 20.");
            }
            Console.WriteLine();

            if (Goldbach(7, out p, out q))
            {
                Console.WriteLine($"Kiem tra Goldbach cho 7: 7 = {p} + {q}");
            }
            else
            {
                Console.WriteLine("7 khong ap dung gia thuyet Goldbach (n phai la so chan lon hon 2).");
            }
        }
    }
}
