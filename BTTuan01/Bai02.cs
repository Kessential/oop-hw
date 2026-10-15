using System;

namespace BTTuan01
{
    // Exercise 2: Switch statement - kiem tra nguyen am / phu am
    // Da chuyen tu bo nguyen am tieng Anh (a,e,i,o,u) sang bo nguyen am tieng Viet:
    // a, a(breve), a(circumflex), e, e(circumflex), i, o, o(circumflex), o(horn), u, u(horn), y
    public static class Bai02
    {
        public static void Chay()
        {
            Console.WriteLine("=== BAI 02: SWITCH STATEMENT - NGUYEN AM / PHU AM TIENG VIET ===");

            Console.WriteLine("Nhap mot ky tu:");
            string? dong = Console.ReadLine();
            char ch = string.IsNullOrEmpty(dong) ? ' ' : dong[0];
            char chThuong = char.ToLower(ch);

            switch (chThuong)
            {
                case 'a':
                case 'ă':
                case 'â':
                case 'e':
                case 'ê':
                case 'i':
                case 'o':
                case 'ô':
                case 'ơ':
                case 'u':
                case 'ư':
                case 'y':
                    Console.WriteLine("{0} la nguyen am.", ch);
                    break;
                default:
                    Console.WriteLine("{0} la phu am.", ch);
                    break;
            }
        }
    }
}
