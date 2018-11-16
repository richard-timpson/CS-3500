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
    public class DrawingPanel : Panel
    {
        public World theWorld;
        private int WorldSize;
        public DrawingPanel(int WorldSize, World _theWorld)
        {
            DoubleBuffered = true;
            this.theWorld = _theWorld;
            this.WorldSize = WorldSize;
        }


        /// <summary>
        /// helper method for DrawObjectsWithTransform
        /// </summary>
        /// <param name="size"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, Ship s, int worldSize, double locX, double locY, float angle,  ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, locX);
            int y = WorldSpaceToImageSpace(worldSize, locY);
            e.Graphics.TranslateTransform(x, y);
            object o = s;
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        private void DrawObjectWithTransform(PaintEventArgs e, Projectile p, int worldSize, double locX, double locY, float angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, locX);
            int y = WorldSpaceToImageSpace(worldSize, locY);
            e.Graphics.TranslateTransform(x, y);
            object o = p;
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        private void DrawObjectWithTransform(PaintEventArgs e, Star s, int worldSize, double locX, double locY, float angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, locX);
            int y = WorldSpaceToImageSpace(worldSize, locY);
            e.Graphics.TranslateTransform(x, y);
            object o = s;
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }



        //private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)


        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            int shipWidth = 35;
            Ship s = o as Ship;
            Image ship1;
            if (s.thrust == false)
            {
                if (s.ID == 0)
                    ship1 = Resource1.ship_coast_red;
                else if (s.ID == 1)
                    ship1 = Resource1.ship_coast_blue;
                else if (s.ID == 2)
                    ship1 = Resource1.ship_coast_yellow;
                else if (s.ID == 3)
                    ship1 = Resource1.ship_coast_violet;
                else if (s.ID == 4)
                    ship1 = Resource1.ship_coast_green;
                else if (s.ID == 5)
                    ship1 = Resource1.ship_coast_grey;
                else if (s.ID == 6)
                    ship1 = Resource1.ship_coast_brown;
                else if (s.ID == 7)
                    ship1 = Resource1.ship_coast_white;
                else if (s.ID == 8)
                    ship1 = Resource1.ship_coast_red;
                else if (s.ID == 9)
                    ship1 = Resource1.ship_coast_blue;
                else if (s.ID == 10)
                    ship1 = Resource1.ship_coast_yellow;
                else if (s.ID == 11)
                    ship1 = Resource1.ship_coast_violet;
                else if (s.ID == 12)
                    ship1 = Resource1.ship_coast_green;
                else if (s.ID == 13)
                    ship1 = Resource1.ship_coast_grey;
                else if (s.ID == 14)
                    ship1 = Resource1.ship_coast_brown;
                else 
                    ship1 = Resource1.ship_coast_white;
            }
            else
            {
                if (s.ID == 0)
                    ship1 = Resource1.ship_thrust_red;
                else if (s.ID == 1)
                    ship1 = Resource1.ship_thrust_blue;
                else if (s.ID == 2)
                    ship1 = Resource1.ship_thrust_yellow;
                else if (s.ID == 3)
                    ship1 = Resource1.ship_thrust_violet;
                else if (s.ID == 4)
                    ship1 = Resource1.ship_thrust_green;
                else if (s.ID == 5)
                    ship1 = Resource1.ship_thrust_grey;
                else if (s.ID == 6)
                    ship1 = Resource1.ship_thrust_brown;
                else if (s.ID == 7)
                    ship1 = Resource1.ship_thrust_white;
                else if (s.ID == 8)
                    ship1 = Resource1.ship_thrust_red;
                else if (s.ID == 9)
                    ship1 = Resource1.ship_thrust_blue;
                else if (s.ID == 10)
                    ship1 = Resource1.ship_thrust_yellow;
                else if (s.ID == 11)
                    ship1 = Resource1.ship_thrust_violet;
                else if (s.ID == 12)
                    ship1 = Resource1.ship_thrust_green;
                else if (s.ID == 13)
                    ship1 = Resource1.ship_thrust_grey;
                else if (s.ID == 14)
                    ship1 = Resource1.ship_thrust_brown;
                else
                    ship1 = Resource1.ship_thrust_white;
            }

            Rectangle r = new Rectangle(-(shipWidth/2), -(shipWidth/2), shipWidth, shipWidth);
            e.Graphics.DrawImage(ship1, r);
        }

        private void StarDrawer(object o, PaintEventArgs e)
        {
            int starWidth = 35;
            Star s = o as Star;
            Image star1 = Resource1.star;

            Rectangle r = new Rectangle(-(starWidth / 2), -(starWidth / 2), starWidth, starWidth);
            e.Graphics.DrawImage(star1, r);
        }

        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            int projWidth = 24;
            Projectile p = o as Projectile;
            Image proj1;
            if (p.owner == 0)
                proj1 = Resource1.shot_red;
            else if (p.owner == 1)
                proj1 = Resource1.shot_blue;
            else if (p.owner == 2)
                proj1 = Resource1.shot_yellow;
            else if (p.owner == 3)
                proj1 = Resource1.shot_violet;
            else if (p.owner == 4)
                proj1 = Resource1.shot_green;
            else if (p.owner == 5)
                proj1 = Resource1.shot_grey;
            else if (p.owner == 6)
                proj1 = Resource1.shot_brown;
            else if (p.owner == 7)
                proj1 = Resource1.shot_white;
            else if (p.owner == 8)
                proj1 = Resource1.shot_red;
            else if (p.owner == 9)
                proj1 = Resource1.shot_blue;
            else if (p.owner == 10)
                proj1 = Resource1.shot_yellow;
            else if (p.owner == 11)
                proj1 = Resource1.shot_violet;
            else if (p.owner == 12)
                proj1 = Resource1.shot_green;
            else if (p.owner == 13)
                proj1 = Resource1.shot_grey;
            else if (p.owner == 14)
                proj1 = Resource1.shot_brown;
            else 
                proj1 = Resource1.shot_white;


            Rectangle r = new Rectangle(-(projWidth / 2), -(projWidth / 2), projWidth, projWidth);
            e.Graphics.DrawImage(proj1, r);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(new Point(WorldSize, 30), new Size(200, WorldSize));
            e.Graphics.FillRectangle(new SolidBrush(Color.White), rect);
            lock (this.theWorld)
            {
                foreach (Ship s in theWorld.GetShipsActive())
                {
                    DrawObjectWithTransform(e, s, WorldSize, s.loc.GetX(), s.loc.GetY(), s.dir.ToAngle(), ShipDrawer);
                }

                foreach (Star s in theWorld.GetStars())
                {
                    DrawObjectWithTransform(e, s, WorldSize, s.loc.GetX(), s.loc.GetY(), 0, StarDrawer);
                }

                foreach (Projectile p in theWorld.GetProjectiles())
                {
                    DrawObjectWithTransform(e, p, WorldSize, p.loc.GetX(), p.loc.GetY(), p.dir.ToAngle(), ProjectileDrawer);
                }
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
