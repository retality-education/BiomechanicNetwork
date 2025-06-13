using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.Auth
{
    public partial class RegisterForm : Form
    {
        private readonly UserRepository _userRepo;
        private bool _passwordVisible = false;
        private bool _confirmPasswordVisible = false;

        public RegisterForm()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
            ConfigureForm();
        }

        private void ConfigureForm()
        {
            this.KeyPreview = true;
            this.KeyDown += RegisterForm_KeyDown;
            UpdatePasswordVisibility();
            SetPlaceholderStates();
        }

        private void UpdatePasswordVisibility()
        {
            txtPassword.UseSystemPasswordChar = !_passwordVisible;
            txtPassword.PasswordChar = _passwordVisible ? '\0' : '•';

            txtConfirmPassword.UseSystemPasswordChar = !_confirmPasswordVisible;
            txtConfirmPassword.PasswordChar = _confirmPasswordVisible ? '\0' : '•';
        }

        private void SetPlaceholderStates()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                txtFullName.Text = "ФИО";
                txtFullName.ForeColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Логин";
                txtUsername.ForeColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Пароль";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                txtConfirmPassword.Text = "Подтверждение пароля";
                txtConfirmPassword.ForeColor = Color.Gray;
                txtConfirmPassword.UseSystemPasswordChar = false;
            }
        }

        private void TogglePasswordVisibility(ref bool visibilityFlag)
        {
            visibilityFlag = !visibilityFlag;
            UpdatePasswordVisibility();
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility(ref _passwordVisible);
            txtPassword.Focus();
        }

        private void btnToggleConfirmPassword_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility(ref _confirmPasswordVisible);
            txtConfirmPassword.Focus();
        }

        private void TextBox_Enter(TextBox textBox, string placeholder)
        {
            if (textBox.Text == placeholder)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
                if (textBox == txtPassword || textBox == txtConfirmPassword)
                {
                    textBox.UseSystemPasswordChar = true;
                }
            }
        }

        private void TextBox_Leave(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;
                if (textBox == txtPassword || textBox == txtConfirmPassword)
                {
                    textBox.UseSystemPasswordChar = false;
                }
            }
        }

        private void txtFullName_Enter(object sender, EventArgs e) => TextBox_Enter(txtFullName, "ФИО");
        private void txtFullName_Leave(object sender, EventArgs e) => TextBox_Leave(txtFullName, "ФИО");
        private void txtUsername_Enter(object sender, EventArgs e) => TextBox_Enter(txtUsername, "Логин");
        private void txtUsername_Leave(object sender, EventArgs e) => TextBox_Leave(txtUsername, "Логин");
        private void txtPassword_Enter(object sender, EventArgs e) => TextBox_Enter(txtPassword, "Пароль");
        private void txtPassword_Leave(object sender, EventArgs e) => TextBox_Leave(txtPassword, "Пароль");
        private void txtConfirmPassword_Enter(object sender, EventArgs e) => TextBox_Enter(txtConfirmPassword, "Подтверждение пароля");
        private void txtConfirmPassword_Leave(object sender, EventArgs e) => TextBox_Leave(txtConfirmPassword, "Подтверждение пароля");

        private void RegisterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnRegister.PerformClick();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                var registrationResult = _userRepo.Register(
                    txtFullName.Text.Trim(),
                    txtUsername.Text.Trim(),
                    txtPassword.Text
                );

                if (registrationResult)
                {
                    MessageBox.Show("Регистрация прошла успешно!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    ShowRegistrationError("");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (txtFullName.Text == "ФИО" || string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowValidationError(txtFullName, "Введите ФИО");
                return false;
            }

            if (txtUsername.Text == "Логин" || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowValidationError(txtUsername, "Введите логин");
                return false;
            }

            if (txtPassword.Text == "Пароль" || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowValidationError(txtPassword, "Введите пароль");
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                ShowValidationError(txtPassword, "Пароль должен содержать минимум 6 символов");
                return false;
            }

            if (txtConfirmPassword.Text == "Подтверждение пароля" || string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                ShowValidationError(txtConfirmPassword, "Подтвердите пароль");
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowValidationError(txtConfirmPassword, "Пароли не совпадают");
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

        private void ShowRegistrationError(string message)
        {
            txtUsername.BackColor = Color.FromArgb(255, 230, 230);
            timerErrorHighlight.Start();
            MessageBox.Show(message, "Ошибка регистрации",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtUsername.SelectAll();
            txtUsername.Focus();
        }

        private void timerErrorHighlight_Tick(object sender, EventArgs e)
        {
            txtUsername.BackColor = Color.White;
            timerErrorHighlight.Stop();
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}