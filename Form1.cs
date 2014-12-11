using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphic
{
    public partial class GraficForm : Form
    {
        public Thread rainbowThread;
        public Rainbow rainbow;

        public bool initialized = false;

        public GraficForm()
        {
            InitializeComponent();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {

            if (!initialized)
            {
                rainbow = new Rainbow(canvas);

                rainbowThread = new Thread(new ThreadStart(rainbow.start));
                rainbowThread.Start();

                initialized = true;
            }
        }

        private void GraficForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            rainbowThread.Abort();
        }
    }

    public class Rainbow
    {
        public int frame;
        public Graphics ctx;
        public Panel canvas;
        public Pen pen = new Pen(Color.Yellow);
        public SolidBrush brush = new SolidBrush(Color.Yellow);
        public Color[] rainbows = {
                                         //         hex   a r g b
                                         Color.FromArgb(0x78FF0000),
                                         Color.FromArgb(0x78FF7F00),
                                         Color.FromArgb(0x78FFFF00),
                                         Color.FromArgb(0x787FFF00),
                                         Color.FromArgb(0x7800FF00),
                                         Color.FromArgb(0x7800FF7F),
                                         Color.FromArgb(0x7800FFFF),
                                         Color.FromArgb(0x78007FFF),
                                         Color.FromArgb(0x780000FF),
                                         Color.FromArgb(0x787F00FF),
                                         Color.FromArgb(0x78FF00FF),
                                         Color.FromArgb(0x78FF007F)
                                     };

        public int width;
        public int height;

        public int centerX;
        public int centerY;

        public DateTime lastTime;
        public TimeSpan elapsedTime = new TimeSpan(0, 0, 0, 0, 0);

        public TimeSpan updateTime = new TimeSpan(0, 0, 0, 0, 6);

        public Rainbow(Panel canvasP)
        {
            canvas = canvasP;
            ctx = canvas.CreateGraphics();

            width = canvas.Width;
            height = canvas.Height;

            centerX = width / 2;
            centerY = height / 2;
        }

        public void start()
        {
            lastTime = DateTime.Now;

            while (true)
            {
                DateTime now = DateTime.Now;
                elapsedTime += now - lastTime;

                while (elapsedTime >= updateTime)
                {
                    elapsedTime -= updateTime;
                    update();
                }

                render();

                lastTime = now;
            }
        }

        public void update()
        {
            frame += 1;
        }

        public void render()
        {
            brush.Color = Color.FromArgb(40, Color.Black);
            ctx.FillRectangle(brush, 0, 0, width, height);

            int y = 0;
            int upY = width / (rainbows.Length + 1);

            foreach (Color color in rainbows)
            {
                y += upY;

                brush.Color = color;
                ctx.FillRectangle(brush, 0, (y + frame) % height, width, 5);
            }


        }
    }
}
