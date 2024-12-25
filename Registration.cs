using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

namespace попытка2
{
    public partial class Registration : Form
    {
        private Timer timer;
        private List<Snowflake> snowflakes;
        private static Random rand;
        private bool isClosing = false;
        public Registration()
        {
            InitializeComponent(); this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            guna2TextBox4.KeyPress += TextBox_KeyPress;
            guna2TextBox1.UseSystemPasswordChar = true;
            guna2TextBox3.UseSystemPasswordChar = true;// Инициализация снежинок
            snowflakes = new List<Snowflake>();
            rand = new Random();

            // Таймер для обновления анимации
            timer = new Timer();
            timer.Interval = 50; // Обновление каждый 50 миллисекунд
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Добавляем новые снежинки с небольшой вероятностью
            if (rand.Next(10) < 2) // 20% шанс, что появится новая снежинка
            {
                snowflakes.Add(new Snowflake(rand.Next(0, this.Width), 0, rand.Next(2, 5)));
            }

            // Обновляем позиции снежинок
            for (int i = 0; i < snowflakes.Count; i++)
            {
                snowflakes[i].Y += snowflakes[i].Speed;
                if (snowflakes[i].Y > this.Height) // Если снежинка вышла за пределы формы, удаляем её
                {
                    snowflakes.RemoveAt(i);
                    i--;
                }
            }

            // Перерисовываем форму
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Рисуем снежинки
            foreach (var snowflake in snowflakes)
            {
                e.Graphics.FillEllipse(Brushes.White, snowflake.X, snowflake.Y, snowflake.Size, snowflake.Size);
            }
        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }
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


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Autorization form = new Autorization();
            form.Show();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2TextBox1.UseSystemPasswordChar = !guna2TextBox1.UseSystemPasswordChar;
            guna2TextBox3.UseSystemPasswordChar = !guna2TextBox3.UseSystemPasswordChar;
        }


        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            string login = guna2TextBox2.Text.Trim();
            string password = guna2TextBox1.Text.Trim();
            string passwordCheck = guna2TextBox3.Text.Trim();
            string name = guna2TextBox4.Text.Trim();

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                           $"Пожалуйста, заполните все поля.",
                           "Не заполненные поля",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);
                return;
            }

            if (login.Length < 3)
            {
                MessageBox.Show(
                          $"Логин должен содержать не менее 3 символов.",
                          "Не заполненные поля",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show(
                          $"Пароль должен содержать не менее 6 символов.",
                          "Не заполненные поля",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
                return;
            }
            
            if (password != passwordCheck)
            {
                MessageBox.Show(
                         $"Введеные пароли не совпадают.",
                         "Не заполненные поля",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Warning);
                return;
            }

            using (var connection = new MySqlConnection(UserAuthenticator.connectionString))
            {
                try
                {
                    int attempts = 0;
                    while (connection.State != ConnectionState.Open && attempts < 10)
                    {
                        attempts++;
                        try
                        {
                            await Task.Delay(1000);
                            await connection.OpenAsync();
                        }
                        catch
                        {
                            if (attempts == 10)
                            {
                                MessageBox.Show("Не удалось подключиться к базе данных после нескольких попыток.");
                                return;
                            }
                        }
                    }

                    // Проверка на уникальность логина
                    string checkQuery = "SELECT login FROM users WHERE login = @login";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@login", login);
                        using (var reader = await checkCommand.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                MessageBox.Show(
                                    $"Пользователь с таким логином уже существует.",
                                    "Проверка логина",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    // Запрос на вставку данных с MAC-адресом
                    string query = "INSERT INTO users (login, password, name, progress) VALUES (@login, @password, @name, @progress)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@progress", 0);

                        int insertResult = await command.ExecuteNonQueryAsync();

                        if (insertResult > 0)
                        {
                            MessageBox.Show(
                                    $"Пользователь успешно добавлен!",
                                    "Добавление пользователя",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            Autorization form = new Autorization();
                            form.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show(
                                    $"Ошибка при добавлении пользователя.",
                                    "Добавление пользователя",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выполнении операции: {ex.Message}");
                }
            }
        }

        private void Registration_Load_1(object sender, EventArgs e)
        {

        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, является ли символ буквой или управляющим (например, Backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Если нет, запрещаем ввод
                e.Handled = true;
            }
        }
        
        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }
        public class Snowflake
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Size { get; set; }
            public int Speed { get; set; }

            public Snowflake(int x, int y, int speed)
            {
                X = x;
                Y = y;
                Size = rand.Next(5, 15);
                Speed = speed;
            }
        }
    }

}
