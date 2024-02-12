namespace Compiler;

public class StringEventArgs : EventArgs
{
    public string Message { get; private set; }

    public StringEventArgs(string message)
    {
        Message = message;
    }
}
