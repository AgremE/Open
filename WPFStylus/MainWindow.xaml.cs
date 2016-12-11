using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace WPFStylus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        List<Point> listPoints = new List<Point>();

        public IntPtr hWnd;

        private IconSelector selector;

        private int status = 0;
        private List<int> scenarios;


        public MainWindow()
        {
            InitializeComponent();
            icBox.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            scenarios = new List<int>( new int[] { 0, 1, 2, 3, 4, 5 });
            Shuffle(scenarios);
            showScernario(scenarios[status++]);
            
        }

        private void showScernario(int scerscernarioID)
        {
            selector = new IconSelector();
            switch (scerscernarioID)
            {
                case 0:
                    addSmallIcons(false);
                    break;
                case 1:
                    addSmallIcons(true);
                    break;
                case 2:
                    addListView(false);
                    break;
                case 3:
                    addListView(true);
                    break;
                case 4:
                    addLargeIcons(false);
                    break;
                case 5:
                    addLargeIcons(true);
                    break;
            }
            Icon random_icon = selector.randomIcon();
            selector.showIcons(icBox);
            random_icon.hideArea(icBox);
            random_icon.showAreaRed(icBox);
        }

        // Shuffle list
        // http://stackoverflow.com/questions/273313/randomize-a-listt
        public void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void addListView(bool sep)
        {
            int y = 0;
            while (y <= 800)
            {
                // Icon coordinat for explorer from drive C:/ProgramFIle(x86)
                Icon i = new Icon(181, 754, y+22, y);
                selector.AddIcon(i);
                y += 22;
                if (sep)
                    y += 8;
            }
        }

        private void addLargeIcons(bool sep)
        {
            int x=0, y = 0;
            while (x <= 1200)
            {
                while (y <= 800)
                {
                    // Icon coordinat for explorer from drive C:/ProgramFIle(x86)
                    Icon i = new Icon(x, x+81, y + 81, y);
                    selector.AddIcon(i);
                    y += 81;
                    if (sep)
                        y += 8;
                }
                x += 81;
                y = 0;
                if (sep)
                    x += 8;
            }
        }

        private void addSmallIcons(bool sep)
        {
            int x = 0, y = 0;
            while (x <= 1200)
            {
                while (y <= 800)
                {
                    // Icon coordinat for explorer from drive C:/ProgramFIle(x86)
                    Icon i = new Icon(x, x + 27, y + 27, y);
                    selector.AddIcon(i);
                    y += 27;
                    if (sep)
                        y += 8;
                }
                x += 27;
                y = 0;
                if (sep)
                    x += 8;
            }
        }

        private void stylus_move(object sender, StylusEventArgs e)
        {
            Point pos = e.GetPosition(this);
            var point = PointToScreen(pos);
            this.listPoints.Add(pos);
        }

        //Call to check the circle is complete or not
        // Dont use arrayList.exist() each point is an object they treat them differently
        public bool checkFullCircle()
        {
            int x_coordinate = (int)PointToScreen((Point)this.listPoints[0]).X;
            int y_coordinate = (int)PointToScreen((Point)this.listPoints[0]).Y;
            for(int i = 1; i < this.listPoints.Count; i++)
            {
                int x_coordinate_second = (int)PointToScreen((Point)this.listPoints[0]).X;
                int y_coordinate_seconde = (int)PointToScreen((Point)this.listPoints[0]).Y;
                if ((x_coordinate == x_coordinate_second) && (y_coordinate == y_coordinate_seconde))
                {
                    return true;
                }
            }
            return false; 
        }

        // Call only when the circle is not complete
        public void completeCircle()
        {

            int x_coordinate = (int)PointToScreen((Point)this.listPoints[0]).X;
            int y_coordinate = (int)PointToScreen((Point)this.listPoints[0]).Y;

            int x_coordinate_end = (int)PointToScreen((Point)this.listPoints[listPoints.Count - 1]).X;
            int y_coordinate_end = (int)PointToScreen((Point)this.listPoints[listPoints.Count - 1]).Y;
        }

        private void stylus_up(object sender, StylusEventArgs e)
        {
            // Find the best points
            Icon select_icon = selector.Select(listPoints);
            //selector.hideIcons(icBox);
            //select_icon.showArea(icBox);
            //Point select_pt = PointToScreen(select_icon.getMidPoint());
            //Console.Write(select_pt);

            this.listPoints.Clear();
            selector.hideIcons(icBox);
            showScernario(scenarios[status++]);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                selector.showIcons(icBox);
            else if (e.Key == Key.Escape)
                selector.hideIcons(icBox);
            else if (e.Key == Key.Q)
                Application.Current.Shutdown();
        }

        private void deactivated(object sender, EventArgs e)
        {
            // When ever deactivated, put this app back to topmost
            Window window = (Window)sender;
            //window.Topmost = true;
        }
    }
}
