using LJX8000.Core.IoC.Interface;
using LJX8000.Core.ViewModels.ApplicationViewModel;
using LJX8000.Core.ViewModels.SideBarMessageItemViewModel;

namespace UI.DataAccess
{
    public class SidebarLogger : IUILogger
    {
        public void Log(string message)
        {
            ApplicationViewModel.Instance.Enqueue(new SideBarMessageItemViewModel()
            {
                Message = message,
                Time = "Time"
            });
        }
    }
}