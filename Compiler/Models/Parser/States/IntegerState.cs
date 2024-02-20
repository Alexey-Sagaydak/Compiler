using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class IntegerState : IState
{
    private Parser _parser;

    public IntegerState(Parser parser)
    {
        _parser = parser;
    }

    public bool Handle(Lexeme lexeme, LexemeType? nextType)
    {
        bool flag = true;

        if (lexeme.Type != LexemeType.INTEGER)
        {
            lexeme.Message = "Ожидалось ключевое слово \"INTEGER\"";
            _parser.IncorrectLexemes.Add(lexeme);
            flag = false;
        }
        else
        {
            _parser.CurrentState = _parser.AssignmentOperatorState;
        }

        return flag;
    }
}
