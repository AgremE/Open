using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFStylus
{
    public class Icon
    {
        struct IconBound
        {
            public double left;
            public double right;
            public double top;
            public double bottom;

            public IconBound(double l, double r, double t, double b)
            {
                left = l; right = r; top = t; bottom = b;
            }
        }

        private IconBound boundary;

        public Icon(double b_left, double b_right, double b_top, double b_bottom)
        {
            boundary = new IconBound(b_left, b_right, b_top, b_bottom);
        }

        /* Eclipse Area calculate amount of 1-dimention area (in pixel)
         * from given 2 podoubles which assume to have same x coordiante
         * calculate the length in pixel that the line between two podoubles are
         * in the icon box 
         */
        public double EclipseArea(Point above, Point below)
        {
            // if x-coordinate is not the same, return 0;
            if (above.X != below.X)
            {
                Console.Write("EclipseArea: Error, x-coordinate is not the same\n");
                return 0;
            }

            // out of bound
            if (above.X < boundary.left || above.X > boundary.right)
                return 0;

            // just in case the podoubles are swap
            double up = above.Y;
            double down = below.Y;
            if (up < down)
            {
                double temp = up;
                up = down;
                down = temp;
            }

            // up = MIN (up, boundary.top)
            up = Math.Min(up, boundary.top);
            // down = MAX (down, boundary.bottom)
            down = Math.Max(down, boundary.bottom);

            return up - down;
        }

        public double getArea()
        {
            return (boundary.right - boundary.left) * (boundary.top - boundary.bottom);
        }
    }
}
