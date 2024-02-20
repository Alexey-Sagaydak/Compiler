using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class Parser : IParser
{
    public List<Lexeme> IncorrectLexemes { get; set; }

    public IState DeclareState { get; set; }
    public IState IdState { get; set; }
    public IState ConstantState { get; set; }
    public IState IntegerState { get; set; }
    public IState AssignmentOperatorState { get; set; }
    public IState SignState { get; set; }
    public IState NumberState { get; set; }
    public IState SemicolonState { get; set; }

    public IState CurrentState { get; set; }

    public List<Lexeme> Parse(List<Lexeme> lexemes)
    {
        IncorrectLexemes.Clear();
        CurrentState = DeclareState;

        for (int i = 0; i < lexemes.Count; i++)
        {
            bool result = !CurrentState.Handle(lexemes[i], ((lexemes.Count - 1) > i) ? lexemes[i + 1].Type : null);
            if (result && CurrentState == NumberState && lexemes[i + 1].Type == LexemeType.Sign)
            {
                CurrentState = SignState;
            }
        }

        if (lexemes.Count > 0 && lexemes.Last().Type != LexemeType.Semicolon)
        {
            lexemes.Last().Message = "Ожидалось законченное выражение";
            IncorrectLexemes.Add(lexemes.Last());
        }

        return IncorrectLexemes;
    }

    public Parser()
    {
        DeclareState = new DeclareState(this);
        IdState = new IdState(this);
        ConstantState = new ConstantState(this);
        IntegerState = new IntegerState(this);
        AssignmentOperatorState = new AssignmentOperatorState(this);
        SignState = new SignState(this);
        NumberState = new NumberState(this);
        SemicolonState = new SemicolonState(this);

        CurrentState = DeclareState;

        IncorrectLexemes = new List<Lexeme>();
    }
}
