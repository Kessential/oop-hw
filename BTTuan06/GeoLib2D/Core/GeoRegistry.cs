namespace GeoLib2D.Core;

/// <summary>
/// Câu 3b, 3c, 3d: Bộ đếm định danh tĩnh và sinh nhãn tự động theo chuẩn GeoGebra.
/// </summary>
public static class GeoRegistry
{
    // Bảng chữ cái nhãn kiểu GeoGebra (readonly: không thể gán lại tham chiếu)
    private static readonly string LineLetters = "fghijklmnpqrstuvwxyz";
    private static readonly string CircleLetters = "cdeklmnpqrs";

    private static int _nextId = 1;
    private static int _pointCount = 0;
    private static int _lineCount = 0;
    private static int _circleCount = 0;
    private static int _polygonCount = 0;

    // Thuộc tính tĩnh chỉ đọc
    public static int TotalObjects => _nextId - 1;
    public static int PointCount => _pointCount;
    public static int LineCount => _lineCount;
    public static int CircleCount => _circleCount;
    public static int PolygonCount => _polygonCount;

    public static int NextId() => _nextId++;

    /// <summary>"A".."Z", sau đó "A_1".."Z_1", "A_2"...</summary>
    public static string NextPointLabel()
    {
        int idx = _pointCount++;
        char letter = (char)('A' + (idx % 26));
        return idx < 26 ? letter.ToString() : $"{letter}_{idx / 26}";
    }

    /// <summary>"f", "g", "h"... (dùng chung cho đường thẳng, tia, đoạn thẳng).</summary>
    public static string NextLineLabel() => MakeLabel(LineLetters, _lineCount++);

    /// <summary>"c", "d", "e"...</summary>
    public static string NextCircleLabel() => MakeLabel(CircleLetters, _circleCount++);

    /// <summary>"poly1", "poly2"...</summary>
    public static string NextPolygonLabel() => $"poly{++_polygonCount}";

    private static string MakeLabel(string letters, int idx)
    {
        char letter = letters[idx % letters.Length];
        int round = idx / letters.Length;
        return round == 0 ? letter.ToString() : $"{letter}_{round}";
    }

    /// <summary>Đưa toàn bộ bộ đếm về trạng thái ban đầu (File -> New).</summary>
    public static void Reset()
    {
        _nextId = 1;
        _pointCount = 0;
        _lineCount = 0;
        _circleCount = 0;
        _polygonCount = 0;
    }
}
