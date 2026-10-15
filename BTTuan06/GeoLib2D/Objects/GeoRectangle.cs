using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 8d: Hình chữ nhật — kế thừa GeoPolygon.
/// Tạo từ 2 góc đối diện (sinh tự động 2 đỉnh phụ thuộc song song hệ trục) hoặc từ 4 đỉnh bất kỳ.
/// </summary>
public class GeoRectangle : GeoPolygon
{
    private readonly List<GeoPoint> _generatedVertices = new List<GeoPoint>();

    // Câu 8d (1): từ 2 góc đối diện; thứ tự đỉnh: corner → (x2, y1) → opposite → (x1, y2)
    public GeoRectangle(GeoPoint corner, GeoPoint oppositeCorner, string label = null)
        : base(BuildAxisAlignedVertices(corner, oppositeCorner), label, DefaultPolygonStyle)
    {
        _generatedVertices.Add(_vertices[1]);
        _generatedVertices.Add(_vertices[3]);
    }

    // Câu 8d (2): từ 4 đỉnh bất kỳ (dùng IsValidRectangle() để kiểm tra)
    public GeoRectangle(GeoPoint a, GeoPoint b, GeoPoint c, GeoPoint d, string label = null)
        : base(ToVertexList(new[] { a, b, c, d }), label, DefaultPolygonStyle) { }

    private static List<GeoPoint> BuildAxisAlignedVertices(GeoPoint corner, GeoPoint opposite)
    {
        ArgumentNullException.ThrowIfNull(corner);
        ArgumentNullException.ThrowIfNull(opposite);
        if (ReferenceEquals(corner, opposite))
            throw new ArgumentException("Hai góc đối diện phải là hai điểm khác nhau.", nameof(opposite));

        // Đỉnh sinh ra là điểm phụ thuộc: lấy X của một góc và Y của góc kia
        var second = new RectangleVertexPoint(opposite, corner);
        var fourth = new RectangleVertexPoint(corner, opposite);
        return new List<GeoPoint> { corner, second, opposite, fourth };
    }

    /// <summary>Hai đỉnh sinh tự động (rỗng nếu tạo từ 4 đỉnh). UI thêm chúng vào scene trước hình chữ nhật.</summary>
    public IReadOnlyList<GeoPoint> GeneratedVertices => _generatedVertices.AsReadOnly();

    public override string TypeName => "Hình chữ nhật";

    // Câu 8d: 4 góc vuông ⇔ tích vô hướng hai cạnh kề tại mỗi đỉnh ≈ 0 (so sánh với GeoMath.EPSILON)
    public bool IsValidRectangle()
    {
        int n = _vertices.Count;
        if (n != 4) return false;
        for (int i = 0; i < n; i++)
        {
            Point2D cur = _vertices[i].Position;
            var toPrev = new Vector2D(cur, _vertices[(i + n - 1) % n].Position);
            var toNext = new Vector2D(cur, _vertices[(i + 1) % n].Position);
            if (toPrev.Length() < GeoMath.EPSILON || toNext.Length() < GeoMath.EPSILON) return false;
            // Chuẩn hóa để sai số không phụ thuộc độ dài cạnh
            if (!GeoMath.IsZero(toPrev.Normalize().DotProduct(toNext.Normalize()))) return false;
        }
        return true;
    }
}
