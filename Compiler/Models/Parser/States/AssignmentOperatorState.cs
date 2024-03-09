using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Compiler;

public class AssignmentOperatorState : IState
{
    private List<ParserError> errors;
    private StringHelper stringHelper;
    private Dictionary<LexemeType, IState> StateMap;

    public AssignmentOperatorState(List<ParserError> errors, StringHelper stringHelper, Dictionary<LexemeType, IState> StateMap)
    {
        this.errors = errors;
        this.stringHelper = stringHelper;
        this.StateMap = StateMap;
    }

    public bool Handle()
    {
        stringHelper.SkipSpaces();
        char currentSymbol;
        bool IsLeftPartMet = false;

        ParserError error = new ParserError("Ожидался оператор присваивания", stringHelper.Index + 1, stringHelper.Index + 1);
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

            if (currentSymbol == ':' && !IsLeftPartMet)
            {
                IsLeftPartMet = true;
                if (error.Value != string.Empty)
                    errors.Add(error);
                error = new ParserError("Ожидался оператор присваивания", stringHelper.Index + 2, stringHelper.Index + 1);
            }
            else if (currentSymbol == '=')
            {
                if (error.Value != string.Empty)
                    errors.Add(error);
                if (stringHelper.CanGetNext)
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

        StateMap[LexemeType.Sign].Handle();
        return true;
    }
}
