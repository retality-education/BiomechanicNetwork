using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Forms.MainForms;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.Auth
{
    public partial class LoginForm : Form
    {
        private readonly UserRepository _userRepo;

        public LoginForm()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            var user = _userRepo.Authenticate(username, password);

            if (user.Success)
            {
                Program.CurrentUser.Role = (UserRole)user.RoleId;
                Program.CurrentUser.Id = user.UserId;

                this.Hide();
                var mainForm = new ExerciseForm();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль");
            }
        }
    }
}
