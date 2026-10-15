using GeoLib2D.Core;

namespace GeoLib2D.Objects;

/// <summary>
/// Câu 9b, 9d: Trung điểm của hai điểm cha — điểm phụ thuộc màu xám RGB(96, 96, 96).
/// </summary>
public class MidpointPoint : GeoPoint
{
    public GeoPoint ParentA { get; }
    public GeoPoint ParentB { get; }

    public MidpointPoint(GeoPoint parentA, GeoPoint parentB, string label = null)
        : base(0, 0, label, DependentPointStyle)
    {
        ParentA = parentA ?? throw new ArgumentNullException(nameof(parentA));
        ParentB = parentB ?? throw new ArgumentNullException(nameof(parentB));
        Update();
    }

    public override bool IsIndependent => false;

    public override IEnumerable<GeoObject> Parents
    {
        get
        {
            yield return ParentA;
            yield return ParentB;
        }
    }

    // Câu 9b: tọa độ ((xA + xB) / 2, (yA + yB) / 2)
    public override void Update() =>
        _position.MoveTo((ParentA.X + ParentB.X) / 2.0, (ParentA.Y + ParentB.Y) / 2.0);

    // Câu 9d: chỉ kéo được khi cả hai điểm cha đều tự do
    public override bool IsDraggable => ParentA.IsIndependent && ParentB.IsIndependent;

    // Câu 9d: kéo trung điểm = dịch chuyển đồng thời cả hai điểm cha
    public override void DragBy(Vector2D delta)
    {
        if (!IsDraggable) return;
        ParentA.DragBy(delta);
        if (!ReferenceEquals(ParentA, ParentB)) ParentB.DragBy(delta);
        Update();
    }
}
