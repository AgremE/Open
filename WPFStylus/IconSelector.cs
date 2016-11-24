using System;
using System.Collections.Generic;
using System.Collections;]
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
        List<Icon> IconArray;
    
        public IconSelector()
        {
            
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
            
            return false;
        }

        public Icon RemoveAt(int i)
        {
            return new Icon(0, 0, 0, 0);
        }

        public Point Select(ArrayList points)
        {
            return new Point();
        }
    }
}
