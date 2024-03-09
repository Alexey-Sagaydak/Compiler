using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public static class TextCleaner
{
    public static string RemoveIncorrectLexemes(string inputString, ObservableCollection<ParserError> _incorrectLexemes)
    {
        //if (_incorrectLexemes.Last().Type == LexemeType.UnfinishedExpression)
        //{
        //    inputString = inputString.Insert(_incorrectLexemes.Last().EndIndex - 1, "DECLARE var CONSTANT INTEGER = +10;");
        //    _incorrectLexemes.Remove(_incorrectLexemes.Last());
        //}

        foreach (var lexeme in _incorrectLexemes.Reverse())
        {
            int fragmentLength = lexeme.EndIndex - lexeme.StartIndex + 1;
            int fragmentStartIndex = lexeme.StartIndex - 1;

            if (fragmentStartIndex > 0 && inputString[fragmentStartIndex - 1] == ' ' &&
                (fragmentStartIndex + fragmentLength) < (inputString.Length - 1) &&
                inputString[fragmentStartIndex + fragmentLength] == ' ')
                fragmentLength++;

            inputString = inputString.Remove(fragmentStartIndex, fragmentLength);
        }

        inputString = inputString.Replace("  ", " ");

        return inputString;
    }
}
