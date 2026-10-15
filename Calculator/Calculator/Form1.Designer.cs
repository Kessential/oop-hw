namespace Calculator
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
            txtDisplay = new TextBox();
            lblMemory = new Label();
            btnBackspace = new Button();
            btnCE = new Button();
            btnC = new Button();
            btnMC = new Button();
            btnMR = new Button();
            btnMS = new Button();
            btnMPlus = new Button();
            btn7 = new Button();
            btn8 = new Button();
            btn9 = new Button();
            btnDivide = new Button();
            btnSqrt = new Button();
            btn4 = new Button();
            btn5 = new Button();
            btn6 = new Button();
            btnMultiply = new Button();
            btnPercent = new Button();
            btn1 = new Button();
            btn2 = new Button();
            btn3 = new Button();
            btnSubtract = new Button();
            btnReciprocal = new Button();
            btn0 = new Button();
            btnNegate = new Button();
            btnDecimal = new Button();
            btnAdd = new Button();
            btnEquals = new Button();
            SuspendLayout();
            //
            // txtDisplay
            //
            txtDisplay.BackColor = Color.White;
            txtDisplay.Font = new Font("Segoe UI", 11F);
            txtDisplay.Location = new Point(12, 10);
            txtDisplay.Name = "txtDisplay";
            txtDisplay.ReadOnly = true;
            txtDisplay.Size = new Size(324, 27);
            txtDisplay.TabIndex = 0;
            txtDisplay.TabStop = false;
            txtDisplay.Text = "0.";
            txtDisplay.TextAlign = HorizontalAlignment.Right;
            //
            // lblMemory
            //
            lblMemory.BorderStyle = BorderStyle.Fixed3D;
            lblMemory.Location = new Point(12, 44);
            lblMemory.Name = "lblMemory";
            lblMemory.Size = new Size(44, 28);
            lblMemory.TabIndex = 1;
            lblMemory.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBackspace
            // 
            btnBackspace.ForeColor = Color.Red;
            btnBackspace.Location = new Point(72, 44);
            btnBackspace.Name = "btnBackspace";
            btnBackspace.Size = new Size(102, 28);
            btnBackspace.TabIndex = 2;
            btnBackspace.Text = "Backspace";
            btnBackspace.UseVisualStyleBackColor = true;
            btnBackspace.Click += Backspace_Click;
            // 
            // btnCE
            // 
            btnCE.ForeColor = Color.Red;
            btnCE.Location = new Point(180, 44);
            btnCE.Name = "btnCE";
            btnCE.Size = new Size(75, 28);
            btnCE.TabIndex = 3;
            btnCE.Text = "CE";
            btnCE.UseVisualStyleBackColor = true;
            btnCE.Click += ClearEntry_Click;
            // 
            // btnC
            // 
            btnC.ForeColor = Color.Red;
            btnC.Location = new Point(261, 44);
            btnC.Name = "btnC";
            btnC.Size = new Size(75, 28);
            btnC.TabIndex = 4;
            btnC.Text = "C";
            btnC.UseVisualStyleBackColor = true;
            btnC.Click += Clear_Click;
            // 
            // btnMC
            // 
            btnMC.ForeColor = Color.Red;
            btnMC.Location = new Point(12, 80);
            btnMC.Name = "btnMC";
            btnMC.Size = new Size(44, 28);
            btnMC.TabIndex = 5;
            btnMC.Text = "MC";
            btnMC.UseVisualStyleBackColor = true;
            btnMC.Click += MemoryClear_Click;
            // 
            // btnMR
            // 
            btnMR.ForeColor = Color.Red;
            btnMR.Location = new Point(12, 114);
            btnMR.Name = "btnMR";
            btnMR.Size = new Size(44, 28);
            btnMR.TabIndex = 6;
            btnMR.Text = "MR";
            btnMR.UseVisualStyleBackColor = true;
            btnMR.Click += MemoryRecall_Click;
            // 
            // btnMS
            // 
            btnMS.ForeColor = Color.Red;
            btnMS.Location = new Point(12, 148);
            btnMS.Name = "btnMS";
            btnMS.Size = new Size(44, 28);
            btnMS.TabIndex = 7;
            btnMS.Text = "MS";
            btnMS.UseVisualStyleBackColor = true;
            btnMS.Click += MemoryStore_Click;
            // 
            // btnMPlus
            // 
            btnMPlus.ForeColor = Color.Red;
            btnMPlus.Location = new Point(12, 182);
            btnMPlus.Name = "btnMPlus";
            btnMPlus.Size = new Size(44, 28);
            btnMPlus.TabIndex = 8;
            btnMPlus.Text = "M+";
            btnMPlus.UseVisualStyleBackColor = true;
            btnMPlus.Click += MemoryAdd_Click;
            // 
            // btn7
            // 
            btn7.ForeColor = Color.Blue;
            btn7.Location = new Point(72, 80);
            btn7.Name = "btn7";
            btn7.Size = new Size(48, 28);
            btn7.TabIndex = 9;
            btn7.Text = "7";
            btn7.UseVisualStyleBackColor = true;
            btn7.Click += Digit_Click;
            // 
            // btn8
            // 
            btn8.ForeColor = Color.Blue;
            btn8.Location = new Point(126, 80);
            btn8.Name = "btn8";
            btn8.Size = new Size(48, 28);
            btn8.TabIndex = 10;
            btn8.Text = "8";
            btn8.UseVisualStyleBackColor = true;
            btn8.Click += Digit_Click;
            // 
            // btn9
            // 
            btn9.ForeColor = Color.Blue;
            btn9.Location = new Point(180, 80);
            btn9.Name = "btn9";
            btn9.Size = new Size(48, 28);
            btn9.TabIndex = 11;
            btn9.Text = "9";
            btn9.UseVisualStyleBackColor = true;
            btn9.Click += Digit_Click;
            // 
            // btnDivide
            // 
            btnDivide.ForeColor = Color.Red;
            btnDivide.Location = new Point(234, 80);
            btnDivide.Name = "btnDivide";
            btnDivide.Size = new Size(48, 28);
            btnDivide.TabIndex = 12;
            btnDivide.Text = "/";
            btnDivide.UseVisualStyleBackColor = true;
            btnDivide.Click += Operator_Click;
            // 
            // btnSqrt
            // 
            btnSqrt.ForeColor = Color.Blue;
            btnSqrt.Location = new Point(288, 80);
            btnSqrt.Name = "btnSqrt";
            btnSqrt.Size = new Size(48, 28);
            btnSqrt.TabIndex = 13;
            btnSqrt.Text = "sqrt";
            btnSqrt.UseVisualStyleBackColor = true;
            btnSqrt.Click += Sqrt_Click;
            // 
            // btn4
            // 
            btn4.ForeColor = Color.Blue;
            btn4.Location = new Point(72, 114);
            btn4.Name = "btn4";
            btn4.Size = new Size(48, 28);
            btn4.TabIndex = 14;
            btn4.Text = "4";
            btn4.UseVisualStyleBackColor = true;
            btn4.Click += Digit_Click;
            // 
            // btn5
            // 
            btn5.ForeColor = Color.Blue;
            btn5.Location = new Point(126, 114);
            btn5.Name = "btn5";
            btn5.Size = new Size(48, 28);
            btn5.TabIndex = 15;
            btn5.Text = "5";
            btn5.UseVisualStyleBackColor = true;
            btn5.Click += Digit_Click;
            // 
            // btn6
            // 
            btn6.ForeColor = Color.Blue;
            btn6.Location = new Point(180, 114);
            btn6.Name = "btn6";
            btn6.Size = new Size(48, 28);
            btn6.TabIndex = 16;
            btn6.Text = "6";
            btn6.UseVisualStyleBackColor = true;
            btn6.Click += Digit_Click;
            // 
            // btnMultiply
            // 
            btnMultiply.ForeColor = Color.Red;
            btnMultiply.Location = new Point(234, 114);
            btnMultiply.Name = "btnMultiply";
            btnMultiply.Size = new Size(48, 28);
            btnMultiply.TabIndex = 17;
            btnMultiply.Text = "*";
            btnMultiply.UseVisualStyleBackColor = true;
            btnMultiply.Click += Operator_Click;
            // 
            // btnPercent
            // 
            btnPercent.ForeColor = Color.Blue;
            btnPercent.Location = new Point(288, 114);
            btnPercent.Name = "btnPercent";
            btnPercent.Size = new Size(48, 28);
            btnPercent.TabIndex = 18;
            btnPercent.Text = "%";
            btnPercent.UseVisualStyleBackColor = true;
            btnPercent.Click += Percent_Click;
            // 
            // btn1
            // 
            btn1.ForeColor = Color.Blue;
            btn1.Location = new Point(72, 148);
            btn1.Name = "btn1";
            btn1.Size = new Size(48, 28);
            btn1.TabIndex = 19;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = true;
            btn1.Click += Digit_Click;
            // 
            // btn2
            // 
            btn2.ForeColor = Color.Blue;
            btn2.Location = new Point(126, 148);
            btn2.Name = "btn2";
            btn2.Size = new Size(48, 28);
            btn2.TabIndex = 20;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = true;
            btn2.Click += Digit_Click;
            // 
            // btn3
            // 
            btn3.ForeColor = Color.Blue;
            btn3.Location = new Point(180, 148);
            btn3.Name = "btn3";
            btn3.Size = new Size(48, 28);
            btn3.TabIndex = 21;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = true;
            btn3.Click += Digit_Click;
            // 
            // btnSubtract
            // 
            btnSubtract.ForeColor = Color.Red;
            btnSubtract.Location = new Point(234, 148);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.Size = new Size(48, 28);
            btnSubtract.TabIndex = 22;
            btnSubtract.Text = "-";
            btnSubtract.UseVisualStyleBackColor = true;
            btnSubtract.Click += Operator_Click;
            // 
            // btnReciprocal
            // 
            btnReciprocal.ForeColor = Color.Blue;
            btnReciprocal.Location = new Point(288, 148);
            btnReciprocal.Name = "btnReciprocal";
            btnReciprocal.Size = new Size(48, 28);
            btnReciprocal.TabIndex = 23;
            btnReciprocal.Text = "1/x";
            btnReciprocal.UseVisualStyleBackColor = true;
            btnReciprocal.Click += Reciprocal_Click;
            // 
            // btn0
            // 
            btn0.ForeColor = Color.Blue;
            btn0.Location = new Point(72, 182);
            btn0.Name = "btn0";
            btn0.Size = new Size(48, 28);
            btn0.TabIndex = 24;
            btn0.Text = "0";
            btn0.UseVisualStyleBackColor = true;
            btn0.Click += Digit_Click;
            // 
            // btnNegate
            // 
            btnNegate.ForeColor = Color.Blue;
            btnNegate.Location = new Point(126, 182);
            btnNegate.Name = "btnNegate";
            btnNegate.Size = new Size(48, 28);
            btnNegate.TabIndex = 25;
            btnNegate.Text = "+/-";
            btnNegate.UseVisualStyleBackColor = true;
            btnNegate.Click += Negate_Click;
            // 
            // btnDecimal
            // 
            btnDecimal.ForeColor = Color.Blue;
            btnDecimal.Location = new Point(180, 182);
            btnDecimal.Name = "btnDecimal";
            btnDecimal.Size = new Size(48, 28);
            btnDecimal.TabIndex = 26;
            btnDecimal.Text = ".";
            btnDecimal.UseVisualStyleBackColor = true;
            btnDecimal.Click += Decimal_Click;
            // 
            // btnAdd
            // 
            btnAdd.ForeColor = Color.Red;
            btnAdd.Location = new Point(234, 182);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(48, 28);
            btnAdd.TabIndex = 27;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += Operator_Click;
            // 
            // btnEquals
            // 
            btnEquals.ForeColor = Color.Red;
            btnEquals.Location = new Point(288, 182);
            btnEquals.Name = "btnEquals";
            btnEquals.Size = new Size(48, 28);
            btnEquals.TabIndex = 28;
            btnEquals.Text = "=";
            btnEquals.UseVisualStyleBackColor = true;
            btnEquals.Click += Equals_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(348, 222);
            Controls.Add(txtDisplay);
            Controls.Add(lblMemory);
            Controls.Add(btnBackspace);
            Controls.Add(btnCE);
            Controls.Add(btnC);
            Controls.Add(btnMC);
            Controls.Add(btnMR);
            Controls.Add(btnMS);
            Controls.Add(btnMPlus);
            Controls.Add(btn7);
            Controls.Add(btn8);
            Controls.Add(btn9);
            Controls.Add(btnDivide);
            Controls.Add(btnSqrt);
            Controls.Add(btn4);
            Controls.Add(btn5);
            Controls.Add(btn6);
            Controls.Add(btnMultiply);
            Controls.Add(btnPercent);
            Controls.Add(btn1);
            Controls.Add(btn2);
            Controls.Add(btn3);
            Controls.Add(btnSubtract);
            Controls.Add(btnReciprocal);
            Controls.Add(btn0);
            Controls.Add(btnNegate);
            Controls.Add(btnDecimal);
            Controls.Add(btnAdd);
            Controls.Add(btnEquals);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Calculator";
            KeyPress += Form1_KeyPress;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtDisplay;
        private Label lblMemory;
        private Button btnBackspace;
        private Button btnCE;
        private Button btnC;
        private Button btnMC;
        private Button btnMR;
        private Button btnMS;
        private Button btnMPlus;
        private Button btn7;
        private Button btn8;
        private Button btn9;
        private Button btnDivide;
        private Button btnSqrt;
        private Button btn4;
        private Button btn5;
        private Button btn6;
        private Button btnMultiply;
        private Button btnPercent;
        private Button btn1;
        private Button btn2;
        private Button btn3;
        private Button btnSubtract;
        private Button btnReciprocal;
        private Button btn0;
        private Button btnNegate;
        private Button btnDecimal;
        private Button btnAdd;
        private Button btnEquals;
    }
}
