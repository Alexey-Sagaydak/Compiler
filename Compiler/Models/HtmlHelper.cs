using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Compiler;

public class HtmlHelper
{
    public static void OpenInBrowser(string path)
    {
        if (System.IO.File.Exists(path) || IsUrl(path))
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(path) { UseShellExecute = true };
            p.Start();
        }
    }

    private static bool IsUrl(string input)
    {
        string pattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";
        return Regex.IsMatch(input, pattern);
    }
}
