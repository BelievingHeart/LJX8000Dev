using LJX8000.Core.ViewModels.Base;

namespace LJX8000.Core.ViewModels.IpConfigViewModel
{
    public class IpConfigViewModel : ViewModelBase
    {
        public byte FirstByte { get; set; } = 192;
        public byte SecondByte { get; set; } = 168;
        public byte ThirdByte { get; set; } = 0;
        public byte ForthByte { get; set; } = 1;
        public ushort Port { get; set; } = 24691;

        public override string ToString()
        {
            return $"{FirstByte}.{SecondByte}.{ThirdByte}.{ForthByte}@{Port}";
        }
    }
}