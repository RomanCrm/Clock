using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSystems
{
    class Middle
    {
        Image imgCentre = Image.FromFile("img/point.png");
        public Image ImageCentre { get => imgCentre; }

        public int WidthPict { get => ImageCentre.Width; }
        public int HeightPict { get => ImageCentre.Height; }

        public void DrawMidPict(Graphics g, Middle midPict, Centre cntr)
        {
            g.DrawImage(midPict.ImageCentre, cntr.CentrePoint.X - midPict.ImageCentre.Width / (int)Degree.Half, cntr.CentrePoint.Y - midPict.ImageCentre.Height / (int)Degree.Half);
        }
    }
}
