using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class ConstantState : IState
{
    private Parser _parser;

    public ConstantState(Parser parser)
    {
        _parser = parser;
    }

    public bool Handle(Lexeme lexeme, LexemeType? nextType)
    {
        bool flag = true;

        if (lexeme.Type != LexemeType.CONSTANT)
        {
            lexeme.Message = "Ожидалось ключевое слово \"CONSTANT\"";
            _parser.IncorrectLexemes.Add(lexeme);
            flag = false;
        }
        else
        {
            _parser.CurrentState = _parser.IntegerState;
        }

        return flag;
    }
}
