using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class SemicolonState : IState
{
	private Parser _parser;

	public SemicolonState(Parser parser)
	{
		_parser = parser;
	}

	public bool Handle(Lexeme lexeme, LexemeType? nextType)
	{
		bool flag = true;

		if (lexeme.Type != LexemeType.Semicolon)
		{
			lexeme.Message = "Ожидалось \";\"";
			_parser.IncorrectLexemes.Add(lexeme);
			flag = false;
		}
		else
		{
            if (nextType != null && nextType == LexemeType.Identifier)
            {
                _parser.CurrentState = _parser.IdState;
            }
            else
            {
                _parser.CurrentState = _parser.DeclareState;
            }
        }

        return flag;
	}
}
