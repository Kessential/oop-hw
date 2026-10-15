using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 5a, 5b, 6a, 9a, 9d: Lớp cơ sở của mọi đối tượng hình học.
/// </summary>
public abstract class GeoObject
{
    // Câu 5a: các thành viên protected
    protected readonly int _id;
    protected string _label;
    protected bool _visible = true;
    protected bool _labelVisible = true;
    protected bool _isSelected = false;
    protected bool _isHovered = false;
    protected ColorStyle _style;

    // Câu 5b: hàm tạo tự cấp phát ID
    protected GeoObject(string label, ColorStyle style)
    {
        _id = GeoRegistry.NextId();
        _label = label ?? $"obj{_id}";
        _style = style != null ? new ColorStyle(style) : new ColorStyle(21, 101, 192);
    }

    public int Id => _id;

    public string Label
    {
        get => _label;
        set
        {
            if (!string.IsNullOrWhiteSpace(value)) _label = value.Trim();
        }
    }

    public bool Visible
    {
        get => _visible;
        set => _visible = value;
    }

    public bool LabelVisible
    {
        get => _labelVisible;
        set => _labelVisible = value;
    }

    public bool IsSelected => _isSelected;

    public bool IsHovered
    {
        get => _isHovered;
        set => _isHovered = value;
    }

    public ColorStyle Style
    {
        get => _style;
        set => _style = value != null ? new ColorStyle(value) : _style;
    }

    /// <summary>Tên loại đối tượng tiếng Việt dùng cho tiêu đề Algebra ("Điểm", "Đường thẳng"...).</summary>
    public virtual string TypeName => "Đối tượng";

    public void ToggleVisibility() => _visible = !_visible;
    public void Select() => _isSelected = true;
    public void Deselect() => _isSelected = false;

    /// <summary>Ví dụ: "Điểm A", "Đường thẳng f".</summary>
    public string GetAlgebraHeader() => $"{TypeName} {_label}";

    // ===== Câu 6a: các phương thức ảo =====

    public virtual string GetAlgebraDescription() => _label;

    /// <summary>Khoảng cách (đơn vị thực) từ pt tới đối tượng.</summary>
    public virtual double DistanceToWorldPoint(Point2D pt) => double.PositiveInfinity;

    /// <summary>Kiểm tra con trỏ chuột (pixel) có trúng đối tượng không.</summary>
    public virtual bool HitTest(PointF mouseScreenPt, Viewport vp, float pixelTolerance = 6f)
    {
        if (!_visible) return false;
        double d = DistanceToWorldPoint(vp.ScreenToWorld(mouseScreenPt));
        return d * vp.Scale <= pixelTolerance;
    }

    public virtual void Draw(Graphics g, Viewport vp) { }

    /// <summary>Vẽ quầng sáng khi được chọn/di chuột qua (mặc định: không vẽ gì).</summary>
    public virtual void DrawHalo(Graphics g, Viewport vp) { }

    // ===== Câu 9a: cơ chế hình học động =====

    /// <summary>true = Đối tượng tự do (Free), false = Đối tượng phụ thuộc (Dependent).</summary>
    public virtual bool IsIndependent => true;

    /// <summary>Các đối tượng cha mà đối tượng này phụ thuộc vào.</summary>
    public virtual IEnumerable<GeoObject> Parents => Enumerable.Empty<GeoObject>();

    /// <summary>Kiểm tra (đệ quy) đối tượng này có phụ thuộc vào other không.</summary>
    public bool DependsOn(GeoObject other) =>
        Parents.Any(p => ReferenceEquals(p, other) || p.DependsOn(other));

    /// <summary>Tính lại trạng thái từ các đối tượng cha.</summary>
    public virtual void Update() { }

    // Câu 9d: mặc định không kéo được
    public virtual void DragBy(Vector2D delta) { }

    /// <summary>Đối tượng có thể kéo bằng công cụ Move hay không.</summary>
    public virtual bool IsDraggable => false;

    public override string ToString() => GetAlgebraDescription();
}
