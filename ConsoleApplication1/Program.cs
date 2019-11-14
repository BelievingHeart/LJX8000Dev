using System;
using System.Runtime.InteropServices;
using HalconDotNet;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ushort[] array = {1, 2, 3, 4};
            GCHandle pinnedArray = GCHandle.Alloc(array, GCHandleType.Pinned);
//            GCHandle pinnedArray = GCHandle.Alloc(SimpleArrayDataHighSpeed.profileData, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            HImage image = new HImage("uint2", 2, 2, pointer);
//            HImage image = new HImage("uint2", SimpleArrayDataHighSpeed.DataWidth, RowsPerImage, pointer);
            pinnedArray.Free();
            var value = image.GetGrayval(1, 1);

        }
    }
}