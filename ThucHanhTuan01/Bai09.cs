using System;
using System.IO;

namespace ThucHanhTuan01
{
    // Bai 09: File I/O & Bai toan doc ma tran vuong, ghi ma tran chuyen vi (Bien the K = 0)
    // De bai: Doc ma tran vuong tu input.txt, kiem tra hop le, ghi ma tran chuyen vi ra output.txt.
    public static class Bai09
    {
        public static void Chay()
        {
            Console.WriteLine("=== BAI 09: FILE I/O - DOC MA TRAN, GHI MA TRAN CHUYEN VI (K = 0) ===");

            string duongDanVao = "input.txt";
            string duongDanRa = "output.txt";

            if (!File.Exists(duongDanVao))
            {
                Console.WriteLine("Khong tim thay input.txt, tu dong tao file mau (ma tran 3x3)...");
                using (StreamWriter sw = new StreamWriter(duongDanVao))
                {
                    sw.WriteLine("3");
                    sw.WriteLine("1 2 3");
                    sw.WriteLine("4 5 6");
                    sw.WriteLine("7 8 9");
                }
            }

            try
            {
                int n;
                int[,] mat;

                using (StreamReader sr = new StreamReader(duongDanVao))
                {
                    string? dongDau = sr.ReadLine();
                    if (!int.TryParse(dongDau, out n) || n <= 0)
                    {
                        throw new FormatException("Kich thuoc ma tran khong hop le.");
                    }

                    mat = new int[n, n];
                    for (int i = 0; i < n; i++)
                    {
                        string? dong = sr.ReadLine();
                        if (dong == null)
                        {
                            throw new FormatException($"Thieu du lieu dong {i + 1} trong input.txt.");
                        }

                        string[] phanTu = dong.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
                        if (phanTu.Length != n)
                        {
                            throw new FormatException($"Dong {i + 1} khong du {n} phan tu (ma tran phai vuong).");
                        }

                        for (int j = 0; j < n; j++)
                        {
                            mat[i, j] = int.Parse(phanTu[j]);
                        }
                    }
                }

                Console.WriteLine($"Da doc thanh cong ma tran vuong {n}x{n} tu input.txt.");

                int[,] chuyenVi = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        chuyenVi[j, i] = mat[i, j];
                    }
                }

                using (StreamWriter sw = new StreamWriter(duongDanRa))
                {
                    sw.WriteLine($"Ma tran chuyen vi {n}x{n}:");
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            sw.Write($"{chuyenVi[i, j],4}");
                        }
                        sw.WriteLine();
                    }
                    sw.WriteLine($"Thoi gian thuc thi: {DateTime.Now}");
                }

                Console.WriteLine("Da ghi ma tran chuyen vi vao output.txt.");
                Console.WriteLine(File.ReadAllText(duongDanRa));
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Loi: Khong tim thay file - {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Loi: Du lieu trong file khong hop le - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi khong xac dinh: {ex.Message}");
            }
        }
    }
}
