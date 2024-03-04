using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class DeclareState : IState
{
    private Parser _parser;

    public DeclareState(Parser parser)
    {
        _parser = parser;
    }

    public bool Handle(Lexeme lexeme, LexemeType? nextType)
    {
        bool flag = true;

        if (lexeme.Type != LexemeType.DECLARE)
        {
            if (!(_parser.FindLexeme(_parser.IncorrectLexemes, lexeme, "DECLARE")))
            {
                lexeme.Message = "Ожидалось ключевое слово \"DECLARE\"";
                _parser.IncorrectLexemes.Add(lexeme);
            }
            else
            {
                _parser.CurrentState = _parser.IdState;
            }
            flag = false;
        }
        else
        {
            _parser.CurrentState = _parser.IdState;
        }

        return flag;
    }
}
