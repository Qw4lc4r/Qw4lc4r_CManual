using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.IO;
using MySqlConnector;
using FastColoredTextBoxNS;
using static FastColoredTextBoxNS.AutocompleteMenu; // Для автодополнения

namespace попытка2
{
    public partial class TeoryPractice : Form
    {
        private int numToCompile = 0;
        private string task = "";
        private AutocompleteMenu popupMenu;
        //private FastColoredTextBox textEditor;
        public TeoryPractice()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            button1.TabStop = false;
            buttonExecute.TabStop = false;
            textBoxCode.TabStop = false;
            textBoxResult.TabStop = false;
            label1.TabStop = false;
            webView21.MouseDown += webView_MouseDown;
            //fastColoredTextBox1.Dock = DockStyle.Fill;
            fastColoredTextBox1.Language = Language.CSharp;
            fastColoredTextBox1.Font = new System.Drawing.Font("Consolas", 12);

            //// Настройка FastColoredTextBox
            //textEditor = new FastColoredTextBox
            //{
            //    Dock = DockStyle.Fill,
            //    Language = Language.CSharp,
            //    Font = new System.Drawing.Font("Consolas", 12)
            //};
            //textBoxCode.Controls.Add(textEditor);

            // Создание меню автодополнения
            popupMenu = new AutocompleteMenu(fastColoredTextBox1)
            {
                MinFragmentLength = 2 // Начинать показывать подсказки после ввода двух символов
            };

            // Список подсказок
            popupMenu.Items.SetAutocompleteItems(GetAutoCompleteItems());
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            webView21.Focus();
        }

