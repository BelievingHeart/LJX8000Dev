using System;
using System.Windows;
using LJX8000.Core.IoC.Interface;
using LJX8000.Core.ViewModels.Application;
using LJX8000.Core.ViewModels.SideBarMessageItemViewModel;

namespace UI.DataAccess
{
    public class SidebarLogger : IUILogger
    {
        public void LogThreadSafe(string message)
        {
            Application.Current.Dispatcher?.Invoke(() =>
            {
                ApplicationViewModel.Instance.Enqueue(new SideBarMessageItemViewModel()
                {
                    Message = message,
                    Time = DateTime.Now.ToString("h:mm:ss tt zz")
                });
            });
        }
    }
}