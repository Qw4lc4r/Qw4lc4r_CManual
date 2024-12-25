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

////=============================================================Rain================================================//
////namespace попытка2
////{
////    public partial class Autorization : Form
////    {
////        private Timer timer;
////        private List<Raindrop> raindrops;
////        public static Random rand;
////        private bool isClosing = false;
////        private List<Image> raindropImages;  // Список для хранения разных изображений капель

////        public Autorization()
////        {
////            InitializeComponent();
////            this.FormBorderStyle = FormBorderStyle.FixedDialog;
////            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
////            guna2TextBox1.UseSystemPasswordChar = true;

////            // Загрузка 5 изображений капель
////            raindropImages = new List<Image>
////            {
////                Image.FromFile(@"C:\Users\qw4lc4r\Pictures\C#\капля4.png"),  // Укажите путь к третьему изображению
////                Image.FromFile(@"C:\Users\qw4lc4r\Pictures\C#\капля5.png"),  // Укажите путь к третьему изображению

////            };

////            // Инициализация капель дождя
////            raindrops = new List<Raindrop>();
////            rand = new Random();

////            // Таймер для обновления анимации
////            timer = new Timer();
////            timer.Interval = 50; // Обновление каждый 50 миллисекунд
////            timer.Tick += Timer_Tick;
////            timer.Start();
////        }

////        private void Timer_Tick(object sender, EventArgs e)
////        {
////            // Добавляем новые капли с небольшой вероятностью
////            if (rand.Next(2) == 0) // 50% шанс, что появится новая капля
////            {
////                // Случайно выбираем одно из 5 изображений капель
////                Image randomRaindropImage = raindropImages[rand.Next(raindropImages.Count)];
////                raindrops.Add(new Raindrop(rand.Next(0, this.Width), 0, randomRaindropImage, rand.Next(5, 10)));
////            }

////            // Обновляем позиции капель дождя
////            for (int i = 0; i < raindrops.Count; i++)
////            {
////                raindrops[i].Y += raindrops[i].Speed;
////                if (raindrops[i].Y > this.Height) // Если капля вышла за пределы формы, удаляем её
////                {
////                    raindrops.RemoveAt(i);
////                    i--;
////                }
////            }

////            // Перерисовываем форму
////            Invalidate();
////        }

////        protected override void OnPaint(PaintEventArgs e)
////        {
////            base.OnPaint(e);

////            // Рисуем капли дождя
////            foreach (var raindrop in raindrops)
////            {
////                e.Graphics.DrawImage(raindrop.Image, raindrop.X, raindrop.Y, raindrop.Width, raindrop.Height);
////            }
////        }

////        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
////        {
////            if (isClosing) return;

////            isClosing = true;
////            var formsToClose = Application.OpenForms.Cast<Form>().ToList();

////            foreach (Form openForm in formsToClose)
////            {
////                if (openForm != this)
////                {
////                    openForm.Close();
////                }
////            }

////            isClosing = false;
////        }

////        private async void button1_Click(object sender, EventArgs e)
////        {
////            string login = textBox1.Text.Trim();
////            string password = guna2TextBox1.Text.Trim();
////            using (MySqlConnection connection = new MySqlConnection(UserAuthenticator.connectionString))
////            {
////                try
////                {
////                    int attempts = 0;
////                    while (connection.State != ConnectionState.Open && attempts < 10)
////                    {
////                        attempts++;
////                        try
////                        {
////                            await Task.Delay(1000);
////                            await connection.OpenAsync();
////                        }
////                        catch
////                        {
////                            if (attempts == 10)
////                            {
////                                MessageBox.Show("Не удалось подключиться к базе данных после нескольких попыток.");
////                                return;
////                            }
////                        }
////                    }

////                    string query = "SELECT login, name FROM users WHERE login = @login AND password = @password";
////                    MySqlCommand command = new MySqlCommand(query, connection);
////                    command.Parameters.AddWithValue("@login", login);
////                    command.Parameters.AddWithValue("@password", password);

