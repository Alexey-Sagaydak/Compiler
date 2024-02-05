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
        }

        private void CutSelectedText(object sender, RoutedEventArgs e)
        {
            TextRange selectedTextRange = MyRichTextBox.Selection;

            if (selectedTextRange != null && !string.IsNullOrEmpty(selectedTextRange.Text))
            {
                Clipboard.SetText(selectedTextRange.Text);
                selectedTextRange.Text = string.Empty;
            }
        }

        private void CopySelectedText(object sender, RoutedEventArgs e)
        {
            TextRange selectedTextRange = MyRichTextBox.Selection;

            if (selectedTextRange != null && !string.IsNullOrEmpty(selectedTextRange.Text))
            {
                Clipboard.SetText(selectedTextRange.Text);
            }
        }

        private void PasteText(object sender, RoutedEventArgs e)
        {
            TextRange selection = MyRichTextBox.Selection;

            if (selection != null && Clipboard.ContainsText())
            {
                selection.Text = Clipboard.GetText();
            }
        }

        private void DeleteSelectedText(object sender, RoutedEventArgs e)
        {
            TextRange selection = MyRichTextBox.Selection;

            if (selection != null && !selection.IsEmpty)
            {
                selection.Text = string.Empty;
            }
        }

        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(MyRichTextBox.Document.ContentStart, MyRichTextBox.Document.ContentEnd);
            MyRichTextBox.Selection.Select(textRange.Start, textRange.End);
        }

        private void MyRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextPointer caretPosition = MyRichTextBox.CaretPosition;

            if (caretPosition != null)
            {
                CursorPositionTextBlock.Text = $"Строка: {GetLineNumber(MyRichTextBox)}, Столбец: {GetColumnNumber(MyRichTextBox)}";
            }
        }

        private int GetColumnNumber(RichTextBox richTextBox)
        {
            return Math.Max(
                new TextRange(
                    richTextBox.CaretPosition.GetLineStartPosition(0),
                    richTextBox.CaretPosition).Text.Length + 1, 1);
        }

        private int GetLineNumber(RichTextBox richTextBox)
        {
            TextPointer caretLineStart = richTextBox.CaretPosition.GetLineStartPosition(0);
            TextPointer p = richTextBox.Document.ContentStart.GetLineStartPosition(0);
            int currentLineNumber = 0;

            while (true)
            {
                if (p == null || (caretLineStart != null && caretLineStart.CompareTo(p) < 0))
                {
                    break;
                }

                p = p.GetLineStartPosition(1, out var result);

                if (result == 0)
                {
                    break;
                }

                currentLineNumber++;
            }

            return Math.Max(1, currentLineNumber);
        }
    }
}