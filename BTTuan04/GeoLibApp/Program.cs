using System.Text;
using GeoLib;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
Console.Title = "GeoLib - Hình học không gian Oxyz";

string[] items =
{
    "Tích có hướng [a, b]",
    "Ba điểm thẳng hàng",
    "Bốn điểm không đồng phẳng",
    "Hình hộp & tứ diện ABCD",
    "Khảo sát tam giác ABC",
    "Kiểm thử lớp Vector3D",
    "Kiểm thử lớp TamGiac",
};

while (true)
{
    Ui.Clear();
    Ui.Banner();
    Ui.Menu(items);

    Ui.Write("  ➤ Nhập lựa chọn (0-7): ",ConsoleColor.Yellow);
    string? choice = Console.ReadLine()?.Trim();
    if (choice == null || choice == "0") break;

    Ui.Clear();
    switch (choice)
    {
        case "1": Problems.Problem1(); break;
        case "2": Problems.Problem2(); break;
        case "3": Problems.Problem3(); break;
        case "4": Problems.Problem4(); break;
        case "5": Problems.Problem5(); break;
        case "6": Problems.VectorTests(); break;
        case "7": Problems.TamGiacTests(); break;
        default: Ui.Line("  ✖ Lựa chọn không hợp lệ, vui lòng nhập số từ 0 đến 7.", ConsoleColor.Red); break;
    }
    Ui.Pause();
}

Ui.Line();
Ui.Line("  ★ Cảm ơn bạn đã sử dụng GeoLib. Tạm biệt! ★", ConsoleColor.Magenta);

static class Ui
{
    const int Width = 56;

    public static void Write(string s, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(s);
        Console.ResetColor();
    }

    public static void Line(string s = "", ConsoleColor color = ConsoleColor.Gray) => Write(s + Environment.NewLine, color);

    public static void Clear()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
        else Console.WriteLine();
    }

    public static void Pause()
    {
        Line();
        Write("  ⏎ Nhấn Enter để quay lại menu...", ConsoleColor.DarkGray);
        Console.ReadLine();
        Line();
    }

    public static void Banner()
    {
        Line("  ╔" + new string('═', Width) + "╗", ConsoleColor.Cyan);
        BoxRow("GEOLIB  ◆  VECTOR3D", ConsoleColor.White, center: true);
        BoxRow("Giải toán hình học không gian Oxyz", ConsoleColor.DarkCyan, center: true);
        Line("  ╚" + new string('═', Width) + "╝", ConsoleColor.Cyan);
        Line();
    }

    public static void Menu(string[] items)
    {
        Line("  ┌" + new string('─', Width) + "┐", ConsoleColor.DarkGray);
        for (int i = 0; i < items.Length; i++)
        {
            string tag = i < 5 ? $"Bài {i + 1}" : "Test ";
            Write("  │ ", ConsoleColor.DarkGray);
            Write($"[{i + 1}]", ConsoleColor.Yellow);
            Write($" {tag} ", ConsoleColor.DarkCyan);
            Write("• ", ConsoleColor.DarkGray);
            Write(items[i].PadRight(Width - 13), ConsoleColor.White);
            Line("│", ConsoleColor.DarkGray);
        }
        Line("  ├" + new string('─', Width) + "┤", ConsoleColor.DarkGray);
        Write("  │ ", ConsoleColor.DarkGray);
        Write("[0]", ConsoleColor.Red);
        Write(" Thoát chương trình".PadRight(Width - 4), ConsoleColor.Gray);
        Line("│", ConsoleColor.DarkGray);
        Line("  └" + new string('─', Width) + "┘", ConsoleColor.DarkGray);
        Line();
    }

    public static void Header(string s)
    {
        Line("  ╭" + new string('─', Width) + "╮", ConsoleColor.Green);
        BoxRow(s, ConsoleColor.White, center: true, border: '│', borderColor: ConsoleColor.Green);
        Line("  ╰" + new string('─', Width) + "╯", ConsoleColor.Green);
        Line();
    }

    /// <summary>In một dòng kết quả, tô màu kết luận (=>) và các ý (a), b), ...).</summary>
    public static void Out(string s)
    {
        if (s.Contains("=>"))
            Line("  " + s, ConsoleColor.Green);
        else if (s.StartsWith("A(") || s.StartsWith("a =") || s.StartsWith("u ="))
            Line("  " + s, ConsoleColor.Cyan);
        else
            Line("  " + s);
    }

    static void BoxRow(string s, ConsoleColor color, bool center, char border = '║', ConsoleColor borderColor = ConsoleColor.Cyan)
    {
        int pad = Math.Max(0, Width - s.Length);
        string text = center ? new string(' ', pad / 2) + s + new string(' ', pad - pad / 2) : s.PadRight(Width);
        Write($"  {border}", borderColor);
        Write(text, color);
        Line(border.ToString(), borderColor);
    }
}

