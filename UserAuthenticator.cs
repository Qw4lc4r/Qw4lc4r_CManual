using System;
using System.Security.Principal;
using MySqlConnector;
using System.Windows.Forms;
using System.Net.Sockets;

namespace попытка2
{
    public class UserAuthenticator
    {
        public static string connectionString = "Server=188.235.162.32;Database=csharp_tutorial;User ID=csTutorial;Password=cstutorial;SslMode=none;";
        public static string UserName = string.Empty;
        public static string Login = string.Empty;
        public static bool internetConnector = true;
        
        
        public static bool TryLoginWithGuid()
        {
            string currentGuid = GetMachineGuid();
            var user = GetUserFromDatabase(currentGuid);

            if (user.userName != null)
            {
                UserName = user.userName;
                Login = user.login;
                Application.Run(new MainForm());
                return true;
            }

            else
            {

                Application.Run(new Autorization());
                return false;
            }
        }

        public static string GetMachineGuid()
        {
            // Получаем GUID машины (компьютера)
            string guid = WindowsIdentity.GetCurrent().User.Value; // Пример с использованием WindowsIdentity
            return guid;
        }

        private static (string userName, string login) GetUserFromDatabase(string machineGuid)
        {
            // Запрос к базе данных, чтобы найти имя пользователя и логин по MachineGuid
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT login, name FROM users WHERE MachineGuid = @MachineGuid";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MachineGuid", machineGuid);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Возвращаем имя пользователя и логин (email)
                            string userName = reader.GetString("name");
                            string login = reader.GetString("login");
                            return (userName, login);
                        }
                        else
                        {
                            return (null, null); // Если не найдено совпадение
                        }
                    }
                }
            }
        }

    }
}
