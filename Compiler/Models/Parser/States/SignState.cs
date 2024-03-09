using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class SignState : IState
{
    private List<ParserError> errors;
    private StringHelper stringHelper;
    private Dictionary<LexemeType, IState> StateMap;

    public SignState(List<ParserError> errors, StringHelper stringHelper, Dictionary<LexemeType, IState> StateMap)
    {
        this.errors = errors;
        this.stringHelper = stringHelper;
        this.StateMap = StateMap;
    }

    public bool Handle()
    {
        stringHelper.SkipSpaces();
        char currentSymbol;

        ParserError error = new ParserError("Ожидался знак или целое число", stringHelper.Index + 1, stringHelper.Index + 1);
        while (true)
        {
            if (!stringHelper.CanGetNext)
            {
                if (error.Value != string.Empty)
                    errors.Add(error);
                errors.Add(new ParserError("Обнаружено незаконченное выражение", stringHelper.Index, stringHelper.Index, ErrorType.UnfinishedExpression));

                return false;
            }

            currentSymbol = stringHelper.Current;

            if (currentSymbol == '-' || currentSymbol == '+')
            {
                if (error.Value != string.Empty)
                    errors.Add(error);
                if (stringHelper.CanGetNext)
                    currentSymbol = stringHelper.Next;
                break;
            }
            else if (char.IsDigit(currentSymbol))
            {
                if (error.Value != string.Empty)
                    errors.Add(error);
                StateMap[LexemeType.Number].Handle();
                return true;
            }
            else
            {
                error.Value += currentSymbol;
                error.EndIndex = stringHelper.Index + 1;
            }
            currentSymbol = stringHelper.Next;
        }
        StateMap[LexemeType.Number].Handle();
        return true;
    }
}
