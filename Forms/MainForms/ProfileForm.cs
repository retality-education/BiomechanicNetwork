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
        private readonly CommentRepository _commentRepository;
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
            _commentRepository = new CommentRepository();
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

            Panel userInfoPanel = new Panel
            {
                Width = contentPanel.ClientSize.Width - 40,
                Height = 180,
                Top = 20,
                Left = 0,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            contentPanel.Controls.Add(userInfoPanel);

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
                    if (!string.IsNullOrEmpty(user.AvatarPublicId))
                    {
                        var image = _cloudinaryService.GetImage(user.AvatarPublicId);
                        _avatarPictureBox.Image = image ?? Properties.Resources.default_avatar;
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
                var videos = _videoRepository.GetUserVideos(_userId, Program.CurrentUser.Id);
                int topPosition = _userId == Program.CurrentUser.Id ? (_uploadButton?.Bottom ?? 200) + 20 : 200;

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
            int videoId = Convert.ToInt32(videoRow["id"]);
            bool isViewed = (bool)videoRow["is_viewed"];
            bool isLiked = (bool)videoRow["is_liked"];
            var videoControl = new VideoPlayerControl(videoUrl, videoId, Program.CurrentUser.Id, isViewed, isLiked)
            {
                Width = 300,
                Height = 375,
                Tag = videoId,
                Top = topPosition,
                Left = (contentPanel.ClientSize.Width - 300) / 2
            };

            videoControl.SetVideoInfo(
                $"Видео №{videoIndex}",
                videoRow["muscle_group_name"].ToString(),
                videoRow["exercise_name"].ToString()
            );

            int likes = _videoRepository.GetLikeCount(videoId);
            int comments = _commentRepository.GetCommentsCount(videoId);
            int views = _videoRepository.GetViewsCount(videoId);

            videoControl.SetMetrics(likes, comments, views);

            videoControl.Click += VideoControl_Click;
            videoControl.LikeClicked += VideoControl_LikeClicked;
            videoControl.ViewAdded += VideoControl_ViewAdded;
            videoControl.CommentClicked += VideoControl_CommentClicked;

            return videoControl;
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
        private void VideoControl_ViewAdded(object sender, int videoId)
        {
            _videoRepository.AddView(videoId, Program.CurrentUser.Id);
            var control = (VideoPlayerControl)sender;
            control.SetMetrics(
                _videoRepository.GetLikeCount(videoId),
                _commentRepository.GetCommentsCount(videoId, false),
                _videoRepository.GetViewsCount(videoId));
        }

        private void VideoControl_LikeClicked(object sender, int videoId)
        {
            _videoRepository.ToggleLike(videoId, Program.CurrentUser.Id);
            var control = (VideoPlayerControl)sender;
            control.SetMetrics(
                _videoRepository.GetLikeCount(videoId),
                _commentRepository.GetCommentsCount(videoId, false),
                _videoRepository.GetViewsCount(videoId));
        }

        private void VideoControl_CommentClicked(object sender, int videoId)
        {
            var commentsForm = new CommentsForm(videoId, false, true);
            commentsForm.ShowDialog();

            // Обновляем счетчик комментариев после закрытия формы комментариев
            var control = (VideoPlayerControl)sender;
            control.SetMetrics(
                _videoRepository.GetLikeCount(videoId),
                _commentRepository.GetCommentsCount(videoId, false),
                _videoRepository.GetViewsCount(videoId));
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

                        bool success = _videoRepository.AddVideo(_userId, uploadForm.SelectedExerciseId, uploadedPublicId);

                        if (!success)
                        {
                            MessageBox.Show("Ошибка сохранения видео в базе данных", "Ошибка",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        MessageBox.Show("Видео успешно загружено!", "Успех",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

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