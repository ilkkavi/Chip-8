using Chip8Emulator.GraphicsControls;
namespace Chip8Emulator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.unload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.showMem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spinningTriangleControl = new Chip8Emulator.GraphicsControls.SpinningTriangleControl();
            this.scale8x = new ToolStripRadioButtonMenuItem();
            this.scale12x = new ToolStripRadioButtonMenuItem();
            this.scale16x = new ToolStripRadioButtonMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(636, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open,
            this.toolStripSeparator1,
            this.unload,
            this.toolStripMenuItem4});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // Open
            // 
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(112, 22);
            this.Open.Text = "Open...";
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // unload
            // 
            this.unload.Name = "unload";
            this.unload.Size = new System.Drawing.Size(112, 22);
            this.unload.Text = "Unload";
            this.unload.Click += new System.EventHandler(this.Unload_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItem4.Text = "Exit";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuExit_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.showMem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(39, 20);
            this.toolStripMenuItem2.Text = "Edit";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.CheckOnClick = true;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem5.Text = "Togglethingie";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripToggleItem_Click);
            // 
            // showMem
            // 
            this.showMem.Name = "showMem";
            this.showMem.Size = new System.Drawing.Size(160, 22);
            this.showMem.Text = "Show Memory...";
            this.showMem.Click += new System.EventHandler(this.toolStripShowMem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scale8x,
            this.scale12x,
            this.scale16x});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // spinningTriangleControl
            // 
            this.spinningTriangleControl.Location = new System.Drawing.Point(0, 24);
            this.spinningTriangleControl.Margin = new System.Windows.Forms.Padding(0);
            this.spinningTriangleControl.Name = "spinningTriangleControl";
            this.spinningTriangleControl.Size = new System.Drawing.Size(644, 497);
            this.spinningTriangleControl.TabIndex = 1;
            this.spinningTriangleControl.Text = "spinningTriangleControl1";
            // 
            // scale8x
            // 
            this.scale8x.Checked = true;
            this.scale8x.CheckOnClick = true;
            this.scale8x.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scale8x.Name = "scale8x";
            this.scale8x.Size = new System.Drawing.Size(124, 22);
            this.scale8x.Text = "8 x Scale";
            this.scale8x.CheckStateChanged += new System.EventHandler(this.checkScale_StateChange);
            // 
            // scale12x
            // 
            this.scale12x.CheckOnClick = true;
            this.scale12x.Name = "scale12x";
            this.scale12x.Size = new System.Drawing.Size(124, 22);
            this.scale12x.Text = "12 x Scale";
            this.scale12x.CheckStateChanged += new System.EventHandler(this.checkScale_StateChange);
            // 
            // scale16x
            // 
            this.scale16x.CheckOnClick = true;
            this.scale16x.Name = "scale16x";
            this.scale16x.Size = new System.Drawing.Size(124, 22);
            this.scale16x.Text = "16 x Scale";
            this.scale16x.CheckStateChanged += new System.EventHandler(this.checkScale_StateChange);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(636, 514);
            this.Controls.Add(this.spinningTriangleControl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "CHIP-8 Emulator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem Open;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private SpinningTriangleControl spinningTriangleControl;
        private EmulatorRendererControl emulatorRendererControl;
        private System.Windows.Forms.ToolStripMenuItem showMem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripRadioButtonMenuItem scale12x;
        private ToolStripRadioButtonMenuItem scale16x;
        protected ToolStripRadioButtonMenuItem scale8x;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem unload;
    }
}

