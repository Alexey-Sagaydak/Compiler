using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class SignState : IState
{
    private Parser _parser;

    public SignState(Parser parser)
    {
        _parser = parser;
    }

    public bool Handle(Lexeme lexeme, LexemeType? nextType)
    {
        bool flag = true;

        if (lexeme.Type != LexemeType.Sign)
        {
            lexeme.Message = "Ожидался знак";
            _parser.IncorrectLexemes.Add(lexeme);
            flag = false;
        }
        else
        {
            _parser.CurrentState = _parser.NumberState;
        }

        return flag;
    }
}
