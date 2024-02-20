using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public interface IState
{
    bool Handle(Lexeme lexeme, LexemeType? nextType);
}
