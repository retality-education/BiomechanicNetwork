using BiomechanicNetwork.Forms.Auth;
using BiomechanicNetwork.Forms.MainForms;
using BiomechanicNetwork.Models;
using BiomechanicNetwork.Utilities;
using System.Runtime.InteropServices;

namespace BiomechanicNetwork
{
    internal static class Program
    {
        private static Form currentForm;
        public static User CurrentUser = new();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            currentForm = new LoginForm();
            Application.Run(currentForm);
        }
        public static void SwitchForm(Form newForm)
        {
            var oldForm = currentForm;
            currentForm = newForm;

            newForm.FormClosed += (s, args) => oldForm.Close();
            oldForm.Hide();
            newForm.Show();
        }
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}