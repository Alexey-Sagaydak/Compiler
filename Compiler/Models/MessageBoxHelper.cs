using System.Windows;

namespace Compiler;

public class MessageBoxHelper
{
    public static MessageBoxResult ShowUnsavedChangesMessage()
    {
        MessageBoxResult result = MessageBox.Show("Есть несохраненные изменения. Хотите сохранить их?", "Предупреждение", 
            MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

        return result;
    }

    public static MessageBoxResult ShowWindowClosingMessage()
    {
        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение? Несохраненные изменения будут потеряны!",
            "Подтверждение закрытия", MessageBoxButton.YesNo, MessageBoxImage.Question);

        return result;
    }
}
