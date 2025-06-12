using BiomechanicNetwork.ExtraControls;
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
    internal partial class BaseForm : Form
    {
        protected HeaderControl header;
        protected NavigationControl navigation;
        protected Panel contentPanel;

        public BaseForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Настройка основной формы
            // Отступы под хедер и навигацию
            this.MinimumSize = new Size(800, 600);

            // Хедер (верхняя панель)
            header = new HeaderControl
            {
                Dock = DockStyle.Top,
                Title = "RetalityApp"
            };
            header.CloseClicked += (s, e) => this.Close();

            // Навигация (нижняя панель)
            navigation = new NavigationControl
            {
                Dock = DockStyle.Bottom
            };
            navigation.NavigateRequested += OnNavigationRequested;

            // Контентная панель
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Порядок добавления важен!
            Controls.Add(contentPanel);
            Controls.Add(navigation);
            Controls.Add(header);
        }

        protected virtual void OnNavigationRequested(string formName)
        {
            // Реализуется в наследниках
        }
    }

}
