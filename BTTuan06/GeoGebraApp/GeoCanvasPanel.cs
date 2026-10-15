using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using GeoLib2D.Core;
using GeoLib2D.Objects;

namespace GeoGebraApp;

/// <summary>Câu 10a: Các công cụ thao tác trên vùng vẽ.</summary>
public enum ActiveTool
{
    Move,
    NewPoint,
    Line,
    Segment,
    Ray,
    Circle,
    CircleRadius,
    Polygon,
    Triangle,
    Rectangle,
    Midpoint,
    Intersect
}

/// <summary>Tham số sự kiện chứa tọa độ thực của con trỏ chuột.</summary>
public class WorldMouseEventArgs : EventArgs
{
    public WorldMouseEventArgs(Point2D world) => World = world;

    public Point2D World { get; }
}

/// <summary>
/// Câu 10a, 10b, 10c: Vùng vẽ hình học (Graphics View) — quản lý danh sách đối tượng, khung nhìn,
/// lưới tọa độ, vẽ theo lớp và xử lý chuột/bàn phím cho từng công cụ.
/// </summary>
public class GeoCanvasPanel : Panel
{
    private readonly List<GeoObject> _objects = new();
    private ActiveTool _tool = ActiveTool.Move;
    private GeoObject _selected;
    private GeoObject _hovered;

    // Trạng thái dựng hình đang dở (các điểm / đường đã chọn)
    private readonly List<GeoPoint> _pendingPoints = new();
    private readonly List<GeoLine> _pendingLines = new();

    // Trạng thái chuột
    private PointF _mouseScreen;
    private bool _mouseInside;
    private Point _lastMouse;
    private bool _rightPanning;
    private bool _leftPanning;
    private GeoObject _dragTarget;

    public GeoCanvasPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.White;
        SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        TabStop = true;

