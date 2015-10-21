using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Emulator.Emulator
{
    class Util
    {
        public static void DumpMemoryToConsole(byte[] input)
        {
            int counter = 1;
            foreach (byte b in input)
            {
                Console.Write(b.ToString().PadRight(5, ' '));
                if ((counter % 8) == 0)
                    Console.WriteLine();
                counter++;
            }
        }

        public static string GetBinaryStringValue(byte input)
        {
            return Convert.ToString(input, 2).PadLeft(8, '0');
        }
        public static string GetBinaryStringValue(int input)
        {
            return Convert.ToString(input, 2).PadLeft(16, '0');
        }
    }
}
