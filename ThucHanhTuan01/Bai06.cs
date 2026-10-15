using System;

namespace ThucHanhTuan01
{
    public struct PhanSo
    {
        public long TuSo;
        public long MauSo;
    }

    // Bai 06: Struct Phan so & Dai so co ban (Bien the K = 0)
    // De bai: Xay dung cac phep toan cong, tru, nhan, chia va so sanh tren phan so,
    // luon rut gon ve dang toi gian sau moi phep tinh.
    public static class Bai06
    {
        public static long UCLN(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b != 0)
            {
                long tmp = b;
                b = a % b;
                a = tmp;
            }
            return a;
        }

        public static void RutGon(ref PhanSo ps)
        {
            if (ps.MauSo == 0)
                throw new DivideByZeroException("Mau so khong duoc bang 0");

            if (ps.MauSo < 0)
            {
                ps.TuSo = -ps.TuSo;
                ps.MauSo = -ps.MauSo;
            }

            long g = UCLN(ps.TuSo, ps.MauSo);
            if (g != 0)
            {
                ps.TuSo /= g;
                ps.MauSo /= g;
            }
        }

        public static string XuatPhanSo(PhanSo ps)
        {
            if (ps.MauSo == 1)
                return $"{ps.TuSo}";
            return $"{ps.TuSo}/{ps.MauSo}";
        }

        public static PhanSo Cong(PhanSo a, PhanSo b)
        {
            PhanSo ketQua = new PhanSo
            {
                TuSo = a.TuSo * b.MauSo + b.TuSo * a.MauSo,
                MauSo = a.MauSo * b.MauSo
            };
            RutGon(ref ketQua);
            return ketQua;
        }

        public static PhanSo Tru(PhanSo a, PhanSo b)
        {
            PhanSo ketQua = new PhanSo
            {
                TuSo = a.TuSo * b.MauSo - b.TuSo * a.MauSo,
                MauSo = a.MauSo * b.MauSo
            };
            RutGon(ref ketQua);
            return ketQua;
        }

        public static PhanSo Nhan(PhanSo a, PhanSo b)
        {
            PhanSo ketQua = new PhanSo
            {
                TuSo = a.TuSo * b.TuSo,
                MauSo = a.MauSo * b.MauSo
            };
            RutGon(ref ketQua);
            return ketQua;
        }

        public static PhanSo Chia(PhanSo a, PhanSo b)
        {
            if (b.TuSo == 0)
                throw new DivideByZeroException("Khong the chia cho phan so 0");

            PhanSo ketQua = new PhanSo
            {
                TuSo = a.TuSo * b.MauSo,
                MauSo = a.MauSo * b.TuSo
            };
            RutGon(ref ketQua);
            return ketQua;
        }

        public static int SoSanh(PhanSo a, PhanSo b)
        {
            long ve_trai = a.TuSo * b.MauSo;
            long ve_phai = b.TuSo * a.MauSo;
            if (ve_trai > ve_phai) return 1;
            if (ve_trai < ve_phai) return -1;
            return 0;
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 06: STRUCT PHAN SO - DAI SO CO BAN (K = 0) ===");

            var a = new PhanSo { TuSo = 2, MauSo = 4 };
            var b = new PhanSo { TuSo = -3, MauSo = 9 };
            RutGon(ref a);
            RutGon(ref b);

            Console.WriteLine($"Phan so a (da rut gon) = {XuatPhanSo(a)}");
            Console.WriteLine($"Phan so b (da rut gon) = {XuatPhanSo(b)}");

            Console.WriteLine($"a + b = {XuatPhanSo(Cong(a, b))}");
            Console.WriteLine($"a - b = {XuatPhanSo(Tru(a, b))}");
            Console.WriteLine($"a * b = {XuatPhanSo(Nhan(a, b))}");
            Console.WriteLine($"a / b = {XuatPhanSo(Chia(a, b))}");

            int soSanh = SoSanh(a, b);
            string quanHe = soSanh > 0 ? ">" : (soSanh < 0 ? "<" : "=");
            Console.WriteLine($"So sanh a va b: a {quanHe} b (ket qua = {soSanh})");

            try
            {
                var zero = new PhanSo { TuSo = 1, MauSo = 0 };
                RutGon(ref zero);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Da bat loi: " + ex.Message);
            }
        }
    }
}
