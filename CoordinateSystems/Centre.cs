using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSystems
{
    class Centre
    {
        Point centre;
        public Point CentrePoint
        {
            get => centre;
            private set => centre = value;
        }

        public Centre(Rectangle clientSize)
        {
            CentrePoint = new Point(clientSize.Width / (int)Degree.Half, clientSize.Height / (int)Degree.Half);
        }
    }
}
