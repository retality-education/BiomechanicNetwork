using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.ExtraControls;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Models.Data;
using BiomechanicNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.MainForms
{
    internal partial class ExerciseForm : BaseForm
    {
        private List<MuscleGroupWithExercises> _muscleGroups;
        private Panel exercisesPanel;
        private Panel currentExercisesPanel;
        private List<VideoPlayerControl> videoControls = new List<VideoPlayerControl>();
        private readonly MuscleGroupRepository _muscleGroupRepo;
        private readonly CommentRepository _commentRepository;
        private readonly VideoRepository _videoRepository;
        public ExerciseForm()
        {
            header.Title = "Упражнения";
            _muscleGroupRepo = new MuscleGroupRepository();
            _commentRepository = new CommentRepository();
            _videoRepository = new VideoRepository();
            LoadMuscleGroups();
            InitializeExercisesView();
            PauseAllVideos();
        }

        private void LoadMuscleGroups()
        {
            _muscleGroups = _muscleGroupRepo.GetAllWithExercises();
        }

        private void InitializeExercisesView()
        {
            exercisesPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };
            contentPanel.Controls.Add(exercisesPanel);

            int x = 80, y = 30;
            int controlWidth = 220;
            int controlHeight = 270;
            int spacing = 50;

            foreach (var group in _muscleGroups)
            {
                var groupControl = new MuscleGroupControl(group.Name, group.Image)
                {
                    Location = new Point(x, y),
                    Tag = group
                };
                groupControl.GroupClicked += MuscleGroup_Click;

                exercisesPanel.Controls.Add(groupControl);

                x += controlWidth + spacing + 100;
                if (x + controlWidth > exercisesPanel.ClientSize.Width - spacing)
                {
                    x = 80;
                    y += controlHeight + spacing;
                }
            }
        }

        private void MuscleGroup_Click(object sender, EventArgs e)
        {
            var control = (MuscleGroupControl)sender;
            var groupData = (MuscleGroupWithExercises)control.Tag;

            exercisesPanel.Visible = false;

            currentExercisesPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };
            contentPanel.Controls.Add(currentExercisesPanel);

            var backButton = new Button
            {
                Text = "← Назад к группам",
                Size = new Size(150, 40),
                Location = new Point(20, 20),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White
            };
            backButton.Click += (s, args) =>
            {
                contentPanel.Controls.Remove(currentExercisesPanel);
                exercisesPanel.Visible = true;
                PauseAllVideos();
            };
            currentExercisesPanel.Controls.Add(backButton);

            var titleLabel = new Label
            {
                Text = groupData.Name,
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                AutoSize = true,
                Location = new Point((currentExercisesPanel.Width - 200) / 2, 25),
                BackColor = Color.Transparent
            };
            currentExercisesPanel.Controls.Add(titleLabel);

            int xPos = 20;
            int yPos = 80;
            int videoWidth = (currentExercisesPanel.Width - 60) / 2;
            if (videoWidth > 350) videoWidth = 350;
            int videoHeight = 350;

            int topPosition = 80;
            var currentUserId = Program.CurrentUser.Id;
            var videos = _videoRepository.GetFeed(currentUserId);

            

            foreach (var row in groupData.Exercises)
            {
                var videoId = Convert.ToInt32(row.Id);
                var isViewed = Convert.ToBoolean(row.IsViewed);
                var isLiked = Convert.ToBoolean(row.IsLiked);

                var videoControl = new VideoPlayerControl(
                    GetVideoUrl(row.VideoPublicId),
                    videoId,
                    currentUserId,
                    isViewed,
                    isLiked)
                {
                    Width = 300,
                    Height = 375,
                    Tag = videoId
                };

                videoControl.SetVideoInfo(
                    groupData.Name,
                    row.Name,
                    row.Recommendations);

                videoControl.SetMetrics(
                    _videoRepository.GetLikeCountExercise(videoId),
                    _commentRepository.GetCommentsCount(videoId, false),
                    _videoRepository.GetViewsCountExercise(videoId));

                videoControl.Click += VideoControl_Click;
                videoControl.LikeClicked += VideoControl_LikeClicked;
                videoControl.ViewAdded += VideoControl_ViewAdded;
                videoControl.CommentClicked += VideoControl_CommentClicked;

                videoControl.Left = (contentPanel.ClientSize.Width - videoControl.Width) / 2;
                videoControl.Top = topPosition;

                currentExercisesPanel.Controls.Add(videoControl);
                videoControls.Add(videoControl);

                topPosition += videoControl.Height + 20;
            }
            Thread.Sleep(2000);
        }
        private void VideoControl_Click(object sender, EventArgs e)
        {
            var clickedControl = (VideoPlayerControl)sender;
            foreach (var control in videoControls)
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
            _videoRepository.AddViewExercise(videoId, Program.CurrentUser.Id);
            var control = (VideoPlayerControl)sender;
            control.SetMetrics(
                _videoRepository.GetLikeCountExercise(videoId),
                _commentRepository.GetCommentsCount(videoId, false),
                _videoRepository.GetViewsCountExercise(videoId));
        }

        private void VideoControl_LikeClicked(object sender, int videoId)
        {
            _videoRepository.ToggleLikeExercise(videoId, Program.CurrentUser.Id);
            var control = (VideoPlayerControl)sender;
            control.SetMetrics(
                _videoRepository.GetLikeCountExercise(videoId),
                _commentRepository.GetCommentsCount(videoId, false),
                _videoRepository.GetViewsCountExercise(videoId));
        }

        private void VideoControl_CommentClicked(object sender, int videoId)
        {
            var commentsForm = new CommentsForm(videoId, false, Program.CurrentUser.Role == UserRole.Expert);
            commentsForm.ShowDialog();

            // Обновляем счетчик комментариев после закрытия формы комментариев
            var control = (VideoPlayerControl)sender;
            control.SetMetrics(
                _videoRepository.GetLikeCountExercise(videoId),
                _commentRepository.GetCommentsCount(videoId, false),
                _videoRepository.GetViewsCountExercise(videoId));
        }

        private void PauseAllVideos()
        {
            foreach (var control in videoControls)
            {
                control.PauseVideo();
            }
        }

        private string GetVideoUrl(string publicId)
        {
            return $"http://res.cloudinary.com/{SecretsManager.GetCloudinaryCloudName()}/video/upload/{publicId}";
        }

        protected override void OnNavigationRequested(string formName)
        {
            PauseAllVideos();
            videoControls.Clear();

            switch (formName)
            {
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