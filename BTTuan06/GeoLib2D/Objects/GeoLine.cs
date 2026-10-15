using System.Text;
using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 6b, 6c: Đường thẳng đi qua hai điểm P1, P2 (quan hệ Kết tập — không sở hữu hai điểm).
/// Phương trình tổng quát: ax + by + c = 0.
/// </summary>
public class GeoLine : GeoObject
{
    /// <summary>Màu mặc định kiểu GeoGebra cho đường thẳng / tia / đoạn thẳng: xám đậm.</summary>
    public static readonly ColorStyle DefaultLineStyle = new ColorStyle(51, 51, 51);

    // Ngưỡng coi một thành phần vector chỉ phương bằng 0 khi cắt khung nhìn
    private const double ParallelTolerance = 1e-12;

    // Câu 6b: kết tập hai điểm xác định đường thẳng
    public GeoPoint P1 { get; }
    public GeoPoint P2 { get; }

    public GeoLine(GeoPoint p1, GeoPoint p2, string label = null)
        : base(label ?? GeoRegistry.NextLineLabel(), DefaultLineStyle)
    {
        P1 = p1 ?? throw new ArgumentNullException(nameof(p1));
        P2 = p2 ?? throw new ArgumentNullException(nameof(p2));
    }

    // Câu 6b: hệ số phương trình, luôn tính từ vị trí hiện tại của P1, P2
    public double A => P1.Y - P2.Y;
    public double B => P2.X - P1.X;
    public double C => P1.X * P2.Y - P2.X * P1.Y;

    /// <summary>Vector chỉ phương P1 → P2.</summary>
    public Vector2D GetDirection() => new Vector2D(P1.Position, P2.Position);

    /// <summary>false khi P1 trùng P2 (không xác định được đường thẳng).</summary>
    public bool IsDefined => GetDirection().Length() >= GeoMath.EPSILON;

    /// <summary>Tham số t của hình chiếu vuông góc của pt lên đường thẳng: H = P1 + t·(P2 − P1).</summary>
    public double GetParameter(Point2D pt)
    {
        Vector2D d = GetDirection();
        double len2 = d.DotProduct(d);
        return len2 < GeoMath.EPSILON * GeoMath.EPSILON ? 0 : new Vector2D(P1.Position, pt).DotProduct(d) / len2;
    }

    /// <summary>Tham số t có thuộc phần đối tượng hay không (đường thẳng: mọi t).</summary>
    public virtual bool ContainsParameter(double t) => true;

    public override string TypeName => "Đường thẳng";

    // Câu 9a: đường thẳng phụ thuộc vào hai điểm
    public override bool IsIndependent => false;

    public override IEnumerable<GeoObject> Parents
    {
        get
        {
            yield return P1;
            yield return P2;
        }
    }

    // Câu 6c: "f: ax + by = -c" theo kiểu GeoGebra (bỏ hệ số 0, ẩn hệ số 1, gộp dấu)
    public override string GetAlgebraDescription()
    {
        if (!IsDefined) return $"{_label}: không xác định";

        var sb = new StringBuilder();
        AppendTerm(sb, A, "x");
        AppendTerm(sb, B, "y");
        if (sb.Length == 0) sb.Append('0');
        return $"{_label}: {sb} = {GeoMath.Fmt(-C)}";
    }

    private static void AppendTerm(StringBuilder sb, double coef, string variable)
    {
        string magnitude = GeoMath.Fmt(Math.Abs(coef));
        if (magnitude == "0.00") return;
        if (magnitude == "1.00") magnitude = "";

        if (sb.Length == 0)
            sb.Append(coef < 0 ? "-" : "");
        else
            sb.Append(coef < 0 ? " - " : " + ");
        sb.Append(magnitude).Append(variable);
    }

    // Câu 6c: khoảng cách từ điểm tới đường thẳng |ax + by + c| / √(a² + b²)
    public override double DistanceToWorldPoint(Point2D pt)
    {
        double a = A, b = B;
        double norm = Math.Sqrt(a * a + b * b);
        if (norm < GeoMath.EPSILON) return P1.Position.DistanceTo(pt);
        return Math.Abs(a * pt.X + b * pt.Y + C) / norm;
    }

