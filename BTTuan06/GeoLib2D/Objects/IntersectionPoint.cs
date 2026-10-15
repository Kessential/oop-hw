using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 9c, 9d: Giao điểm của hai đường thẳng (hoặc tia / đoạn thẳng) — điểm phụ thuộc, không kéo được.
/// </summary>
public class IntersectionPoint : GeoPoint
{
    // true khi điểm đang bị ẩn tự động vì không xác định (để không ghi đè việc người dùng tự ẩn)
    private bool _hiddenBecauseUndefined;

    public GeoLine Line1 { get; }
    public GeoLine Line2 { get; }

    /// <summary>false nếu hai đường song song/trùng nhau hoặc giao điểm nằm ngoài tia/đoạn thẳng.</summary>
    public bool IsDefined { get; private set; }

    public IntersectionPoint(GeoLine line1, GeoLine line2, string label = null)
        : base(0, 0, label, DependentPointStyle)
    {
        Line1 = line1 ?? throw new ArgumentNullException(nameof(line1));
        Line2 = line2 ?? throw new ArgumentNullException(nameof(line2));
        Update();
    }

    public override bool IsIndependent => false;

    public override IEnumerable<GeoObject> Parents
    {
        get
        {
            yield return Line1;
            yield return Line2;
        }
    }

    // Câu 9c: giải hệ a1·x + b1·y = -c1 ; a2·x + b2·y = -c2
    public override void Update()
    {
        bool solved = GeoMath.SolveLinear2x2(
            Line1.A, Line1.B, -Line1.C,
            Line2.A, Line2.B, -Line2.C,
            out double x, out double y);

        if (solved)
        {
            var p = new Point2D(x, y);
            // Với tia/đoạn thẳng, giao điểm phải nằm trong phạm vi tham số t của từng đối tượng
            solved = Line1.ContainsParameter(Line1.GetParameter(p)) &&
                     Line2.ContainsParameter(Line2.GetParameter(p));
            if (solved) _position.MoveTo(x, y);
        }

        IsDefined = solved;
        if (!solved && _visible)
        {
            _visible = false; // Undefined
            _hiddenBecauseUndefined = true;
        }
        else if (solved && _hiddenBecauseUndefined)
        {
            _visible = true;
            _hiddenBecauseUndefined = false;
        }
    }

    public override string GetAlgebraDescription() =>
        IsDefined ? base.GetAlgebraDescription() : $"{_label} không xác định";

    // Câu 9d: khóa kéo — vị trí do hai đường thẳng quyết định
    public override void DragBy(Vector2D delta) { }
}
