
namespace Compiler
{
    public interface ILexicalAnalyzer
    {
        List<Lexeme> Analyze(string input);
    }
}