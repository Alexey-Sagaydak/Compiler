using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class NumberState : IState
{
    private Parser _parser;

    public NumberState(Parser parser)
    {
        _parser = parser;
    }

    public bool Handle(Lexeme lexeme, LexemeType? nextType)
    {
        bool flag = true;

        if (lexeme.Type != LexemeType.UnsignedInteger)
        {
            lexeme.Message = "Ожидалось целое число";
            _parser.IncorrectLexemes.Add(lexeme);
            flag = false;
        }
        else
        {
            _parser.CurrentState = _parser.SemicolonState;
        }

        return flag;
    }
}
