using System.Diagnostics;

namespace Compiler;

public class HtmlHelper
{
    public static void OpenInBrowser(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(filePath) { UseShellExecute = true };
            p.Start();
        }
    }
}
