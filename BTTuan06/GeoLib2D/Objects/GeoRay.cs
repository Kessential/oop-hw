using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 6d: Tia gốc Start (P1) đi qua Through (P2) — kế thừa GeoLine, chỉ vẽ và bắt dính với t ≥ 0.
/// </summary>
public class GeoRay : GeoLine
{
    public GeoRay(GeoPoint start, GeoPoint through, string label = null)
        : base(start, through, label) { }

    public override string TypeName => "Tia";

    public override bool ContainsParameter(double t) => t >= -GeoMath.EPSILON;

    // Câu 6d: hình chiếu vuông góc có tham số t ≥ 0
    public override double DistanceToWorldPoint(Point2D pt) =>
        DistanceByProjection(pt, 0, double.PositiveInfinity);

    // Câu 6d: vẽ từ gốc P1 tới cạnh khung nhìn theo hướng P1 → P2
    public override void Draw(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        if (ClipToViewport(vp, 0, double.PositiveInfinity, out Point2D s, out Point2D e))
            DrawStroke(g, vp, s, e);
    }

    public override void DrawHalo(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        if (ClipToViewport(vp, 0, double.PositiveInfinity, out Point2D s, out Point2D e))
            DrawHaloStroke(g, vp, s, e);
    }
}
