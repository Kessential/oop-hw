using System;
using System.Globalization;

namespace BTTuan01
{
    // Exercise 1: IF statement - giai va bien luan phuong trinh bac hai ax^2+bx+c=0
    // Da mo rong: a, b, c la so thuc (khong bat buoc a != 0) va xet day du moi truong hop cua a, b, c.
    public static class Bai01
    {
        private static double NhapSoThuc(string ten)
        {
            Console.Write(ten + "= ");
            string? dong = Console.ReadLine();
            double.TryParse(dong, NumberStyles.Float, CultureInfo.InvariantCulture, out double giaTri);
            return giaTri;
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 01: IF STATEMENT - GIAI VA BIEN LUAN PHUONG TRINH BAC 2 ===");
            Console.WriteLine("(a, b, c la so thuc, ho tro moi truong hop cua a, b, c)");

            double a = NhapSoThuc("a");
            double b = NhapSoThuc("b");
            double c = NhapSoThuc("c");

            if (a == 0)
            {
                if (b == 0)
                {
                    if (c == 0)
                        Console.WriteLine("Phuong trinh co vo so nghiem (dung voi moi x)");
                    else
                        Console.WriteLine("Phuong trinh vo nghiem");
                }
                else
                {
                    double x = -c / b;
                    Console.WriteLine("Phuong trinh bac nhat co 1 nghiem: x = {0}", x);
                }
            }
            else
            {
                double delta = b * b - 4 * a * c;

                if (delta < 0)
                    Console.WriteLine("Phuong trinh vo nghiem");
                else if (delta == 0)
                {
                    Console.WriteLine("Phuong trinh co nghiem kep");
                    Console.WriteLine("x1=x2={0}", -b / (2 * a));
                }
                else
                {
                    Console.WriteLine("Phuong trinh co 2 nghiem");
                    double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
                    Console.WriteLine("x1={0}", x1);
                    double x2 = (-b - Math.Sqrt(delta)) / (2 * a);
                    Console.WriteLine("x2={0}", x2);
                }
            }
        }
    }
}