        View = new Viewport(Math.Max(1, ClientSize.Width), Math.Max(1, ClientSize.Height));
        Grid = new CoordinateGrid(View);
    }

    // ===== Sự kiện để Form đồng bộ =====

    /// <summary>Danh sách đối tượng thay đổi về cấu trúc (thêm / xóa).</summary>
    public event EventHandler ObjectsChanged;

    /// <summary>Giá trị của đối tượng thay đổi (kéo thả, cập nhật phụ thuộc) — chỉ cần cập nhật text.</summary>
    public event EventHandler ObjectsUpdated;

    public event EventHandler SelectionChanged;

    public event EventHandler<WorldMouseEventArgs> MouseWorldMoved;

    /// <summary>Đổi công cụ hoặc tiến độ dựng hình thay đổi (để cập nhật thanh trạng thái).</summary>
    public event EventHandler ToolStateChanged;

    // ===== Thuộc tính công khai =====

    [Browsable(false)]
    public List<GeoObject> Objects => _objects;

    [Browsable(false)]
    public Viewport View { get; }

    [Browsable(false)]
    public CoordinateGrid Grid { get; }

    [Browsable(false)]
    public GeoObject SelectedObject => _selected;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ActiveTool ActiveTool
    {
        get => _tool;
        set
        {
            CancelConstruction();
            _tool = value;
            SetHovered(null);
            Cursor = value == ActiveTool.Move ? Cursors.Default : Cursors.Cross;
            ToolStateChanged?.Invoke(this, EventArgs.Empty);
            Invalidate();
        }
    }

    // ===== Quản lý đối tượng =====

    public void AddObject(GeoObject obj) => AddObjects(new[] { obj });

    public void AddObjects(IEnumerable<GeoObject> objs)
    {
        foreach (var o in objs)
            if (o != null && !_objects.Contains(o)) _objects.Add(o);
        UpdateAll();
        ObjectsChanged?.Invoke(this, EventArgs.Empty);
        Invalidate();
    }

    public void ClearObjects()
    {
        CancelConstruction();
        _objects.Clear();
        _hovered = null;
        _dragTarget = null;
        bool hadSelection = _selected != null;
        _selected = null;
        ObjectsChanged?.Invoke(this, EventArgs.Empty);
        if (hadSelection) SelectionChanged?.Invoke(this, EventArgs.Empty);
        Invalidate();
    }

    /// <summary>Xóa đối tượng cùng mọi đối tượng phụ thuộc (DependsOn) vào nó.</summary>
    public void DeleteObject(GeoObject obj)
    {
        if (obj == null) return;

        var roots = new List<GeoObject> { obj };
        // Hai đỉnh phụ do hình chữ nhật tự sinh đi theo hình chữ nhật
        if (obj is GeoRectangle rect) roots.AddRange(rect.GeneratedVertices);

        var toRemove = _objects
            .Where(o => roots.Any(r => ReferenceEquals(o, r) || o.DependsOn(r)))
            .ToHashSet();
        if (toRemove.Count == 0) return;

        _objects.RemoveAll(toRemove.Contains);
        CancelConstruction();
        if (_hovered != null && toRemove.Contains(_hovered)) _hovered = null;
        if (_dragTarget != null && toRemove.Contains(_dragTarget)) _dragTarget = null;

        bool selectionLost = _selected != null && toRemove.Contains(_selected);
        if (selectionLost) _selected = null;

        UpdateAll();
        ObjectsChanged?.Invoke(this, EventArgs.Empty);
        if (selectionLost) SelectionChanged?.Invoke(this, EventArgs.Empty);
        Invalidate();
    }

    public void DeleteSelected() => DeleteObject(_selected);

    /// <summary>Chọn một đối tượng (null = bỏ chọn tất cả).</summary>
    public void SelectObject(GeoObject obj)
    {
        if (ReferenceEquals(obj, _selected)) return;
        foreach (var o in _objects) o.Deselect();
        _selected = obj;
        obj?.Select();
        SelectionChanged?.Invoke(this, EventArgs.Empty);
        Invalidate();
    }

    /// <summary>Tính lại toàn bộ đối tượng theo thứ tự tạo (cha luôn trước con).</summary>
    public void UpdateAll()
    {
        foreach (var o in _objects)
        {
            try
            {
                o.Update();
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                Debug.WriteLine($"Update {o.Label}: {ex.Message}");
            }
        }
    }

    /// <summary>Báo cho các bên lắng nghe rằng giá trị đối tượng đã đổi (ví dụ sau khi đổi màu).</summary>
    public void NotifyObjectsUpdated()
    {
        ObjectsUpdated?.Invoke(this, EventArgs.Empty);
        Invalidate();
    }

    public void ResetView()
    {
        View.ResetView();
        Invalidate();
    }

    /// <summary>Hủy thao tác dựng hình đang dở (phím Esc).</summary>
    public void CancelConstruction()
    {
        bool had = _pendingPoints.Count > 0 || _pendingLines.Count > 0;
        _pendingPoints.Clear();
        _pendingLines.Clear();
        if (had)
        {
            ToolStateChanged?.Invoke(this, EventArgs.Empty);
            Invalidate();
        }
    }

    // ===== Mô tả công cụ =====

    public static string GetToolName(ActiveTool tool) => tool switch
    {
        ActiveTool.Move => "Di chuyển",
        ActiveTool.NewPoint => "Điểm mới",
        ActiveTool.Line => "Đường thẳng qua 2 điểm",
        ActiveTool.Segment => "Đoạn thẳng",
        ActiveTool.Ray => "Tia",
        ActiveTool.Circle => "Đường tròn (tâm, điểm)",
        ActiveTool.CircleRadius => "Đường tròn (tâm, bán kính)",
        ActiveTool.Polygon => "Đa giác",
        ActiveTool.Triangle => "Tam giác",
        ActiveTool.Rectangle => "Hình chữ nhật",
        ActiveTool.Midpoint => "Trung điểm",
        ActiveTool.Intersect => "Giao điểm",
        _ => tool.ToString()
    };

    public static string GetToolDescription(ActiveTool tool) => tool switch
    {
        ActiveTool.Move => "Kéo điểm tự do để di chuyển; click để chọn đối tượng",
        ActiveTool.NewPoint => "Click vào vùng vẽ để tạo điểm mới",
        ActiveTool.Line => "Chọn 2 điểm để vẽ đường thẳng đi qua chúng",
        ActiveTool.Segment => "Chọn 2 điểm để vẽ đoạn thẳng",
        ActiveTool.Ray => "Chọn điểm gốc rồi một điểm mà tia đi qua",
        ActiveTool.Circle => "Chọn tâm rồi một điểm trên đường tròn",
        ActiveTool.CircleRadius => "Chọn tâm rồi nhập bán kính",
        ActiveTool.Polygon => "Chọn lần lượt các đỉnh, click lại đỉnh đầu tiên để kết thúc",
        ActiveTool.Triangle => "Chọn 3 đỉnh của tam giác",
        ActiveTool.Rectangle => "Chọn 2 góc đối diện của hình chữ nhật",
        ActiveTool.Midpoint => "Chọn 2 điểm hoặc 1 đoạn thẳng",
        ActiveTool.Intersect => "Chọn 2 đường (đường thẳng, đoạn thẳng, tia)",
        _ => ""
    };

    /// <summary>Hướng dẫn thao tác tiếp theo cho công cụ hiện tại.</summary>
    [Browsable(false)]
    public string CurrentHint
    {
        get
        {
            int n = _pendingPoints.Count;
            return _tool switch
            {
                ActiveTool.Move =>
                    "Kéo điểm tự do để di chuyển • Click để chọn • Chuột phải kéo nền để cuộn • Lăn chuột để thu phóng • Delete để xóa",
                ActiveTool.NewPoint => "Click vào vùng vẽ để tạo điểm mới",
                ActiveTool.Line or ActiveTool.Segment => n == 0
                    ? "Chọn điểm thứ nhất (click điểm có sẵn hoặc chỗ trống)"
                    : "Chọn điểm thứ hai (Esc để hủy)",
                ActiveTool.Ray => n == 0 ? "Chọn điểm gốc của tia" : "Chọn điểm mà tia đi qua (Esc để hủy)",
                ActiveTool.Circle => n == 0 ? "Chọn tâm đường tròn" : "Chọn một điểm trên đường tròn (Esc để hủy)",
                ActiveTool.CircleRadius => "Chọn tâm đường tròn, sau đó nhập bán kính",
                ActiveTool.Polygon => n == 0
                    ? "Chọn đỉnh đầu tiên của đa giác"
                    : $"Đã chọn {n} đỉnh — chọn đỉnh tiếp theo; click lại đỉnh đầu tiên để kết thúc (cần ≥ 3 đỉnh, Esc để hủy)",
                ActiveTool.Triangle => $"Chọn đỉnh thứ {n + 1}/3 của tam giác" + (n > 0 ? " (Esc để hủy)" : ""),
                ActiveTool.Rectangle => n == 0 ? "Chọn góc thứ nhất" : "Chọn góc đối diện (Esc để hủy)",
                ActiveTool.Midpoint => n == 0
                    ? "Chọn 2 điểm hoặc 1 đoạn thẳng"
                    : "Chọn điểm thứ hai (Esc để hủy)",
                ActiveTool.Intersect => _pendingLines.Count == 0
                    ? "Chọn 2 đường (đường thẳng / đoạn / tia), hoặc click thẳng vào chỗ giao nhau"
                    : "Chọn đường thứ hai (Esc để hủy)",
                _ => ""
            };
        }
    }

    // ===== Câu 10b: vẽ =====

    protected override void OnResize(EventArgs eventargs)
    {
        View?.Resize(ClientSize.Width, ClientSize.Height);
        base.OnResize(eventargs);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;

        Grid.Draw(g);

        // Thứ tự lớp: Vùng (Region) → Đường (Line) → đối tượng khác → Điểm (Point)
        DrawLayer(g, o => o is GeoRegion);
        DrawLayer(g, o => o is GeoLine);
        DrawLayer(g, o => o is not GeoRegion && o is not GeoLine && o is not GeoPoint);
        DrawLayer(g, o => o is GeoPoint);

        DrawPendingMarks(g);
        DrawPreview(g);
    }

    private void DrawLayer(Graphics g, Func<GeoObject, bool> filter)
    {
        foreach (var o in _objects)
        {
            if (!o.Visible || !filter(o)) continue;
            try
            {
                if (o.IsSelected || o.IsHovered) o.DrawHalo(g, View);
                o.Draw(g, View);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or OverflowException)
            {
                Debug.WriteLine($"Draw {o.Label}: {ex.Message}");
            }
        }
    }

    /// <summary>Đánh dấu các điểm / đường đã chọn trong thao tác đang dựng.</summary>
    private void DrawPendingMarks(Graphics g)
    {
        using var ring = new Pen(Color.FromArgb(220, 255, 140, 0), 2f);
        foreach (var p in _pendingPoints)
        {
            PointF s = View.WorldToScreen(p.Position);
            float r = p.PointRadius + 5;
            g.DrawEllipse(ring, s.X - r, s.Y - r, 2 * r, 2 * r);
        }
        foreach (var l in _pendingLines)
        {
            l.DrawHalo(g, View);
        }
    }

    /// <summary>Hình xem trước (rubber-band) bằng nét đứt bám theo chuột.</summary>
    private void DrawPreview(Graphics g)
    {
        if (!_mouseInside || _tool == ActiveTool.Move || _tool == ActiveTool.Intersect) return;

        bool onExistingPoint = HitPoint(_mouseScreen) != null;
        PointF m = View.WorldToScreen(TargetWorld(_mouseScreen));
        var pts = _pendingPoints.Select(p => View.WorldToScreen(p.Position)).ToList();

        using var pen = new Pen(Color.FromArgb(200, 80, 80, 80), 1.6f) { DashStyle = DashStyle.Dash };
        using var fill = new SolidBrush(Color.FromArgb(28, 21, 101, 192));

        if (pts.Count > 0)
        {
            switch (_tool)
            {
                case ActiveTool.Line:
                    DrawThrough(g, pen, pts[0], m, bothWays: true);
                    break;
                case ActiveTool.Ray:
                    DrawThrough(g, pen, pts[0], m, bothWays: false);
                    break;
                case ActiveTool.Segment:
                    g.DrawLine(pen, pts[0], m);
                    break;
                case ActiveTool.Midpoint:
                    g.DrawLine(pen, pts[0], m);
                    var mid = new PointF((pts[0].X + m.X) / 2, (pts[0].Y + m.Y) / 2);
                    using (var midBrush = new SolidBrush(Color.FromArgb(150, 96, 96, 96)))
                        g.FillEllipse(midBrush, mid.X - 4, mid.Y - 4, 8, 8);
                    break;
                case ActiveTool.Circle:
                    float r = Distance(pts[0], m);
                    g.FillEllipse(fill, pts[0].X - r, pts[0].Y - r, 2 * r, 2 * r);
                    g.DrawEllipse(pen, pts[0].X - r, pts[0].Y - r, 2 * r, 2 * r);
                    g.DrawLine(pen, pts[0], m);
                    break;
                case ActiveTool.Rectangle:
                    var rect = RectangleF.FromLTRB(Math.Min(pts[0].X, m.X), Math.Min(pts[0].Y, m.Y),
                                                   Math.Max(pts[0].X, m.X), Math.Max(pts[0].Y, m.Y));
                    g.FillRectangle(fill, rect);
                    g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                    break;
                case ActiveTool.Polygon:
                case ActiveTool.Triangle:
                    var poly = new List<PointF>(pts) { m };
                    if (poly.Count >= 3) g.FillPolygon(fill, poly.ToArray());
                    if (poly.Count >= 2) g.DrawLines(pen, poly.ToArray());
                    if (poly.Count >= 3) g.DrawLine(pen, m, pts[0]);
                    break;
            }
        }

        // Điểm "ma" tại vị trí sẽ tạo điểm mới
        if (!onExistingPoint)
        {
            using var ghost = new Pen(Color.FromArgb(160, 21, 101, 192), 1.5f);
            g.DrawEllipse(ghost, m.X - 4, m.Y - 4, 8, 8);
        }
    }

    private void DrawThrough(Graphics g, Pen pen, PointF a, PointF b, bool bothWays)
    {
        float dx = b.X - a.X, dy = b.Y - a.Y;
        float len = MathF.Sqrt(dx * dx + dy * dy);
        if (len < 1f) return;
        float far = 2f * (Width + Height);
        dx /= len;
        dy /= len;
        var end = new PointF(a.X + dx * far, a.Y + dy * far);
        var start = bothWays ? new PointF(a.X - dx * far, a.Y - dy * far) : a;
        g.DrawLine(pen, start, end);
    }

    private static float Distance(PointF a, PointF b)
    {
        float dx = a.X - b.X, dy = a.Y - b.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    // ===== Câu 10c: chuột & bàn phím =====

    protected override bool IsInputKey(Keys keyData)
    {
        switch (keyData & Keys.KeyCode)
        {
            case Keys.Escape:
            case Keys.Delete:
            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
                return true;
        }
        return base.IsInputKey(keyData);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode == Keys.Escape)
        {
            if (_pendingPoints.Count > 0 || _pendingLines.Count > 0) CancelConstruction();
            else SelectObject(null);
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete)
        {
            DeleteSelected();
            e.Handled = true;
        }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _mouseInside = true;
        if (!Focused) Focus();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _mouseInside = false;
        SetHovered(null);
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Focus();
        _mouseInside = true;
        _mouseScreen = e.Location;
        _lastMouse = e.Location;

        if (e.Button == MouseButtons.Right)
        {
            _rightPanning = true;
            Cursor = Cursors.SizeAll;
            return;
        }
        if (e.Button != MouseButtons.Left) return;

        if (_tool == ActiveTool.Move)
        {
            var hits = HitTestAll(e.Location);
            var draggable = hits.FirstOrDefault(h => h.IsDraggable);
            SelectObject(draggable ?? hits.FirstOrDefault());
            if (draggable != null) _dragTarget = draggable;
            else if (hits.Count == 0) _leftPanning = true;
            return;
        }

        HandleToolClick(e.Location);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        _mouseInside = true;
        _mouseScreen = e.Location;
        Point2D world = View.ScreenToWorld(e.Location);
        MouseWorldMoved?.Invoke(this, new WorldMouseEventArgs(world));

        int dx = e.X - _lastMouse.X, dy = e.Y - _lastMouse.Y;

        if (_rightPanning || _leftPanning)
        {
            if (dx != 0 || dy != 0) View.Pan(dx, dy);
            _lastMouse = e.Location;
            Invalidate();
            return;
        }

        if (_dragTarget != null)
        {
            Vector2D delta;
            if (_dragTarget is GeoPoint gp && Grid.SnapToGrid)
                delta = new Vector2D(gp.Position, Grid.Snap(world));
            else
                delta = new Vector2D(View.ScreenToWorld(_lastMouse), world);
            _lastMouse = e.Location;

            if (!GeoMath.IsZero(delta.Length()))
            {
                _dragTarget.DragBy(delta);
                UpdateAll();
                ObjectsUpdated?.Invoke(this, EventArgs.Empty);
            }
            Invalidate();
            return;
        }

        _lastMouse = e.Location;
        SetHovered(FindHoverTarget(e.Location));
        if (_tool == ActiveTool.Move)
            Cursor = _hovered != null && _hovered.IsDraggable ? Cursors.Hand : Cursors.Default;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Right && _rightPanning)
        {
            _rightPanning = false;
            Cursor = _tool == ActiveTool.Move ? Cursors.Default : Cursors.Cross;
        }
        if (e.Button == MouseButtons.Left)
        {
            _leftPanning = false;
            _dragTarget = null;
        }
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (e.Delta == 0) return;
        View.ZoomAt(e.Delta > 0 ? 1.1 : 1 / 1.1, e.Location);
        Invalidate();
    }

    private void SetHovered(GeoObject obj)
    {
        if (ReferenceEquals(obj, _hovered)) return;
        if (_hovered != null) _hovered.IsHovered = false;
        _hovered = obj;
        if (_hovered != null) _hovered.IsHovered = true;
    }

    // ===== Hit test =====

    /// <summary>Các đối tượng trúng chuột, ưu tiên: Điểm → Đường → khác → Vùng; trong mỗi nhóm, đối tượng vẽ sau ưu tiên.</summary>
    private List<GeoObject> HitTestAll(PointF screenPt)
    {
        var ordered = Enumerable.Reverse(_objects).Where(o => o.Visible).ToList();
        var result = new List<GeoObject>();
        result.AddRange(ordered.Where(o => o is GeoPoint && SafeHit(o, screenPt)));
        result.AddRange(ordered.Where(o => o is GeoLine && SafeHit(o, screenPt)));
        result.AddRange(ordered.Where(o => o is not GeoPoint && o is not GeoLine && o is not GeoRegion && SafeHit(o, screenPt)));
        result.AddRange(ordered.Where(o => o is GeoRegion && SafeHit(o, screenPt)));
        return result;
    }

    private bool SafeHit(GeoObject o, PointF screenPt)
    {
        try
        {
            return o.HitTest(screenPt, View);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            return false;
        }
    }

    private GeoPoint HitPoint(PointF screenPt) =>
        Enumerable.Reverse(_objects).OfType<GeoPoint>().FirstOrDefault(p => p.Visible && SafeHit(p, screenPt));

    private List<GeoLine> HitLines(PointF screenPt) =>
        Enumerable.Reverse(_objects).OfType<GeoLine>().Where(l => l.Visible && SafeHit(l, screenPt)).ToList();

    private GeoObject FindHoverTarget(PointF screenPt)
    {
        switch (_tool)
        {
            case ActiveTool.Move:
                var hits = HitTestAll(screenPt);
                return hits.FirstOrDefault(h => h.IsDraggable) ?? hits.FirstOrDefault();
            case ActiveTool.Midpoint:
                return (GeoObject)HitPoint(screenPt)
                       ?? (_pendingPoints.Count == 0 ? HitLines(screenPt).OfType<GeoSegment>().FirstOrDefault() : null);
            case ActiveTool.Intersect:
                return HitLines(screenPt).FirstOrDefault(l => !_pendingLines.Contains(l));
            default:
                return HitPoint(screenPt);
        }
    }

    /// <summary>Tọa độ thực "đích" của chuột: tâm điểm có sẵn nếu trúng, nếu không thì vị trí đã bắt dính lưới.</summary>
    private Point2D TargetWorld(PointF screenPt)
    {
        var p = HitPoint(screenPt);
        return p != null ? p.Position : Grid.Snap(View.ScreenToWorld(screenPt));
    }

    // ===== Dựng hình =====

    /// <summary>Lấy điểm có sẵn tại vị trí click, hoặc tạo điểm tự do mới (đã bắt dính lưới).</summary>
    private GeoPoint PickOrCreatePoint(PointF screenPt)
    {
        var existing = HitPoint(screenPt);
        if (existing != null) return existing;
        var p = new GeoPoint(Grid.Snap(View.ScreenToWorld(screenPt)));
        AddObject(p);
        return p;
    }

    private void HandleToolClick(PointF screenPt)
    {
        switch (_tool)
        {
            case ActiveTool.NewPoint:
            {
                var existing = HitPoint(screenPt);
                if (existing != null) SelectObject(existing);
                else AddObject(new GeoPoint(Grid.Snap(View.ScreenToWorld(screenPt))));
                break;
            }

            case ActiveTool.Line:
            case ActiveTool.Segment:
            case ActiveTool.Ray:
            case ActiveTool.Circle:
            case ActiveTool.Rectangle:
            {
                var p = PickOrCreatePoint(screenPt);
                if (_pendingPoints.Contains(p)) break;
                _pendingPoints.Add(p);
                if (_pendingPoints.Count == 2)
                {
                    GeoPoint a = _pendingPoints[0], b = _pendingPoints[1];
                    _pendingPoints.Clear();
                    TryBuild(() => _tool switch
                    {
                        ActiveTool.Line => new GeoObject[] { new GeoLine(a, b) },
                        ActiveTool.Segment => new GeoObject[] { new GeoSegment(a, b) },
                        ActiveTool.Ray => new GeoObject[] { new GeoRay(a, b) },
                        ActiveTool.Circle => new GeoObject[] { new GeoCircle(a, b) },
                        _ => BuildRectangle(a, b)
                    });
                }
                break;
            }

            case ActiveTool.Triangle:
            {
                var p = PickOrCreatePoint(screenPt);
                if (_pendingPoints.Contains(p)) break;
                _pendingPoints.Add(p);
                if (_pendingPoints.Count == 3)
                {
                    var v = _pendingPoints.ToArray();
                    _pendingPoints.Clear();
                    TryBuild(() => new GeoObject[] { new GeoTriangle(v[0], v[1], v[2]) });
                }
                break;
            }

            case ActiveTool.Polygon:
            {
                var hit = HitPoint(screenPt);
                if (hit != null && _pendingPoints.Count >= 3 && ReferenceEquals(hit, _pendingPoints[0]))
                {
                    var v = _pendingPoints.ToList();
                    _pendingPoints.Clear();
                    TryBuild(() => new GeoObject[] { new GeoPolygon(v) });
                }
                else if (hit == null || !_pendingPoints.Contains(hit))
                {
                    _pendingPoints.Add(hit ?? PickOrCreatePoint(screenPt));
                }
                break;
            }

            case ActiveTool.CircleRadius:
            {
                var center = PickOrCreatePoint(screenPt);
                _pendingPoints.Clear();
                _pendingPoints.Add(center);
                // Hiện hộp thoại sau khi sự kiện chuột kết thúc
                BeginInvoke(() => AskRadius(center));
                break;
            }

            case ActiveTool.Midpoint:
            {
                var hitPt = HitPoint(screenPt);
                if (hitPt == null && _pendingPoints.Count == 0)
                {
                    var seg = HitLines(screenPt).OfType<GeoSegment>().FirstOrDefault();
                    if (seg != null)
                    {
                        TryBuild(() => new GeoObject[] { new MidpointPoint(seg.P1, seg.P2) });
                        break;
                    }
                }
                var p = hitPt ?? PickOrCreatePoint(screenPt);
                if (_pendingPoints.Contains(p)) break;
                _pendingPoints.Add(p);
                if (_pendingPoints.Count == 2)
                {
                    GeoPoint a = _pendingPoints[0], b = _pendingPoints[1];
                    _pendingPoints.Clear();
                    TryBuild(() => new GeoObject[] { new MidpointPoint(a, b) });
                }
                break;
            }

            case ActiveTool.Intersect:
            {
                var lines = HitLines(screenPt);
                if (_pendingLines.Count == 0 && lines.Count >= 2)
                {
                    // Click trúng ngay chỗ hai đường cắt nhau
                    GeoLine l1 = lines[1], l2 = lines[0];
                    TryBuild(() => new GeoObject[] { new IntersectionPoint(l1, l2) });
                    break;
                }
                var line = lines.FirstOrDefault(l => !_pendingLines.Contains(l));
                if (line == null) break;
                _pendingLines.Add(line);
                if (_pendingLines.Count == 2)
                {
                    GeoLine l1 = _pendingLines[0], l2 = _pendingLines[1];
                    _pendingLines.Clear();
                    TryBuild(() => new GeoObject[] { new IntersectionPoint(l1, l2) });
                }
                break;
            }
        }

        SetHovered(FindHoverTarget(screenPt));
        ToolStateChanged?.Invoke(this, EventArgs.Empty);
        Invalidate();
    }

    /// <summary>Hình chữ nhật từ 2 góc đối diện: 2 đỉnh phụ sinh tự động phải được thêm TRƯỚC hình chữ nhật.</summary>
    private static GeoObject[] BuildRectangle(GeoPoint corner, GeoPoint opposite)
    {
        var rect = new GeoRectangle(corner, opposite);
        var list = new List<GeoObject>(rect.GeneratedVertices) { rect };
        return list.ToArray();
    }

    private void AskRadius(GeoPoint center)
    {
        double? radius = NumberInputDialog.Ask(FindForm(), "Đường tròn (tâm, bán kính)",
            $"Nhập bán kính của đường tròn tâm {center.Label}:", 2.0);
        _pendingPoints.Clear();
        if (radius.HasValue)
            TryBuild(() => new GeoObject[] { new GeoCircle(center, radius.Value) });
        ToolStateChanged?.Invoke(this, EventArgs.Empty);
        Invalidate();
    }

    private void TryBuild(Func<GeoObject[]> factory)
    {
        try
        {
            AddObjects(factory());
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            MessageBox.Show(FindForm(), "Không thể dựng hình: " + ex.Message, "GeoGebra 2D",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
