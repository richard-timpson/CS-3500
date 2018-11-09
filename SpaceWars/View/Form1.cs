using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class Form1 : Form
    {
        DrawingPanel drawingPanel;
        public Form1()
        {
            InitializeComponent();
            drawingPanel = new DrawingPanel();
            drawingPanel.Location = new Point(0, 0);
            drawingPanel.Size = new Size(500, 500);
            this.Controls.Add(drawingPanel);
            MethodInvoker m = new MethodInvoker(() => this.Invalidate(true));
        }
    }
}
