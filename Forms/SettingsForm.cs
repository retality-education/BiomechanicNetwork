using System;
using System.Drawing;
using System.Windows.Forms;
using BiomechanicNetwork.Database;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Repositories;
using BiomechanicNetwork.Services;
using Npgsql;

namespace BiomechanicNetwork.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly int _userId;
        private readonly UserRepository _userRepository;
        private readonly CloudinaryService _cloudinaryService;

        private PictureBox avatarPictureBox;
        private Button saveAvatarBtn;
        private TextBox nameTextBox;
        private Button saveNameBtn;
        private TextBox oldPasswordTextBox;
        private TextBox newPasswordTextBox;
        private Button savePasswordButton;

        private string _tempAvatarPath;

        public SettingsForm(int userId)
        {
            _userId = userId;
            _userRepository = new UserRepository();
            _cloudinaryService = new CloudinaryService();

            InitializeForm();
            LoadUserData();
        }

        private void InitializeForm()
        {
            // Настройки формы
            this.Text = "Настройки профиля";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Основной контейнер
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30)
            };
            this.Controls.Add(panel);

            // Аватарка
            avatarPictureBox = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(175, 20),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };
            avatarPictureBox.Click += AvatarPictureBox_Click;
            panel.Controls.Add(avatarPictureBox);

            // Кнопка сохранения аватарки
            saveAvatarBtn = new Button
            {
                Text = "Сохранить аватар",
                Size = new Size(150, 30),
                Location = new Point(175, 180),
                Enabled = false
            };
            saveAvatarBtn.Click += SaveAvatarButton_Click;
            panel.Controls.Add(saveAvatarBtn);

            // Имя пользователя
            var nameLabel = new Label
            {
                Text = "Имя:",
                AutoSize = true,
                Location = new Point(175, 230)
            };
            panel.Controls.Add(nameLabel);

            nameTextBox = new TextBox
            {
                Size = new Size(200, 25),
                Location = new Point(150, 250)
            };
            panel.Controls.Add(nameTextBox);

            // Кнопка сохранения имени
            saveNameBtn = new Button
            {
                Text = "Сохранить имя",
                Size = new Size(150, 30),
                Location = new Point(175, 285)
            };
            saveNameBtn.Click += SaveNameButton_Click;
            panel.Controls.Add(saveNameBtn);

            // Старый пароль
            var oldPasswordLabel = new Label
            {
                Text = "Старый пароль:",
                AutoSize = true,
                Location = new Point(175, 335)
            };
            panel.Controls.Add(oldPasswordLabel);

            oldPasswordTextBox = new TextBox
            {
                Size = new Size(200, 25),
                Location = new Point(150, 355),
                PasswordChar = '*'
            };
            panel.Controls.Add(oldPasswordTextBox);

            // Новый пароль
            var newPasswordLabel = new Label
            {
                Text = "Новый пароль:",
                AutoSize = true,
                Location = new Point(175, 390)
            };
            panel.Controls.Add(newPasswordLabel);

            newPasswordTextBox = new TextBox
            {
                Size = new Size(200, 25),
                Location = new Point(150, 410),
                PasswordChar = '*'
            };
            panel.Controls.Add(newPasswordTextBox);

            // Кнопка сохранения пароля
            savePasswordButton = new Button
            {
                Text = "Сохранить пароль",
                Size = new Size(150, 30),
                Location = new Point(175, 445)
            };
            savePasswordButton.Click += SavePasswordButton_Click;
            panel.Controls.Add(savePasswordButton);
        }

        private void LoadUserData()
        {
            var user = _userRepository.GetUserById(_userId);
            if (user != null)
            {
                // Загружаем имя
                nameTextBox.Text = user.Name;

                // Загружаем аватарку
                if (!string.IsNullOrEmpty(user.AvatarPublicId))
                {
                    var image = _cloudinaryService.GetImage(user.AvatarPublicId);
                    avatarPictureBox.Image = image ?? Properties.Resources.default_avatar;
                }
                else
                {
                    avatarPictureBox.Image = Properties.Resources.default_avatar;
                }
            }
        }

        private void AvatarPictureBox_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Изображения|*.jpg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _tempAvatarPath = openFileDialog.FileName;
                        avatarPictureBox.Image = Image.FromFile(_tempAvatarPath);
                        saveAvatarBtn.Enabled = true;
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось загрузить изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveAvatarButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_tempAvatarPath))
            {
                MessageBox.Show("Сначала выберите изображение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Генерируем уникальный public_id для аватарки
                string publicId = $"user_{_userId}_avatar_{DateTime.Now.Ticks}";

                // Загружаем в Cloudinary
                string uploadedPublicId = _cloudinaryService.UploadImage(_tempAvatarPath, publicId);

                // Обновляем в базе данных
                var query = "UPDATE users SET avatar_public_id = @publicId WHERE id = @userId";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@publicId", uploadedPublicId),
                    new NpgsqlParameter("@userId", _userId)
                };

                bool success = new DatabaseHelper().ExecuteNonQuery(query, parameters) > 0;

                if (success)
                {
                    MessageBox.Show("Аватар успешно обновлен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    saveAvatarBtn.Enabled = false;
                    _tempAvatarPath = null;
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении аватарки", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении аватарки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveNameButton_Click(object sender, EventArgs e)
        {
            string newName = nameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Имя не может быть пустым", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var query = "UPDATE users SET name = @name WHERE id = @userId";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@name", newName),
                    new NpgsqlParameter("@userId", _userId)
                };

                bool success = new DatabaseHelper().ExecuteNonQuery(query, parameters) > 0;

                if (success)
                {
                    MessageBox.Show("Имя успешно обновлено!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении имени", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении имени: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavePasswordButton_Click(object sender, EventArgs e)
        {
            string oldPassword = oldPasswordTextBox.Text;
            string newPassword = newPasswordTextBox.Text;

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Заполните оба поля пароля", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Новый пароль должен быть не менее 6 символов", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = _userRepository.ChangePassword(_userId, oldPassword, newPassword);

                if (success)
                {
                    MessageBox.Show("Пароль успешно изменен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    oldPasswordTextBox.Clear();
                    newPasswordTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Неверный текущий пароль", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении пароля: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}