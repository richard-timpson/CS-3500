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
    public class ScoreBoardPanel : Panel
    {
        private World theWorld;
        private Dictionary<string, int[]> playerScore;
        private Label Label1;

        public ScoreBoardPanel(World _theWorld)
        {
            DoubleBuffered = true;
            this.theWorld = _theWorld;
            playerScore = theWorld.PlayerScores;
        }

        private Dictionary<string, int[]> sortScores(Dictionary<string, int[]> keywordCounts)
        {
            Dictionary<string, int[]> tempDict = new Dictionary<string, int[]>();
            foreach (KeyValuePair<string, int[]> item in keywordCounts.OrderBy(key => key.Value)) 
                {
                    tempDict.Add(item.Key, item.Value);
                }
            return tempDict;
        }


        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        protected override void OnPaint(PaintEventArgs e)
        {
            int Count;
            playerScore = sortScores(playerScore);



        }
    }
}
