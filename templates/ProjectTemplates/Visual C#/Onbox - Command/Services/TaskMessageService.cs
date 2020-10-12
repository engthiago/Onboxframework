using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;

namespace $safeprojectname$.Services
{
    public class TaskMessageService : IMessageService
    {
        private string title;

        public void Error(string message)
        {
            string currentTitle = string.IsNullOrWhiteSpace(title) ? "Error" : title;
            var dialog = new TaskDialog(currentTitle);
            dialog.MainContent = message;
            dialog.MainIcon = TaskDialogIcon.TaskDialogIconError;
            dialog.TitleAutoPrefix = false;
            dialog.Show();
        }

        public bool Question(string message)
        {
            string currentTitle = string.IsNullOrWhiteSpace(title) ? "Question" : title;
            var dialog = new TaskDialog(currentTitle);
            dialog.MainContent = message;
            dialog.MainIcon = TaskDialogIcon.TaskDialogIconInformation;
            dialog.TitleAutoPrefix = false;
            dialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

            if (dialog.Show() == TaskDialogResult.Yes)
            {
                return true;
            }

            return false;
        }

        public void SetTitle(string newTitle)
        {
            this.title = newTitle;
        }

        public void Show(string message)
        {
            string currentTitle = string.IsNullOrWhiteSpace(title) ? "Information" : title;
            var dialog = new TaskDialog(currentTitle);
            dialog.MainContent = message;
            dialog.MainIcon = TaskDialogIcon.TaskDialogIconInformation;
            dialog.TitleAutoPrefix = false;
            dialog.Show();
        }

        public void Warning(string message)
        {
            string currentTitle = string.IsNullOrWhiteSpace(title) ? "Warning!" : title;
            var dialog = new TaskDialog(currentTitle);
            dialog.MainContent = message;
            dialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
            dialog.TitleAutoPrefix = false;
            dialog.Show();
        }
    }
}