using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.ExtraControls;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Models.Data;
using Npgsql;
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

        public ExerciseForm()
        {
            header.Title = "Упражнения";
            _muscleGroupRepo = new MuscleGroupRepository();

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
                    Tag = group.Name
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

            // Кнопка "Назад"
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

            // Заголовок с названием группы (стилизованный)
            var titleLabel = new Label
            {
                Text = groupData.Name,
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255), // Голубой цвет
                AutoSize = true,
                Location = new Point((currentExercisesPanel.Width - 200) / 2, 25),
                BackColor = Color.Transparent
            };
            currentExercisesPanel.Controls.Add(titleLabel);

            // Отображение упражнений
            int xPos = 20;
            int yPos = 80;
            int videoWidth = (currentExercisesPanel.Width - 60) / 2;
            if (videoWidth > 350) videoWidth = 350;
            int videoHeight = 350;

            foreach (var exercise in groupData.Exercises)
            {
                var videoControl = new VideoPlayerControl("test.mp4", false)
                {
                    Width = 300,
                    Height = videoHeight,
                    Location = new Point(xPos, yPos)
                };
                videoControl.SetVideoInfo("", exercise.Name, "");
                videoControl.SetMetrics(0, 0, 0);
                videoControl.Click += VideoControl_Click;

                currentExercisesPanel.Controls.Add(videoControl);
                videoControls.Add(videoControl);

                // Кнопка комментария для экспертов
                if (Program.CurrentUser.Role == UserRole.Expert)
                {
                    var commentButton = new Button
                    {
                        Text = "Добавить комментарий",
                        Size = new Size(videoWidth - 20, 30),
                        Location = new Point(xPos, yPos + videoHeight + 10),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.FromArgb(70, 70, 70),
                        ForeColor = Color.White
                    };
                    commentButton.Click += (s, args) =>
                    {
                        MessageBox.Show($"Комментарий к упражнению {exercise.Recommendations}");
                    };
                    currentExercisesPanel.Controls.Add(commentButton);
                }

                // Переход на следующую строку после каждых 2 упражнений
                xPos += videoWidth + 20;
                if (xPos + videoWidth > currentExercisesPanel.Width - 20)
                {
                    xPos = 20;
                    yPos += videoHeight + 50;
                }
            }

            Thread.Sleep(100);
            PauseAllVideos();
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