using LJX8000.Core.IoC.Interface;
using LJX8000.Core.ViewModels.ApplicationViewModel;

namespace UI.DataAccess
{
    public class SidebarLogger : IUILogger
    {
        public void Log(string message)
        {
            ApplicationViewModel.Instance.Enqueue(message);
        }
    }
}