using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoordinateSystems
{
    enum Time
    {
        Second = 60,
        Minute = 60,
        Hour = 60,
        Millisecond = 1000
    }

    enum Degree
    {
        Circle = 360,
        Quarter = 4,
        Half = 2,
    }

    enum Clock
    {
        Serif = 12
    }

    enum RNumsArr
    {
        SecArr = 30,
        MinArr = 50,
        HourArr = 80
    }

    public partial class Form1 : Form
    {
        Timer timer;

        GraphicsPath circle = new GraphicsPath();

        Matrix serifs = new Matrix();
        float angleBetweenSerifs = (int)Degree.Circle / (int)Clock.Serif; // 30 degrees

        Pen penCircle = new Pen(Color.DarkBlue, 5);
        Pen penSefirs = new Pen(Color.DarkBlue, 3);

        Centre centre;
        Middle middlePict = new Middle();

        Point quart;

        float anglePerSecMinute = (int)Degree.Circle / (int)Time.Minute; // Angle of rotation per second, minute (6 degrees)
        float anglePerHour = (int)Degree.Circle / (int)Clock.Serif; // Angle of rotation per hour ()

        bool isStarted = false; // for start positions of arrows
        bool isNewHour = false; // hour`s arrow 

        // Matrix for second`s arrow
        float angleSecs = 0;
        Matrix sec = new Matrix();

        // Matrix for minute`s arrow
        float angleMins = 0;
        Matrix min = new Matrix();

        // Matrix for hour`s arrow
        float angleHours = 0;
        Matrix hour = new Matrix();

        float angleOneHour;
        float hourPerMinAction = (float)RNumsArr.SecArr / (float)Time.Second;

        public Form1()
        {
            DoubleBuffered = true;
            BackColor = Color.Gray;

            InitializeComponent();
            Paint += Form1_Paint;

            centre = new Centre(ClientRectangle);
            quart = new Point(ClientRectangle.Width / (int)Degree.Quarter, ClientRectangle.Height / (int)Degree.Quarter);

            circle.AddEllipse(quart.X, quart.Y, ClientRectangle.Width / (int)Degree.Half, ClientRectangle.Height / (int)Degree.Half);

            TimerInit();

            AnglesInit();
        }

        private void AnglesInit()
        {
            angleSecs = DateTime.Now.Second * anglePerSecMinute;
            angleMins = DateTime.Now.Minute * anglePerSecMinute;
            angleHours = DateTime.Now.Hour * anglePerHour;

            angleOneHour = (((int)Degree.Circle / (int)Clock.Serif) * DateTime.Now.Minute) / (int)Time.Minute; // degrees per hour
        }

        // Methods for timer
        private void TimerInit()
        {
            timer = new Timer();
            timer.Interval = (int)Time.Millisecond;
            timer.Tick += Time_Tick;
            timer.Start();
        }
        private void Time_Tick(object sender, EventArgs e)
        {
            angleSecs += anglePerSecMinute;

            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (!isStarted)
            {
                sec.RotateAt(angleSecs, centre.CentrePoint);
                g.Transform = sec;
                g.DrawLine(Pens.Bisque, centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.SecArr);

                min.RotateAt(angleMins, centre.CentrePoint);
                g.Transform = min;
                g.DrawLine(new Pen(Color.Crimson, 3), centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.MinArr);

                hour.RotateAt(angleHours + angleOneHour, centre.CentrePoint);
                g.Transform = hour;
                g.DrawLine(new Pen(Color.DarkViolet, 5), centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.HourArr);

                isStarted = true;
            }
            else
            {
                sec.RotateAt(anglePerSecMinute, centre.CentrePoint);
                g.Transform = sec;
                g.DrawLine(Pens.Bisque, centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.SecArr);

                if (angleSecs >= (int)Degree.Circle)
                {
                    min.RotateAt(anglePerSecMinute, centre.CentrePoint);
                    g.Transform = min;
                    g.DrawLine(new Pen(Color.Crimson, 3), centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.MinArr);

                    angleSecs = 0;
                    angleOneHour = (((float)Degree.Circle / (float)Clock.Serif) * DateTime.Now.Minute) / (float)Time.Minute;
                    isNewHour = true;
                }
                else
                {
                    g.Transform = min;
                    g.DrawLine(new Pen(Color.Crimson, 3), centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.MinArr);
                }

                if (isNewHour)
                {
                    hour.RotateAt(hourPerMinAction, centre.CentrePoint);
                    g.Transform = hour;
                    g.DrawLine(new Pen(Color.DarkViolet, 5), centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.HourArr);
                    isNewHour = false;
                }
                else
                {
                    g.Transform = hour;
                    g.DrawLine(new Pen(Color.DarkViolet, 5), centre.CentrePoint.X, centre.CentrePoint.Y, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + (int)RNumsArr.HourArr);
                }
            }

            g.DrawPath(penCircle, circle); // draws the circle

            DrawSerifs(g); // draws serifs

            middlePict.DrawMidPict(g, middlePict, centre); // draws the picture in the middle
        }

        private void DrawSerifs(Graphics g)
        {
            //float rotateAngleNumbers = (int)Degree.Circle + (int)RNumsArr.SecArr;
            //Matrix number;

            for (int i = 1; i <= (int)Clock.Serif; i++)
            {
                //rotateAngleNumbers -= (int)RNumsArr.SecArr;

                serifs.RotateAt(angleBetweenSerifs, centre.CentrePoint);
                g.Transform = serifs;
                g.DrawLine(penSefirs, centre.CentrePoint.X, centre.CentrePoint.Y - 100, centre.CentrePoint.X, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter);

                //number = new Matrix();
                //number.RotateAt(rotateAngleNumbers, new PointF(centre.CentrePoint.X - 6 + DefaultFont.Height / 2, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + 20 + DefaultFont.Height/2));
                //g.Transform = number;
                if (i % 3 == 0)
                {
                    g.DrawString(i.ToString(), DefaultFont, Brushes.YellowGreen, centre.CentrePoint.X - 6, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + 20);
                }
                else
                {
                    g.DrawString(i.ToString(), DefaultFont, Brushes.Aqua, centre.CentrePoint.X - 6, centre.CentrePoint.Y - ClientSize.Height / (int)Degree.Quarter + 20);
                }
            }
        }

    }
}
