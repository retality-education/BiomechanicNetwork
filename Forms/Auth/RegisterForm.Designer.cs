namespace BiomechanicNetwork.Forms.Auth
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtFullName = new TextBox();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            btnRegister = new Button();
            btnBackToLogin = new Button();
            panelSide = new Panel();
            labelTitle = new Label();
            labelSubtitle = new Label();
            picTogglePassword = new PictureBox();
            picToggleConfirmPassword = new PictureBox();
            timerErrorHighlight = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)picTogglePassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picToggleConfirmPassword).BeginInit();
            SuspendLayout();
            // 
            // txtFullName
            // 
            txtFullName.BackColor = Color.WhiteSmoke;
            txtFullName.BorderStyle = BorderStyle.None;
            txtFullName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtFullName.ForeColor = Color.Gray;
            txtFullName.Location = new Point(250, 100);
            txtFullName.Margin = new Padding(3, 4, 3, 4);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(300, 27);
            txtFullName.TabIndex = 0;
            txtFullName.Enter += txtFullName_Enter;
            txtFullName.Leave += txtFullName_Leave;
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.WhiteSmoke;
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtUsername.ForeColor = Color.Gray;
            txtUsername.Location = new Point(250, 150);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(300, 27);
            txtUsername.TabIndex = 1;
            txtUsername.Enter += txtUsername_Enter;
            txtUsername.Leave += txtUsername_Leave;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.WhiteSmoke;
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtPassword.ForeColor = Color.Gray;
            txtPassword.Location = new Point(250, 200);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(300, 27);
            txtPassword.TabIndex = 2;
            txtPassword.Enter += txtPassword_Enter;
            txtPassword.Leave += txtPassword_Leave;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BackColor = Color.WhiteSmoke;
            txtConfirmPassword.BorderStyle = BorderStyle.None;
            txtConfirmPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtConfirmPassword.ForeColor = Color.Gray;
            txtConfirmPassword.Location = new Point(250, 250);
            txtConfirmPassword.Margin = new Padding(3, 4, 3, 4);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(300, 27);
            txtConfirmPassword.TabIndex = 3;
            txtConfirmPassword.Enter += txtConfirmPassword_Enter;
            txtConfirmPassword.Leave += txtConfirmPassword_Leave;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(0, 122, 204);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(250, 320);
            btnRegister.Margin = new Padding(3, 4, 3, 4);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(300, 45);
            btnRegister.TabIndex = 4;
            btnRegister.Text = "Зарегистрироваться";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnBackToLogin
            // 
            btnBackToLogin.BackColor = Color.White;
            btnBackToLogin.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            btnBackToLogin.FlatStyle = FlatStyle.Flat;
            btnBackToLogin.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnBackToLogin.ForeColor = Color.FromArgb(0, 122, 204);
            btnBackToLogin.Location = new Point(250, 380);
            btnBackToLogin.Margin = new Padding(3, 4, 3, 4);
            btnBackToLogin.Name = "btnBackToLogin";
            btnBackToLogin.Size = new Size(300, 40);
            btnBackToLogin.TabIndex = 5;
            btnBackToLogin.Text = "Назад к авторизации";
            btnBackToLogin.UseVisualStyleBackColor = false;
            btnBackToLogin.Click += btnBackToLogin_Click;
            // 
            // panelSide
            // 
            panelSide.BackColor = Color.FromArgb(0, 122, 204);
            panelSide.Dock = DockStyle.Left;
            panelSide.Location = new Point(0, 0);
            panelSide.Margin = new Padding(3, 4, 3, 4);
            panelSide.Name = "panelSide";
            panelSide.Size = new Size(200, 450);
            panelSide.TabIndex = 6;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelTitle.ForeColor = Color.FromArgb(64, 64, 64);
            labelTitle.Location = new Point(250, 29);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(201, 41);
            labelTitle.TabIndex = 7;
            labelTitle.Text = "Регистрация";
            // 
            // labelSubtitle
            // 
            labelSubtitle.AutoSize = true;
            labelSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelSubtitle.ForeColor = Color.Gray;
            labelSubtitle.Location = new Point(250, 70);
            labelSubtitle.Name = "labelSubtitle";
            labelSubtitle.Size = new Size(261, 20);
            labelSubtitle.TabIndex = 8;
            labelSubtitle.Text = "Заполните данные для регистрации";
            // 
            // picTogglePassword
            // 
            picTogglePassword.BackColor = Color.WhiteSmoke;
            picTogglePassword.Cursor = Cursors.Hand;
            picTogglePassword.Image = Properties.Resources.eye_show;
            picTogglePassword.Location = new Point(520, 200);
            picTogglePassword.Name = "picTogglePassword";
            picTogglePassword.Size = new Size(30, 27);
            picTogglePassword.SizeMode = PictureBoxSizeMode.StretchImage;
            picTogglePassword.TabIndex = 9;
            picTogglePassword.TabStop = false;
            picTogglePassword.Click += btnTogglePassword_Click;
            // 
            // picToggleConfirmPassword
            // 
            picToggleConfirmPassword.BackColor = Color.WhiteSmoke;
            picToggleConfirmPassword.Cursor = Cursors.Hand;
            picToggleConfirmPassword.Image = Properties.Resources.eye_show;
            picToggleConfirmPassword.Location = new Point(520, 250);
            picToggleConfirmPassword.Name = "picToggleConfirmPassword";
            picToggleConfirmPassword.Size = new Size(30, 27);
            picToggleConfirmPassword.SizeMode = PictureBoxSizeMode.StretchImage;
            picToggleConfirmPassword.TabIndex = 10;
            picToggleConfirmPassword.TabStop = false;
            picToggleConfirmPassword.Click += btnToggleConfirmPassword_Click;
            // 
            // timerErrorHighlight
            // 
            timerErrorHighlight.Interval = 1000;
            timerErrorHighlight.Tick += timerErrorHighlight_Tick;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 450);
            Controls.Add(picToggleConfirmPassword);
            Controls.Add(picTogglePassword);
            Controls.Add(labelSubtitle);
            Controls.Add(labelTitle);
            Controls.Add(panelSide);
            Controls.Add(btnBackToLogin);
            Controls.Add(btnRegister);
            Controls.Add(txtConfirmPassword);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(txtFullName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Biomechanic Network - Регистрация";
            ((System.ComponentModel.ISupportInitialize)picTogglePassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)picToggleConfirmPassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnBackToLogin;
        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSubtitle;
        private System.Windows.Forms.PictureBox picTogglePassword;
        private System.Windows.Forms.PictureBox picToggleConfirmPassword;
        private System.Windows.Forms.Timer timerErrorHighlight;
    }
}