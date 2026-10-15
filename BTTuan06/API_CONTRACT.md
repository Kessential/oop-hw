# Hợp đồng API giữa GeoLib2D (Câu 6–9) và GeoGebraApp (Câu 10)

Đã có sẵn (KHÔNG sửa chữ ký): `GeoLib2D.Core` — ColorStyle, Point2D, Vector2D, GeoMath, GeoRegistry,
Viewport, CoordinateGrid; `GeoLib2D.Objects` — GeoObject, GeoPoint. Đọc mã nguồn để biết chi tiết.

Mọi lớp dưới đây nằm trong namespace `GeoLib2D.Objects`, thư mục `GeoLib2D/Objects/`, mỗi lớp một file.
`label = null` nghĩa là tự sinh nhãn qua GeoRegistry. Mọi đối tượng phụ thuộc trả về các cha qua
`Parents` và `IsIndependent => false`; `Update()` tính lại từ cha. UI gọi `Update()` lần lượt theo
thứ tự tạo (cha luôn được tạo trước con).

| Lớp | Kế thừa | Hàm tạo / thành viên công khai mà UI sẽ dùng |
|---|---|---|
| GeoLine | GeoObject | `GeoLine(GeoPoint p1, GeoPoint p2, string label = null)`; `GeoPoint P1, P2`; `double A, B, C` (ax+by+c=0, tính từ vị trí hiện tại); `Vector2D GetDirection()`; TypeName "Đường thẳng"; mô tả `"f: ax + by = -c"`; nhãn `NextLineLabel()` |
| GeoSegment | GeoLine | `GeoSegment(GeoPoint p1, GeoPoint p2, string label = null)`; `double GetLength()`; mô tả `"f = 3.61"`; TypeName "Đoạn thẳng" |
| GeoRay | GeoLine | `GeoRay(GeoPoint start, GeoPoint through, string label = null)`; TypeName "Tia" |
| GeoRegion | GeoObject (abstract) | `int FillAlpha` (0..255, mặc định 40); `SolidBrush CreateFillBrush()`; `virtual double GetArea()`, `GetPerimeter()`; `virtual bool ContainsPoint(Point2D worldPt)` |
| GeoCircle | GeoRegion | `GeoCircle(GeoPoint center, GeoPoint pointOnCircle, string label = null)`; `GeoCircle(GeoPoint center, double fixedRadius, string label = null)`; `GeoPoint Center`, `GeoPoint PointOnCircle` (null nếu bán kính cố định), `double FixedRadius`, `double Radius`; nhãn `NextCircleLabel()` |
| GeoPolygon | GeoRegion | `GeoPolygon(IEnumerable<GeoPoint> vertices, string label = null)` (ném ArgumentException nếu < 3 đỉnh); `IReadOnlyList<GeoPoint> Vertices`; nhãn `NextPolygonLabel()` |
| GeoTriangle | GeoPolygon | `GeoTriangle(GeoPoint a, GeoPoint b, GeoPoint c, string label = null)`; `Point2D GetCentroid()`, `GetCircumcenter()`, `GetIncenter()`, `GetOrthocenter()` (null nếu suy biến) |
| GeoRectangle | GeoPolygon | `GeoRectangle(GeoPoint corner, GeoPoint oppositeCorner, string label = null)` — tự sinh 2 đỉnh phụ thuộc, lấy qua `IReadOnlyList<GeoPoint> GeneratedVertices` (UI phải thêm chúng vào scene TRƯỚC hình chữ nhật); `GeoRectangle(GeoPoint a, GeoPoint b, GeoPoint c, GeoPoint d, string label = null)`; `bool IsValidRectangle()` |
| MidpointPoint | GeoPoint | `MidpointPoint(GeoPoint parentA, GeoPoint parentB, string label = null)`; `ParentA, ParentB` |
| IntersectionPoint | GeoPoint | `IntersectionPoint(GeoLine line1, GeoLine line2, string label = null)`; `Line1, Line2`; `bool IsDefined` (false ⇒ Visible = false) |

Thứ tự vẽ ở UI: GeoRegion → GeoLine → GeoPoint. Chọn/hover: dùng `HitTest`, `DrawHalo`, `Select()/Deselect()`, `IsHovered`.
Kéo: chỉ khi `IsDraggable`; gọi `DragBy(Vector2D)` rồi `Update()` toàn bộ.
Xóa: xóa đối tượng và mọi đối tượng `DependsOn(nó)`.
