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
            Lexeme lexeme = new Lexeme(LexemeType.UnfinishedExpression, "", lexemes.Last().EndIndex + 1, lexemes.Last().EndIndex + 1);
            lexeme.Message = "Ожидалось законченное выражение";
            IncorrectLexemes.Add(lexeme);
        }

        return IncorrectLexemes;
    }

    public bool FindLexeme(List<Lexeme> incorrectLexemes, Lexeme source, string key)
    {
        int index = source.Value.IndexOf(key);
        string message = "Ожидалось " + key;

        if (index != -1)
        {
            if (index > 0)
            {
                string leftValue = source.Value.Substring(0, index);
                Lexeme leftLexeme = new Lexeme(source.Type, leftValue, source.StartIndex, source.StartIndex + index - 1);
                leftLexeme.Message = message;
                incorrectLexemes.Add(leftLexeme);
            }

            if (index + key.Length < source.Value.Length)
            {
                string rightValue = source.Value.Substring(index + key.Length);
                Lexeme rightLexeme = new Lexeme(source.Type, rightValue, source.StartIndex + index + key.Length, source.EndIndex);
                rightLexeme.Message = message;
                incorrectLexemes.Add(rightLexeme);
            }

            return true;
        }
        else
        {
            return false;
        }
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
