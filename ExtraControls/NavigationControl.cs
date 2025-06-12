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
            Height = 50;
            BackColor = Color.FromArgb(30, 30, 30);

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            string[] tabs = { "Упражнения", "Рекомендации", "Профиль", "Поддержка" };
            navButtons = new Button[tabs.Length];

            int buttonWidth = 100;
            int spacing = (ClientSize.Width - buttonWidth * tabs.Length) / (tabs.Length + 1);

            for (int i = 0; i < tabs.Length; i++)
            {
                navButtons[i] = new Button
                {
                    Text = tabs[i],
                    Size = new Size(buttonWidth, 40),
                    Location = new Point(spacing + i * (buttonWidth + spacing), 5),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(70, 70, 70),
                    Tag = tabs[i]
                };
                navButtons[i].Click += NavButton_Click;
                Controls.Add(navButtons[i]);
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
            if (navButtons != null && navButtons.Length > 0)
            {
                int buttonWidth = 100;
                int spacing = (ClientSize.Width - buttonWidth * navButtons.Length) / (navButtons.Length + 1);

                for (int i = 0; i < navButtons.Length; i++)
                {
                    navButtons[i].Location = new Point(spacing + i * (buttonWidth + spacing), 5);
                }

            }
        }
    }
}
