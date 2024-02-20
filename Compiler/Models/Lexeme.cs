using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public enum LexemeType
{
    DECLARE = 1,
    CONSTANT = 2,
    INTEGER = 3,
    Identifier = 4,
    Whitespace = 5,
    NewLine = 6,
    AssignmentOperator = 7,
    UnsignedInteger = 8,
    Sign = 9,
    Semicolon = 10,
    InvalidCharacter = 11
}

public class Lexeme
{
    private string[] lexemeNames;
    private string message;
    
    public int LexemeId { get => (int)Type; }
    public string LexemeName { get => lexemeNames[LexemeId - 1]; }
    public LexemeType Type { get; set; }
    public string Value { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public string Position { get => $"с {StartIndex} по {EndIndex} символы"; }
    public string Message {
        get => $"{message} (Отброшенный фрагмент: \"{Value}\")";
        set => message = value;
    }
    
    public Lexeme(LexemeType type, string value, int startIndex, int endIndex)
    {
        Type = type;
        Value = value;
        StartIndex = startIndex;
        EndIndex = endIndex;

        lexemeNames =
        [
            "Ключевое слово",               // 0
            "Ключевое слово",               // 1
            "Ключевое слово",               // 2
            "Идентификатор",                // 3
            "Пробел",                       // 4
            "Перенос на следующую строку",  // 5
            "Оператор присваивания",        // 6
            "Целое число без знака",        // 7
            "Знак",                         // 8
            "Конец оператора",              // 9
            "Недопустимый символ"           // 10
        ];
    }
}
