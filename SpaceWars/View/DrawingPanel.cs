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
        public DrawingPanel()
        {
            DoubleBuffered = true;
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
        private void DrawObjectWithTransform(PaintEventArgs e, int worldSize,  ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, 0);
            int y = WorldSpaceToImageSpace(worldSize, 0);
            e.Graphics.TranslateTransform(x, y);
            object o = new Ship();
            //e.Graphics.RotateTransform((float)angle);
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
            Ship p = o as Ship;
            p.color = "red";
            p.loc = new Vector2D((double)10, (double)10);
            
            e.Graphics.Clear(Color.Black);
            Image ship1 = Resource1.ship_coast_blue;
            Rectangle r = new Rectangle((int)p.loc.GetX(), (int)p.loc.GetY(), 20, 20);
            e.Graphics.DrawImage(ship1, r);

            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //using (System.Drawing.SolidBrush blueBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue))
            //using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
            //{
            //    // Rectangles are drawn starting from the top-left corner.
            //    // So if we want the rectangle centered on the player's location, we have to offset it
            //    // by half its size to the left (-width/2) and up (-height/2)
            //    Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

            //    if (p.GetTeam() == 1) // team 1 is blue
            //        e.Graphics.FillRectangle(blueBrush, r);
            //    else                  // team 2 is green
            //        e.Graphics.FillRectangle(greenBrush, r);
            //}
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawObjectWithTransform(e, this.Size.Width, ShipDrawer);
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
