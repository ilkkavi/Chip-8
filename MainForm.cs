using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chip8Emulator.Emulator;
using System.Media;
using System.Diagnostics;
using Chip8Emulator.Util;

namespace Chip8Emulator
{
    using XnaColor = Microsoft.Xna.Framework.Color;
    

    public partial class MainForm : Form
    {
        public Chip8 chip8;
        Timer refreshTimer;
        SoundPlayer player;

        Stopwatch sw;
        readonly TimeSpan TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
        readonly TimeSpan MaxElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 10);
        TimeSpan accumulatedTime;
        TimeSpan lastTime = TimeSpan.FromTicks(0);

        // Reference of the memform kept so only on is alive at a time
        MemoryForm memForm = null;
        private string fileName = String.Empty;

        public MainForm()
        {
            InitializeComponent();
            this.spinningTriangleControl.Size = GetControlSize();
            chip8 = new Chip8();

            // Create an emulator renderer control that will be displayed when a ROM is loaded
            this.emulatorRendererControl = new Chip8Emulator.GraphicsControls.EmulatorRendererControl(chip8);
            this.emulatorRendererControl.Location = new System.Drawing.Point(0, 24);
            this.emulatorRendererControl.Margin = new Padding(0);
            this.emulatorRendererControl.Name = "emulatorRendererControl";
            this.emulatorRendererControl.TabIndex = 1;
            this.emulatorRendererControl.Text = "Emulator renderer control";

            // Attaching the key listeners to the renderer control
            this.emulatorRendererControl.KeyUp += new KeyEventHandler(KeyUpHandler);
            this.emulatorRendererControl.KeyDown += new KeyEventHandler(KeyDownHandler);
            
            // Loading a soundplayer and a sound file
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            String[] hurr = a.GetManifestResourceNames();
            System.IO.Stream s = a.GetManifestResourceStream("Chip8Emulator.Resources.durr_quiet.wav");
            player = new SoundPlayer(s);

            sw = Stopwatch.StartNew();
            // Setting the timer to tick 60 times per second
            // the timer is started when loading applications and stopped unloading
            //refreshTimer = new Timer();
            //refreshTimer.Interval = 1000 / 60;
            //refreshTimer.Tick += delegate { this.UpdateScreen(); };

        }

