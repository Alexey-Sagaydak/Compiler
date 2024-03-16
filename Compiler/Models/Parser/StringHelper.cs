using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Compiler;

public class StringHelper
{
    private string str;
    private int index;

    public string Source
    {
        get => str;
        set
        {
            str = value;
            index = 0;
        }
    }

    public int Index
    {
        get => index;
        set
        {
            if (value > str.Length - 1 ||  value < 0)
            {
                throw new IndexOutOfRangeException();
            }
            index = value;
        }
    }

    public char Current
    {
        get => str[Index];
    }

    public bool CanGetNext
    {
        get => str.Length - 1 > Index;
    }

    public bool CanGetCurrent
    {
        get => str.Length - 1 >= Index;
    }

    public bool CanGetPrevious
    {
        get => Index - 1 >= 0;
    }

    public char Next
    {
        get => str[++Index];
    }

    public char Previous
    {
        get => str[--Index];
    }

    public void SkipSpaces()
    {
        while (index < str.Length && isSpace(str[Index]))
        {
            index++;
        }
    }

    public StringHelper(string str)
    {
        this.str = str;
        index = 0;
    }
    
    public bool isSpace(char c)
    {
        return (c == ' ' || c == '\t' || c == '\r' || c == '\n');
    }
}
