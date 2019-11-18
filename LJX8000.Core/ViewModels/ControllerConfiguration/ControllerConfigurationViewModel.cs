using System.Collections.ObjectModel;
using System.Windows.Input;
using LJX8000.Core.Commands;
using LJX8000.Core.ViewModels.Base;
using MaterialDesignThemes.Wpf;

namespace LJX8000.Core.ViewModels.ControllerConfiguration
{
    public class ControllerConfigurationViewModel : ViewModelBase
    {
        public ControllerConfigurationViewModel()
        {
            AddIpConfigCommand = new RelayCommand(AddIpConfig);
            DeleteIpConfigCommand = new ParameterizedCommand(DeleteIpConfig);
        }

        private void DeleteIpConfig(object sender)
        {
            var chip = (Chip) sender;
            ExistingIpConfigs.Remove(chip.DataContext as IpConfigViewModel.IpConfigViewModel);
        }

        private void AddIpConfig()
        {
            ExistingIpConfigs.Add(InputIpConfig);
        }

    #region Properties
    public string InputIpConfig { get; set; }

    public ICommand DeleteIpConfigCommand { get; }

    public ICommand AddIpConfigCommand { get; }

    public ObservableCollection<IpConfigViewModel.IpConfigViewModel> ExistingIpConfigs { get; } = new ObservableCollection<IpConfigViewModel.IpConfigViewModel>();

    #endregion
    }
}