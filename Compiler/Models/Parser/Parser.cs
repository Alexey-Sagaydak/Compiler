using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Compiler;

public class Parser : IParser
{
    public Dictionary<LexemeType, IState> StateMap;
    public StringHelper StringHelper { get; set; }
    public List<ParserError> Errors { get; set; }

    public List<ParserError> Parse(string text = "")
    {
        Errors.Clear();
        StringHelper.Source = text;

        StateMap[LexemeType.DECLARE].Handle();
        return Errors;
    }

    public Parser(string text)
    {
        Errors = new List<ParserError>();
        StringHelper = new StringHelper(text);
        StateMap = new Dictionary<LexemeType, IState>();

        StateMap.Add(LexemeType.DECLARE, new DeclareState(Errors, StringHelper, StateMap));
        StateMap.Add(LexemeType.Identifier, new IdState(Errors, StringHelper, StateMap));
        StateMap.Add(LexemeType.CONSTANT, new ConstantState(Errors, StringHelper, StateMap));
        StateMap.Add(LexemeType.INTEGER, new IntegerState(Errors, StringHelper, StateMap));
        StateMap.Add(LexemeType.AssignmentOperator, new AssignmentOperatorState(Errors, StringHelper, StateMap));
        StateMap.Add(LexemeType.Sign, new SignState(Errors, StringHelper, StateMap));
        StateMap.Add(LexemeType.Number, new NumberState(Errors, StringHelper, StateMap));
        StateMap.Add(LexemeType.Semicolon, new SemicolonState(Errors, StringHelper, StateMap));
    }
}
