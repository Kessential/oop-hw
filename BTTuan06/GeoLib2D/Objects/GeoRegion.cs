using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 7a, 7b: Lớp trung gian cho các hình khép kín có miền tô màu trong suốt
/// (chuỗi kế thừa nhiều cấp GeoObject → GeoRegion → GeoCircle / GeoPolygon).
/// </summary>
public abstract class GeoRegion : GeoObject
{
    // Câu 7a: độ đậm màu tô miền trong (40 ≈ 15% opacity của GeoGebra)
    protected int _fillAlpha = 40;

    protected GeoRegion(string label, ColorStyle style) : base(label, style) { }

    public int FillAlpha
    {
        get => _fillAlpha;
        set => _fillAlpha = Math.Clamp(value, 0, 255);
    }

    /// <summary>Câu 7a: cọ tô cùng tông màu với viền nhưng trong suốt theo FillAlpha.</summary>
    public SolidBrush CreateFillBrush() =>
        new SolidBrush(Color.FromArgb(_fillAlpha, _style.Red, _style.Green, _style.Blue));

    // Câu 7b: các phương thức đặc trưng của miền phẳng
    public virtual double GetArea() => 0.0;
    public virtual double GetPerimeter() => 0.0;
    public virtual bool ContainsPoint(Point2D worldPt) => false;

    // Câu 9a: miền hình phụ thuộc vào các điểm xác định nó
    public override bool IsIndependent => false;

    /// <summary>Trúng khi con trỏ gần biên hoặc nằm bên trong miền.</summary>
    public override bool HitTest(PointF mouseScreenPt, Viewport vp, float pixelTolerance = 6f)
    {
        if (!_visible) return false;
        Point2D w = vp.ScreenToWorld(mouseScreenPt);
        return DistanceToWorldPoint(w) * vp.Scale <= pixelTolerance || ContainsPoint(w);
    }

    /// <summary>Vẽ nhãn tại vị trí pixel cho trước, cùng màu với viền.</summary>
    protected void DrawLabelAt(Graphics g, PointF screenPt)
    {
        if (!_labelVisible) return;
        using var font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        using var brush = new SolidBrush(_style.ToDrawingColor());
        g.DrawString(_label, font, brush, screenPt);
    }

    /// <summary>Bút vẽ quầng sáng khi được chọn / di chuột qua.</summary>
    protected Pen CreateHaloPen() =>
        new Pen(Color.FromArgb(_isSelected ? 90 : 60, _style.ToDrawingColor()),
                _style.StrokeWidth + (_isSelected ? 6 : 4));
}
