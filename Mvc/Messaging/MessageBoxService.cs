using Onbox.Core.V5.Messaging;

namespace Onbox.Mvc.V5.Messaging
{
    public class MessageBoxService : IMessageService
    {
        public static string title = "";

        public void Show(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        public void Warning(string message)
        {
            System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }

        public void Error(string message)
        {
            System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }

        public bool Question(string message)
        {
            return System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes;
        }

        public void SetTitle(string newTitle)
        {
            title = newTitle;
        }

    }
}
