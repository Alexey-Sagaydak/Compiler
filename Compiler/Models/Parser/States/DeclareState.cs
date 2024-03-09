using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class DeclareState : IState
{
    private List<ParserError> errors;
    private StringHelper stringHelper;
    private Dictionary<LexemeType, IState> stateMap;

    public DeclareState(List<ParserError> errors, StringHelper stringHelper, Dictionary<LexemeType, IState> StateMap)
    {
        this.errors = errors;
        this.stringHelper = stringHelper;
        stateMap = StateMap;
    }

    public bool Handle()
    {
        stringHelper.SkipSpaces();
        foreach (char c in "DECLARE ")
        {
            ParserError error = new ParserError("Ожидалось ключевое слово DECLARE", stringHelper.Index + 1, stringHelper.Index + 1);
            while (true)
            {
                if (!stringHelper.CanGetNext)
                {
                    if (error.Value != string.Empty)
                        errors.Add(error);
                    return false;
                }
                char currentSymbol = stringHelper.Current;
                
                if (currentSymbol == c)
                {
                    if (error.Value != string.Empty)
                        errors.Add(error);
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

        stateMap[LexemeType.Identifier].Handle();
        return true;
    }
}
