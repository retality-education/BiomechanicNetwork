using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiomechanicNetwork.Forms
{
    internal partial class VideoUploadForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FilePath { get; private set; }

        private Button _browseButton;
        private Label _fileLabel;
        private Button _uploadButton;

        public VideoUploadForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Загрузка видео";
            this.Size = new Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

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

            _uploadButton = new Button
            {
                Text = "Загрузить",
                Width = 100,
                Top = 70,
                Left = 150,
                Enabled = false
            };
            _uploadButton.Click += UploadButton_Click;

            this.Controls.Add(_browseButton);
            this.Controls.Add(_fileLabel);
            this.Controls.Add(_uploadButton);
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
