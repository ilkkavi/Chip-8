using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Emulator.Emulator
{

    /// <summary>
    /// A CHIP-8 virtual machine 
    /// </summary>
    public class Chip8
    {

        // Fonts are constructed from a 4 bit nibble of each byte, five bytes per character
        private readonly byte[] fontset = {0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                                           0x20, 0x60, 0x20, 0x20, 0x70, // 1
                                           0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                                           0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                                           0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                                           0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                                           0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                                           0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                                           0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                                           0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                                           0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                                           0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                                           0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                                           0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                                           0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                                           0xF0, 0x80, 0xF0, 0x80, 0x80  // F
                                          };

        private static readonly int memSize = 1024*4;

        // Index register
        private int I;
        
        // Program counter
        private int pc;
        
        // Stack for storing jump points, stack has 16 levels
        private int[] stack = new int[16];
        private int sp;

        /// <summary>
        /// The current opcode
        /// </summary>
        public int opcode { get; private set; }

        // 4KB of memory
        /// <summary>
        /// A byte array representing the CHIP-8 4KB memory
        /// </summary>
        public byte[] memory { get; private set; }

        /// <summary>
        /// A byte array emulating 15 + 1 8-bit CPU-registers
        /// </summary>
        public byte[] V { get; private set; }

        /// <summary>
        /// Graphics array, 64 * 32 pixels as bytes
        /// </summary>
        public byte[] gfx { get; private set; }

        /// <summary>
        /// CHIP-8 keypad as 16 byte array
        /// </summary>
        public byte[] key { get; set; }
        
        public byte delayTimer { get; private set; }
        public byte soundTimer { get; private set; } 

        /// <summary>
        /// Update screen if flag is set 
        /// </summary>
        public bool drawFlag { get; set; }
        
        /// <summary>
        /// Play a sound if flag is set
        /// </summary>
        public bool soundFlag { get; set; }


        public Chip8()
        {
            this.memory = new byte[memSize];
            V = new byte[16];
            gfx = new byte[64 * 32];
            drawFlag = false;
            soundFlag = false;
            key = new byte[16];
        }

        /// <summary>
        /// Initializes the VM: resets memory, registers, display, timers
        /// </summary>
        public void Initialize()
        {
            this.ClearVM();
            // Load fontset into memory
            for (int i = 0; i < 80; i++)
                memory[i] = fontset[i];
        }

        /// <summary>
        /// Initializes the VM: resets memory, registers, display, timers
        /// Without loading the fontset
        /// </summary>
        public void ClearVM()
        {
            // Program counter starts at 0x200
            pc = 0x200;
            opcode = 0;
            I = 0;
            sp = 0;

            // Clear display
            for (int i = 0; i < gfx.Length; i++)
                gfx[i] = 0;

            // Reset stack
            for (int i = 0; i < stack.Length; i++)
                stack[i] = 0;

            // Clear memory
            for (int i = 0; i < memory.Length; i++)
                memory[i] = 0;

            // Clear registers
            for (int i = 0; i < V.Length; i++)
                V[i] = 0;

            delayTimer = 0;
            soundTimer = 0;

            drawFlag = true;
        }

        /// <summary>
        /// Fetches, decodes and executes the opcode pointed by the program counter. Updates timers.
        /// List of supported instructions: http://en.wikipedia.org/wiki/CHIP-8#Opcode_table
        /// </summary>
        public void EmulateCycle()
        {
            // Fetch opcode - it consists of two bytes merged into a 16 bit binary value
            opcode = (memory[pc] << 8) | memory[pc + 1];
            ExecuteOpcode(opcode);

            // Updating timers
            if (delayTimer > 0)
                delayTimer--;
            if (soundTimer > 0)
            {
                if (soundTimer == 1)
                    soundFlag = true;
                soundTimer--;
            }

        }

        /// <summary>
        ///  A helper method containing the opcode execution switch
        /// </summary>
        /// <param name="opcode">The merged 16-bit opcode</param>
        private void ExecuteOpcode(int opcode)
        {
            // Declaring some helper variables to increase readability later on
            int X, Y, NN, N;
            X = (opcode & 0x0F00) >> 8;
            Y = (opcode & 0x00F0) >> 4;
            NN = (opcode & 0x00FF);
            N = (opcode & 0x000F);

            // The opcode execution statements
            // Decode opcode - get the first four bits of the instruction and if inconclusive check other bits
            switch (opcode & 0xF000)
            {
                case 0x0000: // Two (+1) opcodes start with 0x0 
                    switch (opcode & 0x000F)
                    {
                        case 0x0000: // 0x00E0: Clears the screen
                            for (int i = 0; i < gfx.Length; i++)
                                gfx[i] = 0;
                            pc += 2;
                            drawFlag = true;
                            break;

                        case 0x000E: // 0x00EE: Returns from subroutine
                            sp--;
                            pc = stack[sp];
                            pc += 2;
                            break;

                        default:
                            throw new ApplicationException("Unknown opcode: " + opcode);
                    }
                    break;

                case 0x1000: // 1NNN: Jumps to the address NNN
                    pc = opcode & 0x0FFF;
                    break;

                case 0x2000: // 2NNN: Calls subroutine at NNN
                    stack[sp] = pc;
                    sp++;
                    pc = opcode & 0x0FFF;
                    break;

                case 0x3000: // 3XNN: Skips the next instruction if VX equals NN
                    if (V[X] == NN)
                        pc += 4;
                    else
                        pc += 2;
                    break;

                case 0x4000: // 4XNN: Skips the next instruction if VX doesn't equal NN
                    if (V[X] != NN)
                        pc += 4;
                    else
                        pc += 2;
                    break;

                case 0x5000: // 5XYO: Skips the next instruction if VX equals VY
                    if (V[X] == V[Y])
                        pc += 4;
                    else
                        pc += 2;
                    break;

                case 0x6000: // 6XNN: Sets VX to NN
                    V[X] = (byte)NN;
                    pc += 2;
                    break;

                case 0x7000: // 7XNN: Adds NN to VX
                    V[X] += (byte)NN;
                    pc += 2;
                    break;

                case 0x8000: // 9 opcodes start with 0x8
                    switch (opcode & 0x000F)
                    {
                        case 0x0000: // 8XY0 Sets VX to the value of VY.
                            V[X] = V[Y];
                            pc += 2;
                            break;

                        case 0x0001: // 8XY1 Sets VX to VX or VY.
                            V[X] |= V[Y];
                            pc += 2;
                            break;
                        case 0x0002: // 8XY2 Sets VX to VX and VY.
                            V[X] &= V[Y];
                            pc += 2;
                            break;
                        case 0x0003: // 8XY3 Sets VX to VX xor VY.
                            V[X] ^= V[Y];
                            pc += 2;
                            break;
                        case 0x0004: // 8XY4 Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't.
                            if (V[Y] > (0xFF - V[X])) // There's a carry if VY+VX is larger than a byte
                                V[0xF] = 1;
                            else
                                V[0xF] = 0;
                            V[X] += V[Y];
                            pc += 2;
                            break;
                        case 0x0005: // 8XY5 VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
                            if (V[X] < V[Y])
                                V[0xF] = 0;
                            else
                                V[0xF] = 1;
                            V[X] -= V[Y];
                            pc += 2;
                            break;
                        case 0x0006: // 8XY6 Shifts VX right by one. VF is set to the value of the least significant bit of VX before the shift.
                            V[0xF] = (byte)(V[X] & 0x1); 
                            V[X] >>= 1;
                            pc += 2;
                            break;
                        case 0x0007: // 8XY7 Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't.
                            if (V[X] > V[Y])
                                V[0xF] = 0;
                            else
                                V[0xF] = 1;
                            V[X] = (byte)(V[Y] - V[X]);
                            pc += 2;
                            break;
                        case 0x000E: // 8XYE Shifts VX left by one. VF is set to the value of the most significant bit of VX before the shift.
                            V[0xF] = (byte)(V[X] >> 7);
                            V[X] <<= 1;
                            pc += 2;
                            break;
                        default:
                            throw new ApplicationException("Unknown opcode: " + opcode);
                    }
                    break;

                case 0x9000: // 9XY0: Skips the next instruction if VX doesn't equal VY.
                    if (V[X] != V[Y])
                        pc += 4;
                    else
                        pc += 2;
                    break;

                case 0xA000: // ANNN: Sets I to the address NNN
                    I = opcode & 0x0FFF;
                    pc += 2;
                    break;

                case 0xB000: // BNNN: Jumps to the address NNN plus V0.
                    pc = (opcode & 0x0FFF) + V[0];
                    break;

                case 0xC000: // CXNN: Sets VX to a random number and NN.
                    byte randomByte = (byte)(new Random()).Next(0, 0xFF);
                    V[X] = (byte)(randomByte & NN);
                    pc += 2;
                    break;
                /*
                 * DXYN:
                 * Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels. 
                 * Each row of 8 pixels is read as bit-coded (with the most significant bit of each byte displayed on the left) starting from memory location I;
                 * I value doesn't change after the execution of this instruction. 
                 * As described above, VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn, and to 0 if that doesn't happen.
                 */
                case 0xD000:
                    byte pixel;
                    V[0xF] = 0;
                    for (int yCoord = 0; yCoord < N; yCoord++)
                    {
                        pixel = memory[I + yCoord];
                        for (int xCoord = 0; xCoord < 8; xCoord++)
                        {
                            if ((pixel & (0x80 >> xCoord)) != 0)
                            {
                                int location = (V[X] + xCoord + ((V[Y] + yCoord) * 64) );
                                if (location >= gfx.Length)
                                    continue;

                                if (gfx[location] == 1)
                                    V[0xF] = 1;
                                gfx[location] ^= 1;
                            }
                        }
                    }
                    drawFlag = true;
                    pc += 2;
                    break;

                case 0xE000: // Two opcodes start with 0xE
                    switch (opcode & 0x000F)
                    {
                        case 0x000E: // EX9E: Skips the next instruction if the key stored in VX is pressed
                            if (key[V[X]] != 0)
                                pc += 4;
                            else
                                pc += 2;
                            break;
                        case 0x0001: // EXA1: Skips the next instruction if the key stored in VX isn't pressed
                            if (key[V[X]] == 0)
                                pc += 4;
                            else
                                pc += 2;
                            break;
                        default:
                            throw new ApplicationException("Unknown opcode: " + opcode);
                    }
                    break;
                case 0xF000: // 9 opcodes start with 0xFX
                    switch (opcode & 0x00FF)
                    {
                        case 0x0007: // FX07: Sets VX to the value of the delay timer
                            V[X] = delayTimer;
                            pc += 2;
                            break;
                        case 0x000A: // FX0A: A key press is awaited, and then stored in VX
                            bool pressed = false;

                            // Iterate through the keypad searching for a pressed key
                            for (int i = 0; i < key.Length; i++)
                            {
                                if (key[i] != 0)
                                {
                                    V[X] = (byte)i;
                                    pressed = true;
                                }
                            }

                            // If no key has been pressed, skip cycle and retry
                            if (!pressed)
                                return;

                            pc += 2;
                            break;
                        case 0x0015: // FX15: Sets the delay timer to VX
                            delayTimer = V[X];
                            pc += 2;
                            break;
                        case 0x0018: // FX18: Sets the sound timer to VX
                            soundTimer = V[X];
                            pc += 2;
                            break;
                        case 0x001E: // FX1E: Adds VX to I
                            // Note: VF is set to 1 when range overflow (I+VX>0xFFF), and 0 when there isn't. 
                            // This is undocumented feature of the Chip-8 and used by Spacefight 2019! game.
                            if (I + V[X] > 0xFFF)
                                V[0xF] = 1;
                            else
                                V[0xF] = 0;
                            I += V[X];
                            pc += 2;
                            break;
                        case 0x0029: // FX29: Sets I to the location of the sprite for the character in VX. Characters 0-F (in hexadecimal) are represented by a 4x5 font
                            I = V[X] * 0x5;
                            pc += 2;
                            break;
                        /*
                         * FX33: Stores the Binary-coded decimal representation of VX, with the most significant of three digits at the address in I, 
                         * the middle digit at I plus 1, and the least significant digit at I plus 2. 
                         * (In other words, take the decimal representation of VX, place the hundreds digit in memory at location in I, 
                         * the tens digit at location I+1, and the ones digit at location I+2.)
                         */
                        case 0x0033:
                            memory[I]     = (byte)(V[X] / 100);
                            memory[I + 1] = (byte)((V[X] / 10) % 10);
                            memory[I + 2] = (byte)((V[X] % 100) % 10);
                            pc += 2;
                            break;
                        case 0x0055: // FX55: Stores V0 to VX in memory starting at address I
                            for (int i = 0; i <= X; i++)
                            {
                                memory[I + i] = V[i];
                            }
                            // Note: On the original interpreter, when the operation is done, I=I+X+1.
                            I += X + 1;
                            pc += 2;
                            break;
                        case 0x0065: // FX65: Fills V0 to VX with values from memory starting at address I
                            for (int i = 0; i <= X; i++)
                            {
                                V[i] = memory[I + i];
                            }
                            // Note: On the original interpreter, when the operation is done, I=I+X+1.
                            I += X + 1;
                            pc += 2;
                            break;
                        default:
                            throw new ApplicationException("Unknown opcode: " + opcode);
                    }
                    break;
                default:
                    throw new ApplicationException("Unknown opcode: " + opcode);
            }
        }

        /// <summary>
        /// Initializes the environment and loads a CHIP-8 application rom into the system memory.
        /// </summary>
        /// <param name="fileName">The full path and filename of the ROM-file to be opened.</param>
        /// <returns>A boolean value indicating success of the operation</returns>
        public bool LoadApplication(string fileName)
        {
            bool success = false;
            Initialize();

            //DebugScreenFill();

            Console.WriteLine("Opening file: " + fileName);

            try
            {
                FileInfo fInfo = new FileInfo(fileName);
                if (fInfo.Length > memSize - 512)
                {
                    Console.WriteLine("Error: ROM file size exceeds maximum memory available in the system!");
                }
                byte[] buffer = File.ReadAllBytes(fileName);

                Console.WriteLine("Total number of read bytes: " + buffer.Length);

                // Copying buffer into CHIP-8 memory starting from 0x200
                for (int i = 0; i < buffer.Length; i++)
                {
                    memory[i + 512] = buffer[i];
                }
                success = true;
            }
            catch (IOException e)
            {
                Console.WriteLine("Error opening file: " + e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unspecified error loading application: " + e.ToString());
            }

            return success;
        }

        public void DebugScreenFill()
        {

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    gfx[(y*64) + x] = ((y+x) % 2 == 0) ? (byte)1 : (byte)0;
                } 
            }
        }
    }
}
