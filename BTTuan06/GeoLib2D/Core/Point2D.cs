using System.Globalization;

namespace GeoLib2D.Core;

/// <summary>
/// Câu 1c, 1d, 2b, 2d: Điểm trong hệ tọa độ thực Descartes.
/// </summary>
public class Point2D
{
    private double _x, _y;

    // Câu 2: các hàm tạo
    public Point2D() : this(0, 0) { }

    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    // Câu 2b: Copy Constructor
    public Point2D(Point2D other)
    {
        ArgumentNullException.ThrowIfNull(other);
        _x = other._x;
        _y = other._y;
    }

    // Câu 1c: không cho phép NaN / Infinity
    public double X
    {
        get => _x;
        set => _x = Validate(value, nameof(X));
    }

    public double Y
    {
        get => _y;
        set => _y = Validate(value, nameof(Y));
    }

    private static double Validate(double value, string name)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException($"Tọa độ {name} không hợp lệ (NaN hoặc Infinity).", name);
        return value;
    }

    public Point2D Clone() => new Point2D(this);

    // Câu 1d: nghiệp vụ toán học
    public double DistanceTo(Point2D other)
    {
        ArgumentNullException.ThrowIfNull(other);
        double dx = other._x - _x, dy = other._y - _y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public void Translate(double dx, double dy)
    {
        X = _x + dx;
        Y = _y + dy;
    }

    public PointF ToPointF() => new PointF((float)_x, (float)_y);

    // Câu 2d: quá tải phương thức dịch chuyển
    public void MoveTo(double x, double y)
    {
        X = x;
        Y = y;
    }

    public void MoveTo(Point2D target)
    {
        ArgumentNullException.ThrowIfNull(target);
        MoveTo(target._x, target._y);
    }

    /// <summary>Trả về điểm mới sau phép tịnh tiến theo vector v.</summary>
    public Point2D AddVector(Vector2D v) => new Point2D(_x + v.Dx, _y + v.Dy);

    /// <summary>Trả về điểm mới sau phép quay quanh center một góc angleRad (radian, ngược chiều kim đồng hồ).</summary>
    public Point2D RotateAround(Point2D center, double angleRad)
    {
        double cos = Math.Cos(angleRad), sin = Math.Sin(angleRad);
        double dx = _x - center._x, dy = _y - center._y;
        return new Point2D(center._x + dx * cos - dy * sin, center._y + dx * sin + dy * cos);
    }

    public static Point2D operator +(Point2D p, Vector2D v) => new Point2D(p.X + v.Dx, p.Y + v.Dy);
    public static Point2D operator -(Point2D p, Vector2D v) => new Point2D(p.X - v.Dx, p.Y - v.Dy);
    public static Vector2D operator -(Point2D end, Point2D start) => new Vector2D(start, end);

    // Câu 1d: hiển thị kiểu cột Algebra của GeoGebra, ví dụ "(2.50, -1.25)"
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "({0:F2}, {1:F2})", _x, _y);
}
