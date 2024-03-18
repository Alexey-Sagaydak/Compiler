using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7;

public class WhileParser
{
    private string result = "";
    private List<Token> tokens;
    private Token CurrToken;
    private int CurrIndex;
    private int MaxIndex;
    private const string sep1 = " → ";

    public string Parse(List<Token> tokensList)
    {
        tokens = tokensList;
        CurrIndex = 0;
        MaxIndex = tokensList.Count - 1;
        CurrToken = tokens[CurrIndex];
        result = string.Empty;

        try
        {
            While(false);
        }
        catch (SyntaxErrorException)
        {
            log("Syntax Error: Обнаружено незаконченное выражение.");
        }

        return result;
    }

    private void log(string str, string sep = sep1)
    {
        result += (result == string.Empty) ? str : $"{sep}{str}";
    }

    private void ChangeCurrentToken()
    {
        if (CanGetNext())
        {
            CurrIndex++;
            CurrToken = tokens[CurrIndex];
        }
        else
        {
            throw new SyntaxErrorException();
        }
    }

    private bool CanGetNext() => CurrIndex < MaxIndex;

    private void While(bool get)
    {
        if (get) ChangeCurrentToken();

        log("<While>", "");

        if (CurrToken.Type == TokenType.While)
        {
            log("while", sep1);
            Cond(true);
            if (CurrToken.Type == TokenType.Do)
            {
                log("do", sep1);
                Stmt(true);
                if (CurrToken.Type == TokenType.End)
                {
                    log("end", sep1);
                    ChangeCurrentToken();
                    if (CurrToken.Type == TokenType.Semicolon)
                    {
                        log(";", sep1);
                        log("\n", "\n");

                        if (CanGetNext())
                        {
                            While(true);
                        }
                    }
                    else
                    {
                        log("Syntax Error: Ожидался оператор конца выражения \";\".");
                    }
                }
                else
                {
                    log("Syntax Error: Ожидалось ключевое слово \"end\".");
                }
            }
            else
            {
                log("Syntax Error: Ожидалось ключевое слово \"do\".");
            }
        }
        else
        {
            log("Syntax Error: Ожидалось ключевое слово \"while\".");
        }
    }

    private void Cond(bool get)
    {
        if (get) ChangeCurrentToken();

        log("<Cond>", sep1);

        LogExpr(false);
        if (CurrToken.Type == TokenType.Or)
        {
            log("or", sep1);

            Cond(true);
        }
    }

    private void LogExpr(bool get)
    {
        if (get) ChangeCurrentToken();
        
        log("<LogExpr>", sep1);

        RelExpr(false);
        if (CurrToken.Type == TokenType.And)
        {
            log("and", sep1);

            LogExpr(true);
        }
    }

    private void RelExpr(bool get, bool isFirstOrSecond = true)
    {
        if (get) ChangeCurrentToken();

        log("<RelExp>", sep1);

        Operand(false);
        ChangeCurrentToken();
        if (CurrToken.Type == TokenType.Rel)
        {
            log($"Rel \"{CurrToken.Value}\"", sep1);

            RelExpr(true, false);
        }
        else if (isFirstOrSecond)
        {
            log("Syntax Error: Ожидалось хотя бы одно законченное логическое выражение.");
        }
    }

    private void Operand(bool get)
    {
        if (get) ChangeCurrentToken();

        log("<Operand>", sep1);

        switch (CurrToken.Type)
        {
            case TokenType.Var:
                log($"Var \"{CurrToken.Value}\"", sep1);
                break;
            case TokenType.Const:
                log($"Const \"{CurrToken.Value}\"", sep1);
                break;
            default:
                log("Syntax Error: Ожидался идентификатор или число.");
                break;

        }
    }

    private void Stmt(bool get)
    {
        if (get) ChangeCurrentToken();

        log("<Stmt>", sep1);

        if (CurrToken.Type == TokenType.Var)
        {
            log($"Var \"{CurrToken.Value}\"", sep1);

            ChangeCurrentToken();
            if (CurrToken.Type == TokenType.Assignment)
            {
                log("=", sep1);

                ArithExpr(true);
            }
            else
            {
                log("Syntax Error: Ожидался оператор присваивания.");
            }
        }
        else
        {
            log("Syntax Error: Ожидался идентификатор.");
        }
    }

    private void ArithExpr(bool get)
    {
        if (get) ChangeCurrentToken();
        
        log("<ArithExpr>", sep1);

        Operand(false);
        ChangeCurrentToken();
        if (CurrToken.Type == TokenType.ArithOp)
        {
            log($"ao \"{CurrToken.Value}\"", sep1);
            ArithExpr(true);
        }
    }
}
