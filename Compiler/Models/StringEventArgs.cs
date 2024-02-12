using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler;

public class StringEventArgs : EventArgs
{
    public string Message { get; private set; }

    public StringEventArgs(string message)
    {
        Message = message;
    }
}
