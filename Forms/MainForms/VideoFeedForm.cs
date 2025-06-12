using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.ExtraControls;
using BiomechanicNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.MainForms
{
    internal partial class VideoFeedForm : BaseForm
    {
        private List<VideoPlayerControl> videoControls = new List<VideoPlayerControl>();
        private VideoRepository _videoRepository;

        public VideoFeedForm()
        {
            header.Title = "Видео лента";
            _videoRepository = new VideoRepository();
            LoadRealVideos();

            Thread.Sleep(100);
            PauseAllVideos();
        }

        private void LoadRealVideos()
        {
            int topPosition = 20;

            // Получаем реальные видео из базы данных
            var videos = _videoRepository.GetFeed();

            foreach (DataRow row in videos.Rows)
            {
                var videoControl = new VideoPlayerControl(GetVideoUrl(row["video_public_id"].ToString()))
                {
                    Width = 300,
                    Height = 400,
                    Tag = row["id"]
                };

                // Устанавливаем информацию о видео, включая ID автора
                videoControl.SetVideoInfo(
                    row["user_name"].ToString(),
                    row["exercise_name"].ToString(),
                    Convert.ToDateTime(row["muscle_group_name"]).ToString(),
                    Convert.ToInt32(row["user_id"])); // Здесь передаем ID пользователя

                // Устанавливаем метрики (лайки, комментарии, просмотры)
                videoControl.SetMetrics(
                    _videoRepository.GetLikeCount(Convert.ToInt32(row["id"])),
                    0, // Можно добавить получение количества комментариев
                    0); // Можно добавить получение количества просмотров

                // Подписываемся на событие клика по заголовку
                videoControl.TitleClicked += VideoControl_TitleClicked;
                videoControl.Click += VideoControl_Click;

                // Центрирование
                videoControl.Left = (contentPanel.ClientSize.Width - videoControl.Width) / 2;
                videoControl.Top = topPosition;

                contentPanel.Controls.Add(videoControl);
                videoControls.Add(videoControl);

                topPosition += videoControl.Height + 20;
            }
        }

        private void VideoControl_TitleClicked(object sender, EventArgs e)
        {
            var videoControl = (VideoPlayerControl)sender;

            // Проверяем, есть ли ID автора
            if (videoControl.AuthorId.HasValue)
            {
                // Открываем профиль автора
                var profileForm = new ProfileForm(videoControl.AuthorId.Value);
                profileForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Информация об авторе недоступна", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