////                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
////                    {
////                        if (reader.Read())
////                        {
////                            UserAuthenticator.UserName = reader["name"].ToString();
////                            UserAuthenticator.Login = reader["login"].ToString();
////                            if (checkBox1.Checked)
////                            {
////                                UpdateMachineGuidInDatabase(UserAuthenticator.Login, UserAuthenticator.GetMachineGuid());
////                            }
////                            MessageBox.Show("Вы успешно вошли в аккаунт!", "Вход в аккаунт", MessageBoxButtons.OK, MessageBoxIcon.Information);
////                            MainForm form2 = new MainForm();
////                            connection.Close();
////                            form2.Show();
////                            this.Hide();
////                        }
////                        else
////                        {
////                            MessageBox.Show("Неправильный логин или пароль", "Неверные данные", MessageBoxButtons.OK, MessageBoxIcon.Error);
////                        }
////                    }
////                }
////                catch (MySqlException ex)
////                {
////                    MessageBox.Show($"MySQL Error: {ex.Message}\nCode: {ex.Number}");
////                }
////            }
////        }

////        private void UpdateMachineGuidInDatabase(string login, string newMachineGuid)
////        {
////            using (var connection = new MySqlConnection(UserAuthenticator.connectionString))
////            {
////                connection.Open();
////                string query = "UPDATE users SET MachineGuid = @MachineGuid WHERE login = @login";
////                using (var command = new MySqlCommand(query, connection))
////                {
////                    command.Parameters.AddWithValue("@MachineGuid", newMachineGuid);
////                    command.Parameters.AddWithValue("@login", login);
////                    command.ExecuteNonQuery();
////                }
////            }
////        }

////        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
////        {
////            Registration form = new Registration();
////            form.Show();
////            this.Hide();
////        }

////        private void button2_Click(object sender, EventArgs e)
////        {
////            // Переключение видимости текста в поле пароля
////            guna2TextBox1.UseSystemPasswordChar = !guna2TextBox1.UseSystemPasswordChar;
////        }

////        public class Raindrop
////        {
////            public int X { get; set; }
////            public int Y { get; set; }
////            public int Width { get; set; }
////            public int Height { get; set; }
////            public int Speed { get; set; }
////            public Image Image { get; set; }  // Ссылка на изображение капли

////            public Raindrop(int x, int y, Image image, int speed)
////            {
////                X = x;
////                Y = y;
////                Image = image;
////                Width = image.Width;
////                Height = image.Height;
////                Speed = rand.Next(20, 50); ;
////            }
////        }
////    }
////}


////================================================Snow==============================================================================//


//namespace попытка2
//{
//    public partial class Autorization : Form
//    {
//        private Timer timer;
//        private List<Snowflake> snowflakes;
//        private List<Raindrop> raindrops;
//        private List<Leaf> leaves;

//        private List<object> currentItems;

//        private static Random rand;
//        private bool isClosing = false;

//        public Autorization()
//        {
//            InitializeComponent();
//            this.FormBorderStyle = FormBorderStyle.FixedDialog;
//            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
//            guna2TextBox1.UseSystemPasswordChar = true;


//        }

//        private void Timer_Tick(object sender, EventArgs e)
//        {
//            // Обновляем позиции текущего списка элементов
//            for (int i = 0; i < currentItems.Count; i++)
//            {
//                dynamic item = currentItems[i];
//                item.Y += item.Speed;
//                if (item.Y > this.Height)
//                {
//                    currentItems.RemoveAt(i);
//                    i--;
//                }
//            }

//            Invalidate(); // Перерисовываем форму
//        }

//        protected override void OnPaint(PaintEventArgs e)
//        {
//            base.OnPaint(e);

//            // Рисуем снежинки
//            foreach (var snowflake in snowflakes)
//            {
//                e.Graphics.FillEllipse(Brushes.White, snowflake.X, snowflake.Y, snowflake.Size, snowflake.Size);
//            }
//        }

//        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            if (isClosing) return;

//            isClosing = true;
//            var formsToClose = Application.OpenForms.Cast<Form>().ToList();

//            foreach (Form openForm in formsToClose)
//            {
//                if (openForm != this)
//                {
//                    openForm.Close();
//                }
//            }

//            isClosing = false;
//        }

