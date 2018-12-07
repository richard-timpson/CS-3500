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

        private Image explosionFire;
        private Image explosionFire2;
        private Image explosionFire3;
        private Image explosionFire4;
        private Image explosionSmokeForming1;
        private Image explosionSmokeForming2;
        private Image explosionSmokeForming3;
        private Image explosionSmokeForming4;
        private Image explosionSmokeForming5;
        private Image explosionSmoke1;
        private Image explosionSmoke2;
        private Image explosionSmoke3;
        private Image explosionSmoke4;
        private Image explosionSmoke5;
        private Image explosionSmoke6;
        private Image explosionSmoke7;
        private Image explosionSmoke8;
        private Image explosionSmoke9;
        private Image ship_coast_red;
        private Image ship_coast_violet;
        private Image ship_coast_grey;
        private Image ship_coast_brown;
        private Image ship_coast_green;
        private Image ship_coast_blue;
        private Image ship_coast_yellow;
        private Image ship_coast_white;
        private Image ship_thrust_red;
        private Image ship_thrust_violet;
        private Image ship_thrust_grey;
        private Image ship_thrust_brown;
        private Image ship_thrust_green;
        private Image ship_thrust_blue;
        private Image ship_thrust_yellow;
        private Image ship_thrust_white;
        private Image shot_red;
        private Image shot_violet;
        private Image shot_grey;
        private Image shot_brown;
        private Image shot_green;
        private Image shot_blue;
        private Image shot_yellow;
        private Image shot_white;
        private Image star1;

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
            Point p = new Point(40, 40);
            Size size = new Size(p);
            Point p2 = new Point(32, 32);
            Size sizeShip = new Size(p2);
            Point p3 = new Point(20, 20);
            Size sizeProj = new Size(p3);

            explosionFire = Resample(Resource1.explosionfire, size);
            explosionFire2 = Resample(Resource1.explosionfire2, size);
            explosionFire3 = Resample(Resource1.explosionfire3, size);
            explosionFire4 = Resample(Resource1.explosionfire4, size);
            explosionSmokeForming1 = Resample(Resource1.ExplosionSmokeForming1, size);
            explosionSmokeForming2 = Resample(Resource1.ExplosionSmokeForming2, size);
            explosionSmokeForming3 = Resample(Resource1.ExplosionSmokeForming3, size);
            explosionSmokeForming4 = Resample(Resource1.ExplosionSmokeForming4, size);
            explosionSmokeForming5 = Resample(Resource1.ExplosionSmokeForming5, size);
            explosionSmoke1 = Resample(Resource1.ExplosionSmoke1, size);
            explosionSmoke2 = Resample(Resource1.ExplosionSmoke2, size);
            explosionSmoke3 = Resample(Resource1.ExplosionSmoke3, size);
            explosionSmoke4 = Resample(Resource1.ExplosionSmoke4, size);
            explosionSmoke5 = Resample(Resource1.ExplosionSmoke5, size);
            explosionSmoke6 = Resample(Resource1.ExplosionSmoke6, size);
            explosionSmoke7 = Resample(Resource1.ExplosionSmoke7, size);
            explosionSmoke8 = Resample(Resource1.ExplosionSmoke8, size);
            explosionSmoke9 = Resample(Resource1.ExplosionSmoke9, size);
            ship_coast_red = Resample(Resource1.ship_coast_red, sizeShip);
            ship_coast_yellow = Resample(Resource1.ship_coast_yellow, sizeShip);
            ship_coast_blue = Resample(Resource1.ship_coast_blue, sizeShip);
            ship_coast_violet = Resample(Resource1.ship_coast_violet, sizeShip);
            ship_coast_grey = Resample(Resource1.ship_coast_grey, sizeShip);
            ship_coast_green = Resample(Resource1.ship_coast_green, sizeShip);
            ship_coast_white = Resample(Resource1.ship_coast_white, sizeShip);
            ship_coast_brown = Resample(Resource1.ship_coast_brown, sizeShip);
            ship_thrust_red = Resample(Resource1.ship_thrust_red, sizeShip);
            ship_thrust_yellow = Resample(Resource1.ship_thrust_yellow, sizeShip);
            ship_thrust_blue = Resample(Resource1.ship_thrust_blue, sizeShip);
            ship_thrust_violet = Resample(Resource1.ship_thrust_violet, sizeShip);
            ship_thrust_grey = Resample(Resource1.ship_thrust_grey, sizeShip);
            ship_thrust_green = Resample(Resource1.ship_thrust_green, sizeShip);
            ship_thrust_white = Resample(Resource1.ship_thrust_white, sizeShip);
            ship_thrust_brown = Resample(Resource1.ship_thrust_brown, sizeShip);
            shot_red = Resample(Resource1.shot_red, sizeProj);
            shot_yellow = Resample(Resource1.shot_yellow, sizeProj);
            shot_blue = Resample(Resource1.shot_blue, sizeProj);
            shot_violet = Resample(Resource1.shot_violet, sizeProj);
            shot_grey = Resample(Resource1.shot_grey, sizeProj);
            shot_green = Resample(Resource1.shot_green, sizeProj);
            shot_white = Resample(Resource1.shot_white, sizeProj);
            shot_brown = Resample(Resource1.shot_brown, sizeProj);
            star1 = Resample(Resource1.star, size);
        }

        private static Bitmap Resample(Image img, Size size)
        {
            var bmp = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var graphic = Graphics.FromImage(bmp))
            {
                graphic.DrawImage(img, new Rectangle(Point.Empty, size));
            }
            return bmp;
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

            string drawString = s.name + ": " + s.score.ToString();
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            e.Graphics.DrawString(drawString, drawFont, drawBrush, locX - 17, locY-42, drawFormat);
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
                    case 16:
                    case 24:
                        ship1 = ship_coast_red;
                        break;
                    case 1:
                    case 9:
                    case 17:
                    case 25:
                        ship1 = ship_coast_blue;
                        break;
                    case 2:
                    case 10:
                    case 18:
                    case 26:
                        ship1 = ship_coast_yellow;
                        break;
                    case 3:
                    case 11:
                    case 19:
                    case 27:
                        ship1 = ship_coast_violet;
                        break;
                    case 4:
                    case 12:
                    case 20:
                    case 28:
                        ship1 = ship_coast_green;
                        break;
                    case 5:
                    case 13:
                    case 21:
                    case 29:
                        ship1 = ship_coast_grey;
                        break;
                    case 6:
                    case 14:
                    case 22:
                    case 30:
                        ship1 = ship_coast_brown;
                        break;
                    case 7:
                    case 15:
                    case 23:
                    case 31:
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
                    case 16:
                    case 24:
                        ship1 = ship_thrust_red;
                        break;
                    case 1:
                    case 9:
                    case 17:
                    case 25:
                        ship1 = ship_thrust_blue;
                        break;
                    case 2:
                    case 10:
                    case 18:
                    case 26:
                        ship1 = ship_thrust_yellow;
                        break;
                    case 3:
                    case 11:
                    case 19:
                    case 27:
                        ship1 = ship_thrust_violet;
                        break;
                    case 4:
                    case 12:
                    case 20:
                    case 28:
                        ship1 = ship_thrust_green;
                        break;
                    case 5:
                    case 13:
                    case 21:
                    case 29:
                        ship1 = ship_thrust_grey;
                        break;
                    case 6:
                    case 14:
                    case 22:
                    case 30:
                        ship1 = ship_thrust_brown;
                        break;
                    case 7:
                    case 15:
                    case 23:
                    case 31:
                        ship1 = ship_thrust_white;
                        break;
                    default:
                        ship1 = ship_thrust_white;
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
            int starWidth = 50;

            Star s = o as Star;
            

            Rectangle r = new Rectangle(-(starWidth / 2), -(starWidth / 2), starWidth, starWidth);
            e.Graphics.DrawImage(star1, r);
        }

        private void ExplosionDrawer(object o, PaintEventArgs e)
        {
            //size of the explosion
            int explosionWidth = 40;

            Image explosion;
            Explosion exp = o as Explosion;

            //make size of explosion random
            //int randNum = 1;
            System.Random rand = new System.Random();
            //randNum = rand.Next(1, 4);

            explosionWidth = rand.Next(25, 45);

            switch (exp.GetCount())
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    explosion = explosionFire;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    explosion = explosionFire3;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    explosion = explosionFire2;
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    explosion = explosionFire;
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    explosion = explosionFire3;
                    break;
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                    explosion = explosionFire4;
                    break;
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                    explosion = explosionSmokeForming1;
                    break;
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                    explosion = explosionSmokeForming2;
                    break;
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                    explosion = explosionSmokeForming3;
                    break;
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                    explosion = explosionSmokeForming4;
                    break;
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                    explosion = explosionSmokeForming5;
                    break;
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                    explosion = explosionSmoke1;
                    break;
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                    explosion = explosionSmoke2;
                    break;
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                    explosion = explosionSmoke3;
                    break;
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                    explosion = explosionSmoke4;
                    break;
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                    explosion = explosionSmoke5;
                    break;
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                    explosion = explosionSmoke6;
                    break;
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                    explosion = explosionSmoke7;
                    break;
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                    explosion = explosionSmoke8;
                    break;
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                    explosion = explosionSmoke9;
                    break;


                default:
                    explosion = explosionFire3;
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
            //size of the projectile
            int projWidth = 20;

            Projectile p = o as Projectile;
            Image proj1;

            //sets the image of the projectile based on owner
            switch (p.owner)
            {
                case 0:
                case 8:
                case 16:
                case 24:
                    proj1 = Resource1.shot_red;
                    break;
                case 1:
                case 9:
                case 17:
                case 25:
                    proj1 = shot_blue;
                    break;
                case 2:
                case 10:
                case 18:
                case 26:
                    proj1 = shot_yellow;
                    break;
                case 3:
                case 11:
                case 19:
                case 27:
                    proj1 = shot_violet;
                    break;
                case 4:
                case 12:
                case 20:
                case 28:
                    proj1 = shot_green;
                    break;
                case 5:
                case 13:
                case 21:
                case 29:
                    proj1 = shot_grey;
                    break;
                case 6:
                case 14:
                case 22:
                case 30:
                    proj1 = shot_brown;
                    break;
                case 7:
                case 15:
                case 23:
                case 31:
                    proj1 = shot_white;
                    break;
                default:
                    proj1 = shot_white;
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
            lock (this.theWorld)
            {

                //Draws each ship with healthbar, name and score above the ship.
                foreach (Ship s in theWorld.GetShipsActive())
                {
                    DrawObjectWithTransform(e, s, WorldSize, s.loc.GetX(), s.loc.GetY(), s.dir.ToAngle(), ShipDrawer);
                    DrawScoreOnShip(e, s, WorldSize, s.loc.GetX(), s.loc.GetY());
                }

                //Draws each star
                foreach (Star s in theWorld.GetStars())
                {
                    DrawObjectWithTransform(e, s, WorldSize, s.loc.GetX(), s.loc.GetY(), 0, StarDrawer);
                }

                //draws each projectile
                foreach (Projectile p in theWorld.GetProjectiles())
                {
                    DrawObjectWithTransform(e, p, WorldSize, p.loc.GetX(), p.loc.GetY(), p.dir.ToAngle(), ProjectileDrawer);
                }

                //Draws each explosion according to the counter in the instance explosion class, and increments counter
                foreach (Explosion s in theWorld.GetExplosions())
                {
                    if (s.GetCount() <= 108)
                    {
                        DrawObjectWithTransform(e, s, WorldSize, s.GetLoc().GetX(), s.GetLoc().GetY(), 0, ExplosionDrawer);
                        s.IncrementCount();
                    }
                }
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
