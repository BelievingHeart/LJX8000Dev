using LJX8000.Core.IoC.Interface;
using LJX8000.Core.ViewModels.Application;

namespace UI.DataAccess
{
    public class SnackbarLogger : IUILogger
    {
        public void LogThreadSafe(string message)
        {
            ApplicationViewModel.Instance.MessageQueue?.Enqueue(message);
        }
        
        
    }
}