//        private async void button1_Click(object sender, EventArgs e)
//        {
//            string login = textBox1.Text.Trim();
//            string password = guna2TextBox1.Text.Trim();
//            using (MySqlConnection connection = new MySqlConnection(UserAuthenticator.connectionString))
//            {
//                try
//                {
//                    int attempts = 0;
//                    while (connection.State != ConnectionState.Open && attempts < 10)
//                    {
//                        attempts++;
//                        try
//                        {
//                            await Task.Delay(1000);
//                            await connection.OpenAsync();
//                        }
//                        catch
//                        {
//                            if (attempts == 10)
//                            {
//                                MessageBox.Show("Не удалось подключиться к базе данных после нескольких попыток.");
//                                return;
//                            }
//                        }
//                    }

//                    string query = "SELECT login, name FROM users WHERE login = @login AND password = @password";
//                    MySqlCommand command = new MySqlCommand(query, connection);
//                    command.Parameters.AddWithValue("@login", login);
//                    command.Parameters.AddWithValue("@password", password);

//                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
//                    {
//                        if (reader.Read())
//                        {
//                            UserAuthenticator.UserName = reader["name"].ToString();
//                            UserAuthenticator.Login = reader["login"].ToString();
//                            if (checkBox1.Checked)
//                            {
//                                UpdateMachineGuidInDatabase(UserAuthenticator.Login, UserAuthenticator.GetMachineGuid());
//                            }
//                            MessageBox.Show("Вы успешно вошли в аккаунт!", "Вход в аккаунт", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                            MainForm form2 = new MainForm();
//                            connection.Close();
//                            form2.Show();
//                            this.Hide();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Неправильный логин или пароль", "Неверные данные", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//                catch (MySqlException ex)
//                {
//                    MessageBox.Show($"MySQL Error: {ex.Message}\nCode: {ex.Number}");
//                }
//            }
//        }

//        private void UpdateMachineGuidInDatabase(string login, string newMachineGuid)
//        {
//            using (var connection = new MySqlConnection(UserAuthenticator.connectionString))
//            {
//                connection.Open();
//                string query = "UPDATE users SET MachineGuid = @MachineGuid WHERE login = @login";
//                using (var command = new MySqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@MachineGuid", newMachineGuid);
//                    command.Parameters.AddWithValue("@login", login);
//                    command.ExecuteNonQuery();
//                }
//            }
//        }

//        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
//        {
//            Registration form = new Registration();
//            form.Show();
//            this.Hide();
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            // Переключение видимости текста в поле пароля
//            guna2TextBox1.UseSystemPasswordChar = !guna2TextBox1.UseSystemPasswordChar;
//        }

//        public class Snowflake
//        {
//            public int X { get; set; }
//            public int Y { get; set; }
//            public int Size { get; set; }
//            public int Speed { get; set; }

//            public Snowflake(int x, int y, int speed)
//            {
//                X = x;
//                Y = y;
//                Size = rand.Next(5, 15);
//                Speed = speed;
//            }
//        }
//        public class Leaf
//        {
//            public int X { get; set; }
//            public int Y { get; set; }
//            public int Width { get; set; }
//            public int Height { get; set; }
//            public int Speed { get; set; }
//            public Image Image { get; set; }  // Ссылка на изображение листа

//            public Leaf(int x, int y, Image image, int speed)
//            {
//                X = x;
//                Y = y;
//                Image = image;
//                Width = image.Width;
//                Height = image.Height;
//                Speed = speed;
//            }
//        }
//        public class Raindrop
//        {
//            public int X { get; set; }
//            public int Y { get; set; }
//            public int Width { get; set; }
//            public int Height { get; set; }
//            public int Speed { get; set; }
//            public Image Image { get; set; }  // Ссылка на изображение капли

//            public Raindrop(int x, int y, Image image, int speed)
//            {
//                X = x;
//                Y = y;
//                Image = image;
//                Width = image.Width;
//                Height = image.Height;
//                Speed = rand.Next(20, 50); ;
//            }
//        }
//        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
//        {
//            Registration reg = new Registration();
//            reg.Show();
//            this.Hide();
//        }

//        private void Autorization_Load(object sender, EventArgs e)
//        {

//        }

//        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
//        {

