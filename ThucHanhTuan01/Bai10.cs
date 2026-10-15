using System;
using System.Collections.Generic;
using System.Text;

namespace ThucHanhTuan01
{
    // Bai 10: Xu ly chuoi & Bai toan loc trung, sap xep dong van ban (Bien the K = 0)
    // De bai: Chuan hoa khoang trang, loc bo dong trung lap, sap xep cac dong con lai.
    public static class Bai10
    {
        public static string ChuanHoaKhoangTrang(string s)
        {
            string[] parts = s.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", parts);
        }

        public static List<string> LocTrungVaSapXep(IEnumerable<string> dong)
        {
            HashSet<string> daThay = new HashSet<string>();
            List<string> ketQua = new List<string>();

            foreach (string d in dong)
            {
                string chuan = ChuanHoaKhoangTrang(d);
                if (chuan.Length == 0)
                {
                    continue;
                }

                if (daThay.Add(chuan))
                {
                    ketQua.Add(chuan);
                }
            }

            ketQua.Sort(StringComparer.Ordinal);
            return ketQua;
        }

        public static void InDanhSachDanhSo(List<string> ds)
        {
            int width = ds.Count.ToString().Length;
            for (int i = 0; i < ds.Count; i++)
            {
                Console.WriteLine($"{(i + 1).ToString().PadLeft(width)}. {ds[i]}");
            }
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 10: XU LY CHUOI - LOC TRUNG & SAP XEP DONG VAN BAN (K = 0) ===");

            var lines = new List<string> { "  banana  ", "apple", "banana", " orange" };

            Console.WriteLine("Danh sach dong van ban dau vao:");
            foreach (string l in lines)
            {
                Console.WriteLine($"  \"{l}\"");
            }

            List<string> ketQua = LocTrungVaSapXep(lines);

            Console.WriteLine("Ket qua sau khi chuan hoa, loc trung va sap xep:");
            InDanhSachDanhSo(ketQua);
        }
    }
}
