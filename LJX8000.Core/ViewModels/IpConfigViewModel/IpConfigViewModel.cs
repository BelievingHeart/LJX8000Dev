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

        public static implicit operator IpConfigViewModel(string s)
        {
            var firstByte = byte.Parse(s.Substring(0, 1));
            var secondByte = byte.Parse(s.Substring(2, 1));
            var thirdByte = byte.Parse(s.Substring(4, 1));
            var forthByte = byte.Parse(s.Substring(6, 1));
            var portNum = ushort.Parse(s.Substring(8, 1));
            return new IpConfigViewModel()
            {
                FirstByte = firstByte,
                SecondByte = secondByte,
                ThirdByte = thirdByte,
                ForthByte = forthByte,
                Port = portNum
            };
        }

    }
}