//            leaves.Clear();
//            raindrops.Clear();
//            snowflakes.Clear();
//            Invalidate();    // Перерисовываем форму
//            if(comboBox1.SelectedIndex == 0)
//            {

//                ChangeCurrentList("snowflakes");
//                // Инициализация снежинок
//                rand = new Random();
//                // Таймер для обновления анимации
//                timer = new Timer();
//                timer.Interval = 50; // Обновление каждый 50 миллисекунд
//                timer.Tick += Timer_Tick;
//                timer.Start();
//            }
//            if (comboBox1.SelectedIndex == 1)
//            {

//                ChangeCurrentList("raindrops");
//                // Инициализация снежинок
//                rand = new Random();
//                // Таймер для обновления анимации
//                timer = new Timer();
//                timer.Interval = 50; // Обновление каждый 50 миллисекунд
//                timer.Tick += Timer_Tick;
//                timer.Start();
//            }
//            if (comboBox1.SelectedIndex == 2)
//            {

//                ChangeCurrentList("leaves");
//                // Инициализация снежинок
//                rand = new Random();
//                // Таймер для обновления анимации
//                timer = new Timer();
//                timer.Interval = 50; // Обновление каждый 50 миллисекунд
//                timer.Tick += Timer_Tick;
//                timer.Start();
//            }

//        }
//        // Метод для смены списка
//        private void ChangeCurrentList(string listType)
//        {
//            switch (listType)
//            {
//                case "snowflakes":
//                    currentItems = snowflakes.Cast<object>().ToList();
//                    break;
//                case "raindrops":
//                    currentItems = raindrops.Cast<object>().ToList();
//                    break;
//                case "leaves":
//                    currentItems = leaves.Cast<object>().ToList();
//                    break;
//            }
//        }
//    }
//}

////=====================================================List============================================//
//namespace попытка2
//{
//    public partial class Autorization : Form
//    {
//        private Timer timer;
//        private List<Leaf> leaves;  // Список для хранения падающих листьев
//        public static Random rand;
//        private bool isClosing = false;
//        private List<Image> leafImages;  // Список для хранения разных изображений листьев

//        public Autorization()
//        {
//            InitializeComponent();
//            this.FormBorderStyle = FormBorderStyle.FixedDialog;
//            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
//            guna2TextBox1.UseSystemPasswordChar = true;

//            // Загрузка 5 изображений листьев
//            leafImages = new List<Image>
//            {
//                Image.FromFile(@"C:\Users\qw4lc4r\Pictures\C#\лист1.png"),  // Укажите путь к первому изображению
//                Image.FromFile(@"C:\Users\qw4lc4r\Pictures\C#\лист2.png"),  // Укажите путь ко второму изображению
//                Image.FromFile(@"C:\Users\qw4lc4r\Pictures\C#\лист3.png")  // Укажите путь к пятому изображению
//            };

//            // Инициализация падающих листьев
//            leaves = new List<Leaf>();
//            rand = new Random();

//            // Таймер для обновления анимации
//            timer = new Timer();
//            timer.Interval = 50; // Обновление каждый 50 миллисекунд
//            timer.Tick += Timer_Tick;
//            timer.Start();
//        }

//        private void Timer_Tick(object sender, EventArgs e)
//        {
//            // Добавляем новые листья с небольшой вероятностью
//            if (rand.Next(3) == 0) // 33% шанс, что появится новый лист
//            {
//                // Случайно выбираем одно из 5 изображений листьев
//                Image randomLeafImage = leafImages[rand.Next(leafImages.Count)];
//                leaves.Add(new Leaf(rand.Next(0, this.Width), 0, randomLeafImage, rand.Next(3, 7)));
//            }

//            // Обновляем позиции листьев
//            for (int i = 0; i < leaves.Count; i++)
//            {
//                leaves[i].Y += leaves[i].Speed;
//                leaves[i].X += rand.Next(-2, -2);  // Листья немного двигаются влево или вправо

//                //Если лист вышел за пределы формы, удаляем его
//                if (leaves[i].Y > this.Height || leaves[i].X < 0 || leaves[i].X > this.Width)
//                {
//                    leaves.RemoveAt(i);
//                    i--;
//                }
//            }

