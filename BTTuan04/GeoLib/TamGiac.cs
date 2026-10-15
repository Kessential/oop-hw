namespace GeoLib
{
    /// <summary>
    /// Tam giác ABC trong không gian Oxyz; mỗi đỉnh là một Vector3D (véc-tơ vị trí).
    /// </summary>
    public class TamGiac
    {
        /// <summary>Ba đỉnh A, B, C (chỉ số 0..2).</summary>
        public Vector3D[] Items { get; }

        // ===== Constructors =====

        /// <summary>Tam giác mặc định A(0;0;0), B(1;0;0), C(0;1;0).</summary>
        public TamGiac() : this(new Vector3D(0, 0, 0), new Vector3D(1, 0, 0), new Vector3D(0, 1, 0)) { }

        /// <summary>Tam giác từ ba véc-tơ vị trí (sao chép giá trị).</summary>
        public TamGiac(Vector3D a, Vector3D b, Vector3D c)
        {
            Items = new[] { new Vector3D(a), new Vector3D(b), new Vector3D(c) };
        }

        /// <summary>Tam giác từ ba điểm.</summary>
        public TamGiac(Point3D a, Point3D b, Point3D c)
            : this(new Vector3D(a.X, a.Y, a.Z), new Vector3D(b.X, b.Y, b.Z), new Vector3D(c.X, c.Y, c.Z)) { }

        // ===== Chỉ mục =====

        /// <summary>Đỉnh thứ i (i = 1, 2, 3): ABC[1] = A, ABC[2] = B, ABC[3] = C.</summary>
        public Vector3D this[int i]
        {
            get
            {
                CheckIndex(i);
                return Items[i - 1];
            }
        }

        /// <summary>Cạnh nối đỉnh i và đỉnh j, là véc-tơ từ đỉnh i đến đỉnh j: ABC[1, 2] = AB.</summary>
        public Vector3D this[int i, int j]
        {
            get
            {
                CheckIndex(i);
                CheckIndex(j);
                if (i == j)
                    throw new ArgumentException("Hai đỉnh của một cạnh phải khác nhau.");
                return Items[j - 1] - Items[i - 1];
            }
        }

        // ===== Tính toán =====

        /// <summary>Ba đỉnh có tạo thành tam giác thật sự (không thẳng hàng) hay không.</summary>
        public bool IsValid => !(this[1, 2] / this[1, 3]).IsZero;

        /// <summary>Chu vi = AB + BC + CA.</summary>
        public double GetPerimeter() => this[1, 2].Length + this[2, 3].Length + this[3, 1].Length;

        /// <summary>Diện tích = 1/2 * |[AB, AC]|.</summary>
        public double GetArea() => (this[1, 2] / this[1, 3]).Length / 2;

        public override string ToString() => $"A{Items[0]}, B{Items[1]}, C{Items[2]}";

        private static void CheckIndex(int i)
        {
            if (i < 1 || i > 3)
                throw new IndexOutOfRangeException("Chỉ số đỉnh phải là 1, 2 hoặc 3.");
        }
    }
}
