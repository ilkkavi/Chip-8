#region Using Statements
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Chip8Emulator.Emulator;
#endregion

namespace Chip8Emulator.GraphicsControls
{
    /// <summary>
    /// A graphics device control to render emulator display into the GUI
    /// </summary>
    class EmulatorRendererControl : GraphicsDeviceControl
    {
        SpriteBatch batch;
        ContentManager content;
        SpriteFont font;

        Chip8 chip8;

        Rectangle[,] coordsArray;
        Texture2D blackPixel;
        Texture2D whitePixel;

        int scale;

        
        public EmulatorRendererControl(Chip8 chip8)
        {
            this.chip8 = chip8;
        }

        /// <summary>
        /// Initializes the control, creates a content manager and loads a font.
        /// </summary>
        protected override void Initialize()
        {
            content = new ContentManager(Services, "Content");
            batch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("hudFont");

            // Initial scale, check again in draw method
            scale = this.Width / 64;

            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData (new[] { Color.White });
            blackPixel = new Texture2D(GraphicsDevice, 1, 1);
            blackPixel.SetData(new[] { Color.Black });

            this.coordsArray = new Rectangle[32,64];
            for (int row = 0; row < coordsArray.GetLength(0); row++ )
            {
                for (int col = 0; col < coordsArray.GetLength(1); col++)
                {
                    int x = col * scale;
                    int y = row * scale;
                    coordsArray[row, col] = new Rectangle(x, y, scale, scale);
                }
            }

        }

        private void UpdateCoordRectangles() 
        {
            for (int row = 0; row < coordsArray.GetLength(0); row++)
            {
                for (int col = 0; col < coordsArray.GetLength(1); col++)
                {
                    int x = col * scale;
                    int y = row * scale;
                    coordsArray[row, col] = new Rectangle(x, y, scale, scale);
                }
            }
        }

        /// <summary>
        /// Disposes the control, unloading the ContentManager.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Unload();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            // If control size has changed, update the coordinate rectangles
            if ((this.Width / 64) != this.scale)
            {
                this.scale = this.Width / 64;
                this.UpdateCoordRectangles();
            }

            GraphicsDevice.Clear(Color.Black);

            batch.Begin();

            // Draw the emulator graphics on the screen
            for (int row = 0; row < coordsArray.GetLength(0); row++)
            {
                for (int col = 0; col < coordsArray.GetLength(1); col++)
                {
                    Rectangle coords = coordsArray[row, col];
                    // Check if corresponding pixel is on in the vm graphics
                    bool pixelOn = chip8.gfx[col + (row * 64)] == 1;
                    if (pixelOn)
                        batch.Draw(whitePixel, coords, Color.White);
                    else
                        batch.Draw(blackPixel, coords, Color.Black);

                }
            }

            batch.End();
        }
    }
}
