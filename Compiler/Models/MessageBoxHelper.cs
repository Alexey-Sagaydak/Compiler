using System.Windows;

namespace Compiler;

public class MessageBoxHelper
{
    public static MessageBoxResult ShowUnsavedChangesMessage()
    {
        MessageBoxResult result = MessageBox.Show("Есть несохраненные изменения. Хотите сохранить их?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

        return result;
    }
}
