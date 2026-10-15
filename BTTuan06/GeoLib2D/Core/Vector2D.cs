using System.Globalization;

namespace GeoLib2D.Core;

/// <summary>
/// Câu 2a, 2b, 2c: Vector 2D với constructor overloading, copy constructor và operator overloading.
/// </summary>
public class Vector2D
{
    public double Dx { get; set; }
    public double Dy { get; set; }

    // (1) Vector không
    public Vector2D() : this(0, 0) { }

    // (2) Từ hai thành phần
    public Vector2D(double dx, double dy)
    {
        Dx = dx;
        Dy = dy;
    }

    // (3) Từ điểm start đến điểm end
    public Vector2D(Point2D start, Point2D end)
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(end);
        Dx = end.X - start.X;
        Dy = end.Y - start.Y;
    }

    // Copy Constructor
    public Vector2D(Vector2D other)
    {
        ArgumentNullException.ThrowIfNull(other);
        Dx = other.Dx;
        Dy = other.Dy;
    }

    public Vector2D Clone() => new Vector2D(this);

    // Câu 2c: quá tải toán tử
    public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.Dx + b.Dx, a.Dy + b.Dy);
    public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.Dx - b.Dx, a.Dy - b.Dy);
    public static Vector2D operator -(Vector2D v) => new Vector2D(-v.Dx, -v.Dy);
    public static Vector2D operator *(Vector2D v, double k) => new Vector2D(v.Dx * k, v.Dy * k);
    public static Vector2D operator *(double k, Vector2D v) => v * k;

    public double DotProduct(Vector2D v) => Dx * v.Dx + Dy * v.Dy;

    /// <summary>Tích có hướng (thành phần z) — dùng để xét chiều quay, song song.</summary>
    public double CrossProduct(Vector2D v) => Dx * v.Dy - Dy * v.Dx;

    public double Length() => Math.Sqrt(Dx * Dx + Dy * Dy);

    /// <summary>Trả về vector đơn vị cùng hướng (vector không thì trả về vector không).</summary>
    public Vector2D Normalize()
    {
        double len = Length();
        return len < GeoMath.EPSILON ? new Vector2D() : new Vector2D(Dx / len, Dy / len);
    }

    /// <summary>Vector pháp tuyến (quay 90° ngược chiều kim đồng hồ).</summary>
    public Vector2D Perpendicular() => new Vector2D(-Dy, Dx);

    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "({0:F2}, {1:F2})", Dx, Dy);
}
