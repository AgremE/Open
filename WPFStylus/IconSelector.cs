using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStylus
{   
    /* Class IconSelector
     * ------------------
     * Maintain list of icon and select one icon in the list
     * such that the circle matches
     */
    public class IconSelector
    {
        private List<Icon> IconArray;

        public class PointsComparer : IComparer
        {   
            /* Compare the two points to sort
             * Compare X-value first then Y-value
             */
            public int Compare(object x, object y)
            {
                Point l = (Point)x;
                Point r = (Point)y;
                if (l.X == r.X)
                {
                    return (int)(l.Y - r.Y);
                }

                return (int)(l.X - r.X);
            }
        }


    
        public IconSelector()
        {
            IconArray = new List<Icon>();
            // add NULL icon
            IconArray.Add(new Icon(0, 0, 0, 0));
        }
        
        public bool AddIcon(Icon i)
        {
            if (!IconArray.Contains(i))
            {
                IconArray.Add(i);
                return true;
            }
            return false;
        }

        public bool RemoveIcon(Icon i)
        {
            // TODO: implememt removing icon
            return IconArray.Remove(i);
        }

        public Icon RemoveAt(int i)
        {
            // remove icon from index
            if (i <= 0 || i >= IconArray.Count)
                return new Icon(0, 0, 0, 0);
            Icon x = IconArray[i];
            IconArray.RemoveAt(i);
            return x;
        }

        public Point Select(ArrayList points)
        {
            // sort the points in the array list


            // scoring
            long[] scores = new long[IconArray.Count];
            // for each icon
            
            return new Point();
        }


    }
}
