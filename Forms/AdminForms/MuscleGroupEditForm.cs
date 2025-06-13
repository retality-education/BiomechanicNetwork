using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Database.Repositories.BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Services;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BiomechanicNetwork.Forms.AdminForms
{
    internal partial class MuscleGroupEditForm : Form
    {
        private readonly AdminRepository _adminRepository;
        private readonly int? _muscleGroupId;
        private string _imagePublicId;

        public MuscleGroupEditForm(int? muscleGroupId = null, string name = "")
        {
            InitializeComponent();
            _adminRepository = new AdminRepository();
            _muscleGroupId = muscleGroupId;

            if (_muscleGroupId.HasValue)
            {
                Text = "Редактирование группы мышц";
                txtName.Text = name;
            }
            else
            {
                Text = "Добавление новой группы мышц";
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var cloudinaryService = new CloudinaryService();
                    _imagePublicId = cloudinaryService.UploadImage(openFileDialog.FileName, "muscle_groups");

                    var image = Image.FromFile(openFileDialog.FileName);
                    pictureBox.Image = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название группы мышц", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool result;
            if (_muscleGroupId.HasValue)
            {
                result = _adminRepository.UpdateMuscleGroup(
                    _muscleGroupId.Value,
                    txtName.Text,
                    _imagePublicId);
            }
            else
            {
                result = _adminRepository.AddMuscleGroup(
                    txtName.Text,
                    _imagePublicId);
            }

            if (result)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить группу мышц", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}