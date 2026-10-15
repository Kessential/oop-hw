using System;

namespace BTTuan01
{
    // Exercise 3: For statement - tim uoc so nho nhat trong {2,3,5,X}
    // Da tong quat hoa: uoc so thu 4 (von co dinh la 7) nay do nguoi dung nhap.
    public static class Bai03
    {
        public static void Chay()
        {
            Console.WriteLine("=== BAI 03: FOR STATEMENT - UOC SO NHO NHAT ===");

            Console.Write("Nhap uoc so thu 4 (thay cho 7): ");
            int uocSo4 = Convert.ToInt32(Console.ReadLine());

            int num;
            for (num = 2; num < 12; num++)
            {
                if ((num % 2) == 0)
                    Console.WriteLine("Smallest factor of " + num + " is 2.");
                else if ((num % 3) == 0)
                    Console.WriteLine("Smallest factor of " + num + " is 3.");
                else if ((num % 5) == 0)
                    Console.WriteLine("Smallest factor of " + num + " is 5.");
                else if (uocSo4 != 0 && (num % uocSo4) == 0)
                    Console.WriteLine("Smallest factor of " + num + " is " + uocSo4 + ".");
                else
                    Console.WriteLine(num + " is not divisible by 2, 3, 5, or " + uocSo4 + ".");
            }
        }
    }
}
