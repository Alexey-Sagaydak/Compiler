
namespace Compiler
{
    public interface IParser
    {
        List<Lexeme> IncorrectLexemes { get; set; }
        List<Lexeme> Parse(List<Lexeme> lexemes);
    }
}