    // Câu 6c: vẽ xuyên suốt màn hình (t ∈ (-∞, +∞) cắt bởi 4 cạnh khung nhìn)
    public override void Draw(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        if (ClipToViewport(vp, double.NegativeInfinity, double.PositiveInfinity, out Point2D s, out Point2D e))
            DrawStroke(g, vp, s, e);
    }

    public override void DrawHalo(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        if (ClipToViewport(vp, double.NegativeInfinity, double.PositiveInfinity, out Point2D s, out Point2D e))
            DrawHaloStroke(g, vp, s, e);
    }

    // ===== Các hàm dùng chung cho GeoLine, GeoSegment, GeoRay =====

    /// <summary>
    /// Câu 6c: Tìm phần nhìn thấy của tập điểm P1 + t·(P2 − P1), t ∈ [tMin, tMax], bằng cách tính
    /// giao điểm với 4 cạnh khung nhìn (trái, phải, dưới, trên) — thuật toán Liang–Barsky.
    /// </summary>
    protected bool ClipToViewport(Viewport vp, double tMin, double tMax, out Point2D start, out Point2D end)
    {
        start = end = null;
        if (!IsDefined) return false;

        Point2D p = P1.Position;
        Vector2D d = GetDirection();
        double margin = vp.PixelsToWorldLength(4);
        double xMin = vp.MinWorldX - margin, xMax = vp.MaxWorldX + margin;
        double yMin = vp.MinWorldY - margin, yMax = vp.MaxWorldY + margin;

        double[] pk = { -d.Dx, d.Dx, -d.Dy, d.Dy };
        double[] qk = { p.X - xMin, xMax - p.X, p.Y - yMin, yMax - p.Y };

        double t0 = tMin, t1 = tMax;
        for (int k = 0; k < 4; k++)
        {
            if (Math.Abs(pk[k]) < ParallelTolerance)
            {
                if (qk[k] < 0) return false; // song song với cạnh và nằm ngoài khung
                continue;
            }
            double r = qk[k] / pk[k];
            if (pk[k] < 0) t0 = Math.Max(t0, r);
            else t1 = Math.Min(t1, r);
        }
        if (t0 > t1) return false;

        start = p + d * t0;
        end = p + d * t1;
        return true;
    }

    /// <summary>Câu 6d: khoảng cách tới hình chiếu vuông góc với tham số t bị chặn trong [tMin, tMax].</summary>
    protected double DistanceByProjection(Point2D pt, double tMin, double tMax)
    {
        if (!IsDefined) return P1.Position.DistanceTo(pt);
        double t = GeoMath.Clamp(GetParameter(pt), tMin, tMax);
        Point2D foot = P1.Position + GetDirection() * t;
        return foot.DistanceTo(pt);
    }

    /// <summary>Vẽ đoạn [start, end] bằng Style.CreatePen() và nhãn ở giữa phần nhìn thấy.</summary>
    protected void DrawStroke(Graphics g, Viewport vp, Point2D start, Point2D end)
    {
        PointF s = vp.WorldToScreen(start), e = vp.WorldToScreen(end);
        using (var pen = _style.CreatePen())
            g.DrawLine(pen, s, e);

        if (_labelVisible)
        {
            var mid = new PointF((s.X + e.X) / 2f, (s.Y + e.Y) / 2f);
            using var font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            using var brush = new SolidBrush(_style.ToDrawingColor());
            g.DrawString(_label, font, brush, mid.X + 4, mid.Y - 20);
        }
    }

    /// <summary>Quầng sáng: nét dày, trong suốt, cùng tông màu với đường.</summary>
    protected void DrawHaloStroke(Graphics g, Viewport vp, Point2D start, Point2D end)
    {
        float width = _style.StrokeWidth + (_isSelected ? 6 : 4);
        using var halo = new Pen(Color.FromArgb(_isSelected ? 90 : 60, _style.ToDrawingColor()), width);
        g.DrawLine(halo, vp.WorldToScreen(start), vp.WorldToScreen(end));
    }
}
