using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class NumberState : IState
{
    private List<ParserError> errors;
    private StringHelper stringHelper;
    private Dictionary<LexemeType, IState> StateMap;

    public NumberState(List<ParserError> errors, StringHelper stringHelper, Dictionary<LexemeType, IState> StateMap)
    {
        this.errors = errors;
        this.stringHelper = stringHelper;
        this.StateMap = StateMap;
    }

    public bool Handle()
    {
        stringHelper.SkipSpaces();
        while (stringHelper.Current != ';')
        {
            ParserError error = new ParserError("Ожидалось число или оператор конца выражения \";\"", stringHelper.Index + 1, stringHelper.Index + 1);
            while (true)
            {
                if (!stringHelper.CanGetNext)
                {
                    if (error.Value != string.Empty)
                        errors.Add(error);
                    errors.Add(new ParserError("Обнаружено незаконченное выражение", stringHelper.Index, stringHelper.Index, ErrorType.UnfinishedExpression));
                    return false;
                }
                char currentSymbol = stringHelper.Current;

                if (char.IsDigit(currentSymbol) || currentSymbol == ';')
                {
                    if (error.Value != string.Empty)
                        errors.Add(error);
                    if (currentSymbol != ';')
                        currentSymbol = stringHelper.Next;
                    break;
                }
                else
                {
                    error.Value += currentSymbol;
                    error.EndIndex = stringHelper.Index + 1;
                }
                currentSymbol = stringHelper.Next;
            }
        }

        StateMap[LexemeType.Semicolon].Handle();
        return true;
    }
}
