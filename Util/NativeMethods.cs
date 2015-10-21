using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Emulator.Util
{
    /// <summary>
    ///  A class to offer native methods for use, namely PeekMessage to allow a fluid
    ///  graphics refresh by hooking the winforms idle event while serving the win32 queue.
    /// </summary>
    class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public uint Msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint Time;
            public System.Drawing.Point Point;
        }

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out Message message, IntPtr hWnd, uint filterMin, uint filterMax, uint flags);
    }
}
