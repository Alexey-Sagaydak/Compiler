
namespace Compiler
{
    public interface IParser
    {
        List<ParserError> Parse(string text = "");
    }
}