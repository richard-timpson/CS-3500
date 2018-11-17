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
        ScoreBoard scoreBoard;
        GameController Controller;
        private StringBuilder message;

        private bool keyRight = false;
        private bool keyLeft = false;
        private bool keyThrust = false;
        private bool keyFire = false;


        /// <summary>
        /// Constructor for the form.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            message = new StringBuilder();
            this.Size = new Size(500, 500);
            KeyPreview = true;
            this.FormClosed += Form1_FormClosed;
        }

        /// <summary>
        /// EventHandler for a clean exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        /// <summary>
        /// Event handler for connect button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            Controller = new GameController();
            try
            {
                Controller.ConnectInitial(nameInput.Text, serverInput.Text);
            }
            catch (Exception exc)
            {
                DisplayError(exc.Message);
            }
            Controller.WorldInitialized += IntilializeGame;
            Controller.SendMessage += SendKeyPress;
            Networking.NetworkController.Error += DisplayError;
            nameInput.Enabled = false;
            serverInput.Enabled = false;
            connectButton.Enabled = false;
        }

        /// <summary>
        /// Initializes the Game after connection to server
        /// </summary>
        /// <param name="WorldSize"></param>
        private void IntilializeGame(int WorldSize)
        {
            MethodInvoker m = new MethodInvoker(() =>
            {
                //create the drawingPanel
                drawingPanel = new DrawingPanel(WorldSize, Controller.theWorld);
                drawingPanel.Location = new Point(0, 30);
                drawingPanel.Size = new Size(WorldSize, WorldSize);
                this.Size = new Size(WorldSize + 200, WorldSize + 70);
                drawingPanel.BackColor = Color.Black;
                this.Controls.Add(drawingPanel);
                drawingPanel.Focus();

                //create the scoreboard
                scoreBoard = new ScoreBoard(Controller.theWorld);
                scoreBoard.Location = new Point(WorldSize, 30);
                scoreBoard.Size = new Size(200, WorldSize);
                scoreBoard.BackColor = Color.White;

                this.Controls.Add(scoreBoard);

                Controller.WorldUpdated += UpdateWorld;
                
                this.Invalidate(true);
            });
            this.Invoke(m);
        }

        /// <summary>
        /// Refreshes the world every frame.
        /// </summary>
        private void UpdateWorld()
        {
            MethodInvoker me = new MethodInvoker(() =>
            {
                this.KeyDown += Form1_KeyDown;
                this.KeyUp += Form1_KeyUp;
                
                drawingPanel.Refresh();
                scoreBoard.Refresh();
                this.Invalidate(true);
            });
            this.Invoke(me);
            
        }

        /// <summary>
        /// Appends command to string builder for flags set to true.
        /// </summary>
        /// <param name="ss"></param>
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
            try
            {
            Controller.SendControls(message, ss);
            }
            catch(Exception e)
            {
                DisplayError(e.Message);
            }
            this.message.Clear();
        }

        /// <summary>
        /// Sets flags for keyinputs to true for key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
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

        /// <summary>
        /// Sets flag for key inputs to false for key up
        /// </summary>
        /// <param name="sneder"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sneder, KeyEventArgs e)
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

        private void DisplayError(string message)
        {
            MessageBox.Show(message);
            MethodInvoker me = new MethodInvoker(() =>
            {
                nameInput.Enabled = true;
                serverInput.Enabled = true;
                connectButton.Enabled = true;
            });
            this.Invoke(me);
        }

    }
}
