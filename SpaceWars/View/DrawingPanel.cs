using GameModel;
using Resources;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace View
{
    public class DrawingPanel : Panel
    {
        public World theWorld;
        private int WorldSize;

        /// <summary>
        /// DrawingPanel constructor that takes in a worldsize value and the world.
        /// </summary>
        /// <param name="WorldSize"></param>
        /// <param name="_theWorld"></param>
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

        //Delegate for Drawing an object
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
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double locX, double locY, float angle,  ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, locX);
            int y = WorldSpaceToImageSpace(worldSize, locY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        private void DrawScoreOnShip(PaintEventArgs e, Ship s, int worldSize, double locX, double locY)
        {
            int x = WorldSpaceToImageSpace(worldSize, locX);
            int y = WorldSpaceToImageSpace(worldSize, locY);
            
            scoreDrawer(s, e, x, y);
            e.Graphics.ResetTransform();
        }

        private void scoreDrawer(Ship s, PaintEventArgs e, int locX, int locY)
        {
            int rectWidth = 0;

            switch(s.hp)
            {
                case 1:
                    rectWidth = 7;
                    break;
                case 2:
                    rectWidth = 14;
                    break;
                case 3:
                    rectWidth = 21;
                    break;
                case 4:
                    rectWidth = 28;
                    break;
                case 5:
                    rectWidth = 35;
                    break;
                default:
                    rectWidth = 0;
                    break;
            }

            Rectangle scoreRect = new Rectangle(new Point(locX - 17, locY - 28), new Size(rectWidth, 5));
            e.Graphics.FillRectangle(new SolidBrush(Color.Green), scoreRect);
        }
        

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

            //if thrust is not active, set image for ship to draw based on player ID
            if (s.thrust == false)
            {
                switch(s.ID)
                {
                    case 0:
                    case 8:
                        ship1 = Resource1.ship_coast_red;
                        break;
                    case 1:
                    case 9:
                        ship1 = Resource1.ship_coast_blue;
                        break;
                    case 2:
                    case 10:
                        ship1 = Resource1.ship_coast_yellow;
                        break;
                    case 3:
                    case 11:
                        ship1 = Resource1.ship_coast_violet;
                        break;
                    case 4:
                    case 12:
                        ship1 = Resource1.ship_coast_green;
                        break;
                    case 5:
                    case 13:
                        ship1 = Resource1.ship_coast_grey;
                        break;
                    case 6:
                    case 14:
                        ship1 = Resource1.ship_coast_brown;
                        break;
                    case 7:
                    case 15:
                        ship1 = Resource1.ship_coast_white;
                        break;
                    default:
                        ship1 = Resource1.ship_coast_white;
                        break;
                }
            }
            //if thrust is active, set image for ship to draw based on player ID
            else
            {
                switch (s.ID)
                {
                    case 0:
                    case 8:
                        ship1 = Resource1.ship_thrust_red;
                        break;
                    case 1:
                    case 9:
                        ship1 = Resource1.ship_thrust_blue;
                        break;
                    case 2:
                    case 10:
                        ship1 = Resource1.ship_thrust_yellow;
                        break;
                    case 3:
                    case 11:
                        ship1 = Resource1.ship_thrust_violet;
                        break;
                    case 4:
                    case 12:
                        ship1 = Resource1.ship_thrust_green;
                        break;
                    case 5:
                    case 13:
                        ship1 = Resource1.ship_thrust_grey;
                        break;
                    case 6:
                    case 14:
                        ship1 = Resource1.ship_thrust_brown;
                        break;
                    case 7:
                    case 15:
                        ship1 = Resource1.ship_thrust_white;
                        break;
                    default:
                        ship1 = Resource1.ship_thrust_white;
                        break;
                }

            }

            Rectangle r = new Rectangle(-(shipWidth/2), -(shipWidth/2), shipWidth, shipWidth);
            e.Graphics.DrawImage(ship1, r);
        }
        
        /// <summary>
        /// Delegate used to draw star.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            int starWidth = 35;
            Star s = o as Star;
            Image star1 = Resource1.star;

            Rectangle r = new Rectangle(-(starWidth / 2), -(starWidth / 2), starWidth, starWidth);
            e.Graphics.DrawImage(star1, r);
        }

        private void ExplosionDrawer(object o, PaintEventArgs e)
        {
            int explosionWidth = 40;
            Image explosion;
            Explosion exp = o as Explosion;
            int randNum = 1;
            System.Random rand = new System.Random();
            randNum = rand.Next(1, 4);

            explosionWidth = rand.Next(15, 45);

            switch (exp.GetCount())
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    explosion = Resource1.explosionfire;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    explosion = Resource1.explosionfire2;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    explosion = Resource1.explosionfire3;
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    explosion = Resource1.explosionfire4;
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    explosion = Resource1.ExplosionSmokeForming1;
                    break;
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                    explosion = Resource1.ExplosionSmokeForming2;
                    break;
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                    explosion = Resource1.ExplosionSmokeForming3;
                    break;
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                    explosion = Resource1.ExplosionSmokeForming4;
                    break;
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                    explosion = Resource1.ExplosionSmokeForming5;
                    break;
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                    explosion = Resource1.ExplosionSmoke1;
                    break;
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                    explosion = Resource1.ExplosionSmoke2;
                    break;
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                    explosion = Resource1.ExplosionSmoke3;
                    break;
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                    explosion = Resource1.ExplosionSmoke4;
                    break;
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                    explosion = Resource1.ExplosionSmoke5;
                    break;
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                    explosion = Resource1.ExplosionSmoke6;
                    break;
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                    explosion = Resource1.ExplosionSmoke7;
                    break;
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                    explosion = Resource1.ExplosionSmoke8;
                    break;
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                    explosion = Resource1.ExplosionSmoke9;
                    break;
                case 91:
                case 92:
                case 93:
                    explosion = Resource1.ExplosionSmoke9;
                    break;
                default:
                    explosion = Resource1.explosionfire3;
                    break;
            }


            Rectangle r = new Rectangle(-(explosionWidth / 2), -(explosionWidth / 2), explosionWidth, explosionWidth);
            e.Graphics.DrawImage(explosion, r);
        }

        /// <summary>
        /// Delegate used to draw projectile.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            int projWidth = 24;
            Projectile p = o as Projectile;
            Image proj1;

            //sets the image of the projectile based on owner
            switch (p.owner)
            {
                case 0:
                case 8:
                    proj1 = Resource1.shot_red;
                    break;
                case 1:
                case 9:
                    proj1 = Resource1.shot_blue;
                    break;
                case 2:
                case 10:
                    proj1 = Resource1.shot_yellow;
                    break;
                case 3:
                case 11:
                    proj1 = Resource1.shot_violet;
                    break;
                case 4:
                case 12:
                    proj1 = Resource1.shot_green;
                    break;
                case 5:
                case 13:
                    proj1 = Resource1.shot_grey;
                    break;
                case 6:
                case 14:
                    proj1 = Resource1.shot_brown;
                    break;
                case 7:
                case 15:
                    proj1 = Resource1.shot_white;
                    break;
                default:
                    proj1 = Resource1.shot_white;
                    break;
            }

            Rectangle r = new Rectangle(-(projWidth / 2), -(projWidth / 2), projWidth, projWidth);
            e.Graphics.DrawImage(proj1, r);
        }


        /// <summary>
        /// Paints the images to the drawing board using loops to obtain each object in the world
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(new Point(WorldSize, 30), new Size(200, WorldSize));
            e.Graphics.FillRectangle(new SolidBrush(Color.White), rect);
            lock (this.theWorld)
            {
                foreach(Explosion s in theWorld.GetExplosions())
                {
                    if (s.GetCount() <= 93)
                    {
                        DrawObjectWithTransform(e, s, WorldSize, s.GetLoc().GetX(), s.GetLoc().GetY(), 0, ExplosionDrawer);
                        s.IncrementCount();
                    }
                    //else
                        //theWorld.RemoveExplosion(s.GetID());
                }

                foreach (Ship s in theWorld.GetShipsActive())
                {
                    DrawObjectWithTransform(e, s, WorldSize, s.loc.GetX(), s.loc.GetY(), s.dir.ToAngle(), ShipDrawer);
                    DrawScoreOnShip(e, s, WorldSize, s.loc.GetX(), s.loc.GetY());
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
