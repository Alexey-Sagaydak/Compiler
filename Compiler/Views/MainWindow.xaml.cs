﻿using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Windows.Media.Animation;

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
            viewModel.LexemeSent += OnLexemeReceived;
            viewModel.ErrorSent += OnErrorReceived;
            viewModel.RequestClose += (sender, e) => Close();
            Closing += MainWindow_Closing;

            DataContext = viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (StreamReader s = new StreamReader(@"Resources\SQL.xshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
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

        public void OnLexemeReceived(object sender, Lexeme e)
        {
            if (e != null && e.EndIndex <= textEditor.Document.Text.Length)
            {
                textEditor.Select(e.StartIndex - 1, e.EndIndex - e.StartIndex + 1);
            }
        }

        public void OnErrorReceived(object sender, ParserError e)
        {
            if (e != null && e.EndIndex <= textEditor.Document.Text.Length)
            {
                textEditor.Select(e.StartIndex - 1, e.EndIndex - e.StartIndex + 1);
            }
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
            undoButton.IsEnabled = textEditor.CanUndo;
            redoButton.IsEnabled = textEditor.CanRedo;

            undoMenuItem.IsEnabled = textEditor.CanUndo;
            redoMenuItem.IsEnabled = textEditor.CanRedo;

            GetCaretPosition();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popupMenu.IsOpen = true;
        }

        private void textEditor_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            GetCaretPosition();
        }

        private void textEditor_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            GetCaretPosition();
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Undo();
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Redo();
        }

        private void fontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    int fontSize = Convert.ToInt32(fontSizeComboBox.Text);
                    textEditor.FontSize = fontSize;
                    parserDataGrid.FontSize = fontSize;
                    lexerDataGrid.FontSize = fontSize;
                    outputTextBlock.FontSize = fontSize;
                }
                catch { }
            }));
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ((MainWindowViewModel)(DataContext)).StartAnalyzersCommand.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.F6 && errorStackPanel.Visibility == Visibility.Visible)
            {
                ((MainWindowViewModel)(DataContext)).RemoveErrorsCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("""
                    Для грамматики G[<While>] разработать и реализовать алгоритм анализа на основе метода рекурсивного спуска.
                
                G[<While>]:
                1. <While> → while <Cond> do <Stmt> end ;
                2. <Cond> → <LogExpr> {or <LogExpr>}
                3. <LogExpr> → <RelExpr> {and <RelExpr>}
                4. <RelExpr> → <Operand> [rel <Operand>]
                5. <Operand> → var | const
                6. <Stmt> → var as <ArithExpr>
                7. <ArithExpr> → <Operand> {ao <Operand>}

                    Примечание: while, do, end, and, or – ключевые слова. В тип rel объединили операции сравнения <,<=, >=, >, != и ==, в тип ao арифметические операции + и -, в тип as оператор присваивания =, тип var – название переменной (только буквы), тип const – числа. Причина, по которой не объединены в один тип логические операции and и or заключается в том, что эти операции имеют различный приоритет. Пример цепочки: while a < b and b <= c do b=b+c-20 end;
                """, "Задание", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            textEditor.Text = """
                while a < b do b = b - 20 end;
                while a < b and c != d or e == f or g <= h and i > j do abc = cde - 20 + 40 - 8 + efg end;
                while a < b do b = a + ? end;
                """;
        }
    }
}