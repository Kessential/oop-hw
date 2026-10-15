namespace GeoLib2D.Core;

/// <summary>
/// Câu 4a, 4b, 4c: Khung nhìn — biến đổi giữa tọa độ thực (World) và tọa độ pixel (Screen).
/// Quan hệ Hợp thành (Composition): Viewport tự tạo và sở hữu Point2D OriginScreen.
/// </summary>
public class Viewport
{
    public const double MinScale = 5.0;
    public const double MaxScale = 500.0;
    public const double DefaultScale = 50.0;

    private double _scale = DefaultScale;
    private int _width, _height;

    /// <summary>Tọa độ pixel của gốc O(0, 0) trên Panel (thành phần do Viewport sở hữu).</summary>
    public Point2D OriginScreen { get; }

    public Viewport(int width, int height)
    {
        _width = Math.Max(1, width);
        _height = Math.Max(1, height);
        OriginScreen = new Point2D(_width / 2.0, _height / 2.0);
    }

    /// <summary>Số pixel trên 1 đơn vị toán học, giới hạn [5, 500].</summary>
    public double Scale
    {
        get => _scale;
        set => _scale = GeoMath.Clamp(value, MinScale, MaxScale);
    }

    public int Width => _width;
    public int Height => _height;

    /// <summary>Cập nhật kích thước vùng vẽ, giữ nguyên tâm khung nhìn.</summary>
    public void Resize(int width, int height)
    {
        width = Math.Max(1, width);
        height = Math.Max(1, height);
        OriginScreen.Translate((width - _width) / 2.0, (height - _height) / 2.0);
        _width = width;
        _height = height;
    }

    /// <summary>Đưa gốc tọa độ về giữa vùng vẽ và Scale về mặc định.</summary>
    public void ResetView()
    {
        _scale = DefaultScale;
        OriginScreen.MoveTo(_width / 2.0, _height / 2.0);
    }

    // Câu 4b
    public PointF WorldToScreen(Point2D w) => new PointF(
        (float)(OriginScreen.X + w.X * Scale),
        (float)(OriginScreen.Y - w.Y * Scale));

    public PointF WorldToScreen(double x, double y) => new PointF(
        (float)(OriginScreen.X + x * Scale),
        (float)(OriginScreen.Y - y * Scale));

    public Point2D ScreenToWorld(PointF s) => new Point2D(
        (s.X - OriginScreen.X) / Scale,
        (OriginScreen.Y - s.Y) / Scale);

    public float WorldLengthToPixels(double length) => (float)(length * Scale);
    public double PixelsToWorldLength(double pixels) => pixels / Scale;

    // Phạm vi tọa độ thực đang hiển thị
    public double MinWorldX => -OriginScreen.X / Scale;
    public double MaxWorldX => (_width - OriginScreen.X) / Scale;
    public double MinWorldY => (OriginScreen.Y - _height) / Scale;
    public double MaxWorldY => OriginScreen.Y / Scale;

    // Câu 4c: kéo nền
    public void Pan(float deltaPxX, float deltaPxY) => OriginScreen.Translate(deltaPxX, deltaPxY);

    /// <summary>Phóng to/thu nhỏ, giữ cố định điểm thực nằm dưới con trỏ chuột.</summary>
    public void ZoomAt(double zoomFactor, PointF mouseScreenPos)
    {
        if (zoomFactor <= 0 || double.IsNaN(zoomFactor)) return;
        Point2D anchor = ScreenToWorld(mouseScreenPos);
        Scale = _scale * zoomFactor;
        // Dời gốc để anchor vẫn ở đúng vị trí pixel mouseScreenPos
        OriginScreen.MoveTo(mouseScreenPos.X - anchor.X * Scale, mouseScreenPos.Y + anchor.Y * Scale);
    }
}
