using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class ParserError
{
    private string message;

    public string Value { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public string Position { get => $"с {StartIndex} по {EndIndex} символы"; }
    public bool IsModified {  get; set; }
    public string Message
    {
        get => $"{message} (Отброшенный фрагмент: \"{Value}\")";
        set => message = value;
    }

    public ParserError(string message, int startIndex, int endIndex)
    {
        Message = message;
        Value = string.Empty;
        StartIndex = startIndex;
        EndIndex = endIndex;
        IsModified = false;
    }
}
