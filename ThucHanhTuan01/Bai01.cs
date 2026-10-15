using System;

namespace ThucHanhTuan01
{
    // Bai 01: Cau truc re nhanh & Bai toan tinh cuoc dien luy tien (Bien the K = 0)
    // De bai: Tinh tien dien sinh hoat theo 5 bac luy tien, kem VAT 10%.
    public static class Bai01
    {
        private static readonly double[] MucBac = { 100, 50, 50, 100, double.PositiveInfinity };
        private static readonly double[] DonGia = { 550, 900, 1250, 1450, 1700 };
        private static readonly string[] TenBac =
        {
            "Bac 1 (0-100 kWh)", "Bac 2 (101-150 kWh)", "Bac 3 (151-200 kWh)",
            "Bac 4 (201-300 kWh)", "Bac 5 (>300 kWh)"
        };

        public static double TinhTienDien(double kwh, out double thueVAT)
        {
            if (kwh <= 0)
            {
                thueVAT = 0;
                return 0;
            }

            double conLai = kwh, tienTruocThue = 0;
            for (int i = 0; i < MucBac.Length && conLai > 0; i++)
            {
                double dung = Math.Min(conLai, MucBac[i]);
                tienTruocThue += dung * DonGia[i];
                conLai -= dung;
            }

            thueVAT = tienTruocThue * 0.10;
            return tienTruocThue + thueVAT;
        }

        public static void InHoaDon(double kwh)
        {
            Console.WriteLine($"--- Hoa don tien dien: {kwh:N1} kWh ---");
            if (kwh <= 0)
            {
                Console.WriteLine("  Du lieu khong hop le: luong dien tieu thu phai la so duong!");
                return;
            }

            double conLai = kwh, tienTruocThue = 0;
            for (int i = 0; i < MucBac.Length && conLai > 0; i++)
            {
                double dung = Math.Min(conLai, MucBac[i]);
                double thanhTien = dung * DonGia[i];
                tienTruocThue += thanhTien;
                Console.WriteLine($"  {TenBac[i],-20}: {dung,6:N1} kWh x {DonGia[i],5:N0} d = {thanhTien,12:N0} d");
                conLai -= dung;
            }

            double thueVAT = tienTruocThue * 0.10;
            Console.WriteLine($"  {"Tien truoc thue",-20}: {"",6} {"",5}   {tienTruocThue,12:N0} d");
            Console.WriteLine($"  {"Thue VAT (10%)",-20}: {"",6} {"",5}   {thueVAT,12:N0} d");
            Console.WriteLine($"  {"TONG TIEN PHAI TRA",-20}: {"",6} {"",5}   {tienTruocThue + thueVAT,12:N0} d");
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 01: TINH CUOC DIEN SINH HOAT LUY TIEN (K = 0) ===");
            Console.WriteLine("Ket qua mong doi: 175 kWh -> Tien bac = 131.250d | VAT = 13.125d | Tong = 144.375d\n");

            double[] testCases = { 75, 175, 350 };
            foreach (double kwh in testCases)
            {
                InHoaDon(kwh);
                Console.WriteLine();
            }
        }
    }
}
