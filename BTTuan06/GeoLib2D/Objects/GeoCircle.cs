using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 7c, 7d: Đường tròn — kế thừa 3 cấp GeoObject → GeoRegion → GeoCircle.
/// </summary>
public class GeoCircle : GeoRegion
{
    /// <summary>Màu mặc định kiểu GeoGebra cho đường tròn: xám đậm.</summary>
    public static readonly ColorStyle DefaultCircleStyle = new ColorStyle(51, 51, 51);

    // Giới hạn bán kính pixel để GDI+ không tràn số khi phóng to quá lớn
    private const float MaxPixelRadius = 1e6f;

    private double _fixedRadius;

    public GeoPoint Center { get; }

    /// <summary>Điểm nằm trên đường tròn (null nếu dùng bán kính cố định).</summary>
    public GeoPoint PointOnCircle { get; }

    // Câu 7c (1): đường tròn qua tâm và một điểm trên đường tròn
    public GeoCircle(GeoPoint center, GeoPoint pointOnCircle, string label = null)
        : base(label ?? GeoRegistry.NextCircleLabel(), DefaultCircleStyle)
    {
        Center = center ?? throw new ArgumentNullException(nameof(center));
        PointOnCircle = pointOnCircle ?? throw new ArgumentNullException(nameof(pointOnCircle));
    }

    // Câu 7c (2): đường tròn qua tâm với bán kính cố định
    public GeoCircle(GeoPoint center, double fixedRadius, string label = null)
        : base(label ?? GeoRegistry.NextCircleLabel(), DefaultCircleStyle)
    {
        Center = center ?? throw new ArgumentNullException(nameof(center));
        FixedRadius = fixedRadius;
    }

    /// <summary>Bán kính cố định (chỉ có ý nghĩa khi PointOnCircle == null).</summary>
    public double FixedRadius
    {
        get => _fixedRadius;
        set
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Bán kính phải là số thực không âm.");
            _fixedRadius = value;
        }
    }

    public double Radius =>
        PointOnCircle != null ? Center.Position.DistanceTo(PointOnCircle.Position) : _fixedRadius;

    public override string TypeName => "Đường tròn";

    public override IEnumerable<GeoObject> Parents
    {
        get
        {
            yield return Center;
            if (PointOnCircle != null) yield return PointOnCircle;
        }
    }

    // Câu 7d: diện tích, chu vi
    public override double GetArea() => Math.PI * Radius * Radius;
    public override double GetPerimeter() => 2 * Math.PI * Radius;

    public override bool ContainsPoint(Point2D worldPt) =>
        Center.Position.DistanceTo(worldPt) <= Radius;

    /// <summary>Khoảng cách tới đường viền tròn.</summary>
    public override double DistanceToWorldPoint(Point2D pt) =>
        Math.Abs(Center.Position.DistanceTo(pt) - Radius);

    // Câu 7d: phương trình chính tắc "c: (x - x0)² + (y - y0)² = R²"
    public override string GetAlgebraDescription()
    {
        double r = Radius;
        return $"{_label}: {SquareTerm("x", Center.X)} + {SquareTerm("y", Center.Y)} = {GeoMath.Fmt(r * r)}";
    }

    private static string SquareTerm(string variable, double center)
    {
        string magnitude = GeoMath.Fmt(Math.Abs(center));
        if (magnitude == "0.00") return $"{variable}²";
        return $"({variable} {(center < 0 ? "+" : "-")} {magnitude})²";
    }

    // Câu 7d: tô miền trong bằng FillEllipse, vẽ viền bằng DrawEllipse
    public override void Draw(Graphics g, Viewport vp)
    {
        if (!_visible || !TryGetScreenCircle(vp, out PointF c, out float r)) return;
        var rect = new RectangleF(c.X - r, c.Y - r, 2 * r, 2 * r);

        using (var fill = CreateFillBrush())
            g.FillEllipse(fill, rect);
        using (var pen = _style.CreatePen())
            g.DrawEllipse(pen, rect);

        // Nhãn đặt cạnh đường tròn ở góc 45° phía trên bên phải
        DrawLabelAt(g, new PointF(c.X + r * 0.7071f + 4, c.Y - r * 0.7071f - 18));
    }

    public override void DrawHalo(Graphics g, Viewport vp)
    {
        if (!_visible || !TryGetScreenCircle(vp, out PointF c, out float r)) return;
        using var halo = CreateHaloPen();
        g.DrawEllipse(halo, c.X - r, c.Y - r, 2 * r, 2 * r);
    }

    private bool TryGetScreenCircle(Viewport vp, out PointF center, out float radiusPx)
    {
        center = vp.WorldToScreen(Center.Position);
        radiusPx = vp.WorldLengthToPixels(Radius);
        return radiusPx > 0 && radiusPx < MaxPixelRadius;
    }
}
