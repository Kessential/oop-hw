using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 6d: Đoạn thẳng P1P2 — kế thừa GeoLine, chỉ vẽ và bắt dính trong phạm vi t ∈ [0, 1].
/// </summary>
public class GeoSegment : GeoLine
{
    public GeoSegment(GeoPoint p1, GeoPoint p2, string label = null)
        : base(p1, p2, label) { }

    public double GetLength() => P1.Position.DistanceTo(P2.Position);

    public override string TypeName => "Đoạn thẳng";

    public override string GetAlgebraDescription() => $"{_label} = {GeoMath.Fmt(GetLength())}";

    public override bool ContainsParameter(double t) => t >= -GeoMath.EPSILON && t <= 1 + GeoMath.EPSILON;

    // Câu 6d: khoảng cách tới hình chiếu vuông góc có tham số t bị chặn trong [0, 1]
    public override double DistanceToWorldPoint(Point2D pt) => DistanceByProjection(pt, 0, 1);

    // Câu 6d: chỉ vẽ giữa hai đầu mút P1 (t = 0) và P2 (t = 1)
    public override void Draw(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        if (ClipToViewport(vp, 0, 1, out Point2D s, out Point2D e))
            DrawStroke(g, vp, s, e);
    }

    public override void DrawHalo(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        if (ClipToViewport(vp, 0, 1, out Point2D s, out Point2D e))
            DrawHaloStroke(g, vp, s, e);
    }
}
