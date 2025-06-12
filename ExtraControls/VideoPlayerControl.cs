using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace BiomechanicNetwork.ExtraControls
{
    public partial class VideoPlayerControl : UserControl
    {
        // Элементы управления и медиа
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private VideoView _videoView;
        private Media _media;

        // Верхние метки
        private Label _lblTitle;
        private Label _lblAuthor;
        private Label _lblDate;

        // Нижняя панель
        private Panel _bottomPanel;
        private Button _btnLikes;
        private Button _btnComments;
        private Button _btnViews;
        private Button _btnPause;
        private Button _btnFullscreen;

        // Состояние
        private string _videoPath;
        private long _lastPosition = 0;
        private bool _isPlaying = true;
        private bool _isViewed = false;
        private bool _isLiked = false;
        private int _videoId;
        private int _currentUserId;

        // События
        public event EventHandler TitleClicked;
        public event EventHandler<int> LikeClicked;
        public event EventHandler<int> CommentClicked;
        public event EventHandler<int> ViewAdded;

        public VideoPlayerControl(string videoPath, int videoId, int currentUserId, bool isViewed, bool isLiked, bool showTitle = true)
        {
            _videoPath = videoPath;
            _videoId = videoId;
            _currentUserId = currentUserId;
            _isViewed = isViewed;
            _isLiked = isLiked;

            InitializeComponent();
            InitializeControls(showTitle);
            InitializeVLC();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? AuthorId { get; set; }
        public bool IsPlaying() => _isPlaying;

        private void InitializeControls(bool showTitle)
        {
            this.Size = new Size(300, 450);
            this.BackColor = Color.White;

            // Верхние метки
            _lblTitle = new Label
            {
                Text = "Название видео",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            _lblTitle.Click += (s, e) => TitleClicked?.Invoke(this, e);

            _lblAuthor = new Label
            {
                Text = "Автор видео",
                Font = new Font("Arial", 8),
                Location = new Point(10, 30),
                AutoSize = true
            };

            _lblDate = new Label
            {
                Text = DateTime.Now.ToShortDateString(),
                Font = new Font("Arial", 8),
                Location = new Point(10, 50),
                AutoSize = true
            };

            // Видеоплеер
            _videoView = new VideoView
            {
                Size = new Size(280, 200),
                Location = new Point(10, 80),
                BackColor = Color.Black,
                Cursor = Cursors.Hand
            };
            this.Click += VideoView_Click;

            // Нижняя панель
            _bottomPanel = new Panel
            {
                Size = new Size(280, 80),
                Location = new Point(10, 290),
                BackColor = Color.LightGray
            };

            // Кнопки метрик
            _btnLikes = CreateMetricButton("👍 0", 10);
            _btnLikes.Click += (s, e) => {
                _isLiked = !_isLiked;
                UpdateLikeButton();
                LikeClicked?.Invoke(this, _videoId);
            };
            UpdateLikeButton();

            _btnComments = CreateMetricButton("💬 0", 100);
            _btnComments.Click += (s, e) => CommentClicked?.Invoke(this, _videoId);

            _btnViews = CreateMetricButton("👁️ 0", 190);
            _btnViews.Enabled = false;

            // Кнопка паузы
            _btnPause = new Button
            {
                Text = "⏸ Пауза",
                Size = new Size(120, 30),
                Location = new Point(20, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            _btnPause.Click += (s, e) => TogglePause();

            // Кнопка полного экрана
            _btnFullscreen = new Button
            {
                Text = "⛶ Полный экран",
                Size = new Size(120, 30),
                Location = new Point(150, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            _btnFullscreen.Click += (s, e) => ShowFullscreen();

            // Добавление элементов
            _bottomPanel.Controls.Add(_btnLikes);
            _bottomPanel.Controls.Add(_btnComments);
            _bottomPanel.Controls.Add(_btnViews);
            _bottomPanel.Controls.Add(_btnPause);
            _bottomPanel.Controls.Add(_btnFullscreen);

            this.Controls.Add(_lblTitle);
            this.Controls.Add(_lblAuthor);
            this.Controls.Add(_lblDate);
            this.Controls.Add(_videoView);
            this.Controls.Add(_bottomPanel);

            if (!showTitle)
            {
                _lblTitle.Visible = false;
                var offset = -_lblTitle.Height;
                _lblAuthor.Top += offset;
                _lblDate.Top += offset;
                _videoView.Top += offset;
                _bottomPanel.Top += offset;
                this.Height += offset;
            }
        }

        private void UpdateLikeButton()
        {
            _btnLikes.BackColor = _isLiked ? Color.LightCoral : Color.White;
            _btnLikes.ForeColor = _isLiked ? Color.DarkRed : SystemColors.ControlText;
        }

        private Button CreateMetricButton(string text, int x)
        {
            return new Button
            {
                Text = text,
                Size = new Size(80, 30),
                Location = new Point(x, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Font = new Font("Arial", 8)
            };
        }

        private void InitializeVLC()
        {
            try
            {
                Core.Initialize();
                _libVLC = new LibVLC("--network-caching=5000");
                _mediaPlayer = new MediaPlayer(_libVLC);
                _videoView.MediaPlayer = _mediaPlayer;

                _media = new Media(_libVLC, new Uri(_videoPath), ":input-repeat=65535");
                _mediaPlayer.Play(_media);
                _mediaPlayer.Pause(); // Начинаем с паузы
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}");
            }
        }

        private void VideoView_Click(object sender, EventArgs e)
        {
            if (!_isViewed)
            {
                _isViewed = true;
                ViewAdded?.Invoke(this, _videoId);
            }
            TogglePause();
        }

        public void TogglePause()
        {
            if (_isPlaying)
            {
                _mediaPlayer.Pause();
                _btnPause.Text = "▶ Воспроизвести";
            }
            else
            {
                _mediaPlayer.Play();
                _btnPause.Text = "⏸ Пауза";
            }
            _isPlaying = !_isPlaying;
        }

        public void SetVideoInfo(string title, string author, string date, int? authorId = null)
        {
            _lblTitle.Text = title;
            _lblAuthor.Text = author;
            _lblDate.Text = date;
            AuthorId = authorId;
        }

        public void SetMetrics(int likes, int comments, int views)
        {
            _btnLikes.Text = $"👍 {likes}";
            _btnComments.Text = $"💬 {comments}";
            _btnViews.Text = $"👁️ {views}";
        }

        public void PlayVideo()
        {
            if (!_isPlaying)
            {
                _mediaPlayer.Play();
                _isPlaying = true;
                _btnPause.Text = "⏸ Пауза";
            }
        }

        public void PauseVideo()
        {
            if (_isPlaying)
            {
                _mediaPlayer.Pause();
                _isPlaying = false;
                _btnPause.Text = "▶ Воспроизвести";
            }
        }

        private void ShowFullscreen()
        {
            // Сохраняем текущую позицию и состояние
            _lastPosition = _mediaPlayer.Time;
            bool wasPlaying = _isPlaying;

            // Останавливаем основной плеер
            _mediaPlayer.Stop();

            // Создаем полноэкранную форму
            var fullscreenForm = new Form
            {
                WindowState = FormWindowState.Maximized,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                Text = _lblTitle.Text
            };

            // Создаем новый VideoView для полноэкранного режима
            var fullscreenView = new VideoView
            {
                Dock = DockStyle.Fill,
                MediaPlayer = new MediaPlayer(_libVLC)
            };

            // Настраиваем медиа
            var fullscreenMedia = new Media(_libVLC, new Uri(_videoPath), ":input-repeat=65535");
            fullscreenView.MediaPlayer.Play(fullscreenMedia);
            fullscreenView.MediaPlayer.Time = _lastPosition;

            // Если видео было на паузе, ставим на паузу и в полноэкранном режиме
            if (!wasPlaying)
            {
                fullscreenView.MediaPlayer.Pause();
            }

            // Обработка закрытия формы
            fullscreenForm.FormClosed += (s, e) =>
            {
                // Сохраняем позицию перед закрытием
                _lastPosition = fullscreenView.MediaPlayer.Time;

                // Освобождаем ресурсы
                fullscreenView.MediaPlayer.Stop();
                fullscreenView.Dispose();
                fullscreenMedia.Dispose();

                // Восстанавливаем воспроизведение в основном контроле
                _media = new Media(_libVLC, new Uri(_videoPath), ":input-repeat=65535");
                _mediaPlayer.Play(_media);
                _mediaPlayer.Time = _lastPosition;

                // Восстанавливаем состояние паузы
                if (!wasPlaying)
                {
                    _mediaPlayer.Pause();
                }
            };

            // Обработка нажатия ESC
            fullscreenForm.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                    fullscreenForm.Close();
            };

            fullscreenForm.Controls.Add(fullscreenView);
            fullscreenForm.ShowDialog(); // Используем ShowDialog чтобы блокировать основной поток
        }
    }
}