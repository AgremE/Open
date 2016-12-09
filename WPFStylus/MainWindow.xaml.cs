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

        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public IntPtr hWnd;

        private IconSelector selector;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }


        public MainWindow()
        {
            InitializeComponent();
            icBox.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            selector = new IconSelector();
            
            // Add Dummy Icon
            // Icon coordinat for explorer from drive C:/ProgramFIle(x86)
            Icon i = new Icon(181, 754, 201, 179);
            selector.AddIcon(i);
            //close icon
            i = new Icon(1205, 1250, 35, 0);
            selector.AddIcon(i);
            //C:/User
            i = new Icon(181, 754, 223, 201);
            selector.AddIcon(i);
            //C:/Windows
            i = new Icon(181, 754, 245, 223);
            selector.AddIcon(i);
            //Internetl explorer setting button top right coner
            i = new Icon(1195, 1243, 80, 38);
            selector.AddIcon(i);
            //Internet explorer setting button
            i = new Icon(845, 1243, 690, 645);
            selector.AddIcon(i);
            // button what new and tip in interent explorer
            i = new Icon(845, 1243, 645, 600);
            selector.AddIcon(i);
           
            //Setting button
            i = new Icon(421, 451, 1060, 892);
            selector.AddIcon(i);
            // Navigator to kakoa program
            //i = new Icon(917, 1016, 786, 745);
            i = new Icon(885, 912, 820, 790);
            selector.AddIcon(i);
           // i = new Icon(892, 1060, 745, 723);
           // Kakao Icon
            i = new Icon(915, 940, 785, 755);
            selector.AddIcon(i);
            /*
            i = new Icon(100, 200, 300, 100);
            selector.AddIcon(i);
            i = new Icon(50, 100, 200, 100);
            selector.AddIcon(i);
            i = new Icon(300, 400, 300, 100);
            selector.AddIcon(i);*/
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
            selector.hideIcons(icBox);
            select_icon.showArea(icBox);
            Point select_pt = PointToScreen(select_icon.getMidPoint());
            Console.Write(select_pt);

            // Set windows to transporent
            var process = Process.GetCurrentProcess();
            hWnd = process.MainWindowHandle;
            var extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);

            // Perform click
            LeftMouseClick((int)select_pt.X, (int)select_pt.Y);

            // Restore windows
            SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle);

            this.listPoints.Clear();
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
            window.Topmost = true;
        }
    }
}
