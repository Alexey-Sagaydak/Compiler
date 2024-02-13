using System.IO;

namespace Compiler;

public class FileManager
{
    public bool CreateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            return true;
        }
        else
        {
            return false;
        }
    }

    public string OpenFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    public bool SaveFile(string filePath, string text)
    {
        try
        {
            File.WriteAllText(filePath, text);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
