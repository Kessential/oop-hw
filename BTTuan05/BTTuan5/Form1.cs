namespace BTTuan5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lblTieuDe_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTenDangNhap_Click(object sender, EventArgs e)
        {

        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            if (txtTenDangNhap.Text != "Admin")
            {
                MessageBox.Show("Sai tên đăng nhập", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDangNhap.Focus();
                return;
            }

            if (txtMatKhau.Text != "Admin")
            {
                MessageBox.Show("Sai mật khẩu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
                return;
            }

            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát không?", "Thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
