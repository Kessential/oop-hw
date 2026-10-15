using System.Globalization;

namespace GeoLib2D.Core;

/// <summary>
/// Câu 3a: Lớp tĩnh tiện ích toán học.
/// </summary>
public static class GeoMath
{
    public const double EPSILON = 1e-6;

    public static bool AreEqual(double a, double b) => Math.Abs(a - b) < EPSILON;

    public static bool IsZero(double a) => Math.Abs(a) < EPSILON;

    public static double Clamp(double val, double min, double max) =>
        val < min ? min : (val > max ? max : val);

    /// <summary>
    /// Giải hệ  a1·x + b1·y = c1 ;  a2·x + b2·y = c2  bằng định thức Cramer.
    /// Trả về false nếu hệ vô nghiệm hoặc vô số nghiệm (định thức ≈ 0).
    /// </summary>
    public static bool SolveLinear2x2(double a1, double b1, double c1,
                                      double a2, double b2, double c2,
                                      out double x, out double y)
    {
        double d = a1 * b2 - a2 * b1;
        if (IsZero(d))
        {
            x = y = double.NaN;
            return false;
        }
        x = (c1 * b2 - c2 * b1) / d;
        y = (a1 * c2 - a2 * c1) / d;
        return true;
    }

    public static double DegToRad(double deg) => deg * Math.PI / 180.0;
    public static double RadToDeg(double rad) => rad * 180.0 / Math.PI;

    /// <summary>Định dạng số kiểu GeoGebra: 2 chữ số thập phân, dấu chấm, bỏ "-0.00".</summary>
    public static string Fmt(double v)
    {
        if (Math.Abs(v) < 0.005) v = 0;
        return v.ToString("F2", CultureInfo.InvariantCulture);
    }
}
