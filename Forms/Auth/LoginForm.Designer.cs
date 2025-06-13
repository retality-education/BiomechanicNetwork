namespace BiomechanicNetwork.Forms.Auth
{
    partial class LoginForm
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
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnRegister = new Button();
            panelSide = new Panel();
            labelTitle = new Label();
            labelSubtitle = new Label();
            timerErrorHighlight = new System.Windows.Forms.Timer(components);
            picTogglePassword = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picTogglePassword).BeginInit();
            SuspendLayout();
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
            txtUsername.TabIndex = 0;
            txtUsername.Enter += txtUsername_Enter;
            txtUsername.Leave += txtUsername_Leave;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.WhiteSmoke;
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtPassword.ForeColor = Color.Gray;
            txtPassword.Location = new Point(250, 220);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(270, 27);
            txtPassword.TabIndex = 1;
            txtPassword.Enter += txtPassword_Enter;
            txtPassword.Leave += txtPassword_Leave;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(250, 300);
            btnLogin.Margin = new Padding(3, 4, 3, 4);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(300, 45);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Войти";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.White;
            btnRegister.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btnRegister.ForeColor = Color.FromArgb(0, 122, 204);
            btnRegister.Location = new Point(250, 370);
            btnRegister.Margin = new Padding(3, 4, 3, 4);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(300, 40);
            btnRegister.TabIndex = 3;
            btnRegister.Text = "Создать аккаунт";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // panelSide
            // 
            panelSide.BackColor = Color.FromArgb(0, 122, 204);
            panelSide.Dock = DockStyle.Left;
            panelSide.Location = new Point(0, 0);
            panelSide.Margin = new Padding(3, 4, 3, 4);
            panelSide.Name = "panelSide";
            panelSide.Size = new Size(200, 500);
            panelSide.TabIndex = 4;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelTitle.ForeColor = Color.FromArgb(64, 64, 64);
            labelTitle.Location = new Point(250, 70);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(212, 41);
            labelTitle.TabIndex = 5;
            labelTitle.Text = "Авторизация";
            // 
            // labelSubtitle
            // 
            labelSubtitle.AutoSize = true;
            labelSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelSubtitle.ForeColor = Color.Gray;
            labelSubtitle.Location = new Point(250, 110);
            labelSubtitle.Name = "labelSubtitle";
            labelSubtitle.Size = new Size(265, 20);
            labelSubtitle.TabIndex = 6;
            labelSubtitle.Text = "Введите данные для входа в систему";
            // 
            // timerErrorHighlight
            // 
            timerErrorHighlight.Interval = 1000;
            timerErrorHighlight.Tick += timerErrorHighlight_Tick;
            // 
            // picTogglePassword
            // 
            picTogglePassword.BackColor = Color.WhiteSmoke;
            picTogglePassword.Cursor = Cursors.Hand;
            picTogglePassword.Image = Properties.Resources.eye_show;
            picTogglePassword.Location = new Point(520, 220);
            picTogglePassword.Name = "picTogglePassword";
            picTogglePassword.Size = new Size(30, 27);
            picTogglePassword.SizeMode = PictureBoxSizeMode.StretchImage;
            picTogglePassword.TabIndex = 10;
            picTogglePassword.TabStop = false;
            picTogglePassword.Click += btnTogglePassword_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 500);
            Controls.Add(picTogglePassword);
            Controls.Add(labelSubtitle);
            Controls.Add(labelTitle);
            Controls.Add(panelSide);
            Controls.Add(btnRegister);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Biomechanic Network - Вход";
            ((System.ComponentModel.ISupportInitialize)picTogglePassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Panel panelSide;

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSubtitle;
        private System.Windows.Forms.Timer timerErrorHighlight;
        private PictureBox picTogglePassword;
    }
}