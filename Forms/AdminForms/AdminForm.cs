using BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Database.Repositories.BiomechanicNetwork.Database.Repositories;
using BiomechanicNetwork.Forms.MainForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms.AdminForms
{
    internal partial class AdminForm : Form
    {
        private readonly AdminRepository _adminRepository;
        private readonly int _currentAdminId;

        public AdminForm(int currentAdminId)
        {
            InitializeComponent();
            _adminRepository = new AdminRepository();
            _currentAdminId = currentAdminId;

            InitializeTabs();
            LoadUsers();
            LoadRoles();
            LoadSuggestions();
            LoadMuscleGroups();
        }

        private void InitializeTabs()
        {
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
        }

        #region Управление пользователями
        private void LoadRoles()
        {
            var roles = _adminRepository.GetAllRoles();

            // Настройка ComboBox в DataGridView
            var roleColumn = (DataGridViewComboBoxColumn)dataGridViewUsers.Columns["role_id"];
            roleColumn.DataSource = roles;
            roleColumn.DisplayMember = "name";
            roleColumn.ValueMember = "id";

            // Настройка ComboBox для фильтрации
            comboBoxRolesFilter.DataSource = roles;
            comboBoxRolesFilter.DisplayMember = "name";
            comboBoxRolesFilter.ValueMember = "id";
            comboBoxRolesFilter.SelectedIndex = -1;
        }
        private void LoadUsers()
        {
            var users = _adminRepository.GetAllUsersExceptAdmin(_currentAdminId);
            dataGridViewUsers.DataSource = users;
            ConfigureUsersGrid();
        }

        private void ConfigureUsersGrid()
        {
            dataGridViewUsers.AutoGenerateColumns = false;
            dataGridViewUsers.Columns.Clear();

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "username",
                Name = "username",
                HeaderText = "Логин",
                Width = 100
            });

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "name",
                Name = "name",
                HeaderText = "Имя",
                Width = 150
            });

            var roleColumn = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "role_id",
                Name = "role_id",
                HeaderText = "Роль",
                Width = 120
            };
            dataGridViewUsers.Columns.Add(roleColumn);

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "created_at",
                Name = "created_at",
                HeaderText = "Дата регистрации",
                Width = 150
            });
        }

        private void dataGridViewUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 3) // Колонка с ролью
            {
                var userId = Convert.ToInt32(dataGridViewUsers.Rows[e.RowIndex].Cells["ID"].Value);
                var roleId = Convert.ToInt32(dataGridViewUsers.Rows[e.RowIndex].Cells["role_id"].Value);

                if (_adminRepository.UpdateUserRole(userId, roleId))
                {
                    MessageBox.Show("Роль пользователя успешно обновлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось обновить роль пользователя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnShowUsers_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageUsers;
        }
        #endregion

        #region Управление предложениями
        private void LoadSuggestions()
        {
            var suggestions = _adminRepository.GetUnresolvedSuggestions();
            dataGridViewSuggestions.DataSource = suggestions;
            ConfigureSuggestionsGrid();
        }

        private void ConfigureSuggestionsGrid()
        {
            dataGridViewSuggestions.AutoGenerateColumns = false;
            dataGridViewSuggestions.Columns.Clear();

            dataGridViewSuggestions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                Name = "id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridViewSuggestions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "user_name",
                DataPropertyName = "user_name",
                HeaderText = "Пользователь",
                Width = 150
            });

            dataGridViewSuggestions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "topic",
                DataPropertyName = "topic",
                HeaderText = "Тема",
                Width = 150
            });

            dataGridViewSuggestions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "comment",
                DataPropertyName = "comment",
                HeaderText = "Комментарий",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dataGridViewSuggestions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "suggestion_date",
                DataPropertyName = "suggestion_date",
                HeaderText = "Дата",
                Width = 150
            });

            var resolveButtonColumn = new DataGridViewButtonColumn
            {
                Text = "Отметить",
                UseColumnTextForButtonValue = true,
                HeaderText = "Действие",
                Width = 100
            };
            dataGridViewSuggestions.Columns.Add(resolveButtonColumn);
        }

        private void dataGridViewSuggestions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 5) // Кнопка "Отметить"
            {
                var suggestionId = Convert.ToInt32(dataGridViewSuggestions.Rows[e.RowIndex].Cells["id"].Value);

                if (_adminRepository.MarkSuggestionAsResolved(suggestionId, _currentAdminId))
                {
                    MessageBox.Show("Предложение отмечено как рассмотренное", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSuggestions();
                }
                else
                {
                    MessageBox.Show("Не удалось отметить предложение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnShowSuggestions_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageSuggestions;
        }
        #endregion

        #region Управление группами мышц и упражнениями
        private void LoadMuscleGroups()
        {
            var muscleGroups = _adminRepository.GetAllMuscleGroups();
            dataGridViewMuscleGroups.DataSource = muscleGroups;
            ConfigureMuscleGroupsGrid();
        }

        private void ConfigureMuscleGroupsGrid()
        {
            dataGridViewMuscleGroups.AutoGenerateColumns = false;
            dataGridViewMuscleGroups.Columns.Clear();

            dataGridViewMuscleGroups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridViewMuscleGroups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "name",
                DataPropertyName = "name",
                HeaderText = "Название",
                Width = 200
            });

            var imageColumn = new DataGridViewImageColumn
            {
                Name = "image_public_id",
                DataPropertyName = "image_public_id",
                HeaderText = "Изображение",
                Width = 150,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridViewMuscleGroups.Columns.Add(imageColumn);

            var editButtonColumn = new DataGridViewButtonColumn
            {
                Text = "Изменить",
                UseColumnTextForButtonValue = true,
                HeaderText = "Действие",
                Width = 100
            };
            dataGridViewMuscleGroups.Columns.Add(editButtonColumn);

            var exercisesButtonColumn = new DataGridViewButtonColumn
            {
                Text = "Упражнения",
                UseColumnTextForButtonValue = true,
                HeaderText = "Упражнения",
                Width = 100
            };
            dataGridViewMuscleGroups.Columns.Add(exercisesButtonColumn);
        }

        private void dataGridViewMuscleGroups_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value != null && e.Value != DBNull.Value) // Колонка с изображением
            {
                var publicId = e.Value.ToString();
                e.Value = _adminRepository.GetImage(publicId);
            }
        }

        private void dataGridViewMuscleGroups_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var muscleGroupId = Convert.ToInt32(dataGridViewMuscleGroups.Rows[e.RowIndex].Cells["id"].Value);
                var muscleGroupName = dataGridViewMuscleGroups.Rows[e.RowIndex].Cells["name"].Value.ToString();

                if (e.ColumnIndex == 3) // Кнопка "Изменить"
                {
                    var editForm = new MuscleGroupEditForm(muscleGroupId, muscleGroupName);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadMuscleGroups();
                    }
                }
                else if (e.ColumnIndex == 4) // Кнопка "Упражнения"
                {
                    var exercisesForm = new ExercisesForm(muscleGroupId, muscleGroupName);
                    exercisesForm.ShowDialog();
                }
            }
        }

        private void btnAddMuscleGroup_Click(object sender, EventArgs e)
        {
            var editForm = new MuscleGroupEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadMuscleGroups();
            }
        }

        private void btnShowMuscleGroups_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageMuscleGroups;
        }
        #endregion
    }
}