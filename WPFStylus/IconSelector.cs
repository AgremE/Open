using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Shapes;
using System.Windows.Controls;

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
            IconArray.Add(new Icon(1, 2, 4, 3));
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
                return new Icon(1, 2, 4, 3);
            Icon x = IconArray[i];
            IconArray.RemoveAt(i);
            return x;
        }

        
        public Icon Select(List<Point> points)
        {
            // Sort the points in the array list by x
            List<Point> preprocessed_pts = PreprocessPointsForCircleCompleting(points);

            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\CS472\Desktop\Pie's workspace\WriteLines2.txt", true);
            
            foreach (Point x in preprocessed_pts)
            {
                file.WriteLine(points[0].X + "          " + points[0].Y);
            }
            file.Close();
            
            if (!is_completeCircle(points))
            {
             //   file.WriteLine("Circle is not complete");
                points = completeTheCircleByLine(points);
            }
            /*
            file.WriteLine("There after complete the cirlce");
            
            foreach (Point x in preprocessed_pts)
            {
                file.WriteLine(x.X + "          " + x.Y);
            }
            file.Close();
            */
            preprocessed_pts = preprocessed_pts.Distinct().ToList();
            preprocessed_pts.Sort(new PointComparer());
            
            //System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\CS472\Desktop\Pie's workspace\WriteLines2.txt", true);
            


            // scoring
            double[] scores = new double[IconArray.Count];
            for (int i = 0; i < IconArray.Count; i++)
            {
                for (int j = 0; j < preprocessed_pts.Count - 1; j++)
                {
                    Point cur_pt = preprocessed_pts[j];
                    Point next_pt = preprocessed_pts[j + 1];
                    scores[i] += IconArray[i].EclipseArea(cur_pt, next_pt);
                }
                //Get area in the form of percentage
                scores[i] = scores[i] / IconArray[i].getArea();
            }

            // Find icon with max score
            double maxValue = scores.Max();
            int maxIndex = scores.ToList().IndexOf(maxValue);

            // Return the mid-point of that Icon
            return IconArray[maxIndex];
        }


        /// <summary>
        /// Preprocess all the points from the stylus to be contineous and convert to int type
        /// </summary>
        /// <param name="points">Raw list of Points from stylus</param>
        /// <returns>List of contigeous Points objects</returns>
        private List<Point> PreprocessPointsForCircleCompleting(List<Point> points)
        {
            List<Point> preprocessed_pts = new List<Point>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                Point cur_pt = points[i]; // Current point
                Point next_pt = points[i + 1]; // Next point

                // Add the original rounded point
                preprocessed_pts.Add(new Point(Math.Round(cur_pt.X), Math.Round(cur_pt.Y)));

                //Add any point between this point and the next point
                preprocessed_pts.AddRange(GetPointsOnLine((int)cur_pt.X, (int)cur_pt.Y, (int)next_pt.X, (int)next_pt.Y).ToList());
            }

            // Remove duplicates and return
            return preprocessed_pts;
        }


        // Helper class for sorting the ArrayList of Points to sort by X first then Y ascending
        class PointComparer : IComparer<Point>
        {
            public int Compare(Point first, Point second)
            {
                if (first.X == second.X)
                {
                    return (int) (first.Y - second.Y);
                }
                else
                {
                    return (int) (first.X - second.X);
                }
            }
        }

        public void showIcons(InkCanvas canvas)
        {
            foreach (Icon icon in IconArray){
                icon.showArea(canvas);
            }
        }

        public void hideIcons(InkCanvas canvas)
        {
            canvas.Children.Clear();
        }

        // Bresenham's Line Algorithm 
        // Finding all the pixels between two points
        // Copy from: http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        public bool is_completeCircle(List<Point> points)
        {
            bool is_complete = false;
            Point firstPoint = points[0];
            for(int i = 1; i < points.Count -1; i++)
            {
                if(points[i].X == firstPoint.X)
                {
                    if(points[i].Y == firstPoint.Y)
                    {
                        is_complete = true;
                    }
                }
            }
            return is_complete;
        }
        // Call only when the circle is not complete
        public List<Point> completeTheCircleByLine(List<Point> points)
        {
            //List<Point> complete_circle = new List<Point>();
            Point firstPoint = points[0];
            Point finalPoint = points[points.Count - 1];
            points.AddRange(GetPointsOnLine((int)firstPoint.X, 
                                                (int)firstPoint.Y,
                                                    (int)finalPoint.X, 
                                                           (int)finalPoint.Y).ToList());
            return points;
        }

        //End of the class
    }
    
}