//            // Перерисовываем форму
//            Invalidate();
//        }

//        protected override void OnPaint(PaintEventArgs e)
//        {
//            base.OnPaint(e);

//            // Рисуем листья
//            foreach (var leaf in leaves)
//            {
//                e.Graphics.DrawImage(leaf.Image, leaf.X, leaf.Y, leaf.Width, leaf.Height);
//            }
//        }

//        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            if (isClosing) return;

//            isClosing = true;
//            var formsToClose = Application.OpenForms.Cast<Form>().ToList();

//            foreach (Form openForm in formsToClose)
//            {
//                if (openForm != this)
//                {
//                    openForm.Close();
//                }
//            }

//            isClosing = false;
//        }

//        private async void button1_Click(object sender, EventArgs e)
//        {
//            string login = textBox1.Text.Trim();
//            string password = guna2TextBox1.Text.Trim();
//            using (MySqlConnection connection = new MySqlConnection(UserAuthenticator.connectionString))
//            {
//                try
//                {
//                    int attempts = 0;
//                    while (connection.State != ConnectionState.Open && attempts < 10)
//                    {
//                        attempts++;
//                        try
//                        {
//                            await Task.Delay(1000);
//                            await connection.OpenAsync();
//                        }
//                        catch
//                        {
//                            if (attempts == 10)
//                            {
//                                MessageBox.Show("Не удалось подключиться к базе данных после нескольких попыток.");
//                                return;
//                            }
//                        }
//                    }

//                    string query = "SELECT login, name FROM users WHERE login = @login AND password = @password";
//                    MySqlCommand command = new MySqlCommand(query, connection);
//                    command.Parameters.AddWithValue("@login", login);
//                    command.Parameters.AddWithValue("@password", password);

//                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
//                    {
//                        if (reader.Read())
//                        {
//                            UserAuthenticator.UserName = reader["name"].ToString();
//                            UserAuthenticator.Login = reader["login"].ToString();
//                            if (checkBox1.Checked)
//                            {
//                                UpdateMachineGuidInDatabase(UserAuthenticator.Login, UserAuthenticator.GetMachineGuid());
//                            }
//                            MessageBox.Show("Вы успешно вошли в аккаунт!", "Вход в аккаунт", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                            MainForm form2 = new MainForm();
//                            connection.Close();
//                            form2.Show();
//                            this.Hide();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Неправильный логин или пароль", "Неверные данные", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//                catch (MySqlException ex)
//                {
//                    MessageBox.Show($"MySQL Error: {ex.Message}\nCode: {ex.Number}");
//                }
//            }
//        }

//        private void UpdateMachineGuidInDatabase(string login, string newMachineGuid)
//        {
//            using (var connection = new MySqlConnection(UserAuthenticator.connectionString))
//            {
//                connection.Open();
//                string query = "UPDATE users SET MachineGuid = @MachineGuid WHERE login = @login";
//                using (var command = new MySqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@MachineGuid", newMachineGuid);
//                    command.Parameters.AddWithValue("@login", login);
//                    command.ExecuteNonQuery();
//                }
//            }
//        }

//        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
//        {
//            Registration form = new Registration();
//            form.Show();
//            this.Hide();
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            // Переключение видимости текста в поле пароля
//            guna2TextBox1.UseSystemPasswordChar = !guna2TextBox1.UseSystemPasswordChar;
//        }

//        public class Leaf
//        {
//            public int X { get; set; }
//            public int Y { get; set; }
//            public int Width { get; set; }
//            public int Height { get; set; }
//            public int Speed { get; set; }
//            public Image Image { get; set; }  // Ссылка на изображение листа

//            public Leaf(int x, int y, Image image, int speed)
//            {
//                X = x;
//                Y = y;
//                Image = image;
//                Width = image.Width;
//                Height = image.Height;
//                Speed = speed;
//            }
//        }
//    }
//}
namespace попытка2
{
    public partial class Autorization : Form
    {
        private Timer timer;
        private List<Snowflake> snowflakes;
        private List<Raindrop> raindrops;
        private List<Leaf> leaves;

        private List<object> currentItems;

        private static Random rand;
        private bool isClosing = false;

