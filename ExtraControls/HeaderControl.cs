using BiomechanicNetwork.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace BiomechanicNetwork.ExtraControls
{
    public partial class HeaderControl : UserControl
    {
        public event EventHandler SettingsClicked;
        public event EventHandler CloseClicked;

        private PictureBox settingsIcon;
        private PictureBox closeIcon;
        private Label titleLabel;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get => titleLabel.Text;
            set => titleLabel.Text = value;
        }

        public HeaderControl()
        {
            InitializeHeader();
        }

        private void InitializeHeader()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Height = 50;
            this.Dock = DockStyle.Top;

            // Иконка настроек
            settingsIcon = new PictureBox
            {
                Image = Properties.Resources.settings,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(32, 32),
                Location = new Point(10, 9),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            settingsIcon.Click += (s, e) => {
                new SettingsForm(Program.CurrentUser.Id).Show();
                this.Hide();
            };

            // Иконка закрытия
            closeIcon = new PictureBox
            {
                Image = Properties.Resources.cancel,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(32, 32),
                Location = new Point(this.Width - 42, 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            closeIcon.Click += (s, e) => CloseClicked?.Invoke(this, e);

            // Заголовок
            titleLabel = new Label
            {
                Text = "RetalityApp",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point((this.Width - 100) / 2, 15)
            };

            this.Controls.Add(settingsIcon);
            this.Controls.Add(closeIcon);
            this.Controls.Add(titleLabel);

            this.Resize += (s, e) => {
                closeIcon.Left = this.Width - closeIcon.Width - 10;
                titleLabel.Left = (this.Width - titleLabel.Width) / 2;
            };
        }
    }
}
