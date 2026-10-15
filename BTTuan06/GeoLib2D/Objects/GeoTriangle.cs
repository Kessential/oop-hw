using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 8c: Tam giác ABC — kế thừa GeoPolygon, bổ sung các tâm đặc biệt.
/// Các tâm trả về null khi tam giác suy biến (3 đỉnh thẳng hàng); riêng trọng tâm luôn xác định.
/// </summary>
public class GeoTriangle : GeoPolygon
{
    public GeoTriangle(GeoPoint a, GeoPoint b, GeoPoint c, string label = null)
        : base(ToVertexList(new[] { a, b, c }), label, DefaultPolygonStyle) { }

    public GeoPoint A => _vertices[0];
    public GeoPoint B => _vertices[1];
    public GeoPoint C => _vertices[2];

    public override string TypeName => "Tam giác";

    /// <summary>3 đỉnh thẳng hàng (diện tích ≈ 0).</summary>
    public bool IsDegenerate => GeoMath.IsZero(GetArea());

    // Trọng tâm G = (A + B + C) / 3
    public Point2D GetCentroid() => new Point2D(
        (A.X + B.X + C.X) / 3.0,
        (A.Y + B.Y + C.Y) / 3.0);

    // Tâm đường tròn ngoại tiếp O: |OA| = |OB| = |OC|
    //   2(xB - xA)x + 2(yB - yA)y = xB² + yB² - xA² - yA²
    //   2(xC - xA)x + 2(yC - yA)y = xC² + yC² - xA² - yA²
    public Point2D GetCircumcenter()
    {
        if (IsDegenerate) return null;
        double sqA = A.X * A.X + A.Y * A.Y;
        return GeoMath.SolveLinear2x2(
                2 * (B.X - A.X), 2 * (B.Y - A.Y), B.X * B.X + B.Y * B.Y - sqA,
                2 * (C.X - A.X), 2 * (C.Y - A.Y), C.X * C.X + C.Y * C.Y - sqA,
                out double x, out double y)
            ? new Point2D(x, y) : null;
    }

    // Tâm đường tròn nội tiếp I = (a·A + b·B + c·C) / (a + b + c), a = |BC|, b = |CA|, c = |AB|
    public Point2D GetIncenter()
    {
        if (IsDegenerate) return null;
        double a = B.Position.DistanceTo(C.Position);
        double b = C.Position.DistanceTo(A.Position);
        double c = A.Position.DistanceTo(B.Position);
        double p = a + b + c;
        return new Point2D((a * A.X + b * B.X + c * C.X) / p, (a * A.Y + b * B.Y + c * C.Y) / p);
    }

    // Trực tâm H: AH ⊥ BC và BH ⊥ CA
    //   (xC - xB)x + (yC - yB)y = (xC - xB)xA + (yC - yB)yA
    //   (xA - xC)x + (yA - yC)y = (xA - xC)xB + (yA - yC)yB
    public Point2D GetOrthocenter()
    {
        if (IsDegenerate) return null;
        double a1 = C.X - B.X, b1 = C.Y - B.Y;
        double a2 = A.X - C.X, b2 = A.Y - C.Y;
        return GeoMath.SolveLinear2x2(
                a1, b1, a1 * A.X + b1 * A.Y,
                a2, b2, a2 * B.X + b2 * B.Y,
                out double x, out double y)
            ? new Point2D(x, y) : null;
    }
}
