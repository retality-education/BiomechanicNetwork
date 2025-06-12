using BiomechanicNetwork.Database.Repositories.BiomechanicNetwork.Database.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.MainForms
{
    internal partial class SupportForm : BaseForm
    {
        private ComboBox topicComboBox;
        private TextBox messageTextBox;
        private Button sendButton;
        private readonly SupportTicketRepository _supportRepository;

        public SupportForm()
        {
            header.Title = "Поддержка";
            _supportRepository = new SupportTicketRepository();
            InitializeSupportView();
        }

        private void InitializeSupportView()
        {
            // Основная панель с градиентным фоном
            var supportPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(40, 40, 40)
            };
            contentPanel.Controls.Add(supportPanel);

            // Контейнер для центрирования элементов
            var centerPanel = new Panel
            {
                Width = 400,
                AutoSize = true,
                Location = new Point((contentPanel.Width - 400) / 2, 50),
                BackColor = Color.Transparent
            };
            supportPanel.Controls.Add(centerPanel);

            // Выбор темы
            var topicLabel = new Label
            {
                Text = "Тема обращения:",
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255), // Голубой цвет
                AutoSize = true,
                Location = new Point(0, 0)
            };
            centerPanel.Controls.Add(topicLabel);

            topicComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Технические проблемы", "Вопрос по функционалу", "Предложения", "Другое" },
                Size = new Size(400, 30),
                Location = new Point(0, 30),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            centerPanel.Controls.Add(topicComboBox);

            // Поле сообщения
            var messageLabel = new Label
            {
                Text = "Ваше сообщение:",
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255), // Голубой цвет
                AutoSize = true,
                Location = new Point(0, 80)
            };
            centerPanel.Controls.Add(messageLabel);

            messageTextBox = new TextBox
            {
                Multiline = true,
                Size = new Size(400, 150),
                Location = new Point(0, 110),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };
            centerPanel.Controls.Add(messageTextBox);

            // Кнопка отправки
            sendButton = new Button
            {
                Text = "Отправить обращение",
                Size = new Size(400, 40),
                Location = new Point(0, 270),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 215), // Синий цвет
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 100, 190);
            sendButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 80, 160);
            sendButton.Click += SendButton_Click;
            centerPanel.Controls.Add(sendButton);

            // Обработчик изменения размера для центрирования
            supportPanel.Resize += (s, e) => {
                centerPanel.Left = (supportPanel.ClientSize.Width - centerPanel.Width) / 2;
            };
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (topicComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тему обращения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(messageTextBox.Text))
            {
                MessageBox.Show("Введите текст сообщения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string topic = topicComboBox.SelectedItem.ToString();
                string message = messageTextBox.Text;

                // Определяем, относится ли обращение к другим пользователям
                bool isAboutUsers = topic == "Предложения" || topic == "Другое";

                bool success = _supportRepository.CreateSupportTicket(
                    Program.CurrentUser.Id,
                    topic,
                    message,
                    isAboutUsers);

                if (success)
                {
                    MessageBox.Show("Ваше обращение успешно отправлено!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Очищаем форму после отправки
                    topicComboBox.SelectedIndex = -1;
                    messageTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Не удалось отправить обращение. Попробуйте позже.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при отправке обращения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnNavigationRequested(string formName)
        {
            switch (formName)
            {
                case "Упражнения":
                    (new ExerciseForm()).Show();
                    this.Close();
                    break;
                case "Рекомендации":
                    (new VideoFeedForm()).Show();
                    this.Close();
                    break;
                case "Профиль":
                    (new ProfileForm(Program.CurrentUser.Id)).Show();
                    this.Close();
                    break;
            }
        }
    }
}