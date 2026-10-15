namespace BTTuan5
{
    partial class Form2
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
            grpThongTinChung = new GroupBox();
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            lblNgaySinh = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblQueQuan = new Label();
            cboQueQuan = new ComboBox();
            lblGioiTinh = new Label();
            radNam = new RadioButton();
            radNu = new RadioButton();
            grpSoThich = new GroupBox();
            lblFreeTime = new Label();
            chkXemFilm = new CheckBox();
            chkChoi = new CheckBox();
            chkNgheNhac = new CheckBox();
            lblTheThao = new Label();
            lstTheThao = new ListBox();
            lblDoc = new Label();
            clbDoc = new CheckedListBox();
            btnLayThongTin = new Button();
            btnDongForm = new Button();
            rtbThongTin = new RichTextBox();
            grpThongTinChung.SuspendLayout();
            grpSoThich.SuspendLayout();
            SuspendLayout();
            //
            // lblTieuDe
            //
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTieuDe.ForeColor = Color.Blue;
            lblTieuDe.Location = new Point(190, 12);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(285, 41);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Thông tin sinh viên";
            //
            // grpThongTinChung
            //
            grpThongTinChung.Controls.Add(lblHoTen);
            grpThongTinChung.Controls.Add(txtHoTen);
            grpThongTinChung.Controls.Add(lblNgaySinh);
            grpThongTinChung.Controls.Add(dtpNgaySinh);
            grpThongTinChung.Controls.Add(lblQueQuan);
            grpThongTinChung.Controls.Add(cboQueQuan);
            grpThongTinChung.Controls.Add(lblGioiTinh);
            grpThongTinChung.Controls.Add(radNam);
            grpThongTinChung.Controls.Add(radNu);
            grpThongTinChung.ForeColor = Color.RoyalBlue;
            grpThongTinChung.Location = new Point(20, 65);
            grpThongTinChung.Name = "grpThongTinChung";
            grpThongTinChung.Size = new Size(620, 115);
            grpThongTinChung.TabIndex = 1;
            grpThongTinChung.TabStop = false;
            grpThongTinChung.Text = "Thông tin chung";
            //
            // lblHoTen
            //
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new Point(30, 33);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(54, 20);
            lblHoTen.TabIndex = 0;
            lblHoTen.Text = "Họ tên";
            //
            // txtHoTen
            //
            txtHoTen.ForeColor = SystemColors.WindowText;
            txtHoTen.Location = new Point(130, 30);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(180, 27);
            txtHoTen.TabIndex = 1;
            //
            // lblNgaySinh
            //
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Location = new Point(30, 73);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(74, 20);
            lblNgaySinh.TabIndex = 2;
            lblNgaySinh.Text = "Ngày sinh";
            //
            // dtpNgaySinh
            //
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.Location = new Point(130, 70);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(180, 27);
            dtpNgaySinh.TabIndex = 3;
            //
            // lblQueQuan
            //
            lblQueQuan.AutoSize = true;
            lblQueQuan.Location = new Point(340, 33);
            lblQueQuan.Name = "lblQueQuan";
            lblQueQuan.Size = new Size(73, 20);
            lblQueQuan.TabIndex = 4;
            lblQueQuan.Text = "Quê quán";
            //
            // cboQueQuan
            //
            cboQueQuan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQueQuan.FormattingEnabled = true;
            cboQueQuan.Items.AddRange(new object[] { "Hà Nội", "Hoà Bình", "Hải Phòng", "Đà Nẵng", "Huế", "TP. Hồ Chí Minh", "Cần Thơ" });
            cboQueQuan.Location = new Point(430, 30);
            cboQueQuan.Name = "cboQueQuan";
            cboQueQuan.Size = new Size(170, 28);
            cboQueQuan.TabIndex = 5;
            //
            // lblGioiTinh
            //
            lblGioiTinh.AutoSize = true;
            lblGioiTinh.Location = new Point(340, 73);
            lblGioiTinh.Name = "lblGioiTinh";
            lblGioiTinh.Size = new Size(68, 20);
            lblGioiTinh.TabIndex = 6;
            lblGioiTinh.Text = "Giới tính";
            //
            // radNam
            //
            radNam.AutoSize = true;
            radNam.Checked = true;
            radNam.Location = new Point(430, 71);
            radNam.Name = "radNam";
            radNam.Size = new Size(62, 24);
            radNam.TabIndex = 7;
            radNam.TabStop = true;
            radNam.Text = "Nam";
            radNam.UseVisualStyleBackColor = true;
            //
            // radNu
            //
            radNu.AutoSize = true;
            radNu.Location = new Point(520, 71);
            radNu.Name = "radNu";
            radNu.Size = new Size(50, 24);
            radNu.TabIndex = 8;
            radNu.Text = "Nữ";
            radNu.UseVisualStyleBackColor = true;
            //
            // grpSoThich
            //
            grpSoThich.Controls.Add(lblFreeTime);
            grpSoThich.Controls.Add(chkXemFilm);
            grpSoThich.Controls.Add(chkChoi);
            grpSoThich.Controls.Add(chkNgheNhac);
            grpSoThich.Controls.Add(lblTheThao);
            grpSoThich.Controls.Add(lstTheThao);
            grpSoThich.Controls.Add(lblDoc);
            grpSoThich.Controls.Add(clbDoc);
            grpSoThich.ForeColor = Color.Red;
            grpSoThich.Location = new Point(20, 190);
            grpSoThich.Name = "grpSoThich";
            grpSoThich.Size = new Size(620, 170);
            grpSoThich.TabIndex = 2;
            grpSoThich.TabStop = false;
            grpSoThich.Text = "Sở thích";
            //
            // lblFreeTime
            //
            lblFreeTime.AutoSize = true;
            lblFreeTime.Location = new Point(30, 35);
            lblFreeTime.Name = "lblFreeTime";
            lblFreeTime.Size = new Size(76, 20);
            lblFreeTime.TabIndex = 0;
            lblFreeTime.Text = "Free time:";
            //
            // chkXemFilm
            //
            chkXemFilm.AutoSize = true;
            chkXemFilm.Location = new Point(130, 33);
            chkXemFilm.Name = "chkXemFilm";
            chkXemFilm.Size = new Size(90, 24);
            chkXemFilm.TabIndex = 1;
            chkXemFilm.Text = "Xem film";
            chkXemFilm.UseVisualStyleBackColor = true;
            //
            // chkChoi
            //
            chkChoi.AutoSize = true;
            chkChoi.Location = new Point(290, 33);
            chkChoi.Name = "chkChoi";
            chkChoi.Size = new Size(62, 24);
            chkChoi.TabIndex = 2;
            chkChoi.Text = "Chơi";
            chkChoi.UseVisualStyleBackColor = true;
            //
            // chkNgheNhac
            //
            chkNgheNhac.AutoSize = true;
            chkNgheNhac.Location = new Point(430, 33);
            chkNgheNhac.Name = "chkNgheNhac";
            chkNgheNhac.Size = new Size(103, 24);
            chkNgheNhac.TabIndex = 3;
            chkNgheNhac.Text = "Nghe nhạc";
            chkNgheNhac.UseVisualStyleBackColor = true;
            //
            // lblTheThao
            //
            lblTheThao.AutoSize = true;
            lblTheThao.Location = new Point(30, 80);
            lblTheThao.Name = "lblTheThao";
            lblTheThao.Size = new Size(70, 20);
            lblTheThao.TabIndex = 4;
            lblTheThao.Text = "Thể thao";
            //
            // lstTheThao
            //
            lstTheThao.ForeColor = SystemColors.WindowText;
            lstTheThao.FormattingEnabled = true;
            lstTheThao.Items.AddRange(new object[] { "Bóng đá", "Bơi lội", "Cờ vua", "Cầu lông", "Bóng chuyền" });
            lstTheThao.Location = new Point(130, 75);
            lstTheThao.Name = "lstTheThao";
            lstTheThao.SelectionMode = SelectionMode.MultiSimple;
            lstTheThao.Size = new Size(170, 84);
            lstTheThao.TabIndex = 5;
            //
            // lblDoc
            //
            lblDoc.AutoSize = true;
            lblDoc.Location = new Point(340, 80);
            lblDoc.Name = "lblDoc";
            lblDoc.Size = new Size(37, 20);
            lblDoc.TabIndex = 6;
            lblDoc.Text = "Đọc";
            //
            // clbDoc
            //
            clbDoc.CheckOnClick = true;
            clbDoc.ForeColor = SystemColors.WindowText;
            clbDoc.FormattingEnabled = true;
            clbDoc.Items.AddRange(new object[] { "Tiểu thuyết", "Truyện tranh", "Truyện ngắn", "Thơ" });
            clbDoc.Location = new Point(430, 75);
            clbDoc.Name = "clbDoc";
            clbDoc.Size = new Size(170, 84);
            clbDoc.TabIndex = 7;
            //
            // btnLayThongTin
            //
            btnLayThongTin.Location = new Point(180, 375);
            btnLayThongTin.Name = "btnLayThongTin";
            btnLayThongTin.Size = new Size(140, 35);
            btnLayThongTin.TabIndex = 3;
            btnLayThongTin.Text = "Lấy thông tin";
            btnLayThongTin.UseVisualStyleBackColor = true;
            btnLayThongTin.Click += btnLayThongTin_Click;
            //
            // btnDongForm
            //
            btnDongForm.Location = new Point(350, 375);
            btnDongForm.Name = "btnDongForm";
            btnDongForm.Size = new Size(140, 35);
            btnDongForm.TabIndex = 4;
            btnDongForm.Text = "Đóng form";
            btnDongForm.UseVisualStyleBackColor = true;
            btnDongForm.Click += btnDongForm_Click;
            //
            // rtbThongTin
            //
            rtbThongTin.Location = new Point(20, 425);
            rtbThongTin.Name = "rtbThongTin";
            rtbThongTin.ReadOnly = true;
            rtbThongTin.Size = new Size(620, 170);
            rtbThongTin.TabIndex = 5;
            rtbThongTin.Text = "";
            //
            // Form2
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(660, 615);
            Controls.Add(lblTieuDe);
            Controls.Add(grpThongTinChung);
            Controls.Add(grpSoThich);
            Controls.Add(btnLayThongTin);
            Controls.Add(btnDongForm);
            Controls.Add(rtbThongTin);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin sinh viên";
            FormClosed += Form2_FormClosed;
            grpThongTinChung.ResumeLayout(false);
            grpThongTinChung.PerformLayout();
            grpSoThich.ResumeLayout(false);
            grpSoThich.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTieuDe;
        private GroupBox grpThongTinChung;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblQueQuan;
        private ComboBox cboQueQuan;
        private Label lblGioiTinh;
        private RadioButton radNam;
        private RadioButton radNu;
        private GroupBox grpSoThich;
        private Label lblFreeTime;
        private CheckBox chkXemFilm;
        private CheckBox chkChoi;
        private CheckBox chkNgheNhac;
        private Label lblTheThao;
        private ListBox lstTheThao;
        private Label lblDoc;
        private CheckedListBox clbDoc;
        private Button btnLayThongTin;
        private Button btnDongForm;
        private RichTextBox rtbThongTin;
    }
}