static class Problems
{
    const double Eps = Vector3D.Epsilon;

    static string F(double v)
    {
        double r = Math.Round(v, 4);
        return (r == 0 ? 0 : r).ToString();
    }

    static void Title(string s) => Ui.Header(s);

    public static void Problem1()
    {
        Title("BÀI TOÁN 1: KHỞI ĐỘNG TÍCH CÓ HƯỚNG");
        var a = new Vector3D(2, -1, 3);
        var b = new Vector3D(1, 2, 4);
        Ui.Out($"a = {a}, b = {b}");

        // a) Dùng toán tử /
        Vector3D c = a / b;
        Ui.Out($"a) [a, b] = a / b = {c}");

        // b) Chi tiết các định thức
        double d1 = Vector3D.Det2(a.Y, a.Z, b.Y, b.Z);
        double d2 = Vector3D.Det2(a.X, a.Z, b.X, b.Z);
        double d3 = Vector3D.Det2(a.X, a.Y, b.X, b.Y);
        Ui.Out("b) Chi tiết:");
        Ui.Out($"   |{F(a.Y)} {F(a.Z)}; {F(b.Y)} {F(b.Z)}| = ({F(a.Y)})*({F(b.Z)}) - ({F(a.Z)})*({F(b.Y)}) = {F(d1)}");
        Ui.Out($"  -|{F(a.X)} {F(a.Z)}; {F(b.X)} {F(b.Z)}| = -[({F(a.X)})*({F(b.Z)}) - ({F(a.Z)})*({F(b.X)})] = {F(-d2)}");
        Ui.Out($"   |{F(a.X)} {F(a.Y)}; {F(b.X)} {F(b.Y)}| = ({F(a.X)})*({F(b.Y)}) - ({F(a.Y)})*({F(b.X)}) = {F(d3)}");
        Ui.Out($"   => [a, b] = ({F(d1)}; {F(-d2)}; {F(d3)})");

        // c) Tính trực giao
        double ca = c * a, cb = c * b;
        Ui.Out($"c) [a, b] * a = {F(ca)} -> {(Math.Abs(ca) < Eps ? "trực giao với a" : "KHÔNG trực giao")}");
        Ui.Out($"   [a, b] * b = {F(cb)} -> {(Math.Abs(cb) < Eps ? "trực giao với b" : "KHÔNG trực giao")}");
    }

    public static void Problem2()
    {
        Title("BÀI TOÁN 2: BA ĐIỂM THẲNG HÀNG");
        var A = new Point3D(1, 3, 1);
        var B = new Point3D(0, 1, 2);
        var C = new Point3D(3, 7, -1);
        Ui.Out($"A{A}, B{B}, C{C}");

        var AB = new Vector3D(A, B);
        var AC = new Vector3D(A, C);
        Ui.Out($"a) AB = {AB}, AC = {AC}");

        Vector3D cross = AB / AC;
        Ui.Out($"b) [AB, AC] = {cross}");

        if (cross.IsZero)
        {
            // Tìm k sao cho AC = k * AB
            double k = (AC * AB) / (AB * AB);
            Ui.Out($"c) [AB, AC] = 0 => AB, AC cùng phương (AC = {F(k)}AB)");
            Ui.Out("   => Ba điểm A, B, C THẲNG HÀNG.");
        }
        else
        {
            Ui.Out("c) [AB, AC] != 0 => Ba điểm A, B, C KHÔNG thẳng hàng.");
        }
    }

    public static void Problem3()
    {
        Title("BÀI TOÁN 3: BỐN ĐIỂM KHÔNG ĐỒNG PHẲNG");
        var A = new Point3D(1, -1, 1);
        var B = new Point3D(1, 3, 1);
        var C = new Point3D(4, 3, 2);
        var D = new Point3D(4, 1, 3);
        Ui.Out($"A{A}, B{B}, C{C}, D{D}");

        var AB = new Vector3D(A, B);
        var AC = new Vector3D(A, C);
        var AD = new Vector3D(A, D);
        Ui.Out($"a) AB = {AB}, AC = {AC}, AD = {AD}");

        Vector3D cross = AB / AC;
        double mixed = cross * AD;
        Ui.Out($"b) [AB, AC] = {cross}");
        Ui.Out($"   [AB, AC] * AD = {F(mixed)}");

        if (Math.Abs(mixed) > Eps)
            Ui.Out($"c) Tích hỗn tạp = {F(mixed)} != 0 => A, B, C, D KHÔNG đồng phẳng, tạo thành tứ diện.");
        else
            Ui.Out("c) Tích hỗn tạp = 0 => A, B, C, D đồng phẳng.");
    }

