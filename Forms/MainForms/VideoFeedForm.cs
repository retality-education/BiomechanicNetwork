using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.ExtraControls;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.MainForms
{
    internal partial class VideoFeedForm : BaseForm
    {
        private List<VideoPlayerControl> videoControls = new List<VideoPlayerControl>();
        private VideoRepository _videoRepository;
        private CommentRepository _commentRepository;

        public VideoFeedForm()
        {
            header.Title = "Видео лента";
            _videoRepository = new VideoRepository();
            _commentRepository = new CommentRepository();
            LoadRealVideos();

            Thread.Sleep(100);
            PauseAllVideos();
        }

        private void LoadRealVideos()
        {
            int topPosition = 20;
            var currentUserId = Program.CurrentUser.Id;
            var videos = _videoRepository.GetFeed(currentUserId);

            foreach (DataRow row in videos.Rows)
            {
                var videoId = Convert.ToInt32(row["id"]);
                var isViewed = Convert.ToBoolean(row["is_viewed"]);
                var isLiked = Convert.ToBoolean(row["is_liked"]);

                var videoControl = new VideoPlayerControl(
                    GetVideoUrl(row["video_public_id"].ToString()),
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
                    row["user_name"].ToString(),
                    row["exercise_name"].ToString(),
                    row["muscle_group_name"].ToString(),
                    Convert.ToInt32(row["user_id"]));

                videoControl.SetMetrics(
                    _videoRepository.GetLikeCount(videoId),
                    _commentRepository.GetCommentsCount(videoId, false),
                    _videoRepository.GetViewsCount(videoId));

                videoControl.TitleClicked += VideoControl_TitleClicked;
                videoControl.Click += VideoControl_Click;
                videoControl.LikeClicked += VideoControl_LikeClicked;
                videoControl.ViewAdded += VideoControl_ViewAdded;
                videoControl.CommentClicked += VideoControl_CommentClicked;

                videoControl.Left = (contentPanel.ClientSize.Width - videoControl.Width) / 2;
                videoControl.Top = topPosition;

                contentPanel.Controls.Add(videoControl);
                videoControls.Add(videoControl);

                topPosition += videoControl.Height + 20;
            }
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

        private void VideoControl_TitleClicked(object sender, EventArgs e)
        {
            var videoControl = (VideoPlayerControl)sender;
            if (videoControl.AuthorId.HasValue)
            {
                var profileForm = new ProfileForm(videoControl.AuthorId.Value);
                profileForm.Show();
                this.Close();
            }
        }

        private string GetVideoUrl(string publicId)
        {
            return $"http://res.cloudinary.com/{SecretsManager.GetCloudinaryCloudName()}/video/upload/{publicId}";
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

        private void PauseAllVideos()
        {
            foreach (var control in videoControls)
            {
                control.PauseVideo();
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