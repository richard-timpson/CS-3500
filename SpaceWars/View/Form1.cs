using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game;

namespace View
{
    public partial class Form1 : Form
    {
        DrawingPanel drawingPanel;
        GameController Controller;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(900, 800);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Controller = new GameController();
            Controller.ConnectInitial(nameInput.Text, serverInput.Text);
            Controller.WorldInitialized += IntilializeGame;
        }

        private void IntilializeGame(int WorldSize)
        {
            MethodInvoker m = new MethodInvoker(() =>
            {
                drawingPanel = new DrawingPanel(WorldSize, Controller.theWorld);
                drawingPanel.Location = new Point(0, 30);
                drawingPanel.Size = new Size(WorldSize, WorldSize);
                drawingPanel.BackColor = Color.Black;
                this.Controls.Add(drawingPanel);
                Controller.WorldUpdated += UpdateWorld;
                this.Invalidate(true);

            });
            this.Invoke(m);
        }
        private void UpdateWorld()
        {
            MethodInvoker me = new MethodInvoker(() =>
            {
                drawingPanel.Refresh();
                this.Invalidate(true);
            });
            this.Invoke(me);
            
        }
    }
}
