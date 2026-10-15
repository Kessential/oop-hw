using System.Drawing.Drawing2D;
using System.Globalization;

namespace GeoLib2D.Core;

/// <summary>
/// Câu 4d: Lưới và hệ trục tọa độ. Quan hệ Kết tập (Aggregation): chỉ giữ tham chiếu tới
/// Viewport được truyền từ bên ngoài, không sở hữu vòng đời của nó.
/// </summary>
public class CoordinateGrid
{
    private Viewport _viewport;

    public CoordinateGrid(Viewport viewport)
    {
        _viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
    }

    public Viewport Viewport
    {
        get => _viewport;
        set => _viewport = value ?? throw new ArgumentNullException(nameof(value));
    }

    public bool ShowAxes { get; set; } = true;
    public bool ShowGrid { get; set; } = true;
    public bool SnapToGrid { get; set; } = false;

    public Color AxisColor { get; set; } = Color.FromArgb(37, 37, 37);
    public Color MajorGridColor { get; set; } = Color.FromArgb(192, 192, 192);
    public Color MinorGridColor { get; set; } = Color.FromArgb(232, 232, 232);

    /// <summary>Bước lưới chính (đơn vị thực) chọn tự động theo dãy 1-2-5 để vạch cách nhau ≥ 50px.</summary>
    public double GridStep
    {
        get
        {
            double raw = 50.0 / _viewport.Scale;
            double pow = Math.Pow(10, Math.Floor(Math.Log10(raw)));
            foreach (double m in new[] { 1.0, 2.0, 5.0, 10.0 })
                if (m * pow >= raw) return m * pow;
            return 10 * pow;
        }
    }

    /// <summary>Bắt dính điểm vào nút lưới gần nhất (nếu SnapToGrid bật).</summary>
    public Point2D Snap(Point2D worldPt)
    {
        if (!SnapToGrid) return worldPt;
        double step = GridStep / (GridStep >= 1 ? 2 : 1);
        return new Point2D(Math.Round(worldPt.X / step) * step, Math.Round(worldPt.Y / step) * step);
    }

    public void Draw(Graphics g)
    {
        double step = GridStep;

        if (ShowGrid)
        {
            using var minorPen = new Pen(MinorGridColor, 1f);
            using var majorPen = new Pen(MajorGridColor, 1f);
            DrawGridLines(g, minorPen, step / 5);
            DrawGridLines(g, majorPen, step);
        }

        if (ShowAxes) DrawAxes(g, step);
    }

    private void DrawGridLines(Graphics g, Pen pen, double step)
    {
        var vp = _viewport;
        if (step * vp.Scale < 4) return;

        for (double x = Math.Ceiling(vp.MinWorldX / step) * step; x <= vp.MaxWorldX; x += step)
        {
            float sx = vp.WorldToScreen(x, 0).X;
            g.DrawLine(pen, sx, 0, sx, vp.Height);
        }
        for (double y = Math.Ceiling(vp.MinWorldY / step) * step; y <= vp.MaxWorldY; y += step)
        {
            float sy = vp.WorldToScreen(0, y).Y;
            g.DrawLine(pen, 0, sy, vp.Width, sy);
        }
    }

    private void DrawAxes(Graphics g, double step)
    {
        var vp = _viewport;
        PointF o = vp.WorldToScreen(0, 0);
        using var axisPen = new Pen(AxisColor, 1.5f)
        {
            CustomEndCap = new AdjustableArrowCap(4, 6, true)
        };
        using var tickPen = new Pen(AxisColor, 1f);
        using var font = new Font("Segoe UI", 8f);
        using var brush = new SolidBrush(AxisColor);

        // Trục chỉ vẽ khi nằm trong khung; nhãn số bám mép nếu trục ra ngoài
        float axisY = Math.Clamp(o.Y, 0, vp.Height - 1);
        float axisX = Math.Clamp(o.X, 0, vp.Width - 1);

        if (o.Y >= 0 && o.Y <= vp.Height) g.DrawLine(axisPen, 0, o.Y, vp.Width - 2, o.Y);
        if (o.X >= 0 && o.X <= vp.Width) g.DrawLine(axisPen, o.X, vp.Height, o.X, 2);

        // Nhãn trục
        g.DrawString("x", font, brush, vp.Width - 14, axisY - 18);
        g.DrawString("y", font, brush, axisX + 6, 2);

        string Format(double v) => Math.Round(v, 10).ToString("0.######", CultureInfo.InvariantCulture);

        // Vạch chia trục Ox
        for (double x = Math.Ceiling(vp.MinWorldX / step) * step; x <= vp.MaxWorldX; x += step)
        {
            if (Math.Abs(x) < step / 2) continue;
            float sx = vp.WorldToScreen(x, 0).X;
            g.DrawLine(tickPen, sx, axisY - 3, sx, axisY + 3);
            string text = Format(x);
            SizeF size = g.MeasureString(text, font);
            float ty = axisY + 4 + size.Height > vp.Height ? axisY - 4 - size.Height : axisY + 4;
            g.DrawString(text, font, brush, sx - size.Width / 2, ty);
        }

        // Vạch chia trục Oy
        for (double y = Math.Ceiling(vp.MinWorldY / step) * step; y <= vp.MaxWorldY; y += step)
        {
            if (Math.Abs(y) < step / 2) continue;
            float sy = vp.WorldToScreen(0, y).Y;
            g.DrawLine(tickPen, axisX - 3, sy, axisX + 3, sy);
            string text = Format(y);
            SizeF size = g.MeasureString(text, font);
            float tx = axisX - 4 - size.Width < 0 ? axisX + 4 : axisX - 4 - size.Width;
            g.DrawString(text, font, brush, tx, sy - size.Height / 2);
        }

        // Gốc O
        if (o.X >= 0 && o.X <= vp.Width && o.Y >= 0 && o.Y <= vp.Height)
            g.DrawString("O", font, brush, o.X - 13, o.Y + 2);
    }
}
