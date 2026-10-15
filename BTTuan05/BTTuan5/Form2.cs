namespace BTTuan5
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnLayThongTin_Click(object sender, EventArgs e)
        {
            // lay danh sach free time
            string freeTime = "";
            if (chkXemFilm.Checked) freeTime += chkXemFilm.Text + ", ";
            if (chkChoi.Checked) freeTime += chkChoi.Text + ", ";
            if (chkNgheNhac.Checked) freeTime += chkNgheNhac.Text + ", ";

            // lay danh sach cac mon the thao
            string theThao = "";
            foreach (string a in lstTheThao.SelectedItems)
            {
                theThao += a + " - ";
            }

            // lay danh sach cac chuyen thich doc
            string doc = "";
            foreach (string b in clbDoc.CheckedItems)
            {
                doc += b + " - ";
            }

            string gioiTinh = radNam.Checked ? radNam.Text : radNu.Text;

            rtbThongTin.Text =
                "Họ tên: " + txtHoTen.Text + "\n" +
                "Quê quán: " + cboQueQuan.Text + "\n" +
                "Ngày sinh: " + dtpNgaySinh.Value.ToString("dd/MM/yyyy") + "\n" +
                "Giới tính: " + gioiTinh + "\n" +
                "Free time: " + freeTime + "\n" +
                "Thể thao: " + theThao + "\n" +
                "Đọc: " + doc;
        }

        private void btnDongForm_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Form1 dang an, nen dong Form2 thi tat luon chuong trinh
            Application.Exit();
        }
    }
}