        private void toolStripMenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TickWhileIdle(object sender, EventArgs e)
        {
            NativeMethods.Message message;

            while (!NativeMethods.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
            {
                UpdateScreen();
            }
        }

        private void toolStripToggleItem_Click(object sender, EventArgs e)
        {
            bool colors = ((ToolStripMenuItem)sender).Checked;
            Random rand = new Random();
            for (int i = 0; i < spinningTriangleControl.Vertices.Length; i++)
            {
                XnaColor verticeColor;
                if (colors)
                    verticeColor = new XnaColor(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
                else
                    verticeColor = XnaColor.Black;

                spinningTriangleControl.Vertices[i].Color = verticeColor;
            }
        }

        // Opens a file open dialog
        private void Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"D:\dev\csharp\chip8roms";
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
                fileName = ofd.FileName;

            if (!String.IsNullOrEmpty(fileName))
            {
                bool success = chip8.LoadApplication(fileName);
                if (success)
                {
                    // Dispose the placenholder animation control and replace it with the renderer
                    InstantiateRenderer();
                }
            }
        }

        private void InstantiateRenderer() {

            if (!this.spinningTriangleControl.IsDisposed)
            {
                // Temporarily setting form autosizing off the prevent shrinkage
                this.AutoSize = false; 
                // Kill the placeholder animation control
                this.spinningTriangleControl.Dispose();

                // Add the emulator renderer control into the control collection and get the desired size
                this.emulatorRendererControl.Size = GetControlSize();
                this.Controls.Add(this.emulatorRendererControl);
                this.emulatorRendererControl.Focus();
                
                this.AutoSize = true;
            }
            // Setting the timer 
            Application.Idle += TickWhileIdle;
        }

        /// <summary>
        /// A helper method to get the selected scale as an integer
        /// </summary>
        /// <returns></returns>
        private int GetControlScale()
        {
            int scale = 0;
            if (scale8x.Checked)
                scale = 8;
            else if (scale12x.Checked)
                scale = 12;
            else if (scale16x.Checked)
                scale = 16;

            return scale;
        }

        /// <summary>
        /// Gets a Size-object of the control size corresponding to the selected scale
        /// </summary>
        /// <returns></returns>
        private Size GetControlSize()
        {
            int scale = this.GetControlScale();
            return new Size(scale * 64, scale * 32);
        }

        private void toolStripShowMem_Click(object sender, EventArgs e)
        {
            if (memForm == null)
            {
                memForm = new MemoryForm();
                memForm.FormClosed += (s, ev) => memForm = null;
                memForm.Show(this);
            }
        }

        /// <summary>
        /// A method called in the frequency denoted by the mainform time
        /// Calls the emulation cycle and redraws the display if needed 
        /// (by invalidating the emulator renderer control)
        /// </summary>
        private void UpdateScreen()
        {
            if (sw.Elapsed.TotalMilliseconds < (1000 / 180))
                return;

            this.chip8.EmulateCycle();

            if (this.chip8.drawFlag)
            {
                this.emulatorRendererControl.Invalidate();
                this.chip8.drawFlag = false;
            }
            if (this.chip8.soundFlag)
            {
                player.Play();
                this.chip8.soundFlag = false;
            }
            sw.Restart();
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1: chip8.key[1] = 1; break;
                case Keys.D2: chip8.key[2] = 1; break;
                case Keys.D3: chip8.key[3] = 1; break;
                case Keys.D4: chip8.key[12] = 1; break;
                case Keys.Q: chip8.key[4] = 1; break;
                case Keys.W: chip8.key[5] = 1; break;
                case Keys.E: chip8.key[6] = 1; break;
                case Keys.R: chip8.key[13] = 1; break;
                case Keys.A: chip8.key[7] = 1; break;
                case Keys.S: chip8.key[8] = 1; break;
                case Keys.D: chip8.key[9] = 1; break;
                case Keys.F: chip8.key[14] = 1; break;
                case Keys.Z: chip8.key[10] = 1; break;
                case Keys.X: chip8.key[0] = 1; break;
                case Keys.C: chip8.key[11] = 1; break;
                case Keys.V: chip8.key[15] = 1; break;
                default:
                    Console.WriteLine("Keycode not mapped to controls.");
                    break;
            }
        }
        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1: chip8.key[1]  = 0; break;
                case Keys.D2: chip8.key[2]  = 0; break;
                case Keys.D3: chip8.key[3]  = 0; break;
                case Keys.D4: chip8.key[12] = 0; break;
                case Keys.Q: chip8.key[4] = 0; break;
                case Keys.W: chip8.key[5] = 0; break;
                case Keys.E: chip8.key[6] = 0; break;
                case Keys.R: chip8.key[13] = 0; break;
                case Keys.A: chip8.key[7] = 0; break;
                case Keys.S: chip8.key[8] = 0; break;
                case Keys.D: chip8.key[9] = 0; break;
                case Keys.F: chip8.key[14] = 0; break;
                case Keys.Z: chip8.key[10] = 0; break;
                case Keys.X: chip8.key[0] = 0; break;
                case Keys.C: chip8.key[11] = 0; break;
                case Keys.V: chip8.key[15] = 0; break;
                default:
                    Console.WriteLine("Keycode not mapped to controls.");
                    break;
            }
        }

        private void checkScale_StateChange(object sender, EventArgs e)
        {
            bool senderIsChecked = ((ToolStripRadioButtonMenuItem)sender).CheckState == CheckState.Checked;
            if (senderIsChecked) {
                if (!this.spinningTriangleControl.IsDisposed)
                    this.spinningTriangleControl.Size = GetControlSize();
                else
                    this.emulatorRendererControl.Size = GetControlSize();
            }
        }

        private void Unload_Click(object sender, EventArgs e)
        {
            
            Application.Idle -= TickWhileIdle;
            Console.WriteLine("Stopping drawing on the screen...");
            this.chip8.ClearVM();
            this.emulatorRendererControl.Invalidate();
        }
    }
}
