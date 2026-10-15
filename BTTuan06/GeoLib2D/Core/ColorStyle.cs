using System.Drawing.Drawing2D;

namespace GeoLib2D.Core;

/// <summary>
/// Câu 1a, 1b, 2b: Đóng gói thuộc tính hiển thị (màu ARGB, độ dày nét, kiểu nét) tương thích GDI+.
/// </summary>
public class ColorStyle
{
    // Câu 1a: các trường dữ liệu riêng tư
    private int _red, _green, _blue, _alpha = 255;
    private float _strokeWidth = 2.0f;
    private DashStyle _dashStyle = DashStyle.Solid;

    // Hàm tạo mặc định: màu xanh đậm kiểu GeoGebra
    public ColorStyle() : this(21, 101, 192) { }

    public ColorStyle(int red, int green, int blue, int alpha = 255, float strokeWidth = 2.0f,
                      DashStyle dashStyle = DashStyle.Solid)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
        StrokeWidth = strokeWidth;
        DashStyle = dashStyle;
    }

    public ColorStyle(Color color, float strokeWidth = 2.0f)
        : this(color.R, color.G, color.B, color.A, strokeWidth) { }

    // Câu 2b: Copy Constructor (deep copy)
    public ColorStyle(ColorStyle other)
    {
        ArgumentNullException.ThrowIfNull(other);
        _red = other._red;
        _green = other._green;
        _blue = other._blue;
        _alpha = other._alpha;
        _strokeWidth = other._strokeWidth;
        _dashStyle = other._dashStyle;
    }

    // Câu 1b: Properties có kiểm tra ràng buộc bằng Math.Clamp
    public int Red
    {
        get => _red;
        set => _red = Math.Clamp(value, 0, 255);
    }

    public int Green
    {
        get => _green;
        set => _green = Math.Clamp(value, 0, 255);
    }

    public int Blue
    {
        get => _blue;
        set => _blue = Math.Clamp(value, 0, 255);
    }

    public int Alpha
    {
        get => _alpha;
        set => _alpha = Math.Clamp(value, 0, 255);
    }

    public float StrokeWidth
    {
        get => _strokeWidth;
        set => _strokeWidth = float.IsNaN(value) ? 1.0f : Math.Clamp(value, 1.0f, 50.0f);
    }

    public DashStyle DashStyle
    {
        get => _dashStyle;
        set => _dashStyle = value == DashStyle.Custom ? DashStyle.Solid : value;
    }

    public ColorStyle Clone() => new ColorStyle(this);

    /// <summary>Gán lại màu từ System.Drawing.Color (dùng cho ColorDialog), giữ nguyên nét vẽ.</summary>
    public void SetColor(Color color)
    {
        Red = color.R;
        Green = color.G;
        Blue = color.B;
        Alpha = color.A;
    }

    public Color ToDrawingColor() => Color.FromArgb(_alpha, _red, _green, _blue);

    public Pen CreatePen() => new Pen(ToDrawingColor(), _strokeWidth) { DashStyle = _dashStyle };

    public override string ToString() =>
        $"ARGB({_alpha}, {_red}, {_green}, {_blue}), {_strokeWidth:0.#}px, {_dashStyle}";
}
