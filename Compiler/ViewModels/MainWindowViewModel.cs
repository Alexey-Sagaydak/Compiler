using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Compiler;

public class MainWindowViewModel : ViewModelBase
{
    private FileManager _fileManager;
    private const string _aboutPath = @"Resources\About.html";
    private const string _helpPath = @"Resources\Help.html";
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

    public event EventHandler<StringEventArgs> StringSent;

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
        }
    }

    public string CurrentFilePath
    {
        get { return _currentFilePath; }
        set
        {
            _currentFilePath = value;
            OnPropertyChanged(nameof(CurrentFilePath));
        }
    }

    public MainWindowViewModel()
    {
        _fileManager = new FileManager();
        FileContent = string.Empty;
        IsFileModified = false;
    }

    public RelayCommand CreateNewFileCommand
    {
        get => _createNewFileCommand ??= new RelayCommand(CreateNewFile, _ => !_isFileModified);
    }

    public RelayCommand OpenFileCommand
    {
        get => _openFileCommand ??= new RelayCommand(OpenFile);
    }

    public RelayCommand SaveFileCommand
    {
        get => _saveFileCommand ??= new RelayCommand(SaveFile);
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

    public void CreateNewFile(object obj)
    {
        FileContent = string.Empty;
        IsFileModified = true;
    }

    public void OpenFile(object obj)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            _currentFilePath = openFileDialog.FileName;
            ReadFileContent();
        }
    }

    public void SaveAsFile(object obj)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Текстовый файл (*.txt)|*.txt|C# файл (*.cs)|*.cs|C файл (*.c)|*.c|C++ файл (*.cpp)|*.cpp|Все файлы (*.*)|*.*";

        if (saveFileDialog.ShowDialog() == true)
        {
            _currentFilePath = saveFileDialog.FileName;
            _fileManager.SaveFile(_currentFilePath, FileContent);
            IsFileModified = false;
        }
    }

    public void SaveFile(object obj)
    {
        if (string.IsNullOrEmpty(_currentFilePath))
        {
            SaveAsFile(null);
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
        IsFileModified = false;
        SendString(_fileContent);
    }

    public void HandleDroppedFiles(string[] files)
    {
        CurrentFilePath = files[0];
        ReadFileContent();
    }
}