    public static void Problem4()
    {
        Title("BÀI TOÁN 4: HÌNH HỘP VÀ TỨ DIỆN ABCD");
        var A = new Point3D(1, 2, 1);
        var B = new Point3D(2, -1, 4);
        var C = new Point3D(5, 2, -3);
        var D = new Point3D(4, 5, -6);
        Ui.Out($"A{A}, B{B}, C{C}, D{D}");

        var AB = new Vector3D(A, B);
        var AC = new Vector3D(A, C);
        var AD = new Vector3D(A, D);
        Vector3D cross = AB / AC;
        double mixed = cross * AD;
        Ui.Out($"AB = {AB}, AC = {AC}, AD = {AD}");
        Ui.Out($"[AB, AC] = {cross}, [AB, AC] * AD = {F(mixed)}");

        double vBox = Math.Abs(mixed);
        Ui.Out($"4a) V hình hộp = |[AB, AC] * AD| = {F(vBox)}");
        Ui.Out($"4b) V tứ diện ABCD = 1/6 * V hộp = {F(vBox / 6)}");
        Ui.Out($"4c) S tam giác ABC = 1/2 * |[AB, AC]| = {F(cross.Length / 2)}");
        if (Math.Abs(mixed) > Eps)
            Ui.Out($"4d) [AB, AC] * AD = {F(mixed)} != 0 => AB, AC, AD không đồng phẳng => A, B, C, D là 4 đỉnh của một tứ diện.");
        else
            Ui.Out("4d) [AB, AC] * AD = 0 => A, B, C, D đồng phẳng, KHÔNG tạo thành tứ diện.");
    }

    public static void Problem5()
    {
        Title("BÀI TOÁN 5: KHẢO SÁT TAM GIÁC ABC");
        var A = new Point3D(1, -1, 2);
        var B = new Point3D(5, -6, 2);
        var C = new Point3D(1, 3, -1);
        Ui.Out($"A{A}, B{B}, C{C}");

        var AB = new Vector3D(A, B);
        var AC = new Vector3D(A, C);
        Vector3D cross = AB / AC;
        Ui.Out($"AB = {AB}, AC = {AC}");

        // (5) Tam giác
        Ui.Out($"(5) [AB, AC] = {cross}");
        if (cross.IsZero)
        {
            Ui.Out("    [AB, AC] = 0 => A, B, C thẳng hàng, KHÔNG tạo thành tam giác.");
            return;
        }
        Ui.Out("    [AB, AC] != 0 => A, B, C không thẳng hàng => là 3 đỉnh của một tam giác.");

        // (6) Không vuông
        var BA = new Vector3D(B, A);
        var BC = new Vector3D(B, C);
        var CA = new Vector3D(C, A);
        var CB = new Vector3D(C, B);
        double dA = AB * AC, dB = BA * BC, dC = CA * CB;
        Ui.Out($"(6) AB * AC = {F(dA)} (góc A = {F(AB.AngleBetween(AC).Degree)}°)");
        Ui.Out($"    BA * BC = {F(dB)} (góc B = {F(BA.AngleBetween(BC).Degree)}°)");
        Ui.Out($"    CA * CB = {F(dC)} (góc C = {F(CA.AngleBetween(CB).Degree)}°)");
        bool right = Math.Abs(dA) < Eps || Math.Abs(dB) < Eps || Math.Abs(dC) < Eps;
        Ui.Out(right
            ? "    => Tam giác ABC là tam giác vuông."
            : "    => Cả ba tích vô hướng đều khác 0 => Tam giác ABC KHÔNG vuông.");

        // (7) Diện tích
        Ui.Out($"(7) S ABC = 1/2 * |[AB, AC]| = 1/2 * {F(cross.Length)} = {F(cross.Length / 2)}");
    }

