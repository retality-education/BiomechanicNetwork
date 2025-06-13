namespace BiomechanicNetwork.Forms.AdminForms
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPageUsers = new TabPage();
            dataGridViewUsers = new DataGridView();
            tabPageSuggestions = new TabPage();
            dataGridViewSuggestions = new DataGridView();
            tabPageMuscleGroups = new TabPage();
            btnAddMuscleGroup = new Button();
            dataGridViewMuscleGroups = new DataGridView();
            btnShowUsers = new Button();
            btnShowSuggestions = new Button();
            btnShowMuscleGroups = new Button();
            comboBoxRolesFilter = new ComboBox();
            labelRoleFilter = new Label();
            tabControl1.SuspendLayout();
            tabPageUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).BeginInit();
            tabPageSuggestions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSuggestions).BeginInit();
            tabPageMuscleGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMuscleGroups).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPageUsers);
            tabControl1.Controls.Add(tabPageSuggestions);
            tabControl1.Controls.Add(tabPageMuscleGroups);
            tabControl1.Location = new Point(16, 77);
            tabControl1.Margin = new Padding(4, 5, 4, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1147, 769);
            tabControl1.TabIndex = 0;
            // 
            // tabPageUsers
            // 
            tabPageUsers.Controls.Add(dataGridViewUsers);
            tabPageUsers.Location = new Point(4, 29);
            tabPageUsers.Margin = new Padding(4, 5, 4, 5);
            tabPageUsers.Name = "tabPageUsers";
            tabPageUsers.Padding = new Padding(4, 5, 4, 5);
            tabPageUsers.Size = new Size(1139, 736);
            tabPageUsers.TabIndex = 0;
            tabPageUsers.Text = "Пользователи";
            tabPageUsers.UseVisualStyleBackColor = true;
            // 
            // dataGridViewUsers
            // 
            dataGridViewUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsers.Location = new Point(8, 9);
            dataGridViewUsers.Margin = new Padding(4, 5, 4, 5);
            dataGridViewUsers.Name = "dataGridViewUsers";
            dataGridViewUsers.RowHeadersWidth = 51;
            dataGridViewUsers.Size = new Size(1120, 711);
            dataGridViewUsers.TabIndex = 0;
            dataGridViewUsers.CellValueChanged += dataGridViewUsers_CellValueChanged;
            // 
            // tabPageSuggestions
            // 
            tabPageSuggestions.Controls.Add(dataGridViewSuggestions);
            tabPageSuggestions.Location = new Point(4, 29);
            tabPageSuggestions.Margin = new Padding(4, 5, 4, 5);
            tabPageSuggestions.Name = "tabPageSuggestions";
            tabPageSuggestions.Padding = new Padding(4, 5, 4, 5);
            tabPageSuggestions.Size = new Size(1139, 736);
            tabPageSuggestions.TabIndex = 1;
            tabPageSuggestions.Text = "Предложения";
            tabPageSuggestions.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSuggestions
            // 
            dataGridViewSuggestions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewSuggestions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSuggestions.Location = new Point(8, 9);
            dataGridViewSuggestions.Margin = new Padding(4, 5, 4, 5);
            dataGridViewSuggestions.Name = "dataGridViewSuggestions";
            dataGridViewSuggestions.RowHeadersWidth = 51;
            dataGridViewSuggestions.Size = new Size(1120, 711);
            dataGridViewSuggestions.TabIndex = 0;
            dataGridViewSuggestions.CellContentClick += dataGridViewSuggestions_CellContentClick;

            // 
            // tabPageMuscleGroups
            // 
            tabPageMuscleGroups.Controls.Add(btnAddMuscleGroup);
            tabPageMuscleGroups.Controls.Add(dataGridViewMuscleGroups);
            tabPageMuscleGroups.Location = new Point(4, 29);
            tabPageMuscleGroups.Margin = new Padding(4, 5, 4, 5);
            tabPageMuscleGroups.Name = "tabPageMuscleGroups";
            tabPageMuscleGroups.Padding = new Padding(4, 5, 4, 5);
            tabPageMuscleGroups.Size = new Size(1139, 736);
            tabPageMuscleGroups.TabIndex = 2;
            tabPageMuscleGroups.Text = "Группы мышц";
            tabPageMuscleGroups.UseVisualStyleBackColor = true;
            // 
            // btnAddMuscleGroup
            // 
            btnAddMuscleGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddMuscleGroup.Location = new Point(948, 9);
            btnAddMuscleGroup.Margin = new Padding(4, 5, 4, 5);
            btnAddMuscleGroup.Name = "btnAddMuscleGroup";
            btnAddMuscleGroup.Size = new Size(180, 35);
            btnAddMuscleGroup.TabIndex = 1;
            btnAddMuscleGroup.Text = "Добавить группу";
            btnAddMuscleGroup.UseVisualStyleBackColor = true;
            btnAddMuscleGroup.Click += btnAddMuscleGroup_Click;
            // 
            // dataGridViewMuscleGroups
            // 
            dataGridViewMuscleGroups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewMuscleGroups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewMuscleGroups.Location = new Point(8, 54);
            dataGridViewMuscleGroups.Margin = new Padding(4, 5, 4, 5);
            dataGridViewMuscleGroups.Name = "dataGridViewMuscleGroups";
            dataGridViewMuscleGroups.RowHeadersWidth = 51;
            dataGridViewMuscleGroups.Size = new Size(1120, 666);
            dataGridViewMuscleGroups.TabIndex = 0;
            dataGridViewMuscleGroups.CellContentClick += dataGridViewMuscleGroups_CellContentClick;
            dataGridViewMuscleGroups.CellFormatting += dataGridViewMuscleGroups_CellFormatting;
            // 
            // btnShowUsers
            // 
            btnShowUsers.Location = new Point(16, 18);
            btnShowUsers.Margin = new Padding(4, 5, 4, 5);
            btnShowUsers.Name = "btnShowUsers";
            btnShowUsers.Size = new Size(160, 46);
            btnShowUsers.TabIndex = 1;
            btnShowUsers.Text = "Пользователи";
            btnShowUsers.UseVisualStyleBackColor = true;
            btnShowUsers.Click += btnShowUsers_Click;
            // 
            // btnShowSuggestions
            // 
            btnShowSuggestions.Location = new Point(184, 18);
            btnShowSuggestions.Margin = new Padding(4, 5, 4, 5);
            btnShowSuggestions.Name = "btnShowSuggestions";
            btnShowSuggestions.Size = new Size(160, 46);
            btnShowSuggestions.TabIndex = 2;
            btnShowSuggestions.Text = "Предложения";
            btnShowSuggestions.UseVisualStyleBackColor = true;
            btnShowSuggestions.Click += btnShowSuggestions_Click;
            // 
            // btnShowMuscleGroups
            // 
            btnShowMuscleGroups.Location = new Point(352, 18);
            btnShowMuscleGroups.Margin = new Padding(4, 5, 4, 5);
            btnShowMuscleGroups.Name = "btnShowMuscleGroups";
            btnShowMuscleGroups.Size = new Size(200, 46);
            btnShowMuscleGroups.TabIndex = 3;
            btnShowMuscleGroups.Text = "Группы мышц";
            btnShowMuscleGroups.UseVisualStyleBackColor = true;
            btnShowMuscleGroups.Click += btnShowMuscleGroups_Click;
            // 
            // comboBoxRolesFilter
            // 
            comboBoxRolesFilter.FormattingEnabled = true;
            comboBoxRolesFilter.Location = new Point(560, 23);
            comboBoxRolesFilter.Margin = new Padding(4, 5, 4, 5);
            comboBoxRolesFilter.Name = "comboBoxRolesFilter";
            comboBoxRolesFilter.Size = new Size(199, 28);
            comboBoxRolesFilter.TabIndex = 4;
            // 
            // labelRoleFilter
            // 
            labelRoleFilter.AutoSize = true;
            labelRoleFilter.Location = new Point(560, 0);
            labelRoleFilter.Margin = new Padding(4, 0, 4, 0);
            labelRoleFilter.Name = "labelRoleFilter";
            labelRoleFilter.Size = new Size(124, 20);
            labelRoleFilter.TabIndex = 5;
            labelRoleFilter.Text = "Фильтр по роли:";
            // 
            // AdminForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1179, 865);
            Controls.Add(labelRoleFilter);
            Controls.Add(comboBoxRolesFilter);
            Controls.Add(btnShowMuscleGroups);
            Controls.Add(btnShowSuggestions);
            Controls.Add(btnShowUsers);
            Controls.Add(tabControl1);
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(1194, 898);
            Name = "AdminForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Административная панель";
            tabControl1.ResumeLayout(false);
            tabPageUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).EndInit();
            tabPageSuggestions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewSuggestions).EndInit();
            tabPageMuscleGroups.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewMuscleGroups).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageUsers;
        private System.Windows.Forms.DataGridView dataGridViewUsers;
        private System.Windows.Forms.TabPage tabPageSuggestions;
        private System.Windows.Forms.DataGridView dataGridViewSuggestions;
        private System.Windows.Forms.TabPage tabPageMuscleGroups;
        private System.Windows.Forms.DataGridView dataGridViewMuscleGroups;
        private System.Windows.Forms.Button btnShowUsers;
        private System.Windows.Forms.Button btnShowSuggestions;
        private System.Windows.Forms.Button btnShowMuscleGroups;
        private System.Windows.Forms.Button btnAddMuscleGroup;
        private System.Windows.Forms.ComboBox comboBoxRolesFilter;
        private System.Windows.Forms.Label labelRoleFilter;
    }
}