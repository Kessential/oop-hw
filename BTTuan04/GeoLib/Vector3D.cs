namespace GeoLib
{
    /// <summary>
    /// Véc-tơ 3 chiều trong không gian Oxyz.
    /// </summary>
    public class Vector3D
    {
        public const double Epsilon = 1e-9;

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        // ===== Constructors =====

        /// <summary>Véc-tơ không (0; 0; 0).</summary>
        public Vector3D() : this(0, 0, 0) { }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>Hàm khởi tạo sao chép.</summary>
        public Vector3D(Vector3D other) : this(other.X, other.Y, other.Z) { }

        /// <summary>Véc-tơ AB = B - A.</summary>
        public Vector3D(Point3D a, Point3D b) : this(b.X - a.X, b.Y - a.Y, b.Z - a.Z) { }

        // ===== Thuộc tính & phương thức =====

        /// <summary>Độ dài véc-tơ |v| = sqrt(X² + Y² + Z²).</summary>
        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        public double GetLength() => Length;

        public bool IsZero => Length < Epsilon;

        /// <summary>Thuộc tính chỉ đọc: véc-tơ đơn vị cùng hướng (v / |v|).</summary>
        public Vector3D Unit => Normalize(this);

        /// <summary>
        /// Thuộc tính chỉ ghi: nhân véc-tơ hiện tại với hệ số k (thay đổi trực tiếp X, Y, Z).
        /// </summary>
        public double Scale
        {
            set
            {
                X *= value;
                Y *= value;
                Z *= value;
            }
        }

        /// <summary>Thuộc tính tĩnh: véc-tơ không (0; 0; 0).</summary>
        public static Vector3D Zero => new Vector3D();

        /// <summary>Phương thức tĩnh: chuẩn hoá véc-tơ về độ dài 1.</summary>
        public static Vector3D Normalize(Vector3D v)
        {
            if (v.IsZero)
                throw new ArgumentException("Không thể chuẩn hoá véc-tơ không.");
            return v * (1.0 / v.Length);
        }

        /// <summary>
        /// Góc giữa hai véc-tơ, trả về theo radian và độ.
        /// </summary>
        public static (double Radian, double Degree) AngleBetween(Vector3D u, Vector3D v)
        {
            if (u.IsZero || v.IsZero)
                throw new ArgumentException("Không xác định được góc với véc-tơ không.");

            double cos = (u * v) / (u.Length * v.Length);
            cos = Math.Clamp(cos, -1.0, 1.0);
            double rad = Math.Acos(cos);
            return (rad, rad * 180.0 / Math.PI);
        }

        public (double Radian, double Degree) AngleBetween(Vector3D other) => AngleBetween(this, other);

        /// <summary>
        /// Véc-tơ phân giác c = a|b| + b|a|.
        /// </summary>
        public static Vector3D GetBisector(Vector3D a, Vector3D b)
        {
            if (a.IsZero || b.IsZero)
                throw new ArgumentException("Véc-tơ a, b phải khác véc-tơ không.");

            Vector3D c = a * b.Length + b * a.Length;
            if (c.IsZero)
                throw new ArgumentException("Hai véc-tơ đối nhau, không có véc-tơ phân giác.");
            return c;
        }

        public Vector3D GetBisector(Vector3D other) => GetBisector(this, other);

        /// <summary>Kiểm tra hai véc-tơ cùng phương: [u, v] = 0.</summary>
        public static bool IsCollinear(Vector3D u, Vector3D v) => (u / v).IsZero;

        /// <summary>Tích hỗn tạp [u, v] * w.</summary>
        public static double MixedProduct(Vector3D u, Vector3D v, Vector3D w) => (u / v) * w;

        // ===== Nạp chồng toán tử =====

        public static Vector3D operator +(Vector3D u, Vector3D v)
            => new Vector3D(u.X + v.X, u.Y + v.Y, u.Z + v.Z);

        public static Vector3D operator -(Vector3D u, Vector3D v)
            => new Vector3D(u.X - v.X, u.Y - v.Y, u.Z - v.Z);

        public static Vector3D operator -(Vector3D u)
            => new Vector3D(-u.X, -u.Y, -u.Z);

        /// <summary>Tích vô hướng.</summary>
        public static double operator *(Vector3D u, Vector3D v)
            => u.X * v.X + u.Y * v.Y + u.Z * v.Z;

        public static Vector3D operator *(Vector3D u, double k)
            => new Vector3D(k * u.X, k * u.Y, k * u.Z);

        public static Vector3D operator *(double k, Vector3D u) => u * k;

        /// <summary>Tích có hướng [u, v].</summary>
        public static Vector3D operator /(Vector3D u, Vector3D v)
            => new Vector3D(
                Det2(u.Y, u.Z, v.Y, v.Z),
                -Det2(u.X, u.Z, v.X, v.Z),
                Det2(u.X, u.Y, v.X, v.Y));

        /// <summary>Định thức cấp hai |a b; c d| = ad - bc.</summary>
        public static double Det2(double a, double b, double c, double d) => a * d - b * c;

        public override string ToString() => $"({Fmt(X)}; {Fmt(Y)}; {Fmt(Z)})";

        private static string Fmt(double v)
        {
            double r = Math.Round(v, 4);
            return (r == 0 ? 0 : r).ToString();
        }
    }
}
