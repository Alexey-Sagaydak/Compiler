using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Compiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel vm = new MainWindowViewModel();
            DataContext = vm;
            vm.StringSent += OnStringReceived;
        }

        public void OnStringReceived(object sender, StringEventArgs e)
        {
            Console.WriteLine("\n\n######");
            Console.WriteLine(e.Message);
            Console.WriteLine("\n\n######");
            textEditor.Document.Text = e.Message;
        }

        private void CutSelectedText(object sender, RoutedEventArgs e)
        {
            if (textEditor.SelectionLength > 0)
            {
                Clipboard.SetText(textEditor.SelectedText);
                textEditor.Document.Remove(textEditor.SelectionStart, textEditor.SelectionLength);
            }
        }

        private void CopySelectedText(object sender, RoutedEventArgs e)
        {
            if (textEditor.SelectionLength > 0)
            {
                Clipboard.SetText(textEditor.SelectedText);
            }
        }

        private void PasteText(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textEditor.Document.Insert(textEditor.CaretOffset, Clipboard.GetText());
            }
        }

        private void DeleteSelectedText(object sender, RoutedEventArgs e)
        {
            if (textEditor.SelectionLength > 0)
            {
                textEditor.Document.Remove(textEditor.SelectionStart, textEditor.SelectionLength);
            }
        }

        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            textEditor.SelectAll();
        }

        private void MyRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int lineNumber = textEditor.CaretOffset = 0;
            //int lineNumber = textEditor.TextArea.Document.GetLineByOffset(textEditor.CaretOffset).LineNumber + 1;
            int columnNumber = textEditor.CaretOffset - textEditor.TextArea.Document.GetLineByOffset(textEditor.CaretOffset).Offset + 1;
            CursorPositionTextBlock.Text = $"Строка: {lineNumber}, Столбец: {columnNumber}";
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            // Проверяем, что файлы действительно перетаскиваются
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Разрешаем копирование файлов
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                // Запрещаем перетаскивание
                e.Effects = DragDropEffects.None;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            // Получаем список путей к перетаскиваемым файлам
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Если есть хотя бы один файл
            if (files != null && files.Length > 0)
            {
                // Передаем путь к файлу в свойство вашего ViewModel
                (DataContext as MainWindowViewModel)?.HandleDroppedFiles(files);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}