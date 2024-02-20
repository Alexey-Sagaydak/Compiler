using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Compiler;

public class AssignmentOperatorState : IState
{
    private Parser _parser;

    public AssignmentOperatorState(Parser parser)
    {
        _parser = parser;
    }

    public bool Handle(Lexeme lexeme, LexemeType? nextType)
    {
        bool flag = true;

        if (lexeme.Type != LexemeType.AssignmentOperator)
        {
            lexeme.Message = "Ожидался оператор присваивания";
            _parser.IncorrectLexemes.Add(lexeme);
            flag = false;
        }
        else
        {
            if (nextType != null && nextType == LexemeType.Sign)
            {
                _parser.CurrentState = _parser.SignState;
            }
            else
            {
                _parser.CurrentState = _parser.NumberState;
            }
        }

        return flag;
    }
}
