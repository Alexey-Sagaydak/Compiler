using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace Compiler;

public class MainWindowViewModel : ViewModelBase
{
    private FileManager _fileManager;
    private ILexicalAnalyzer _lexicalAnalyzer;
    private const string _aboutPath = @"Resources\About.html";
    private const string _helpPath = @"Resources\Help.html";
    private const string _testCasePath = @"Resources\test_case.txt";
    private string _currentFilePath;
    private string _fileContent;
    private bool _isFileModified;

    private RelayCommand _createNewFileCommand;
    private RelayCommand _openFileCommand;
    private RelayCommand _saveFileCommand;
    private RelayCommand _saveAsFileCommand;
    private RelayCommand _aboutCommand;
    private RelayCommand _helpCommand;
    private RelayCommand _exitCommand;
    private RelayCommand _openTestCaseCommand;
    private RelayCommand _startAnalyzersCommand;

    public event EventHandler<StringEventArgs> StringSent;
    public event EventHandler<Lexeme> LexemeSent;

    private ObservableCollection<Lexeme> _lexemes;
    private Lexeme _selectedLexeme;

    public event EventHandler RequestClose;

    public ObservableCollection<Lexeme> Lexemes
    {
        get { return _lexemes; }
        set
        {
            _lexemes = value;
            OnPropertyChanged(nameof(Lexemes));
        }
    }
    public Lexeme SelectedLexeme
    {
        get { return _selectedLexeme; }
        set
        {
            _selectedLexeme = value;
            LexemeSent(this, value);
            OnPropertyChanged(nameof(SelectedLexeme));
        }
    }

    public string FileContent
    {
        get { return _fileContent; }
        set
        {
            _fileContent = value;
            IsFileModified = true;
            OnPropertyChanged(nameof(FileContent));
        }
    }

    public bool IsFileModified
    {
        get { return _isFileModified; }
        set
        {
            _isFileModified = value;
            OnPropertyChanged(nameof(IsFileModified));
            OnPropertyChanged(nameof(WindowTitle));
        }
    }

    public string CurrentFilePath
    {
        get { return _currentFilePath; }
        set
        {
            _currentFilePath = value;
            OnPropertyChanged(nameof(CurrentFilePath));
            OnPropertyChanged(nameof(WindowTitle));
        }
    }

    public string WindowTitle
    {
        get => $"Компилятор — {((CurrentFilePath == string.Empty) ? "Новый файл.txt" : "")}{_currentFilePath.Split(@"\").Last()}{(IsFileModified ? "*" : "")} {((CurrentFilePath != string.Empty) ? "(" : "")}{_currentFilePath}{((CurrentFilePath != string.Empty) ? ")" : "")}";
    }

    public MainWindowViewModel()
    {
        _fileManager = new FileManager();
        _fileContent = string.Empty;
        CurrentFilePath = string.Empty;
        IsFileModified = false;
        _lexicalAnalyzer = new LexicalAnalyzer();
    }

    public RelayCommand CreateNewFileCommand
    {
        get => _createNewFileCommand ??= new RelayCommand(CreateNewFile);
    }

    public RelayCommand OpenFileCommand
    {
        get => _openFileCommand ??= new RelayCommand(OpenFile);
    }

    public RelayCommand SaveFileCommand
    {
        get => _saveFileCommand ??= new RelayCommand(SaveFile, _ => _isFileModified || CurrentFilePath == string.Empty);
    }

    public RelayCommand SaveAsFileCommand
    {
        get => _saveAsFileCommand ??= new RelayCommand(SaveAsFile);
    }
    
    public RelayCommand AboutCommand
    {
        get => _aboutCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_aboutPath));
    }
    
    public RelayCommand HelpCommand
    {
        get => _helpCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_helpPath));
    }

    public RelayCommand OpenTestCaseCommand
    {
        get => _openTestCaseCommand ??= new RelayCommand(OpenTestCase);
    }

    public RelayCommand ExitCommand
    {
        get => _exitCommand ??= new RelayCommand(Exit);
    }

    public RelayCommand StartAnalyzersCommand
    {
        get => _startAnalyzersCommand ??= new RelayCommand(StartAnalysis);
    }

    public void OpenTestCase(object obj)
    {
        CurrentFilePath = _testCasePath;
        ReadFileContent();
    }

    public void StartAnalysis(object obj)
    {
        LexicalAnalysis();
    }

    public void LexicalAnalysis()
    {
        Lexemes = new ObservableCollection<Lexeme>(_lexicalAnalyzer.Analyze(FileContent));
    }

    public void Exit(object obj = null)
    {
        if (CancelOperationAfterCheckingForUnsavedChanges())
            return;

        OnRequestClose();
    }

    public void CreateNewFile(object obj)
    {
        if (CancelOperationAfterCheckingForUnsavedChanges())
            return;

        FileContent = string.Empty;
        SendString(_fileContent);
        CurrentFilePath = string.Empty;
        IsFileModified = true;
    }

    public void OpenFile(object obj = null)
    {
        if (CancelOperationAfterCheckingForUnsavedChanges())
            return;

        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            _currentFilePath = openFileDialog.FileName;
            ReadFileContent();
        }
    }

    public void SaveAsFile(object obj = null)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Текстовый файл (*.txt)|*.txt|C# файл (*.cs)|*.cs|C файл (*.c)|*.c|C++ файл (*.cpp)|*.cpp|Python файл (*.py)|*.py|JavaScript файл (*.js)|*.js|HTML файл (*.html)|*.html|CSS файл (*.css)|*.css|XML файл (*.xml)|*.xml|JSON файл (*.json)|*.json|Markdown файл (*.md)|*.md|PHP файл (*.php)|*.php|Java файл (*.java)|*.java|Все файлы (*.*)|*.*";

        if (saveFileDialog.ShowDialog() == true)
        {
            _currentFilePath = saveFileDialog.FileName;
            _fileManager.SaveFile(_currentFilePath, FileContent);
            IsFileModified = false;
        }
    }

    public void SaveFile(object obj = null)
    {
        if (string.IsNullOrEmpty(_currentFilePath))
        {
            SaveAsFile();
        }
        else
        {
            _fileManager.SaveFile(_currentFilePath, FileContent);
            IsFileModified = false;
        }
    }

    public void SendString(string message)
    {
        if (StringSent != null)
        {
            StringSent(this, new StringEventArgs(message));
        }
    }

    public void ReadFileContent()
    {
        FileContent = _fileManager.OpenFile(_currentFilePath);
        SendString(_fileContent);
        IsFileModified = false;
    }

    public void HandleDroppedFiles(string[] files)
    {
        if (CancelOperationAfterCheckingForUnsavedChanges())
            return;

        CurrentFilePath = files[0];
        ReadFileContent();
    }

    public bool CancelOperationAfterCheckingForUnsavedChanges()
    {
        if (_isFileModified)
        {
            var result = MessageBoxHelper.ShowUnsavedChangesMessage();

            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveFile();
                    return false;
                case MessageBoxResult.Cancel:
                    return true;
            }
        }

        return false;
    }

    private void OnRequestClose()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}
