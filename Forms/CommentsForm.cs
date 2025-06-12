using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Repositories;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms
{
    internal partial class CommentsForm : Form
    {
        private readonly int _contentId;
        private readonly bool _isExercise;
        private readonly bool _canComment;
        private readonly CommentRepository _commentRepo;
        private readonly UserRepository _userRepo;
        private TextBox _commentTextBox;
        private Panel _commentsPanel;
        private Button _sendButton;

        public CommentsForm(int contentId, bool isExercise, bool canComment)
        {
            _contentId = contentId;
            _isExercise = isExercise;
            _canComment = canComment;
            _commentRepo = new CommentRepository();
            _userRepo = new UserRepository();

            InitializeComponent();
            Configure();
            LoadComments();
        }

        private void Configure()
        {
            this.Text = _isExercise ? "Комментарии к упражнению" : "Комментарии к видео";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            _commentsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            _commentTextBox = new TextBox
            {
                Dock = DockStyle.Bottom,
                Multiline = true,
                Height = 60,
                Margin = new Padding(10),
                Enabled = _canComment,
                Visible = _canComment
            };

            _sendButton = new Button
            {
                Text = "Отправить",
                Dock = DockStyle.Bottom,
                Height = 40,
                Enabled = _canComment,
                Visible = _canComment
            };
            _sendButton.Click += SendButton_Click;

            this.Controls.Add(_sendButton);
            this.Controls.Add(_commentTextBox);
            this.Controls.Add(_commentsPanel);
           
            
        }

        private void LoadComments()
        {
            _commentsPanel.Controls.Clear();
            DataTable comments = _isExercise
                ? _commentRepo.GetExerciseComments(_contentId)
                : _commentRepo.GetVideoComments(_contentId);

            int yPos = 10;
            foreach (DataRow row in comments.Rows)
            {
                var user = _userRepo.GetUserById(Convert.ToInt32(row["user_id"]));
                bool isExpert = (UserRole)user?.RoleId == UserRole.Expert;

                var nameLabel = new Label
                {
                    Text = user?.Name + ":",
                    AutoSize = true,
                    Location = new Point(10, yPos),
                    ForeColor = isExpert ? Color.Orange : Color.LightBlue,
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };

                var commentLabel = new Label
                {
                    Text = row["text"].ToString(),
                    AutoSize = true,
                    Location = new Point(10, yPos + 20),
                    MaximumSize = new Size(_commentsPanel.Width - 30, 0)
                };

                _commentsPanel.Controls.Add(nameLabel);
                _commentsPanel.Controls.Add(commentLabel);

                yPos += commentLabel.Height + 30;
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_commentTextBox.Text))
            {
                MessageBox.Show("Комментарий не может быть пустым", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = _isExercise
                ? _commentRepo.AddExerciseComment(Program.CurrentUser.Id, _contentId, _commentTextBox.Text)
                : _commentRepo.AddVideoComment(Program.CurrentUser.Id, _contentId, _commentTextBox.Text);

            if (success)
            {
                _commentTextBox.Clear();
                LoadComments();
            }
            else
            {
                MessageBox.Show("Не удалось добавить комментарий", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}