        private int snowflakeTimeElapsed;
        private int raindropTimeElapsed;
        private int leafTimeElapsed;
        private const int SnowflakeAddDelay = 500; // Задержка для снежинок в миллисекундах
        private const int RaindropAddDelay = 500;  // Задержка для капель дождя в миллисекундах
        private const int LeafAddDelay = 700;      // Задержка для листьев в миллисекундах
        private bool animationStarted; // Флаг, который отслеживает, началась ли анимация

        private int _lastSelectedIndex = -1;

        public Autorization()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            guna2TextBox1.UseSystemPasswordChar = true;

            snowflakes = new List<Snowflake>();
            raindrops = new List<Raindrop>();
            leaves = new List<Leaf>();
            rand = new Random();

            // Инициализация таймера
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
            timer.Start();

            // Инициализация времени для каждого типа элемента
            snowflakeTimeElapsed = 0;
            raindropTimeElapsed = 0;
            leafTimeElapsed = 0;

            animationStarted = false; // Анимация еще не началась
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged; // Подписка на событие изменения выбранного индекса
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Запуск анимации только при изменении выбранного элемента
            snowflakes.Clear();
            raindrops.Clear();
            leaves.Clear();
            animationStarted = true;
            _lastSelectedIndex = comboBox1.SelectedIndex;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Проверка, началась ли анимация
            if (!animationStarted)
                return;

            // Обновляем позиции всех элементов
            foreach (var snowflake in snowflakes)
            {
                snowflake.Y += snowflake.Speed;
                if (snowflake.Y > this.Height) // Когда снежинка выходит за экран
                {
                    snowflake.Y = -snowflake.Size; // Появляется сверху
                    snowflake.X = rand.Next(this.Width); // Случайное место по X
                }
            }

            foreach (var raindrop in raindrops)
            {
                raindrop.Y += raindrop.Speed;
                if (raindrop.Y > this.Height) // Когда капля выходит за экран
                {
                    raindrop.Y = -raindrop.Height; // Появляется сверху
                    raindrop.X = rand.Next(this.Width); // Случайное место по X
                }
            }

            foreach (var leaf in leaves)
            {
                leaf.Y += leaf.Speed;
                leaf.X += rand.Next(-2, 3); // Листья немного двигаются вбок
                if (leaf.Y > this.Height) // Когда лист выходит за экран
                {
                    leaf.Y = -leaf.Height; // Появляется сверху
                    leaf.X = rand.Next(this.Width); // Случайное место по X
                }
            }

            // Добавляем новые элементы постепенно с задержкой
            snowflakeTimeElapsed += timer.Interval;
            raindropTimeElapsed += timer.Interval;
            leafTimeElapsed += timer.Interval;

