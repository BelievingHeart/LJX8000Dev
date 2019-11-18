using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using LJX8000.Core.Commands;
using LJX8000.Core.Enums;
using LJX8000.Core.ViewModels.Application;
using LJX8000.Core.ViewModels.Base;
using LJX8000.Core.ViewModels.Controller;
using MaterialDesignThemes.Wpf;

namespace LJX8000.Core.ViewModels.ControllerConfiguration
{
    public class ControllerConfigurationViewModel : ViewModelBase
    {
        public ControllerConfigurationViewModel()
        {
            AddIpConfigCommand = new SimpleCommand(o=>AddController(), o=>!string.IsNullOrEmpty(InputIpConfig));
            DeleteIpConfigCommand = new ParameterizedCommand(DeleteController);
            GoToControllerHostViewCommand = new RelayCommand(GoToControllerHostView);
        }

        private void GoToControllerHostView()

        {
            if (ExistingControllers.Count == 0) return;
            RemoveNotRequiredControllerConfigs();
            ControllerManager.AttachedControllers = ExistingControllers.ToList();
            ControllerManager.Init();
            // Navigate to program page
            ApplicationViewModel.Instance.CurrentAppPage = ApplicationPage.ControllerHostPage;
        }

        /// <summary>
        /// Remove all the controller configs in disk that is not in <see cref="ExistingControllers"/>
        /// </summary>
        private void RemoveNotRequiredControllerConfigs()
        {
            var protectedFileNames = ExistingControllers.Select(controller => controller.Name + ".xml");
            var filesInControllerConfigDir = Directory.GetFiles(ApplicationViewModel.ControllerSerializationBaseDir);
            var pathsToRemove = filesInControllerConfigDir.Where(file => !protectedFileNames.Any(file.EndsWith));
            foreach (var path in pathsToRemove)
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Delete controller from <see cref="ExistingControllers"/>
        /// </summary>
        /// <param name="sender"></param>
        private void DeleteController(object sender)
        {
            var chip = (Chip) sender;
            var type = chip.DataContext.GetType().ToString();
            var controllerToRemove = (ControllerViewModel)chip.DataContext;
            ExistingControllers.Remove(controllerToRemove);
            RemoveControllerConfig(controllerToRemove);
        }

/// <summary>
/// Remove the controller config in disk
/// </summary>
/// <param name="controllerToRemove"></param>
private void RemoveControllerConfig(ControllerViewModel controllerToRemove)
        {
            var pathToRemove = Path.Combine(ApplicationViewModel.ControllerSerializationBaseDir,
                controllerToRemove.Name + ".xml");
            
            File.Delete(pathToRemove);
        }

        private void AddController()
        {
            var controllerToAdd = new ControllerViewModel
            {
                Name = InputIpConfig, 
                SerializationDirectory = ApplicationViewModel.ControllerSerializationBaseDir,
                ShouldAutoSerialize = true
            };
            ExistingControllers.Add(controllerToAdd);
            controllerToAdd.Serialize(null, null);
            
            InputIpConfig = string.Empty;
        }

        #region Properties

        public string InputIpConfig { get; set; }

        public ICommand DeleteIpConfigCommand { get; }

        public ICommand AddIpConfigCommand { get; }

        public ICommand GoToControllerHostViewCommand { get; }


        public ObservableCollection<ControllerViewModel> ExistingControllers { get; } =
            new ObservableCollection<ControllerViewModel>(ControllerManager.AttachedControllers);

        #endregion
    }
}