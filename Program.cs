using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;

namespace попытка2
{
    static class Program
    {
        private static string ConfigFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private static string ConfigFileUpdaterPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updater_config.json");
        public static string versionPath = "3.0";
        public static string versionUpdaterPath = "1.0.3";
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!File.Exists(ConfigFilePath))
            {
                SaveCurrentVersion(versionPath);
            }
            if (File.Exists(ConfigFilePath))
            {
                SaveCurrentVersion(versionPath);
            }
            if (!File.Exists(ConfigFileUpdaterPath))
            {
                SaveCurrentUpdaterVersion(versionUpdaterPath);
            }
            if (File.Exists(ConfigFileUpdaterPath))
            {
                SaveCurrentUpdaterVersion(versionUpdaterPath);
            }
            // Проверка обновлений перед запуском приложения
            if (CheckAndUpdateApplication().Result)
            {               
                RestartUpdaterApplication(); // Запускаем внешнюю программу для обновления
                return;
            }
            UserAuthenticator.TryLoginWithGuid();
            // Если обновление не выполнено или пользователь отказался

        }
        public static bool IsInternetAvailable()
        {
            try
            {
                using (var client = new TcpClient("google.com", 80))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private static async Task<long> GetFileSizeAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "C# Application");

                    // Создаем запрос с методом HEAD
                    var request = new HttpRequestMessage(HttpMethod.Head, url);

                    // Отправляем запрос
                    var response = await client.SendAsync(request);

                    // Проверяем успешность запроса
                    if (response.IsSuccessStatusCode)
                    {
                        // Возвращаем размер файла из заголовков
                        if (response.Content.Headers.Contains("Content-Length"))
                        {
                            return response.Content.Headers.ContentLength ?? 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении размера файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }


        private static async Task<bool> CheckAndUpdateApplication()
        {
            if (!IsInternetAvailable())
            {
                UserAuthenticator.internetConnector = IsInternetAvailable();
                DialogResult result = MessageBox.Show("Нет подключения к интернету. Подключитесь к интернету чтобы продолжить ", "Проверка интернета",
                                                      MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    Environment.Exit(0);
                    return false;
                }
                else
                {
                    MessageBox.Show("Программа завершена из-за отсутствия интернета.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                    return false;
                }
            }
            const string VersionCheckUrl = "https://qwel-host.ru/api/version.json"; // URL для проверки версии
            string currentVersion = LoadCurrentVersion(); // Загружаем текущую версию
            string currentUpdaterVersion = LoadCurrentUpdaterVersion();
            string updateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "C# Manual Updates");

            if (!Directory.Exists(updateFolder))
            {
                Directory.CreateDirectory(updateFolder);
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(VersionCheckUrl);
                    var versionInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<VersionInfo>(response);
                    // Проверяем версию Updater
                    if (versionInfo.UpdaterVersion != currentUpdaterVersion)
                    {
                        string updaterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater.exe");
                        DialogResult result = MessageBox.Show(
                            $"Доступна новая версия Updater, необходимо обновить его",
                            "Обновление доступно",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.OK)
                        {
                            await DownloadAndReplaceFileAsync(versionInfo.DownloadUrlUpdater, updaterPath);
                            SaveCurrentUpdaterVersion(versionInfo.UpdaterVersion);
                        }
                        if (result == DialogResult.Cancel)
                        {
                            Environment.Exit(0);
                        }
                    }
                    if (versionInfo.Version != currentVersion)
                    {
                        // Получаем размеры файлов
                        long sizeOld = await GetFileSizeAsync(versionInfo.DownloadUrlOld);
                        long sizeNew = await GetFileSizeAsync(versionInfo.DownloadUrl);
                        double sizeupdate = sizeNew - sizeOld;

                        double sizeInMb = sizeupdate / 1024.0 / 1024.0;
                        double sizeInKb = sizeupdate / 1024.0;
                        double sizeInB = sizeupdate;

                        // Определяем, какой формат использовать
                        string resultat;
                        if (sizeInMb >= 1)
                        {
                            resultat = $"Обновление весит: {sizeInMb:F2} Мб.";
                        }
                        else if (sizeInKb >= 1)
                        {
                            resultat = $"Обновление весит: {sizeInKb:F2} Кб.";
                        }
                        else
                        {
                            resultat = $"Обновление весит: {sizeInB} байт.";
                        }

                        string changelog = string.Join(Environment.NewLine, versionInfo.Changelog);
                        DialogResult result = MessageBox.Show(
                            $"Доступна новая версия: {versionInfo.Version}\n\n{resultat}\n\nСписок изменений:\n{changelog}\n\nХотите обновиться?",
                            "Обновление доступно",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            SaveCurrentVersion(versionInfo.Version); // Сохраняем новую версию
                            return true; // Требуется перезапуск
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке обновлений: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false; // Обновление не требуется
        }
        public static async Task DownloadAndReplaceFileAsync(string downloadUrl, string targetFilePath)
        {
            try
            {
                // Создаем временный путь для скачивания
                string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode(); // Проверка успешности запроса

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await stream.CopyToAsync(fileStream); // Копируем содержимое в временный файл
                    }
                }

                // Если файл уже существует, удаляем его
                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }

                // Перемещаем загруженный файл в папку приложения
                File.Move(tempFilePath, targetFilePath);

                MessageBox.Show("Файл успешно обновлен!", "Загрузка завершена", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static void RestartUpdaterApplication()
        {
            try
            {
                // Закрытие текущего приложения
                Application.Exit();

                // Запуск UpdaterC#Manual для обновления
                string updaterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater.exe");
                Process.Start(updaterPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось запустить обновление: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       

        private static string LoadCurrentVersion()
        {
            if (File.Exists(ConfigFilePath))
            {
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(ConfigFilePath));
                return config?.Version ?? versionPath; // Если версия не найдена, устанавливаем дефолтную
            }
            return versionPath; // Дефолтная версия, если конфиг отсутствует
        }

        private static void SaveCurrentVersion(string version)
        {
            var config = new AppConfig { Version = version };
            File.WriteAllText(ConfigFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));
        }
        private static string LoadCurrentUpdaterVersion()
        {
            if (File.Exists(ConfigFilePath))
            {
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(ConfigFileUpdaterPath));
                return config?.Version ?? versionUpdaterPath; // Если версия не найдена, устанавливаем дефолтную
            }
            return versionUpdaterPath; // Дефолтная версия, если конфиг отсутствует
        }

        private static void SaveCurrentUpdaterVersion(string version)
        {
            var config = new AppConfig { Version = version };
            File.WriteAllText(ConfigFileUpdaterPath, Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));
        }
        public class VersionInfo
        {
            public string Version { get; set; }
            public string DownloadUrlOld { get; set; }
            public string DownloadUrl { get; set; }
            public string UpdaterVersion { get; set; }
            public string DownloadUrlUpdater { get; set; }
            public string[] Changelog { get; set; }
        }

        private class AppConfig
        {
            public string Version { get; set; }
        }
    }
}
