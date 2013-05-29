using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PopnTouchi2.Infrastructure
{
    public class MyPoint
    {
        public double X;
        public double Y;

        public MyPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public MyPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public bool equals(Point p)
        {
            return p.X == X && p.Y == Y;
        }

        public bool QuasiEquals(Point p)
        {
            return ((X+1 > p.X && X-1 < p.X) || (Y+1 > p.Y && Y-1 < p.Y));
        }
    }
}