        private bool isClosing = false;
        private void webView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // Проверяем, что нажата правая кнопка мыши
            {
                if (e is HandledMouseEventArgs ev) // Преобразуем к HandledMouseEventArgs
                {
                    ev.Handled = true; // Блокируем событие
                }
            }
        }

        private ICollection<string> GetAutoCompleteItems()
        {
            return new List<string>
{
    "using System;",
    "using System.Collections.Generic;",
    "using System.Linq;",
    "using System.Text;",
    "using System.Threading;",
    "using System.Threading.Tasks;",
    "namespace",
    "class",
    "public class",
    "private class",
    "protected class",
    "internal class",
    "public static class",
    "interface",
    "public interface",
    "private interface",
    "protected interface",
    "internal interface",
    "abstract class",
    "abstract",
    "sealed class",
    "void",
    "int",
    "double",
    "float",
    "bool",
    "string",
    "char",
    "decimal",
    "object",
    "dynamic",
    "var",
    "List<T>",
    "Dictionary<TKey, TValue>",
    "Tuple<T1, T2>",
    "Tuple<T1, T2, T3>",
    "Action",
    "Func<T>",
    "IEnumerable<T>",
    "ICollection<T>",
    "IEnumerator<T>",
    "IList<T>",
    "Queue<T>",
    "Stack<T>",
    "Array",
    "string[]",
    "int[]",
    "for",
    "foreach",
    "while",
    "do",
    "if",
    "else",
    "switch",
    "case",
    "default",
    "try",
    "catch",
    "finally",
    "throw",
    "new",
    "return",
    "break",
    "continue",
    "goto",
    "using",
    "lock",
    "checked",
    "unchecked",
    "async",
    "await",
    "yield return",
    "yield break",
    "null",
    "is",
    "as",
    "nameof",
    "enum",
    "delegate",
    "event",
    "public event",
    "private event",
    "protected event",
    "internal event",
    "get",
    "set",
    "indexer",
    "property",
    "field",
    "static",
    "const",
    "readonly",
    "volatile",
    "dynamic",
    "extension method",
    "partial class",
    "partial method",
    "sealed",
    "unsafe",
    "fixed",
    "default",
    "async Task",
    "async void",
    "continue",
    "null conditional operator",
    "??",
    "??=",
    "&&",
    "||",
    "!",
    "++",
    "--",
    "==",
    "!=",
    "<",
    ">",
    "<=",
    ">=",
    "+",
    "-",
    "*",
    "/",
    "%",
    "&",
    "|",
    "^",
    "<<",
    ">>",
    "=>",
    "=>",
    "=>",
    "+=",
    "-=",
    "*=",
    "/=",
    "%=",
    "&=",
    "|=",
    "^=",
    "<<=",
    ">>=",
    "is null",
    "default(T)",
    "ref",
    "out",
    "in",
    "params",
    "async",
    "await",
    "Task.Delay();",
    "Task.WhenAll();",
    "Task.WhenAny();",
    "Task.Run();",
    "Task.FromResult();",
    "try-catch-finally",
    "Array.Sort();",
    "Array.IndexOf();",
    "String.Join();",
    "String.Format();",
    "String.Concat();",
    "String.Equals();",
    "String.Split();",
    "String.Substring();",
    "String.Replace();",
    "String.Contains();",
    "String.ToLower();",
    "String.ToUpper();",
    "DateTime.Now",
    "DateTime.UtcNow",
    "DateTime.Parse();",
    "DateTime.TryParse();",
    "DateTime.AddDays();",
    "DateTime.AddMonths();",
    "DateTime.AddYears();",
    "DateTime.ToString();",
    "DateTime.ParseExact();",
    "DateTime.TryParseExact();",
    "File.Exists();",
    "File.Delete();",
    "File.Move();",
    "File.Copy();",
    "Directory.CreateDirectory();",
    "Directory.GetFiles();",
    "Directory.GetDirectories();",
    "FileStream",
    "StreamReader",
    "StreamWriter",
    "FileInfo",
    "DirectoryInfo",
    "Path.Combine();",
    "Path.GetFileName();",
    "Path.GetExtension();",
    "Path.GetDirectoryName();",
    "Path.GetFullPath();",
    "Path.IsPathRooted();",
    "Guid.NewGuid();",
    "Random.Next();",
    "Environment.GetEnvironmentVariable();",
    "Environment.SetEnvironmentVariable();",
    "Environment.Exit();",
    "Environment.NewLine",
    "Console.ReadLine();",
    "Console.ReadKey();",
    "Console.Clear();",
    "Console.Beep();",
    "Console.BackgroundColor",
    "Console.ForegroundColor",
    "Console.ResetColor();",
    "Console.WriteLine();",
    "Console.Write();",
    "Console.SetCursorPosition();",
    "Console.Clear();",
    "Thread.Sleep();",
    "Thread.CurrentThread",
    "Thread.Start();",
    "Thread.Join();",
    "Thread.Abort();",
    "Thread.Suspend();",
    "Thread.Resume();",
    "Task.Delay();",
    "Task.WhenAll();",
    "Task.WhenAny();",
    "Task.Wait();",
    "Task.WaitAll();",
    "Task.Run();",
    "Task.CompletedTask",
    "Task.FromResult();",
    "CancellationTokenSource",
    "CancellationToken",
    "CancellationToken.Register();",
    "CancellationTokenSource.Cancel();",
    "CancellationTokenSource.Token",
    "Observable",
    "IObservable<T>",
    "IObserver<T>",
    "Observable.Timer();",
    "Observable.Interval();",
    "Observable.Range();",
    "Observable.FromEvent();",
    "Subject<T>",
    "BehaviorSubject<T>",
    "ReplaySubject<T>",
    "AsyncSubject<T>",
    "ICommand",
    "RelayCommand",
    "ActionCommand",
    "ParameterCommand",
    "ObservableCollection<T>",
    "INotifyPropertyChanged",
    "INotifyCollectionChanged",
    "DependencyProperty",
    "DependencyObject",
    "Binding",
    "DataContext",
    "Dispatcher",
    "DispatcherTimer",
    "Application.Current",
    "Window",
    "UserControl",
    "Button",
    "TextBox",
    "Label",
    "ComboBox",
    "ListBox",
    "RadioButton",
    "CheckBox",
    "ListView",
    "TreeView",
    "DataGrid",
    "StackPanel",
    "WrapPanel",
    "Grid",
    "Canvas",
    "DockPanel",
    "TabControl",
    "TabItem",
    "Menu",
    "MenuItem",
    "ContextMenu",
    "StatusBar",
    "StatusBarItem",
    "ToolBar",
    "ToolBarButton",
    "ProgressBar",
    "Slider",
    "TextBlock",
    "Image",
    "ImageBrush",
    "Brush",
    "SolidColorBrush",
    "LinearGradientBrush",
    "RadialGradientBrush",
    "VisualState",
    "Storyboard",
    "KeyFrame",
    "VisualBrush",
    "Transform",
    "Matrix",
    "Matrix3D",
    "TranslateTransform",
    "RotateTransform",
    "ScaleTransform",
    "SkewTransform",
    "3DModel",
    "Viewport3D",
    "PerspectiveCamera",
    "MatrixTransform",
    "KeyFrameCollection",
    "AnimationClock",
    "EventTrigger",
    "Storyboard.SetTarget();",
    "Storyboard.SetTargetProperty();"
            };

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Если мы уже закрываем формы, не начинаем повторно.
            if (isClosing)
                return;

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
        private void TeoryPractice_Load(object sender, EventArgs e)
        {

        }
        public async Task LoadHtmlContentAsync(string htmlContent, int res)
        {
            numToCompile = res;

            await webView21.EnsureCoreWebView2Async(null);

            webView21.NavigateToString(htmlContent);

            string codeToCompile =
$@"{task}
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Collections.ObjectModel;
public class Program
{{
    public static void Main()
    {{
                        
    }}
}}";

            string methodToCompile =
$@"{task}
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Collections.ObjectModel;
public class Program
{{
    
}}";

            string ClassToCompile =
$@"{task}
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Collections.ObjectModel;";
                

            string codeToCompileFinal;
            switch (numToCompile)
            {
                case 0:
                    codeToCompileFinal = codeToCompile;
                    break;
                case 1:
                    codeToCompileFinal = methodToCompile;
                    break;
                case 2:
                    codeToCompileFinal = ClassToCompile;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(numToCompile),
                        "Invalid value of i"
                    );
            }
            fastColoredTextBox1.Text = codeToCompileFinal;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm();
            form.Show();
            this.Hide();
        }
        public void LoadTasks(string tasksContent)
        {
            // Здесь вы можете установить HTML-контент в WebBrowser или другой элемент
            task = tasksContent;
        }

        public void UpdateLabelText(string text)
        {
            label1.Text = text;
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(fastColoredTextBox1.Text);
            string code = fastColoredTextBox1.Text;
            string preparedCode = PrepareCode(code);

            //string codeToCompile =
            //    $@"
            //        using System.Text.RegularExpressions;
            //        using System;
            //        using System.Text;
            //        using System.Collections.ObjectModel;
            //        public class Program
            //        {{
            //            public static void Main()
            //            {{
            //                {code}
            //            }}
            //        }}";

            //string methodToCompile =
            //    $@"
            //        using System.Text.RegularExpressions;
            //        using System;
            //        using System.Text;
            //        using System.Collections.ObjectModel;
            //        public class Program
            //        {{
            //            {preparedCode}
            //        }}";

            //string ClassToCompile =
            //    $@"               
            //        using System.Text.RegularExpressions;
            //        using System;
            //        using System.Text;
            //        using System.Collections.ObjectModel;
            //        {preparedCode}";

            //string codeToCompileFinal;
            //switch (numToCompile)
            //{
            //    case 0:
            //        codeToCompileFinal = codeToCompile;
            //        break;
            //    case 1:
            //        codeToCompileFinal = methodToCompile;
            //        break;
            //    case 2:
            //        codeToCompileFinal = ClassToCompile;
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(
            //            nameof(numToCompile),
            //            "Invalid value of i"
            //        );
            //}
            string result = ExecuteCode(code);
            TargetCodeResult(result);
            textBoxResult.Text = result;

        }

         
        private async Task<bool> TargetCodeResult(string res)
        {
            using (MySqlConnection conn = new MySqlConnection(UserAuthenticator.connectionString))
            {
                int attempts = 0;
                while (conn.State != ConnectionState.Open && attempts < 10)
                {
                    attempts++;
                    try
                    {
                        await Task.Delay(1000);
                        await conn.OpenAsync();
                    }
                    catch (Exception ex)
                    {
                        if (attempts == 10)
                        {
                            MessageBox.Show($"Не удалось подключиться к базе данных после нескольких попыток. Ошибка: {ex.Message}");
                            return false;
                        }
                    }
                }

                string checkQuery = "SELECT login FROM user_practice WHERE login = @login AND select_index = @select_index AND select_combobox = @select_combobox";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@login", UserAuthenticator.Login);
                checkCmd.Parameters.AddWithValue("@select_index", MainForm.selectIndex);
                checkCmd.Parameters.AddWithValue("@select_combobox", MainForm.selectCombobox);

                try
                {
                    using (MySqlDataReader reader = (MySqlDataReader)await checkCmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }
                    }

                    string insertSelectionQuery = "INSERT INTO user_practice (login, select_index, select_combobox, practiceResult, userResult, practiceAttempt) VALUES (@login, @select_index, @select_combobox, @practiceResult, @userResult, @practiceAttempt)";
                    MySqlCommand insertCmd = new MySqlCommand(insertSelectionQuery, conn);
                    insertCmd.Parameters.AddWithValue("@login", UserAuthenticator.Login);
                    insertCmd.Parameters.AddWithValue("@select_index", MainForm.selectIndex);
                    insertCmd.Parameters.AddWithValue("@select_combobox", MainForm.selectCombobox);
                    insertCmd.Parameters.AddWithValue("@practiceResult", res);
                    insertCmd.Parameters.AddWithValue("@userResult", textBoxCode.Text);
                    insertCmd.Parameters.AddWithValue("@practiceAttempt", 1);

                    await insertCmd.ExecuteNonQueryAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}");
                    return false;
                }
            }
        }


        private string PrepareCode(string code)
        {
            // Добавляем 'public' перед методом, если его нет
            code = Regex.Replace(
                code,
                @"(?<!public\s|private\s|protected\s)(void\s+\w+\s*\()",
                "public $1"
            );

            // Добавляем 'public' перед классом, если его нет
            code = Regex.Replace(code, @"(?<!public\s)(class\s+\w+)", "public $1");

            // Добавляем 'public' перед 'static', если его нет
            code = Regex.Replace(code, @"(?<!public\s)(static\s+\w+\s*\()", "public $1");

            return code;
        }

        private List<MetadataReference> GetDefaultReferences()
        {
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            string coreDir = Path.GetDirectoryName(typeof(object).Assembly.Location);

            if (Environment.Version.Major >= 5 || coreDir.Contains("Microsoft.NETCore"))
            {
                // Для .NET Core и .NET 5/6/7
                references.Add(
                    MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Runtime.dll"))
                );
                references.Add(
                    MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Console.dll"))
                );
                references.Add(
                    MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Linq.dll"))
                );
                references.Add(
                    MetadataReference.CreateFromFile(
                        Path.Combine(coreDir, "System.Collections.dll")
                    )
                );
            }
            else
            {
                // Для .NET Framework 4.x
                references.Add(
                    MetadataReference.CreateFromFile(Path.Combine(coreDir, "mscorlib.dll"))
                );
                references.Add(
                    MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.dll"))
                );
                references.Add(
                    MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Core.dll"))
                );
            }

            return references;
        }

        private string ExecuteCode(string code)
        {
            string result = "";
            string className = "Program";
            string methodName = "Main";

            //if (i == 1 || i == 2)
            //{
            //    methodName = GetMethodName(code);
            //    if (string.IsNullOrWhiteSpace(methodName))
            //    {
            //        return "Method name could not be determined or is missing.";
            //    }
            //}

            //if (i == 2)
            //{
            //    className = GetClassName(code);
            //    if (string.IsNullOrWhiteSpace(className))
            //    {
            //        return "Class name could not be determined or is missing.";
            //    }
            //}

            // Подготовка синтаксического дерева для компиляции
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = GetDefaultReferences();

            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            using (var stream = new MemoryStream())
            {
                var resultCompilation = compilation.Emit(stream);

                if (resultCompilation.Success)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(stream.ToArray());
                    var type = assembly.GetType(className);
                    var method = type?.GetMethod(methodName);

                    using (var sw = new StringWriter())
                    {
                        Console.SetOut(sw);

                        method?.Invoke(Activator.CreateInstance(type), null);

                        result = sw.ToString();
                    }
                }
                else
                {
                    foreach (var diagnostic in resultCompilation.Diagnostics)
                    {
                        result += diagnostic.ToString() + "\n";
                    }
                }
            }
            return result;
        }

        //private string GetMethodName(string code)
        //{
        //    var match = Regex.Match(code, @"public\s+void\s+(\w+)\s*\(");
        //    return match.Success ? match.Groups[1].Value : string.Empty;
        //}

        //private string GetClassName(string code)
        //{
        //    var match = Regex.Match(code, @"public\s+class\s+(\w+)\s*");
        //    return match.Success ? match.Groups[1].Value : string.Empty;
        //}
    }
}