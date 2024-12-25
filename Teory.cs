using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace попытка2
{
    public partial class Teory : Form
    {
        public Teory()
        {
            InitializeComponent();
            button1.TabStop = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private bool isClosing = false;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Если мы уже закрываем формы, не начинаем повторно.
            if (isClosing) return;

            isClosing = true; // Устанавливаем флаг, чтобы избежать рекурсии

            // Создаем копию коллекции форм, чтобы избежать изменений в процессе перечисления
            var formsToClose = Application.OpenForms.Cast<Form>().ToList();

            // Закрываем все формы
            foreach (Form openForm in formsToClose)
            {
                // Пропускаем текущую форму (это важно, чтобы избежать её закрытия)
                if (openForm != this)
                {
                    openForm.Close(); // Закрываем другие формы
                }
            }

            // Разрешаем закрытие текущей формы
            isClosing = false;
        }

        public void LoadHtmlContent(string htmlContent)
        {
            // Здесь вы можете установить HTML-контент в WebBrowser или другой элемент
            webBrowser1.DocumentText = htmlContent;
        }

        private async void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm();
            form.Show();
            this.Hide();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        
    }
}
