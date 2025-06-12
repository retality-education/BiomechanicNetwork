using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms
{
    internal partial class VideoUploadForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FilePath { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedExerciseId { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Description { get; private set; }

        private Button _browseButton;
        private Label _fileLabel;
        private Button _uploadButton;
        private ComboBox _muscleGroupCombo;
        private ComboBox _exerciseCombo;
        private TextBox _descriptionTextBox;
        private readonly MuscleGroupRepository _muscleGroupRepo;

        public VideoUploadForm()
        {
            _muscleGroupRepo = new MuscleGroupRepository();
            InitializeComponents();
            LoadMuscleGroups();
        }

        private void InitializeComponents()
        {
            this.Text = "Загрузка видео";
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Элементы для выбора файла
            _browseButton = new Button
            {
                Text = "Выбрать файл",
                Width = 100,
                Top = 20,
                Left = 20
            };
            _browseButton.Click += BrowseButton_Click;

            _fileLabel = new Label
            {
                Top = 25,
                Left = 130,
                Width = 250,
                Text = "Файл не выбран"
            };

            // Выбор группы мышц
            var muscleGroupLabel = new Label
            {
                Text = "Группа мышц:",
                Top = 60,
                Left = 20,
                AutoSize = true
            };

            _muscleGroupCombo = new ComboBox
            {
                Width = 200,
                Top = 60,
                Left = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _muscleGroupCombo.SelectedIndexChanged += MuscleGroupCombo_SelectedIndexChanged;

            // Выбор упражнения
            var exerciseLabel = new Label
            {
                Text = "Упражнение:",
                Top = 90,
                Left = 20,
                AutoSize = true
            };

            _exerciseCombo = new ComboBox
            {
                Width = 200,
                Top = 90,
                Left = 120,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };

            // Описание видео
            var descLabel = new Label
            {
                Text = "Описание:",
                Top = 120,
                Left = 20,
                AutoSize = true
            };

            _descriptionTextBox = new TextBox
            {
                Width = 200,
                Height = 60,
                Top = 120,
                Left = 120,
                Multiline = true
            };

            // Кнопка загрузки
            _uploadButton = new Button
            {
                Text = "Загрузить",
                Width = 100,
                Top = 200,
                Left = 150,
                Enabled = false
            };
            _uploadButton.Click += UploadButton_Click;

            this.Controls.AddRange(new Control[] {
                _browseButton, _fileLabel,
                muscleGroupLabel, _muscleGroupCombo,
                exerciseLabel, _exerciseCombo,
                descLabel, _descriptionTextBox,
                _uploadButton
            });
        }

        private void LoadMuscleGroups()
        {
            var muscleGroups = _muscleGroupRepo.GetAllWithExercises();

            _muscleGroupCombo.DisplayMember = "Name";
            _muscleGroupCombo.ValueMember = "Id";
            _muscleGroupCombo.DataSource = muscleGroups;
        }

        private void MuscleGroupCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_muscleGroupCombo.SelectedItem is MuscleGroupWithExercises selectedGroup)
            {
                _exerciseCombo.Enabled = selectedGroup.Exercises.Any();

                _exerciseCombo.DisplayMember = "Name";
                _exerciseCombo.ValueMember = "Id";
                _exerciseCombo.DataSource = selectedGroup.Exercises;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mov;*.wmv";
                openFileDialog.Title = "Выберите видео для загрузки";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FilePath = openFileDialog.FileName;
                    _fileLabel.Text = Path.GetFileName(FilePath);
                    _uploadButton.Enabled = true;
                }
            }
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (_exerciseCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите упражнение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedExerciseId = ((Exercise)_exerciseCombo.SelectedItem).Id;
            Description = _descriptionTextBox.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}