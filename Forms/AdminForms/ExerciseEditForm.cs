using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Database.Repositories.BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Services;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BiomechanicNetwork.Forms.AdminForms
{
    public partial class ExerciseEditForm : Form
    {
        private readonly AdminRepository _adminRepository;
        private readonly int? _exerciseId;
        private readonly int? _muscleGroupId;
        private string _videoPublicId;

        public ExerciseEditForm(int? muscleGroupId, int? exerciseId = null, string name = "", string recommendations = "")
        {
            InitializeComponent();
            _adminRepository = new AdminRepository();
            _exerciseId = exerciseId;
            _muscleGroupId = muscleGroupId;

            if (_exerciseId.HasValue)
            {
                Text = "Редактирование упражнения";
                txtName.Text = name;
                txtRecommendations.Text = recommendations;
            }
            else
            {
                Text = "Добавление нового упражнения";
            }
        }

        private void btnSelectVideo_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Video Files (*.mp4, *.mov, *.avi)|*.mp4;*.mov;*.avi",
                Title = "Выберите видео"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var cloudinaryService = new CloudinaryService();
                    _videoPublicId = cloudinaryService.UploadVideo(openFileDialog.FileName, "exercises");

                    // Можно показать превью видео или иконку
                    pictureBox.Image = Properties.Resources.cancel; // Замените на свою иконку
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке видео: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название упражнения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool result;
            if (_exerciseId.HasValue)
            {
                result = _adminRepository.UpdateExercise(
                    _exerciseId.Value,
                    txtName.Text,
                    _muscleGroupId,
                    txtRecommendations.Text,
                    _videoPublicId);
            }
            else
            {
                result = _adminRepository.AddExercise(
                    txtName.Text,
                    _muscleGroupId,
                    txtRecommendations.Text,
                    _videoPublicId);
            }

            if (result)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить упражнение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}