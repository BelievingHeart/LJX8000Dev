using System;
using System.Linq;
using LJX8000.Core.ViewModels.Base;

namespace LJX8000.Core.ViewModels.IpConfig
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
            if(s==null) throw new FormatException("Input string can not be null");
            var numDots = s.Count(c => c == '.');
            if(numDots!=3) throw new FormatException("Exactly 3 dots are required");
            var textSegments = s.Split('.');
            var forthByteAndPort = textSegments[3].Split('@');
            var firstByte = byte.Parse(textSegments[0]);
            var secondByte = byte.Parse(textSegments[1]);
            var thirdByte = byte.Parse(textSegments[2]);
            var forthByte = byte.Parse(forthByteAndPort[0]);
            var portNum = ushort.Parse(forthByteAndPort[1]);
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