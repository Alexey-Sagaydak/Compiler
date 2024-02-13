using System.Windows;

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

            MainWindowViewModel viewModel = new MainWindowViewModel();
            viewModel.StringSent += OnStringReceived;
            viewModel.RequestClose += (sender, e) => Close();
            Closing += MainWindow_Closing;

            DataContext = viewModel;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (((MainWindowViewModel)DataContext).IsFileModified)
            {
                if (MessageBoxHelper.ShowWindowClosingMessage() == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        public void OnStringReceived(object sender, StringEventArgs e)
        {
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

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files != null && files.Length > 0)
            {
                (DataContext as MainWindowViewModel)?.HandleDroppedFiles(files);
            }
        }

        private void GetCaretPosition()
        {
            int offset = textEditor.CaretOffset;
            var location = textEditor.Document.GetLocation(offset);
            CursorPositionTextBlock.Text = $"Строка: {location.Line}, Столбец: {location.Column}";
        }

        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            GetCaretPosition();
        }

        private void textEditor_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            GetCaretPosition();
        }

        private void textEditor_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            GetCaretPosition();
        }
    }
}