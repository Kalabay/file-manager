using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileManager
{
    class Program
    {
        /// <summary>
        /// Метод для вывода пути в данный момент.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void GetPathNow(string pathNow)
        {
            // Покраска вывода для читабельности интерфейса.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ваш путь в данный момент:");
            Console.WriteLine(pathNow);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Метод для выбора диска пользователем.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        /// <param name="driver">хранящийся диск пользователя.</param>
        private static void DriverSelection(out string pathNow, out DriveInfo driver)
        {
            Console.WriteLine("Список дисков:");
            DriveInfo[] drivers = DriveInfo.GetDrives();
            for (int i = 0; i < drivers.Length; ++i)
            {
                Console.WriteLine(i + 1 + ". " + drivers[i]);
            }
            Console.WriteLine("Выберите какой-то один.");
            Console.WriteLine("Для этого введите его номер.");
            int number;
            while (!int.TryParse(Console.ReadLine(), out number)
                || number < 1 || number > drivers.Length)
            {
                Console.WriteLine("Ваш ввод некорректен.");
                Console.WriteLine("Повторите попытку.");
            }
            driver = drivers[number - 1];
            pathNow = driver.ToString();
        }

        /// <summary>
        /// Метод для считывания нового пути пользователя.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        /// <param name="driver">хранящийся диск пользователя.</param>
        private static void NewPath(ref string pathNow, ref DriveInfo driver)
        {
            Console.WriteLine("Введите путь до вашей директории.");
            Console.WriteLine("!!!Введите его без учёта диска.");
            Console.WriteLine("Например, если диск = " + @"C:\");
            Console.WriteLine("А путь = " + @"C:\Users\name\Py");
            Console.WriteLine("То вы должны ввести:");
            Console.WriteLine(@"Users\name\Py");
            Console.WriteLine("То есть вы должны дополнить строку диска до своей.");
            // Покраска вывода для читабельности интерфейса.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("В данный момент выбран диск:");
            Console.WriteLine(driver);
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine();
            while (!Directory.Exists(driver.ToString() + input))
            {
                // Покраска вывода для читабельности интерфейса.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Итог вашего ввода:");
                Console.WriteLine(driver.ToString() + input);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Ваш ввод некорректен.");
                Console.WriteLine("Скорее всего вашей директории не существует.");
                Console.WriteLine("Повторите попытку.");
                input = Console.ReadLine();
            }
            pathNow = driver.ToString() + input;
        }

        /// <summary>
        /// Метод для вывода файлов в директории.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void PrintFiles(ref string pathNow)
        {
            if (Directory.Exists(pathNow))
            {
                string[] files = Directory.GetFiles(pathNow);
                Console.WriteLine("Файлы в вашей директории:");
                foreach (string s in files)
                {
                    Console.WriteLine(s);
                }
                return;
            }
            Console.WriteLine("Указанная директория некорректна.");
            Console.WriteLine("Скорее всего вашей директории не существует.");
        }

        /// <summary>
        /// Метод для получения текста файла в 1 из 4 кодировок.
        /// </summary>
        /// <param name="path">путь до файла.</param>
        /// <param name="encoding">название кодировки.</param>
        /// <returns>текст, записанный в файле.</returns>
        private static string GetText(string path, string encoding)
        {
            string text;
            switch (encoding)
            {
                case "UTF8":
                    text = File.ReadAllText(path, Encoding.UTF8);
                    break;
                case "Unicode":
                    text = File.ReadAllText(path, Encoding.Unicode);
                    break;
                case "ASCII":
                    text = File.ReadAllText(path, Encoding.ASCII);
                    break;
                default:
                    text = File.ReadAllText(path, Encoding.UTF32);
                    break;
            }
            return text;
        }

        /// <summary>
        /// Метод для получения 1 из 4 кодировок.
        /// </summary>
        /// <returns>полученный номер кодировки.</returns>
        private static int GetEncoding()
        {
            Console.WriteLine("Выберите 1 из 4 кодировок.");
            Console.WriteLine("Для этого введите её номер.");
            Console.WriteLine("1. UTF8");
            Console.WriteLine("2. Unicode");
            Console.WriteLine("3. ASCII");
            Console.WriteLine("4. UTF32");
            int number;
            while (!int.TryParse(Console.ReadLine(), out number)
                || number < 1 || number > 4)
            {
                Console.WriteLine("Ваш ввод некорректен.");
                Console.WriteLine("Повторите попытку.");
            }
            return number;
        }

        /// <summary>
        /// Метод для получения файла на пути пользователя.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        /// <param name="checkNew">false, если проверка на существование нужна, иначе true</param>
        /// <returns>полученный путь до файла.</returns>
        private static string GetFileInPath(string pathNow, bool checkNew)
        {
            Console.WriteLine("Введите путь до вашего файла.");
            Console.WriteLine("!!!Введите его без учёта пути в данный момент.");
            Console.WriteLine("Например, если путь в данный момент = " + @"C:\Users\");
            Console.WriteLine("А путь = " + @"C:\Users\name\Py.txt");
            Console.WriteLine("То вы должны ввести:");
            Console.WriteLine(@"name\Py.txt");
            Console.WriteLine("То есть вы должны дополнить строку пути до своей.");
            GetPathNow(pathNow);
            string input = Console.ReadLine();
            if (!checkNew)
            {
                while (!File.Exists(pathNow + input))
                {
                    // Покраска вывода для читабельности интерфейса.
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Итог вашего ввода:");
                    Console.WriteLine(pathNow + input);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Ваш ввод некорректен.");
                    Console.WriteLine("Скорее всего вашего файла не существует.");
                    Console.WriteLine("Повторите попытку.");
                    input = Console.ReadLine();
                }
            }
            // Покраска вывода для читабельности интерфейса.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Итог вашего ввода:");
            Console.WriteLine(pathNow + input);
            Console.ForegroundColor = ConsoleColor.White;
            return pathNow + input;
        }

        /// <summary>
        /// Метод для получения любого пути файла.
        /// </summary>
        /// <returns>полученный путь до файла.</returns>
        private static string GetFile()
        {
            Console.WriteLine("Например, если путь = " + @"C:\Users\name\Py.txt");
            Console.WriteLine("То вы должны ввести:");
            Console.WriteLine(@"C:\Users\name\Py.txt");
            string input = Console.ReadLine();
            // Покраска вывода для читабельности интерфейса.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Итог вашего ввода:");
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
            return input;
        }

        /// <summary>
        /// Метод для вывода файла в 1 из 4 кодировок.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void PrintFileInEncoding(string pathNow)
        {
            int encoding = GetEncoding();
            string text;
            string path = GetFileInPath(pathNow, false);
            switch (encoding)
            {
                case 1:
                    text = GetText(path, "UTF8");
                    break;
                case 2:
                    text = GetText(path, "Unicode");
                    break;
                case 3:
                    text = GetText(path, "ASCII");
                    break;
                default:
                    text = GetText(path, "UTF32");
                    break;
            }
            Console.WriteLine(text);
        }

        /// <summary>
        /// Метод для копирования файла.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void CopyFile(string pathNow)
        {
            Console.WriteLine("Копировать файл в себя нельзя.");
            string path = GetFileInPath(pathNow, false);
            Console.WriteLine("Введите путь, куда хотите скопировать файл.");
            string pathNew = GetFile();
            File.Copy(path, pathNew, true);
        }

        /// <summary>
        /// Метод для перемещения файла.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void MoveFile(string pathNow)
        {
            string path = GetFileInPath(pathNow, false);
            Console.WriteLine("Введите путь, куда хотите перенести файл.");
            string pathNew = GetFile();
            File.Move(path, pathNew, true);
        }

        /// <summary>
        /// Метод для удаления файла.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void DeleteFile(string pathNow)
        {
            string path = GetFileInPath(pathNow, false);
            File.Delete(path);
        }

        /// <summary>
        /// Метод для создания файла в 1 из 4 кодировок.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void CreateFileInEncoding(string pathNow)
        {
            int encoding = GetEncoding();
            string path = GetFileInPath(pathNow, true);
            StringBuilder text = new StringBuilder("");
            Console.WriteLine("Введите текст файла.");
            Console.WriteLine("Для этого задайте количество строк.");
            Console.WriteLine("Оно должно быть неотрицательным.");
            int count;
            while (!int.TryParse(Console.ReadLine(), out count)
                || count < 0)
            {
                Console.WriteLine("Ваш ввод некорректен.");
                Console.WriteLine("Повторите попытку.");
            }
            Console.WriteLine("Теперь введите текст файла.");
            for (int i = 0; i < count; ++i)
            {
                text.Append(Console.ReadLine() + '\n');
            }
            switch (encoding)
            {
                case 1:
                    File.WriteAllText(path, text.ToString(), Encoding.UTF8);
                    break;
                case 2:
                    File.WriteAllText(path, text.ToString(), Encoding.Unicode);
                    break;
                case 3:
                    File.WriteAllText(path, text.ToString(), Encoding.ASCII);
                    break;
                default:
                    File.WriteAllText(path, text.ToString(), Encoding.UTF32);
                    break;
            }
        }

        /// <summary>
        /// Метод для конкатенации содержимого файлов.
        /// </summary>
        private static void ConcatenateFiles()
        {
            Console.WriteLine("Введите количество файлов.");
            Console.WriteLine("Оно должно быть неотрицательным.");
            int count;
            while (!int.TryParse(Console.ReadLine(), out count)
                || count < 0)
            {
                Console.WriteLine("Ваш ввод некорректен.");
                Console.WriteLine("Повторите попытку.");
            }
            string[] files = new string[count];
            for (int i = 0; i < count; ++i)
            {
                Console.WriteLine("Введите путь до файла.");
                files[i] = GetFile();
            }
            StringBuilder texts = new StringBuilder("");
            for (int i = 0; i < count; ++i)
            {
                texts.Append(GetText(files[i], "UTF8"));
            }
            Console.WriteLine(texts);
        }

        /// <summary>
        /// Метод для поиска файлов в директории, подходящих регулярке пользователя.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        private static void SearchFilesInDirectory(string pathNow)
        {
            if (Directory.Exists(pathNow))
            {
                string[] files = Directory.GetFiles(pathNow);
                Console.WriteLine("Введите маску.");
                Console.WriteLine("Маска задаётся в виде регулярного выражения.");
                string input = Console.ReadLine();
                Regex regex = new Regex(input);
                Console.WriteLine("Ваши файлы:");
                for (int i = 0; i < files.Length; ++i)
                {
                    string file = Path.GetFileName(files[i]);
                    if (regex.IsMatch(file))
                    {
                        Console.WriteLine(files[i]);
                    }
                }
                return;
            }
            Console.WriteLine("Указанная директория некорректна.");
            Console.WriteLine("Скорее всего вашей директории не существует.");
        }
        /// <summary>
        /// Метод для поиска файлов в поддиректориях, подходящих регулярке пользователя.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        /// <param name="regex">храним регулярку для передачи дальше.</param>
        /// <param name="first">true, если в 1 раз вызов, иначе false.</param>
        private static void SearchAllFilesInDirectories(string pathNow, Regex regex, bool first)
        {
            if (Directory.Exists(pathNow))
            {
                string[] files = Directory.GetFiles(pathNow);
                if (first)
                {
                    Console.WriteLine("Введите маску.");
                    Console.WriteLine("Маска задаётся в виде регулярного выражения.");
                    string input = Console.ReadLine();
                    regex = new Regex(input);
                    Console.WriteLine("Ваши файлы:");
                }
                for (int i = 0; i < files.Length; ++i)
                {
                    string file = Path.GetFileName(files[i]);
                    if (regex.IsMatch(file))
                    {
                        Console.WriteLine(files[i]);
                    }
                }
                string[] directories = Directory.GetDirectories(pathNow);
                for (int i = 0; i < directories.Length; ++i)
                {
                    SearchAllFilesInDirectories(directories[i], regex, false);
                }
                return;
            }
            Console.WriteLine("Указанная директория некорректна.");
            Console.WriteLine("Скорее всего вашей директории не существует.");
        }

        /// <summary>
        /// Метод для запроса операции, которую выбрал пользователь.
        /// </summary>
        /// <param name="number">номер операции для выполнения.</param>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        /// <param name="driver">хранящийся диск пользователя.</param>
        private static void Operations(int number, ref string pathNow, ref DriveInfo driver)
        {
            switch (number)
            {
                case 1:
                    DriverSelection(out pathNow, out driver);
                    break;
                case 2:
                    NewPath(ref pathNow, ref driver);
                    break;
                case 3:
                    PrintFiles(ref pathNow);
                    break;
                case 4:
                    PrintFileInEncoding(pathNow);
                    break;
                case 5:
                    CopyFile(pathNow);
                    break;
                case 6:
                    MoveFile(pathNow);
                    break;
                case 7:
                    DeleteFile(pathNow);
                    break;
                case 8:
                    CreateFileInEncoding(pathNow);
                    break;
                case 9:
                    ConcatenateFiles();
                    break;
                case 10:
                    SearchFilesInDirectory(pathNow);
                    break;
                default:
                    SearchAllFilesInDirectories(pathNow, new Regex(""), true);
                    break;
            }
        }

        /// <summary>
        /// Меню, в котором происходит вывод операций и считывание номера.
        /// </summary>
        /// <param name="pathNow">хранящийся путь пользователя.</param>
        /// <param name="driver">хранящийся диск пользователя.</param>
        /// <returns>true, если продолжить выполнение, иначе false.</returns>
        private static bool Menu(ref string pathNow, ref DriveInfo driver)
        {
            try
            {
                GetPathNow(pathNow);
                Console.Write(File.ReadAllText("TextMenu.txt"));
                int number;
                while (!int.TryParse(Console.ReadLine(), out number)
                    || number < 1 || number > 13)
                {
                    Console.WriteLine("Ваш ввод некорректен.");
                    Console.WriteLine("Повторите попытку.");
                }
                if (number == 12)
                {
                    Console.Write(File.ReadAllText("README.txt"));
                    return true;
                }
                if (number == 13)
                {
                    return false;
                }
                Operations(number, ref pathNow, ref driver);
            }
            catch
            {
                Console.WriteLine("Не удалось выполнить операцию.");
                Console.WriteLine("Проверьте корректность ваших данных.");
                Console.WriteLine("Проверьте, что пути написаны правильно.");
                Console.WriteLine("Они очень часто выводятся, так что их можно найти.");
                Console.WriteLine("Проверьте, что вы ничего случайно не удалили.");
                Console.WriteLine("Или случайно не перекрыли куда-то доступ.");
                Console.WriteLine("Возможно вы некорректно ввели регулярку.");
            }
            return true;
        }

        /// <summary>
        /// Main, в котором происходит приветствие и вызывается меню.
        /// </summary>
        private static void Main()
        {
            // Покраска вывода для читабельности интерфейса.
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Добро пожаловать в файловый менеджер!");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Здесь вы можете работать с файлами.");
            Console.WriteLine("На протяжении всей работы программы,");
            Console.WriteLine("для вашего удобства она будет хранить путь.");
            Console.WriteLine("Например вы хотите ввести " + @"C:\Users\name\Py");
            Console.WriteLine("А у вас уже есть " + @"C:\Users\");
            Console.WriteLine("Тогда вам будет нужно ввести только " + @"name\Py");
            Console.WriteLine("Перед началом работы надо выбрать диск.");
            Console.WriteLine("В дальнейшем вы сможете его поменять.");
            DriveInfo driver;
            string pathNow;
            DriverSelection(out pathNow, out driver);
            // Проверка на окончание программы.
            bool notEnd = true;
            while (notEnd)
            {
                notEnd = Menu(ref pathNow, ref driver);
            }
        }
    }
}