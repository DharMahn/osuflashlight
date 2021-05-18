using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace osu_flashlight
{
    public partial class Form1 : Form
    {
        int circleRadius = 150;
        Rectangle[] rekts = new Rectangle[2];
        Point oldPos;
        public Form1()
        {
            InitializeComponent();
        }

        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        public enum LWA
        {
            ColorKey = 0x1,
            Alpha = 0x2
        }
        Stopwatch sw = new Stopwatch();
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oldPos = Cursor.Position;
            sw = new Stopwatch();
            int wl = GetWindowLong(Handle, GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            SetWindowLong(Handle, GWL.ExStyle, wl);
            SetLayeredWindowAttributes(Handle, 0, 255, LWA.Alpha);
            Opacity = 0.95;
            TopMost = true;
            BackColor = Color.Black;
            TransparencyKey = Color.Magenta;
            Invalidate();
            Bounds = Screen.AllScreens.Select(screen => screen.Bounds).Aggregate(Rectangle.Union);
            DoubleBuffered = true;
            ShowInTaskbar = false;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.Magenta, new Rectangle(Cursor.Position.X - circleRadius, Cursor.Position.Y - circleRadius, circleRadius * 2, circleRadius * 2));
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            rekts[0] = new Rectangle(oldPos.X - circleRadius, oldPos.Y - circleRadius, circleRadius * 2, circleRadius * 2);
            rekts[1] = new Rectangle(Cursor.Position.X - circleRadius, Cursor.Position.Y - circleRadius, circleRadius * 2, circleRadius * 2);
            Invalidate(rekts.Aggregate(Rectangle.Union));
            oldPos = Cursor.Position;
        }
    }
}
