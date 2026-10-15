using System;
using System.Text;

namespace ThucHanhTuan01
{
    class Program
    {
        static void Main(string[] args)
        {
            // Thiet lap bang ma UTF-8 de in tieng Viet co dau khong loi font tren Console Windows
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            int chon = -1;
            do
            {
                Console.WriteLine();
                Console.WriteLine("=========================================================");
                Console.WriteLine("    BAI THUC HANH TUAN 01: LAP TRINH CAU TRUC VOI C#    ");
                Console.WriteLine("    Tham so bien the K = 0");
                Console.WriteLine("=========================================================");
                Console.WriteLine("  1. Bai 01: Tinh cuoc dien sinh hoat luy tien 5 bac (if-else)");
                Console.WriteLine("  2. Bai 02: Phan tich thua so nguyen to (dang luy thua)");
                Console.WriteLine("  3. Bai 03: Modul so nguyen to & Gia thuyet Goldbach");
                Console.WriteLine("  4. Bai 04: Mang 1 chieu - Loc va xoa so am tan cung bang 6");
                Console.WriteLine("  5. Bai 05: Ma tran 2 chieu - Sap xep Zigzag");
                Console.WriteLine("  6. Bai 06: Struct Phan so - Dai so co ban");
                Console.WriteLine("  7. Bai 07: Danh sach lien ket don - CRUD & Tim kiem");
                Console.WriteLine("  8. Bai 08: De quy - Thap Ha Noi co dien");
                Console.WriteLine("  9. Bai 09: File I/O - Doc ma tran, ghi ma tran chuyen vi");
                Console.WriteLine(" 10. Bai 10: Xu ly chuoi - Loc trung & sap xep dong van ban");
                Console.WriteLine("  0. Thoat chuong trinh");
                Console.WriteLine("=========================================================");
                Console.Write("Vui long chon bai can chay (0 - 10): ");

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
                    case 0: Console.WriteLine("Tam biet!"); break;
                    default: Console.WriteLine("Bai tap khong ton tai!"); break;
                }
            } while (chon != 0);
        }
    }
}
