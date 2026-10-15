using System;
using System.Text;

namespace BTTuan01
{
    class Program
    {
        private static readonly Action[] DanhSachBai =
        {
            Bai01.Chay, Bai02.Chay, Bai03.Chay, Bai04.Chay, Bai05.Chay,
            Bai06.Chay, Bai07.Chay, Bai08.Chay, Bai09.Chay, Bai10.Chay
        };

        private static void ChayTatCa()
        {
            for (int i = 0; i < DanhSachBai.Length; i++)
            {
                Console.WriteLine($"--------------------- ({i + 1}/{DanhSachBai.Length}) ---------------------");
                DanhSachBai[i]();
                Console.WriteLine();
            }
            Console.WriteLine("Da chay xong tat ca 10 bai!");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            int chon = -1;
            do
            {
                Console.WriteLine();
                Console.WriteLine("=========================================================");
                Console.WriteLine("   BAI TAP TUAN 01 - LAB 02: STATEMENT, OPERATOR, ARRAY   ");
                Console.WriteLine("=========================================================");
                Console.WriteLine("  1. Bai 01: If statement - Giai phuong trinh bac 2");
                Console.WriteLine("  2. Bai 02: Switch statement - Kiem tra nguyen am/phu am");
                Console.WriteLine("  3. Bai 03: For statement - Uoc so nho nhat");
                Console.WriteLine("  4. Bai 04: Foreach statement - Mang so ngau nhien & trung binh");
                Console.WriteLine("  5. Bai 05: Array - Tham chieu mang (reference type)");
                Console.WriteLine("  6. Bai 06: Reverse Array - Dao nguoc mang");
                Console.WriteLine("  7. Bai 07: Multi-dimensional Array - Mang 2 chieu");
                Console.WriteLine("  8. Bai 08: Array Class - IndexOf, Sort, Reverse");
                Console.WriteLine("  9. Bai 09: Array of Object - Mang doi tuong Employee");
                Console.WriteLine(" 10. Bai 10: SIN Validator (Do it yourself)");
                Console.WriteLine(" 11. Chay tat ca cac bai (1 -> 10)");
                Console.WriteLine("  0. Thoat chuong trinh");
                Console.WriteLine("=========================================================");
                Console.Write("Vui long chon bai can chay (0 - 11): ");

                string? input = Console.ReadLine();
                if (!int.TryParse(input, out chon))
                {
                    Console.WriteLine("Loi: Lua chon khong hop le!");
                    continue;
                }

                Console.WriteLine();
                switch (chon)
                {
                    case 1: Bai01.Chay(); break;
                    case 2: Bai02.Chay(); break;
                    case 3: Bai03.Chay(); break;
                    case 4: Bai04.Chay(); break;
                    case 5: Bai05.Chay(); break;
                    case 6: Bai06.Chay(); break;
                    case 7: Bai07.Chay(); break;
                    case 8: Bai08.Chay(); break;
                    case 9: Bai09.Chay(); break;
                    case 10: Bai10.Chay(); break;
                    case 11: ChayTatCa(); break;
                    case 0: Console.WriteLine("Tam biet!"); break;
                    default: Console.WriteLine("Bai tap khong ton tai!"); break;
                }
            } while (chon != 0);
        }
    }
}
