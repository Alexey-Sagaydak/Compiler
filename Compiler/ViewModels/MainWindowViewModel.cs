using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using Lab7;

namespace Compiler;

public class MainWindowViewModel : ViewModelBase
{
    private IParser _parser;
    private WhileParser _whileParser;

    private FileManager _fileManager;
    private ILexicalAnalyzer _lexicalAnalyzer;
    private WhileLexer _whileLexer;

    private const string _aboutPath = @"Resources\About.html";
    private const string _helpPath = @"Resources\Help.html";
    private const string _grammarClassificationPath = @"Resources\GrammarClassification.html";
    private const string _grammarPath = @"Resources\Grammar.html";
    private const string _methodOfAnalysisPath = @"Resources\MethodOfAnalysis.html";
    private const string _literaturePath = @"Resources\ListOfLiterature.html";
    private const string _neutralizingErrorsPath = @"Resources\NeutralizingErrors.html";
    private const string _problemStatementPath = @"Resources\ProblemStatement.html";
    private const string _correctTestCasePath = @"Resources\correct_test_case.txt";
    private const string _wrongTestCasePath = @"Resources\wrong_test_case.txt";
    private const string _sourceCode = @"https://github.com/Alexey-Sagaydak/Compiler";

    private string _currentFilePath;
    private string _fileContent;
    private bool _isFileModified;

    private RelayCommand _createNewFileCommand;
    private RelayCommand _openFileCommand;
    private RelayCommand _saveFileCommand;
    private RelayCommand _saveAsFileCommand;
    private RelayCommand _aboutCommand;
    private RelayCommand _literatureCommand;
    private RelayCommand _helpCommand;
    private RelayCommand _exitCommand;
    private RelayCommand _problemStatementCommand;
    private RelayCommand _openTestCaseCommand;
    private RelayCommand _openWrongTestCaseCommand;
    private RelayCommand _grammarCommand;
    private RelayCommand _grammarClassificationCommand;
    private RelayCommand _neutralizingErrorsCommand;
    private RelayCommand _methodOfAnalysisCommand;
    private RelayCommand _startAnalyzersCommand;
    private RelayCommand _viewSourceCodeCommand;
    private RelayCommand _removeErrorsCommand;
    private RelayCommand _parseWhileCommand;

    public event EventHandler<StringEventArgs> StringSent;
    public event EventHandler<Lexeme> LexemeSent;
    public event EventHandler<ParserError> ErrorSent;

    private List<Lexeme> _lexemesList;
    private List<Token> _tokensList;
    private ObservableCollection<Lexeme> _lexemes;
    private ObservableCollection<ParserError> _incorrectLexemes;
    private Lexeme _selectedLexeme;
    private ParserError _selectedError;
    private string _vmText;

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

    public ObservableCollection<ParserError> IncorrectLexemes
    {
        get { return _incorrectLexemes; }
        set
        {
            _incorrectLexemes = value;
            OnPropertyChanged(nameof(IncorrectLexemes));
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

    public ParserError SelectedError
    {
        get { return _selectedError; }
        set
        {
            _selectedError = value;
            ErrorSent(this, value);
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

    public string VMText
    {
        get { return _vmText; }
        set
        {
            _vmText = value;
            OnPropertyChanged(nameof(VMText));
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
        IncorrectLexemes = new ObservableCollection<ParserError>();
        _whileLexer = new WhileLexer();
        _whileParser = new WhileParser();
        _parser = new Parser(string.Empty);
    }

    public RelayCommand NeutralizingErrorsCommand
    {
        get => _neutralizingErrorsCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_neutralizingErrorsPath));
    }
    
    public RelayCommand MethodOfAnalysisCommand
    {
        get => _methodOfAnalysisCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_methodOfAnalysisPath));
    }
    
    public RelayCommand GrammarClassificationCommand
    {
        get => _grammarClassificationCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_grammarClassificationPath));
    }

    public RelayCommand GrammarCommand
    {
        get => _grammarCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_grammarPath));
    }

    public RelayCommand ProblemStatementCommand
    {
        get => _problemStatementCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_problemStatementPath));
    }

    public RelayCommand LiteratureCommand
    {
        get => _literatureCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_literaturePath));
    }

    public RelayCommand ViewSourceCodeCommand
    {
        get => _viewSourceCodeCommand ??= new RelayCommand(_ => HtmlHelper.OpenInBrowser(_sourceCode));
    }

    public RelayCommand CreateNewFileCommand
    {
        get => _createNewFileCommand ??= new RelayCommand(CreateNewFile);
    }

    public RelayCommand ParseWhileCommand
    {
        get => _parseWhileCommand ??= new RelayCommand(ParseWhile);
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
    
    public RelayCommand OpenWrongTestCaseCommand
    {
        get => _openWrongTestCaseCommand ??= new RelayCommand(OpenWrongTestCase);
    }

    public RelayCommand ExitCommand
    {
        get => _exitCommand ??= new RelayCommand(Exit);
    }

    public RelayCommand StartAnalyzersCommand
    {
        get => _startAnalyzersCommand ??= new RelayCommand(StartAnalysis);
    }
    
    public RelayCommand RemoveErrorsCommand
    {
        get => _removeErrorsCommand ??= new RelayCommand(RemoveErrors);
    }

    public void ParseWhile(object obj)
    {
        VMText = _whileParser.Parse(_whileLexer.Analyze(_fileContent));
    }

    public void RemoveErrors(object obj)
    {
        StartAnalysis();
        FileContent = TextCleaner.RemoveIncorrectLexemes(_fileContent, _incorrectLexemes);
        StartAnalysis();

        SendString(_fileContent);
    }

    public void OpenTestCase(object obj)
    {
        CurrentFilePath = _correctTestCasePath;
        ReadFileContent();
    }

    public void OpenWrongTestCase(object obj)
    {
        CurrentFilePath = _wrongTestCasePath;
        ReadFileContent();
    }

    public void StartAnalysis(object obj = null)
    {
        LexicalAnalysis();
        Parsing();
    }

    public void LexicalAnalysis()
    {
        _lexemesList = _lexicalAnalyzer.Analyze(FileContent);

        Lexemes = new ObservableCollection<Lexeme>(_lexemesList);
    }

    public void Parsing()
    {
        IncorrectLexemes = new ObservableCollection<ParserError>(_parser.Parse(FileContent));
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
