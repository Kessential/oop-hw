using System;
using System.Text;

namespace ThucHanhTuan01
{
    public class Node
    {
        public int Data;
        public Node? Next;

        public Node(int data)
        {
            Data = data;
            Next = null;
        }
    }

    // Bai 07: Danh sach lien ket don - CRUD & Tim kiem (Bien the K = 0)
    // De bai: Them phan tu vao cuoi danh sach, xoa tat ca cac nut co gia tri cho truoc,
    // tim gia tri lon nhat va tim so nguyen to dau tien trong danh sach.
    public static class Bai07
    {
        public static void PushBack(ref Node? head, int val)
        {
            if (head == null)
            {
                head = new Node(val);
                return;
            }

            Node cur = head;
            while (cur.Next != null)
                cur = cur.Next;
            cur.Next = new Node(val);
        }

        public static void XoaTatCaGiaTri(ref Node? head, int x)
        {
            while (head != null && head.Data == x)
                head = head.Next;

            if (head == null)
                return;

            Node cur = head;
            while (cur.Next != null)
            {
                if (cur.Next.Data == x)
                    cur.Next = cur.Next.Next;
                else
                    cur = cur.Next;
            }
        }

        public static int? TimMax(Node? head)
        {
            if (head == null)
                return null;

            int max = head.Data;
            Node? cur = head.Next;
            while (cur != null)
            {
                if (cur.Data > max)
                    max = cur.Data;
                cur = cur.Next;
            }
            return max;
        }

        private static bool IsPrime(int n)
        {
            if (n < 2)
                return false;
            for (int i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }

        public static int? TimSntDauTien(Node? head)
        {
            Node? cur = head;
            while (cur != null)
            {
                if (IsPrime(cur.Data))
                    return cur.Data;
                cur = cur.Next;
            }
            return null;
        }

        public static void InDanhSach(Node? head)
        {
            if (head == null)
            {
                Console.WriteLine("(rong)");
                return;
            }

            StringBuilder sb = new StringBuilder();
            Node? cur = head;
            while (cur != null)
            {
                sb.Append(cur.Data);
                if (cur.Next != null)
                    sb.Append(" -> ");
                cur = cur.Next;
            }
            Console.WriteLine(sb.ToString());
        }

        public static void Chay()
        {
            Console.WriteLine("=== BAI 07: DANH SACH LIEN KET DON - CRUD & TIM KIEM (K = 0) ===");

            Node? head = null;
            int[] giaTri = { 10, 15, 7, 20, 15 };
            foreach (int v in giaTri)
                PushBack(ref head, v);

            Console.Write("Danh sach ban dau: ");
            InDanhSach(head);

            Console.WriteLine($"Gia tri lon nhat trong danh sach: {TimMax(head)}");
            Console.WriteLine($"So nguyen to dau tien trong danh sach: {TimSntDauTien(head)}");

            XoaTatCaGiaTri(ref head, 15);
            Console.Write("Danh sach sau khi xoa tat ca gia tri 15: ");
            InDanhSach(head);
        }
    }
}
