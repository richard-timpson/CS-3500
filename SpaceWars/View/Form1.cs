﻿using System;
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
                this.Invalidate(true);
                drawingPanel = new DrawingPanel();
                drawingPanel.Location = new Point(0, 30);
                drawingPanel.Size = new Size(WorldSize, WorldSize);
                this.Controls.Add(drawingPanel);
            });
            this.Invoke(m);
        }
    }
}
