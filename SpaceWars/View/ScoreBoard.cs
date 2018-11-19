using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using GameModel;
using Vector;
using Resources;

namespace View
{
    public class ScoreBoard : Panel
    {
        public World theWorld;
        private List<Ship> ships;

        /// <summary>
        /// Constructor for the scoreboard.
        /// </summary>
        /// <param name="_theWorld"></param>
        public ScoreBoard(World _theWorld)
        {
            DoubleBuffered = true;
            this.theWorld = _theWorld;
            
        }

        /// <summary>
        /// Sorts the Ships based on score
        /// </summary>
        /// <param name="ships"></param>
        private void sortShipList(List<Ship> ships)
        {
            ships.Sort(((x, y) => y.score.CompareTo(x.score)));
        }

        /// <summary>
        /// Draws the scoreboard.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld)
            {
                ships = theWorld.GetShipsAll().ToList<Ship>();
                sortShipList(ships);
                float count = 30;
                foreach (Ship s in ships)
                {
                    
                    string drawString = s.name + ": " + s.score.ToString();
                    System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                    e.Graphics.DrawString(drawString, drawFont, drawBrush, 10, count, drawFormat);

                    Rectangle OutlineRect = new Rectangle(new Point(10, (int)count + 30), new Size(150, 20));
                    e.Graphics.DrawRectangle(new Pen(Brushes.Black), OutlineRect);

                    Rectangle ScoreRect;

                    switch (s.hp)
                    {
                        case 1:
                            ScoreRect = new Rectangle(new Point(11, (int)count + 31), new Size(29, 19));
                            e.Graphics.FillRectangle(new SolidBrush(Color.Green), ScoreRect);
                            break;
                        case 2:
                            ScoreRect = new Rectangle(new Point(11, (int)count + 31), new Size(59, 19));
                            e.Graphics.FillRectangle(new SolidBrush(Color.Green), ScoreRect);

                            break;
                        case 3:
                            ScoreRect = new Rectangle(new Point(11, (int)count + 31), new Size(89, 19));
                            e.Graphics.FillRectangle(new SolidBrush(Color.Green), ScoreRect);

                            break;
                        case 4:
                            ScoreRect = new Rectangle(new Point(11, (int)count + 31), new Size(119, 19));
                            e.Graphics.FillRectangle(new SolidBrush(Color.Green), ScoreRect);

                            break;
                        case 5:
                            ScoreRect = new Rectangle(new Point(11, (int)count + 31), new Size(149, 19));
                            e.Graphics.FillRectangle(new SolidBrush(Color.Green), ScoreRect);

                            break;
                    }




                    count += 60;
                }
            }
            
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
