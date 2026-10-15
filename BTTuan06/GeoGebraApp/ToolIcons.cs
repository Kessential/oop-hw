using System.Drawing.Drawing2D;

namespace GeoGebraApp;

/// <summary>Biểu tượng cho các nút công cụ, vẽ hoàn toàn bằng code GDI+ (thiết kế trên lưới 32×32).</summary>
internal static class ToolIcons
{
    private static readonly Color Ink = Color.FromArgb(70, 70, 70);
    private static readonly Color Blue = Color.FromArgb(21, 101, 192);
    private static readonly Color Gray = Color.FromArgb(96, 96, 96);
    private static readonly Color FillBlue = Color.FromArgb(60, 21, 101, 192);

    public static Bitmap Create(ActiveTool tool, int size = 32)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.ScaleTransform(size / 32f, size / 32f);

        using var pen = new Pen(Ink, 2f);
        using var fill = new SolidBrush(FillBlue);

        switch (tool)
        {
            case ActiveTool.Move:
            {
                PointF[] arrow =
                {
                    new(9, 3), new(9, 26), new(14, 21), new(18, 29.5f),
                    new(21.5f, 28), new(17.5f, 19.5f), new(24.5f, 19.5f)
                };
                using var body = new SolidBrush(Color.FromArgb(45, 45, 45));
                g.FillPolygon(body, arrow);
                using var outline = new Pen(Color.White, 1f);
                g.DrawPolygon(outline, arrow);
                break;
            }
            case ActiveTool.NewPoint:
                Dot(g, 13, 19, Blue, 5f);
                using (var font = new Font("Segoe UI", 10f, FontStyle.Bold, GraphicsUnit.Pixel))
                using (var brush = new SolidBrush(Blue))
                    g.DrawString("A", font, brush, 18, 2);
                break;
            case ActiveTool.Line:
                g.DrawLine(pen, 1, 27, 31, 5);
                Dot(g, 9, 21, Blue);
                Dot(g, 23, 11, Blue);
                break;
            case ActiveTool.Segment:
                g.DrawLine(pen, 6, 25, 26, 7);
                Dot(g, 6, 25, Blue);
                Dot(g, 26, 7, Blue);
                break;
            case ActiveTool.Ray:
                using (var rayPen = new Pen(Ink, 2f) { CustomEndCap = new AdjustableArrowCap(3.5f, 4f, true) })
                    g.DrawLine(rayPen, 5, 26, 29, 6);
                Dot(g, 5, 26, Blue);
                Dot(g, 16, 17, Blue);
                break;
            case ActiveTool.Circle:
                g.FillEllipse(fill, 4, 4, 24, 24);
                g.DrawEllipse(pen, 4, 4, 24, 24);
                Dot(g, 16, 16, Blue);
                Dot(g, 24.5f, 7.5f, Blue);
                break;
            case ActiveTool.CircleRadius:
                g.FillEllipse(fill, 4, 4, 24, 24);
                g.DrawEllipse(pen, 4, 4, 24, 24);
                using (var dash = new Pen(Ink, 1.5f) { DashStyle = DashStyle.Dash })
                    g.DrawLine(dash, 16, 16, 28, 16);
                Dot(g, 16, 16, Blue);
                using (var font = new Font("Segoe UI", 10f, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.Pixel))
                using (var brush = new SolidBrush(Ink))
                    g.DrawString("r", font, brush, 18, 3);
                break;
            case ActiveTool.Polygon:
                PolygonIcon(g, pen, fill, new PointF[] { new(16, 3), new(29, 12.5f), new(24, 28), new(8, 28), new(3, 12.5f) });
                break;
            case ActiveTool.Triangle:
                PolygonIcon(g, pen, fill, new PointF[] { new(16, 4), new(29, 27), new(3, 27) });
                break;
            case ActiveTool.Rectangle:
                PolygonIcon(g, pen, fill, new PointF[] { new(4, 8), new(28, 8), new(28, 25), new(4, 25) });
                break;
            case ActiveTool.Midpoint:
                g.DrawLine(pen, 4, 25, 28, 7);
                Dot(g, 4, 25, Gray, 2.5f);
                Dot(g, 28, 7, Gray, 2.5f);
                Dot(g, 16, 16, Blue, 4f);
                break;
            case ActiveTool.Intersect:
                g.DrawLine(pen, 2, 8, 30, 24);
                g.DrawLine(pen, 2, 24, 30, 8);
                Dot(g, 16, 16, Blue, 4f);
                break;
        }
        return bmp;
    }

    private static void PolygonIcon(Graphics g, Pen pen, Brush fill, PointF[] pts)
    {
        g.FillPolygon(fill, pts);
        g.DrawPolygon(pen, pts);
        foreach (var p in pts) Dot(g, p.X, p.Y, Blue, 2.5f);
    }

    private static void Dot(Graphics g, float x, float y, Color color, float r = 3.2f)
    {
        using var brush = new SolidBrush(color);
        g.FillEllipse(brush, x - r, y - r, 2 * r, 2 * r);
        using var border = new Pen(Color.FromArgb(30, 30, 30), 0.8f);
        g.DrawEllipse(border, x - r, y - r, 2 * r, 2 * r);
    }
}
