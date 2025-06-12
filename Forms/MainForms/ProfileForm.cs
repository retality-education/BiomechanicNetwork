using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.ExtraControls;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Repositories;
using BiomechanicNetwork.Services;
using BiomechanicNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.MainForms
{
    internal partial class ProfileForm : BaseForm
    {
        private readonly int _userId;
        private readonly VideoRepository _videoRepository;
        private readonly UserRepository _userRepository;
        private readonly CloudinaryService _cloudinaryService;
        private List<VideoPlayerControl> _videoControls = new List<VideoPlayerControl>();
        private Button _uploadButton;
        private PictureBox _avatarPictureBox;
        private Label _nameLabel;
        private Label _idLabel;

        public ProfileForm(int userId)
        {
            _userId = userId;
            _videoRepository = new VideoRepository();
            _userRepository = new UserRepository();
            _cloudinaryService = new CloudinaryService();

            InitializeForm();
            LoadUserInfo();
            LoadUserVideos();
        }

        private void InitializeForm()
        {
            header.Title = _userId == Program.CurrentUser.Id ? "Мой профиль" : "Профиль пользователя";
            contentPanel.AutoScroll = true;
            contentPanel.Padding = new Padding(20);

            // Создаем панель для информации о пользователе (для удобства центрирования)
            Panel userInfoPanel = new Panel
            {
                Width = contentPanel.ClientSize.Width - 40,
                Height = 180,
                Top = 20,
                Left = 0,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            contentPanel.Controls.Add(userInfoPanel);

            // Инициализация элементов информации о пользователе (центрированных)
            _avatarPictureBox = new PictureBox
            {
                Width = 100,
                Height = 100,
                Top = 10,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            _avatarPictureBox.Left = (userInfoPanel.Width - _avatarPictureBox.Width) / 2;

            _nameLabel = new Label
            {
                AutoSize = true,
                Top = 120,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            _nameLabel.Left = (userInfoPanel.Width - _nameLabel.Width) / 2;

            _idLabel = new Label
            {
                AutoSize = true,
                Top = 150,
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter
            };
            _idLabel.Left = (userInfoPanel.Width - _idLabel.Width) / 2;

            userInfoPanel.Controls.Add(_avatarPictureBox);
            userInfoPanel.Controls.Add(_nameLabel);
            userInfoPanel.Controls.Add(_idLabel);

            if (_userId == Program.CurrentUser.Id)
            {
                _uploadButton = new Button
                {
                    Text = "Загрузить видео",
                    Width = 150,
                    Height = 40,
                    Top = userInfoPanel.Bottom + 20,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                _uploadButton.Left = (contentPanel.ClientSize.Width - _uploadButton.Width) / 2;
                _uploadButton.Click += UploadButton_Click;
                contentPanel.Controls.Add(_uploadButton);
            }
        }

        private void LoadUserInfo()
        {
            try
            {
                var user = _userRepository.GetUserById(_userId);
                if (user != null)
                {
                    // Загрузка аватарки
                    if (!string.IsNullOrEmpty(user.AvatarPublicId))
                    {
                        var image = _cloudinaryService.GetImage(user.AvatarPublicId);
                        if (image != null)
                        {
                            _avatarPictureBox.Image = image;
                        }
                        else
                        {
                            _avatarPictureBox.Image = Properties.Resources.default_avatar;
                        }
                    }
                    else
                    {
                        _avatarPictureBox.Image = Properties.Resources.default_avatar;
                    }

                    _nameLabel.Text = user.Name;
                    _nameLabel.Left = (_nameLabel.Parent.Width - _nameLabel.Width) / 2;

                    _idLabel.Text = $"ID: {user.Id}";
                    _idLabel.Left = (_idLabel.Parent.Width - _idLabel.Width) / 2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки информации о пользователе: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserVideos()
        {
            try
            {
                // Получаем видео пользователя из базы данных
                var videos = _videoRepository.GetUserVideos(_userId);

                // Начальная позиция для первого видео
                int topPosition = _userId == Program.CurrentUser.Id ?
                    (_uploadButton?.Bottom ?? 200) + 20 : 200;

                foreach (DataRow videoRow in videos.Rows)
                {
                    var videoControl = CreateVideoControl(videoRow, topPosition);
                    contentPanel.Controls.Add(videoControl);
                    _videoControls.Add(videoControl);

                    topPosition += videoControl.Height + 20;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки видео: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private VideoPlayerControl CreateVideoControl(DataRow videoRow, int topPosition)
        {
            string videoUrl = GetVideoUrl(videoRow["video_public_id"].ToString());
            int videoIndex = _videoControls.Count + 1;

            var videoControl = new VideoPlayerControl(videoUrl)
            {
                Width = 300,
                Height = 400,
                Tag = videoRow["id"],
                Top = topPosition,
                Left = (contentPanel.ClientSize.Width - 300) / 2 // Центрируем видео
            };

            // Устанавливаем информацию о видео (только 3 поля)
            videoControl.SetVideoInfo(
                title: $"Видео №{videoIndex}",
                author: videoRow["muscle_group_name"].ToString(),
                date: videoRow["exercise_name"].ToString()
            );

            int videoId = Convert.ToInt32(videoRow["id"]);
            int likes = _videoRepository.GetLikeCount(videoId);
            int comments = _videoRepository.GetComments(videoId).Rows.Count;

            videoControl.SetMetrics(likes, comments, 0);
            videoControl.Click += VideoControl_Click;

            return videoControl;
        }
        private async void UploadButton_Click(object sender, EventArgs e)
        {
            using (var uploadForm = new VideoUploadForm())
            {
                if (uploadForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string publicId = $"user_{_userId}_video_{DateTime.Now.Ticks}";
                        string uploadedPublicId = _cloudinaryService.UploadVideo(uploadForm.FilePath, publicId);

                        // Добавляем видео в БД (используем упражнение "Другое" по умолчанию)
                        int defaultExerciseId = 1; // ID упражнения "Другое"
                        bool success = _videoRepository.AddVideo(_userId, defaultExerciseId, uploadedPublicId);

                        if (!success)
                        {
                            MessageBox.Show("Ошибка сохранения видео в базе данных", "Ошибка",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        MessageBox.Show("Видео успешно загружено!", "Успех",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Обновляем список видео
                        contentPanel.Controls.Clear();
                        if (_userId == Program.CurrentUser.Id)
                            contentPanel.Controls.Add(_uploadButton);
                        LoadUserVideos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки видео: {ex.Message}", "Ошибка",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

       


        private string GetVideoUrl(string publicId)
        {
            return $"http://res.cloudinary.com/{SecretsManager.GetCloudinaryCloudName()}/video/upload/{publicId}";
        }

        private void VideoControl_Click(object sender, EventArgs e)
        {
            var clickedControl = (VideoPlayerControl)sender;

            foreach (var control in _videoControls)
            {
                if (control != clickedControl)
                    control.PauseVideo();
            }

            if (clickedControl.IsPlaying())
                clickedControl.PauseVideo();
            else
                clickedControl.PlayVideo();
        }

        private void PauseAllVideos()
        {
            foreach (var control in _videoControls)
            {
                control.PauseVideo();
            }
        }

        protected override void OnNavigationRequested(string formName)
        {
            PauseAllVideos();

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
                case "Поддержка":
                    (new SupportForm()).Show();
                    this.Close();
                    break;
            }
        }
    }
}