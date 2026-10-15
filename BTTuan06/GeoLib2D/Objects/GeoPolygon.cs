using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 8a, 8b: Đa giác tổng quát (tối thiểu 3 đỉnh) — kế thừa GeoRegion.
/// </summary>
public class GeoPolygon : GeoRegion
{
    /// <summary>Màu nâu mặc định của đa giác trong GeoGebra.</summary>
    public static readonly ColorStyle DefaultPolygonStyle = new ColorStyle(153, 51, 0);

    // Câu 8a: danh sách động các đỉnh
    protected List<GeoPoint> _vertices;

    public GeoPolygon(IEnumerable<GeoPoint> vertices, string label = null)
        : this(ToVertexList(vertices), label, DefaultPolygonStyle) { }

    /// <summary>Dành cho lớp con: danh sách đỉnh phải được kiểm tra trước bằng ToVertexList.</summary>
    protected GeoPolygon(List<GeoPoint> vertices, string label, ColorStyle style)
        : base(label ?? GeoRegistry.NextPolygonLabel(), style)
    {
        _vertices = vertices;
    }

    /// <summary>Sao chép và kiểm tra danh sách đỉnh (khác null, tối thiểu 3 đỉnh).</summary>
    protected static List<GeoPoint> ToVertexList(IEnumerable<GeoPoint> vertices)
    {
        ArgumentNullException.ThrowIfNull(vertices);
        var list = vertices.ToList();
        if (list.Any(v => v == null))
            throw new ArgumentException("Danh sách đỉnh chứa phần tử null.", nameof(vertices));
        if (list.Count < 3)
            throw new ArgumentException("Đa giác cần tối thiểu 3 đỉnh.", nameof(vertices));
        return list;
    }

    public IReadOnlyList<GeoPoint> Vertices => _vertices.AsReadOnly();

    public override string TypeName => "Đa giác";

    public override IEnumerable<GeoObject> Parents => _vertices;

    // Mô tả kiểu GeoGebra: "poly1 = 6.00" (diện tích)
    public override string GetAlgebraDescription() => $"{_label} = {GeoMath.Fmt(GetArea())}";

    // Câu 8b: chu vi = tổng độ dài các cạnh nối tiếp
    public override double GetPerimeter()
    {
        double sum = 0;
        for (int i = 0; i < _vertices.Count; i++)
            sum += _vertices[i].Position.DistanceTo(_vertices[(i + 1) % _vertices.Count].Position);
        return sum;
    }

    // Câu 8b: diện tích theo công thức Shoelace (dây giày)
    public override double GetArea()
    {
        double sum = 0;
        for (int i = 0; i < _vertices.Count; i++)
        {
            Point2D p1 = _vertices[i].Position;
            Point2D p2 = _vertices[(i + 1) % _vertices.Count].Position;
            sum += p1.X * p2.Y - p2.X * p1.Y;
        }
        return Math.Abs(sum) * 0.5;
    }

    // Câu 8b: kiểm tra điểm trong đa giác bằng Ray-Casting (tia nằm ngang hướng sang phải)
    public override bool ContainsPoint(Point2D worldPt)
    {
        bool inside = false;
        for (int i = 0, j = _vertices.Count - 1; i < _vertices.Count; j = i++)
        {
            Point2D a = _vertices[i].Position, b = _vertices[j].Position;
            if ((a.Y > worldPt.Y) != (b.Y > worldPt.Y))
            {
                double xCross = a.X + (worldPt.Y - a.Y) * (b.X - a.X) / (b.Y - a.Y);
                if (worldPt.X < xCross) inside = !inside;
            }
        }
        return inside;
    }

    /// <summary>Khoảng cách tới cạnh gần nhất (dùng hình chiếu vuông góc t ∈ [0, 1]).</summary>
    public override double DistanceToWorldPoint(Point2D pt)
    {
        double min = double.PositiveInfinity;
        for (int i = 0; i < _vertices.Count; i++)
        {
            Point2D a = _vertices[i].Position, b = _vertices[(i + 1) % _vertices.Count].Position;
            min = Math.Min(min, DistanceToEdge(pt, a, b));
        }
        return min;
    }

    private static double DistanceToEdge(Point2D p, Point2D a, Point2D b)
    {
        Vector2D ab = b - a;
        double len2 = ab.DotProduct(ab);
        if (len2 < GeoMath.EPSILON * GeoMath.EPSILON) return p.DistanceTo(a);
        double t = GeoMath.Clamp((p - a).DotProduct(ab) / len2, 0, 1);
        return (a + ab * t).DistanceTo(p);
    }

    // Câu 8a: chuyển _vertices sang PointF[] qua Viewport.WorldToScreen
    protected PointF[] ToScreenPoints(Viewport vp) =>
        _vertices.Select(v => vp.WorldToScreen(v.Position)).ToArray();

    // Câu 8a: vẽ bằng FillPolygon + DrawPolygon, nhãn đặt ở trọng tâm các đỉnh
    public override void Draw(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        PointF[] pts = ToScreenPoints(vp);

        using (var fill = CreateFillBrush())
            g.FillPolygon(fill, pts);
        using (var pen = _style.CreatePen())
            g.DrawPolygon(pen, pts);

        DrawLabelAt(g, new PointF(pts.Average(p => p.X) - 10, pts.Average(p => p.Y) - 8));
    }

    public override void DrawHalo(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        using var halo = CreateHaloPen();
        g.DrawPolygon(halo, ToScreenPoints(vp));
    }
}
