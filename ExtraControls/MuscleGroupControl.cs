using BiomechanicNetwork.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiomechanicNetwork.ExtraControls
{
    internal partial class MuscleGroupControl : UserControl
    {
        private PictureBox pictureBox;
        private Label label;
        private Button button;

        public event EventHandler GroupClicked;

        public List<Exercise> ExerciseList = new();

        public MuscleGroupControl(string groupName, Image image)
        {
            this.Size = new Size(200, 250);
            this.BackColor = Color.FromArgb(50, 50, 50);

            // Картинка группы мышц
            pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(180, 150),
                Location = new Point(10, 10),
                BackColor = Color.Transparent
            };

            // Название группы
            label = new Label
            {
                Text = groupName,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(180, 30),
                Location = new Point(10, 170)
            };

            // Кнопка (на самом деле прозрачная поверх всего контрола)
            button = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Size = this.Size,
                Location = Point.Empty
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 90);
            button.Click += (s, e) => GroupClicked?.Invoke(this, EventArgs.Empty);

            this.Controls.Add(pictureBox);
            this.Controls.Add(label);
            this.Controls.Add(button);
        }
    }
}
