using BiomechanicNetwork.Database.Repositories;
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
using System.Xml.Linq;

namespace BiomechanicNetwork.Forms.Auth
{
    public partial class RegisterForm : Form
    {
        private readonly UserRepository _userRepo;

        public RegisterForm()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            var newUser = new User
            {
                Username = txtUsername.Text,
                Name = txtName.Text,
                Role = UserRole.User,
                Password = txtPassword.Text,
            };

            if (_userRepo.Register(newUser.Username, newUser.Password, newUser.Name))
            {
                MessageBox.Show("Регистрация успешна!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка регистрации. Возможно, пользователь с таким именем уже существует.");
            }
        }
    }
}
