using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Database.Repositories.BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Forms.AdminForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.AdminForms
{
    public partial class ExercisesForm : Form
    {
        private readonly AdminRepository _adminRepository;
        private readonly int _muscleGroupId;
        private readonly string _muscleGroupName;

        public ExercisesForm(int muscleGroupId, string muscleGroupName)
        {
            InitializeComponent();
            _adminRepository = new AdminRepository();
            _muscleGroupId = muscleGroupId;
            _muscleGroupName = muscleGroupName;

            Text = $"Упражнения для группы: {_muscleGroupName}";
            LoadExercises();
        }

        private void LoadExercises()
        {
            var exercises = _adminRepository.GetExercisesByMuscleGroup(_muscleGroupId);
            dataGridViewExercises.DataSource = exercises;
            ConfigureExercisesGrid();
        }

        private void ConfigureExercisesGrid()
        {
            dataGridViewExercises.AutoGenerateColumns = false;
            dataGridViewExercises.Columns.Clear();

            dataGridViewExercises.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridViewExercises.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "name",
                HeaderText = "Название",
                Width = 200
            });

            dataGridViewExercises.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "recommendations",
                HeaderText = "Рекомендации",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            var videoColumn = new DataGridViewImageColumn
            {
                DataPropertyName = "video_public_id",
                HeaderText = "Видео",
                Width = 150,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridViewExercises.Columns.Add(videoColumn);

            var editButtonColumn = new DataGridViewButtonColumn
            {
                Text = "Изменить",
                UseColumnTextForButtonValue = true,
                HeaderText = "Действие",
                Width = 100
            };
            dataGridViewExercises.Columns.Add(editButtonColumn);
        }

        private void dataGridViewExercises_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.Value != null && e.Value != DBNull.Value) // Колонка с видео
            {
                // Можно показать иконку видео или первый кадр
                e.Value = Properties.Resources.cancel; // Замените на свою иконку
            }
        }

        private void dataGridViewExercises_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 4) // Кнопка "Изменить"
            {
                var exerciseId = Convert.ToInt32(dataGridViewExercises.Rows[e.RowIndex].Cells["id"].Value);
                var exerciseName = dataGridViewExercises.Rows[e.RowIndex].Cells["name"].Value.ToString();
                var recommendations = dataGridViewExercises.Rows[e.RowIndex].Cells["recommendations"].Value?.ToString();

                var editForm = new ExerciseEditForm(_muscleGroupId, exerciseId, exerciseName, recommendations);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadExercises();
                }
            }
        }

        private void btnAddExercise_Click(object sender, EventArgs e)
        {
            var editForm = new ExerciseEditForm(_muscleGroupId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadExercises();
            }
        }
    }
}