namespace BiomechanicNetwork.Forms.AdminForms
{
    partial class ExercisesForm
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
            this.dataGridViewExercises = new System.Windows.Forms.DataGridView();
            this.btnAddExercise = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExercises)).BeginInit();
            this.SuspendLayout();

            // dataGridViewExercises
            this.dataGridViewExercises.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewExercises.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExercises.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewExercises.Name = "dataGridViewExercises";
            this.dataGridViewExercises.Size = new System.Drawing.Size(760, 400);
            this.dataGridViewExercises.TabIndex = 0;
            this.dataGridViewExercises.CellFormatting += this.dataGridViewExercises_CellFormatting;
            this.dataGridViewExercises.CellContentClick += this.dataGridViewExercises_CellContentClick;

            // btnAddExercise
            this.btnAddExercise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddExercise.Location = new System.Drawing.Point(622, 12);
            this.btnAddExercise.Name = "btnAddExercise";
            this.btnAddExercise.Size = new System.Drawing.Size(150, 30);
            this.btnAddExercise.TabIndex = 1;
            this.btnAddExercise.Text = "Добавить упражнение";
            this.btnAddExercise.UseVisualStyleBackColor = true;
            this.btnAddExercise.Click += this.btnAddExercise_Click;

            // ExercisesForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.btnAddExercise);
            this.Controls.Add(this.dataGridViewExercises);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "ExercisesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Управление упражнениями";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExercises)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewExercises;
        private System.Windows.Forms.Button btnAddExercise;
    }
}