    public static void TamGiacTests()
    {
        Title("KIỂM THỬ LỚP TamGiac");

        var def = new TamGiac();
        Ui.Out($"TamGiac() = {def}");

        // Hàm tạo từ ba Vector3D
        var ABC = new TamGiac(new Vector3D(0, 0, 0), new Vector3D(3, 0, 0), new Vector3D(0, 4, 0));
        Ui.Out($"TamGiac(Vector3D x3) = {ABC}");

        // Hàm tạo từ ba Point3D
        var DEF = new TamGiac(new Point3D(1, -1, 2), new Point3D(5, -6, 2), new Point3D(1, 3, -1));
        Ui.Out($"TamGiac(Point3D x3) = {DEF}");

        // Chỉ mục đỉnh
        Ui.Out($"ABC[1] = {ABC[1]}, ABC[2] = {ABC[2]}, ABC[3] = {ABC[3]}");

        // Chỉ mục cạnh
        Ui.Out($"ABC[1,2] = {ABC[1, 2]}, |AB| = {F(ABC[1, 2].Length)}");
        Ui.Out($"ABC[2,3] = {ABC[2, 3]}, |BC| = {F(ABC[2, 3].Length)}");
        Ui.Out($"ABC[3,1] = {ABC[3, 1]}, |CA| = {F(ABC[3, 1].Length)}");

        Ui.Out($"ABC: chu vi = {F(ABC.GetPerimeter())}, diện tích = {F(ABC.GetArea())}");
        Ui.Out($"DEF: chu vi = {F(DEF.GetPerimeter())}, diện tích = {F(DEF.GetArea())}");

        try
        {
            _ = ABC[4];
        }
        catch (IndexOutOfRangeException ex)
        {
            Ui.Out($"ABC[4] -> Lỗi: {ex.Message}");
        }
    }

    public static void VectorTests()
    {
        Title("KIỂM THỬ LỚP Vector3D");
        var u = new Vector3D(1, 2, 3);
        var v = new Vector3D(4, -5, 6);
        var copy = new Vector3D(u);
        Ui.Out($"Vector3D() = {new Vector3D()}");
        Ui.Out($"u = {u}, v = {v}, bản sao của u = {copy}");
        Ui.Out($"u + v = {u + v}");
        Ui.Out($"u - v = {u - v}");
        Ui.Out($"-u = {-u}");
        Ui.Out($"u * v (vô hướng) = {F(u * v)}");
        Ui.Out($"u * 2 = {u * 2}, 2 * u = {2 * u}");
        Ui.Out($"u / v (có hướng) = {u / v}");
        Ui.Out($"u.Length = {F(u.Length)}, u.GetLength() = {F(u.GetLength())}");

        // Thuộc tính chỉ đọc, chỉ ghi, tĩnh và phương thức tĩnh
        Ui.Out($"u.Unit (chỉ đọc) = {u.Unit}, |u.Unit| = {F(u.Unit.Length)}");
        var w = new Vector3D(u);
        w.Scale = 3; // chỉ ghi
        Ui.Out($"w = bản sao của u; w.Scale = 3 (chỉ ghi) => w = {w}");
        Ui.Out($"Vector3D.Zero (thuộc tính tĩnh) = {Vector3D.Zero}");
        Ui.Out($"Vector3D.Normalize(v) (phương thức tĩnh) = {Vector3D.Normalize(v)}");

        var (rad, deg) = Vector3D.AngleBetween(u, v);
        Ui.Out($"Góc(u, v) = {F(rad)} rad = {F(deg)}°");

        try
        {
            Vector3D.AngleBetween(u, new Vector3D());
        }
        catch (ArgumentException ex)
        {
            Ui.Out($"Góc(u, 0) -> Lỗi: {ex.Message}");
        }

        // Véc-tơ phân giác
        var a = new Vector3D(1, 0, 0);
        var b = new Vector3D(1, 1, 1);
        Vector3D c = Vector3D.GetBisector(a, b);
        double angAC = a.AngleBetween(c).Degree;
        double angBC = b.AngleBetween(c).Degree;
        Ui.Out($"Phân giác của a = {a}, b = {b}: c = a|b| + b|a| = {c}");
        Ui.Out($"  Góc(a, c) = {F(angAC)}°, Góc(b, c) = {F(angBC)}°, Góc(a, b) = {F(a.AngleBetween(b).Degree)}°");
        Ui.Out(Math.Abs(angAC - angBC) < 1e-6
            ? "  => Góc(a, c) = Góc(b, c): c là véc-tơ phân giác. ĐÚNG"
            : "  => SAI");
    }
}
