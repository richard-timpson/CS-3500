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
        int WorldSize;
        public DrawingPanel(int WorldSize)
        {
            DoubleBuffered = true;
            theWorld = new World();
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
            if (s.GetThrust() == false)
            {
                if (s.ID == 0)
                    ship1 = Resource1.ship_coast_red;
                else if (s.ID == 1)
                    ship1 = Resource1.ship_coast_blue;
                else
                    ship1 = Resource1.ship_coast_yellow;
            }
            else
            {
                if (s.ID == 0)
                    ship1 = Resource1.ship_thrust_red;
                else if (s.ID == 1)
                    ship1 = Resource1.ship_thrust_blue;
                else
                    ship1 = Resource1.ship_thrust_yellow;
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
            int projWidth = 12;
            Projectile p = o as Projectile;
            Image proj1;
            if (p.GetID() == 0)
                proj1 = Resource1.shot_red;
            else if (p.GetID() == 1)
                proj1 = Resource1.shot_blue;
            else
                proj1 = Resource1.shot_yellow;

            Rectangle r = new Rectangle(-(projWidth / 2), -(projWidth / 2), projWidth, projWidth);
            e.Graphics.DrawImage(proj1, r);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (Ship s in theWorld.GetShips())
            {
                DrawObjectWithTransform(e, s, WorldSize, s.GetLocation().GetX(), s.GetLocation().GetY(), s.GetOrientation().ToAngle(), ShipDrawer);
            }

            foreach (Star s in theWorld.GetStars())
            {
                DrawObjectWithTransform(e, s, WorldSize, s.GetLocation().GetX(), s.GetLocation().GetY(), 0, StarDrawer);
            }

            foreach (Projectile p in theWorld.GetProjectiles())
            {
                DrawObjectWithTransform(e, p, WorldSize, p.GetLocation().GetX(), p.GetLocation().GetY(), 0, ProjectileDrawer);
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
