using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7;

public class WhileLexer
{
    private List<Token> Tokens;

    public List<Token> Analyze(string input)
    {
        int i;
        string value;

        Tokens.Clear();

        for (i = 0; i < input.Length; i++)
        {
            value = string.Empty + input[i];

            if (char.IsLetter(input[i]))
            {
                int startIndex = i;

                while ((i + 1) < input.Length && char.IsLetter(input[i + 1]))
                {
                    i++;
                    value += input[i];
                }

                switch (value)
                {
                    case "while":
                        Tokens.Add(new Token(TokenType.While, value, startIndex + 1, i + 1));
                        break;
                    case "do":
                        Tokens.Add(new Token(TokenType.Do, value, startIndex + 1, i + 1));
                        break;
                    case "end":
                        Tokens.Add(new Token(TokenType.End, value, startIndex + 1, i + 1));
                        break;
                    case "and":
                        Tokens.Add(new Token(TokenType.And, value, startIndex + 1, i + 1));
                        break;
                    case "or":
                        Tokens.Add(new Token(TokenType.Or, value, startIndex + 1, i + 1));
                        break;
                    default:
                        Tokens.Add(new Token(TokenType.Var, value, startIndex + 1, i + 1));
                        break;
                }
            }
            else
            {
                if (char.IsDigit(input[i]))
                {
                    int startIndex = i;

                    while ((i + 1) < input.Length && char.IsDigit(input[i + 1]))
                    {
                        i++;
                        value += input[i];
                    }

                    Tokens.Add(new Token(TokenType.Const, value, startIndex + 1, i + 1));
                }
                else
                {
                    switch (input[i])
                    {
                        case '\t':
                        case ' ':
                            break;
                        case (char)13:
                            if ((i + 1) < input.Length && input[i + 1] == (char)10)
                            {
                                i++;
                                value = "\\n";
                            }
                            break;
                        case '>':
                        case '<':
                        case '!':
                            if ((i + 1) < input.Length && input[i + 1] == '=')
                            {
                                i++;
                                value += input[i];
                                Tokens.Add(new Token(TokenType.Rel, value, i, i + 1));
                            }
                            else
                            {
                                if (input[i] == '!')
                                {
                                    Tokens.Add(new Token(TokenType.Error, value, i + 1, i + 1));
                                }
                                else
                                {
                                    Tokens.Add(new Token(TokenType.Rel, value, i + 1, i + 1));
                                }
                            }
                            break;
                        case '=':
                            if ((i + 1) < input.Length && input[i + 1] == '=')
                            {
                                i++;
                                value += input[i];
                                Tokens.Add(new Token(TokenType.Rel, value, i, i + 1));
                            }
                            else
                            {
                                Tokens.Add(new Token(TokenType.Assignment, value, i, i + 1));
                            }
                            break;
                        case '+':
                        case '-':
                            Tokens.Add(new Token(TokenType.ArithOp, value, i + 1, i + 1));
                            break;
                        case ';':
                            Tokens.Add(new Token(TokenType.Semicolon, value, i + 1, i + 1));
                            break;
                        default:
                            Tokens.Add(new Token(TokenType.Error, value, i + 1, i + 1));
                            break;
                    }
                }
            }
        }

        return Tokens;
    }

    public WhileLexer()
    {
        Tokens = new List<Token>();
    }
}
