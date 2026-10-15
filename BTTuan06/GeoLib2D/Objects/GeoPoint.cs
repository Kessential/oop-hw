using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 5c, 5d, 6c, 9d: Điểm tự do — lớp dẫn xuất trực tiếp của GeoObject.
/// </summary>
public class GeoPoint : GeoObject
{
    public static readonly ColorStyle FreePointStyle = new ColorStyle(21, 101, 192);
    public static readonly ColorStyle DependentPointStyle = new ColorStyle(96, 96, 96);

    protected Point2D _position;

    public int PointRadius { get; set; } = 5;

    public GeoPoint(double x, double y, string label = null)
        : base(label ?? GeoRegistry.NextPointLabel(), FreePointStyle)
    {
        _position = new Point2D(x, y);
    }

    public GeoPoint(Point2D position, string label = null)
        : this(position.X, position.Y, label) { }

    /// <summary>Dành cho lớp con (điểm phụ thuộc) muốn truyền kiểu dáng riêng.</summary>
    protected GeoPoint(double x, double y, string label, ColorStyle style)
        : base(label ?? GeoRegistry.NextPointLabel(), style)
    {
        _position = new Point2D(x, y);
    }

    /// <summary>Vị trí hiện tại (trả về chính đối tượng thành phần để đọc tọa độ).</summary>
    public Point2D Position => _position;

    public double X => _position.X;
    public double Y => _position.Y;

    public override string TypeName => "Điểm";

    public override bool IsDraggable => IsIndependent;

    /// <summary>Đặt lại vị trí (chỉ có tác dụng với điểm tự do).</summary>
    public virtual void MoveTo(double x, double y)
    {
        if (IsIndependent) _position.MoveTo(x, y);
    }

    public override string GetAlgebraDescription() => $"{_label} = {_position}";

    public override double DistanceToWorldPoint(Point2D pt) => _position.DistanceTo(pt);

    public override bool HitTest(PointF mouseScreenPt, Viewport vp, float pixelTolerance = 6f)
    {
        if (!_visible) return false;
        PointF s = vp.WorldToScreen(_position);
        float dx = s.X - mouseScreenPt.X, dy = s.Y - mouseScreenPt.Y;
        float r = PointRadius + pixelTolerance;
        return dx * dx + dy * dy <= r * r;
    }

    // Câu 9d: điểm tự do kéo được
    public override void DragBy(Vector2D delta)
    {
        if (IsIndependent) _position.Translate(delta.Dx, delta.Dy);
    }

    // Câu 5d: vẽ điểm bằng FillEllipse + DrawEllipse + DrawString
    public override void Draw(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        PointF s = vp.WorldToScreen(_position);
        float r = PointRadius;
        var rect = new RectangleF(s.X - r, s.Y - r, 2 * r, 2 * r);

        using (var fill = new SolidBrush(_style.ToDrawingColor()))
            g.FillEllipse(fill, rect);
        using (var border = new Pen(Color.Black, 1f))
            g.DrawEllipse(border, rect);

        if (_labelVisible)
        {
            using var font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            using var brush = new SolidBrush(_style.ToDrawingColor());
            g.DrawString(_label, font, brush, s.X + r + 1, s.Y - r - 16);
        }
    }

    public override void DrawHalo(Graphics g, Viewport vp)
    {
        if (!_visible) return;
        PointF s = vp.WorldToScreen(_position);
        float r = PointRadius + (_isSelected ? 5 : 3);
        using var halo = new SolidBrush(Color.FromArgb(_isSelected ? 90 : 60, _style.ToDrawingColor()));
        g.FillEllipse(halo, s.X - r, s.Y - r, 2 * r, 2 * r);
    }
}
