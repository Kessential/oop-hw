using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 8d, 9: Đỉnh phụ thuộc của hình chữ nhật song song hệ trục: (XSource.X, YSource.Y).
/// Tự cập nhật khi kéo hai góc đối diện; không kéo trực tiếp được.
/// </summary>
public class RectangleVertexPoint : GeoPoint
{
    public GeoPoint XSource { get; }
    public GeoPoint YSource { get; }

    public RectangleVertexPoint(GeoPoint xSource, GeoPoint ySource, string label = null)
        : base(0, 0, label, DependentPointStyle)
    {
        XSource = xSource ?? throw new ArgumentNullException(nameof(xSource));
        YSource = ySource ?? throw new ArgumentNullException(nameof(ySource));
        Update();
    }

    public override bool IsIndependent => false;

    public override IEnumerable<GeoObject> Parents
    {
        get
        {
            yield return XSource;
            yield return YSource;
        }
    }

    public override void Update() => _position.MoveTo(XSource.X, YSource.Y);

    // Câu 9d: khóa kéo — vị trí do hai góc quyết định
    public override void DragBy(Vector2D delta) { }
}