            // Добавление элементов только если анимация активна
            if (animationStarted)
            {
                if (_lastSelectedIndex == 0 && _lastSelectedIndex != 1 && _lastSelectedIndex != 2 && snowflakes.Count < 50 && snowflakeTimeElapsed >= SnowflakeAddDelay)
                {
                    snowflakes.Add(new Snowflake(rand.Next(this.Width), 0, rand.Next(2, 5)));
                    snowflakeTimeElapsed = 0; // Сбрасываем время для снежинок
                }
                else if (_lastSelectedIndex == 1 && _lastSelectedIndex != 0 && _lastSelectedIndex != 2 && raindrops.Count < 50 && raindropTimeElapsed >= RaindropAddDelay)
                {
                    raindrops.Add(new Raindrop(rand.Next(this.Width), 0, rand.Next(2, 5), rand.Next(10, 20)));
                    raindropTimeElapsed = 0; // Сбрасываем время для капель
                }
                else if (_lastSelectedIndex == 2 && _lastSelectedIndex != 1 && _lastSelectedIndex != 0 && leaves.Count < 30 && leafTimeElapsed >= LeafAddDelay)
                {
                    leaves.Add(new Leaf(rand.Next(this.Width), 0, rand.Next(10, 20), rand.Next(10, 20), rand.Next(1, 4)));
                    leafTimeElapsed = 0; // Сбрасываем время для листьев
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

            // Рисуем капли дождя
            foreach (var raindrop in raindrops)
            {
                e.Graphics.FillRectangle(Brushes.Blue, raindrop.X, raindrop.Y, raindrop.Width, raindrop.Height);
            }

            // Рисуем листья
            foreach (var leaf in leaves)
            {
                e.Graphics.FillEllipse(Brushes.Brown, leaf.X, leaf.Y, leaf.Width, leaf.Height);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosing) return;

            isClosing = true;
            var formsToClose = Application.OpenForms.Cast<Form>().ToList();

            foreach (Form openForm in formsToClose)
            {
                if (openForm != this)
                {
                    openForm.Close();
                }
            }

            isClosing = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string password = guna2TextBox1.Text.Trim();
            using (MySqlConnection connection = new MySqlConnection(UserAuthenticator.connectionString))
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

                    string query = "SELECT login, name FROM users WHERE login = @login AND password = @password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            UserAuthenticator.UserName = reader["name"].ToString();
                            UserAuthenticator.Login = reader["login"].ToString();
                            if (checkBox1.Checked)
                            {
                                UpdateMachineGuidInDatabase(UserAuthenticator.Login, UserAuthenticator.GetMachineGuid());
                            }
                            MessageBox.Show("Вы успешно вошли в аккаунт!", "Вход в аккаунт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MainForm form2 = new MainForm();
                            connection.Close();
                            form2.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Неправильный логин или пароль", "Неверные данные", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"MySQL Error: {ex.Message}\nCode: {ex.Number}");
                }
            }
        }

        private void UpdateMachineGuidInDatabase(string login, string newMachineGuid)
        {
            using (var connection = new MySqlConnection(UserAuthenticator.connectionString))
            {
                connection.Open();
                string query = "UPDATE users SET MachineGuid = @MachineGuid WHERE login = @login";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MachineGuid", newMachineGuid);
                    command.Parameters.AddWithValue("@login", login);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration form = new Registration();
            form.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Переключение видимости текста в поле пароля
            guna2TextBox1.UseSystemPasswordChar = !guna2TextBox1.UseSystemPasswordChar;
        }

        

        private void StartAnimation()
        {
            timer?.Stop();
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Hide();
        }

        private void Autorization_Load(object sender, EventArgs e)
        {

        }
        private void ChangeCurrentList(string listType)
        {
            if (listType == "snowflakes")
            {
                currentItems = snowflakes.Cast<object>().ToList();
            }
            else if (listType == "raindrops")
            {
                currentItems = raindrops.Cast<object>().ToList();
            }
            else if (listType == "leaves")
            {
                currentItems = leaves.Cast<object>().ToList();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Очищаем все списки
            

            //if (comboBox1.SelectedIndex == 0) // Снежинки
            //{
            //    for (int i = 0; i < 50; i++)
            //    {
            //        // Начальная позиция Y на 0 (вверху), X случайное по ширине формы
            //        snowflakes.Add(new Snowflake(rand.Next(this.Width), 0, rand.Next(2, 5)));
            //    }
            //}
            //else if (comboBox1.SelectedIndex == 1) // Дождь
            //{
            //    for (int i = 0; i < 50; i++)
            //    {
            //        // Начальная позиция Y на 0 (вверху), X случайное по ширине формы
            //        raindrops.Add(new Raindrop(rand.Next(this.Width), 0, rand.Next(2, 5), rand.Next(10, 20)));
            //    }
            //}
            //else if (comboBox1.SelectedIndex == 2) // Листья
            //{
            //    for (int i = 0; i < 30; i++)
            //    {
            //        // Начальная позиция Y на 0 (вверху), X случайное по ширине формы
            //        leaves.Add(new Leaf(rand.Next(this.Width), 0, rand.Next(10, 20), rand.Next(10, 20), rand.Next(1, 4)));
            //    }
            //}
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

        public class Raindrop
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Speed { get; set; }

            public Raindrop(int x, int y, int speed, int height)
            {
                X = x;
                Y = y;
                Width = 2;
                Height = height;
                Speed = speed;
            }
        }

        public class Leaf
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Speed { get; set; }

            public Leaf(int x, int y, int width, int height, int speed)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Speed = speed;
            }
        }
    }
}