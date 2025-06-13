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
    public partial class NavigationControl : UserControl
    {
        public event Action<string> NavigateRequested;

        private Button[] navButtons;

        public NavigationControl()
        {
            Height = 60;  // Увеличил высоту
            BackColor = Color.FromArgb(30, 30, 30);

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            string[] tabs = { "Упражнения", "Рекомендации", "Профиль", "Поддержка" };
            navButtons = new Button[tabs.Length];

            // Увеличил ширину кнопок и добавил авто-размер
            int buttonWidth = 120;
            int spacing = 10; // Фиксированный отступ между кнопками
            int totalWidth = buttonWidth * tabs.Length + spacing * (tabs.Length - 1);
            int startX = (ClientSize.Width - totalWidth) / 2;

            for (int i = 0; i < tabs.Length; i++)
            {
                navButtons[i] = new Button
                {
                    Text = tabs[i],
                    Size = new Size(buttonWidth, 45), // Увеличил высоту кнопок
                    Location = new Point(startX + i * (buttonWidth + spacing), 7),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(70, 70, 70),
                    Tag = tabs[i],
                    Font = new Font("Microsoft Sans Serif", 9.5f, FontStyle.Regular) // Увеличил размер шрифта
                };

                // Автоматически подгоняем размер под текст
                Size textSize = TextRenderer.MeasureText(navButtons[i].Text, navButtons[i].Font);
                navButtons[i].Width = Math.Max(buttonWidth, textSize.Width + 20);

                navButtons[i].Click += NavButton_Click;
                Controls.Add(navButtons[i]);
            }

            RecalculateButtonPositions();
        }

        private void RecalculateButtonPositions()
        {
            if (navButtons == null || navButtons.Length == 0) return;

            int spacing = 10;
            int totalWidth = navButtons.Sum(btn => btn.Width) + spacing * (navButtons.Length - 1);
            int startX = (ClientSize.Width - totalWidth) / 2;

            int currentX = startX;
            foreach (var button in navButtons)
            {
                button.Location = new Point(currentX, 7);
                currentX += button.Width + spacing;
            }
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            NavigateRequested?.Invoke(button.Tag.ToString());
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecalculateButtonPositions();
        }
    }
}