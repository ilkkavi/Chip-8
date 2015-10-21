using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chip8Emulator
{
    public partial class MemoryForm : Form
    {
        public MemoryForm()
        {
            InitializeComponent();
        }

        private void MemoryForm_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = this.GetMemoryString((this.Owner as MainForm).chip8.memory, 4);
            this.memoryTimer.Start();
        }

        private void memoryTimer_Tick(object sender, EventArgs e)
        {
            this.textBox1.Text = this.GetMemoryString((this.Owner as MainForm).chip8.memory, 4);
        }


        private string GetMemoryString(byte[] input, int padding)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i += 2) 
            {
                string b = BitConverter.ToString(input, i, 1).Replace('-', ' ');
                sb.Append(b.PadRight(padding, ' '));
            }
            return sb.ToString();
        }
    }
}
