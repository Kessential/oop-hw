namespace BTTuan5
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTieuDe = new Label();
            lblTenDangNhap = new Label();
            lblMatKhau = new Label();
            txtTenDangNhap = new TextBox();
            txtMatKhau = new TextBox();
            btnDongY = new Button();
            btnThoat = new Button();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTieuDe.Location = new Point(202, 36);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(367, 50);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Thông tin đăng nhập";
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTenDangNhap.Location = new Point(36, 154);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(166, 31);
            lblTenDangNhap.TabIndex = 0;
            lblTenDangNhap.Text = "Tên đăng nhập";
            lblTenDangNhap.Click += lblTenDangNhap_Click;
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMatKhau.Location = new Point(45, 214);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(110, 31);
            lblMatKhau.TabIndex = 0;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Location = new Point(271, 160);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(284, 27);
            txtTenDangNhap.TabIndex = 1;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(271, 214);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(284, 27);
            txtMatKhau.TabIndex = 1;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // btnDongY
            // 
            btnDongY.Location = new Point(205, 313);
            btnDongY.Name = "btnDongY";
            btnDongY.Size = new Size(94, 29);
            btnDongY.TabIndex = 2;
            btnDongY.Text = "Đồng ý";
            btnDongY.UseVisualStyleBackColor = true;
            btnDongY.Click += btnDongY_Click;
            // 
            // btnThoat
            // 
            btnThoat.Location = new Point(397, 313);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(94, 29);
            btnThoat.TabIndex = 2;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = true;
            btnThoat.Click += btnThoat_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnThoat);
            Controls.Add(btnDongY);
            Controls.Add(txtMatKhau);
            Controls.Add(txtTenDangNhap);
            Controls.Add(lblMatKhau);
            Controls.Add(lblTenDangNhap);
            Controls.Add(lblTieuDe);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTieuDe;
        private Label lblTenDangNhap;
        private Label lblMatKhau;
        private TextBox txtTenDangNhap;
        private TextBox txtMatKhau;
        private Button btnDongY;
        private Button btnThoat;
    }
}
