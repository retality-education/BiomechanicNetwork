using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Forms.AdminForms;
using BiomechanicNetwork.Forms.MainForms;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.Auth
{
    public partial class LoginForm : Form
    {
        private readonly UserRepository _userRepo;
        private bool _passwordVisible = false;

        public LoginForm()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
            ConfigureForm();
        }

        private void ConfigureForm()
        {
            // Настройка поведения формы
            this.KeyPreview = true;
            this.KeyDown += LoginForm_KeyDown;

            // Инициализация состояния полей
            UpdatePasswordVisibility();
            SetPlaceholderStates();
        }

        private void UpdatePasswordVisibility()
        {
            picTogglePassword.Image = _passwordVisible ?
                Properties.Resources.eye_hide :
                Properties.Resources.eye_show;

            txtPassword.UseSystemPasswordChar = !_passwordVisible;
            txtPassword.PasswordChar = _passwordVisible ? '\0' : '•';
        }

        private void SetPlaceholderStates()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Логин или email";
                txtUsername.ForeColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Пароль";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            _passwordVisible = !_passwordVisible;
            UpdatePasswordVisibility();
            txtPassword.Focus();
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Логин или email")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Логин или email";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Пароль")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Пароль";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                var authResult = _userRepo.Authenticate(
                    txtUsername.Text.Trim(),
                    txtPassword.Text
                );

                if (authResult.Success)
                {
                    ProcessSuccessfulLogin((authResult.UserId, authResult.RoleId));
                }
                else
                {
                    ShowAuthError();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (txtUsername.Text == "Логин или email" || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowValidationError(txtUsername, "Введите имя пользователя");
                return false;
            }

            if (txtPassword.Text == "Пароль" || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowValidationError(txtPassword, "Введите пароль");
                return false;
            }

            return true;
        }

        private void ShowValidationError(Control control, string message)
        {
            control.Focus();
            MessageBox.Show(message, "Ошибка ввода",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ProcessSuccessfulLogin((int, int) authResult)
        {
            Program.CurrentUser = new User
            {
                Id = authResult.Item1,
                Role = (UserRole)authResult.Item2
            };

            this.Hide();

            Form mainForm = Program.CurrentUser.Role == UserRole.Admin
                ? new AdminForm(Program.CurrentUser.Id)
                : new ExerciseForm();

            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }

        private void ShowAuthError()
        {
            txtPassword.BackColor = Color.FromArgb(255, 230, 230);
            timerErrorHighlight.Start();
            MessageBox.Show("Неверные учетные данные", "Ошибка авторизации",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtPassword.SelectAll();
            txtPassword.Focus();
        }

        private void timerErrorHighlight_Tick(object sender, EventArgs e)
        {
            txtPassword.BackColor = Color.White;
            timerErrorHighlight.Stop();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new RegisterForm();
            registerForm.FormClosed += (s, args) => this.Show();
            registerForm.Show();
        }
    }
}