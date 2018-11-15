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
using NetworkController;

namespace View
{
    public partial class Form1 : Form
    {
        DrawingPanel drawingPanel;
        GameController Controller;
        private StringBuilder message;
        private bool keyRight = false;
        private bool keyLeft = false;
        private bool keyThrust = false;
        private bool keyFire = false;
        public Form1()
        {
            InitializeComponent();
            message = new StringBuilder();
            this.Size = new Size(900, 800);
            KeyPreview = true;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Controller = new GameController();
            Controller.ConnectInitial(nameInput.Text, serverInput.Text);
            Controller.WorldInitialized += IntilializeGame;
            Controller.SendMessage += SendKeyPress;
            nameInput.Enabled = false;
            serverInput.Enabled = false;
            connectButton.Enabled = false;
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
                drawingPanel.Focus();
                Controller.WorldUpdated += UpdateWorld;
                this.Invalidate(true);
            });
            this.Invoke(m);
        }
        private void UpdateWorld()
        {
            MethodInvoker me = new MethodInvoker(() =>
            {
                this.KeyDown += Form1_KeyDown;
                this.KeyUp += Form1_KeyUp;
                drawingPanel.Refresh();
                this.Invalidate(true);
            });
            this.Invoke(me);
            
        }
        private void SendKeyPress(Networking.SocketState ss)
        {
            this.message.Append("(");
            if (keyRight == true)
            {
                this.message.Append("R");
            }
            if (keyLeft == true)
            {
                this.message.Append("L");
            }
            if (keyThrust == true)
            {
                this.message.Append("T");
            }
            if (keyFire == true)
            {
                this.message.Append("F");
            }
            this.message.Append(")");
            string message = this.message.ToString();
            Console.WriteLine(message);
            Controller.SendControls(message, ss);
            this.message.Clear();
        }


        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    keyLeft = true;
                    break;
                case Keys.Right:
                    keyRight = true;
                    break;
                case Keys.Up:
                    keyThrust = true;
                    break;
                case Keys.Space:
                    keyFire = true;
                    break;
            }
        }

        void Form1_KeyUp(object sneder, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    keyLeft = false;
                    break;
                case Keys.Right:
                    keyRight = false;
                    break;
                case Keys.Up:
                    keyThrust = false;
                    break;
                case Keys.Space:
                    keyFire = false;
                    break;
            }
        }



    }
}
