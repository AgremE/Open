using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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

        Timer timer = new Timer();

        /*[DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        private const uint SW_SHOW = 0x05;*/

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public IntPtr hWnd;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }


        //System.ComponentModel.BackgroundWorker worker;
        public MainWindow()
        {
            InitializeComponent();
            icBox.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            /*timer.Enabled = true;
            timer.Elapsed += timer_tick;
            timer.Interval = 1000;
            timer.Start();
            worker = new System.ComponentModel.BackgroundWorker();
           worker.DoWork += Worker_DoWork;
           worker.ProgressChanged += Worker_ProgressChanged;
           worker.WorkerReportsProgress = true;
           worker.RunWorkerAsync();*/
        }

        /*private void timer_tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (this.WindowState == WindowState.Minimized)
                {
                    ShowWindow(hWnd, 3);
                    this.WindowState = WindowState.Maximized;
                    Console.Write(hWnd);
                }
            });
        }*/

        /*protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            Console.WriteLine("hwnd: "+hwnd);
            MainWindow.SetWindowExTransparent(hwnd);
        }*/

        /*private void Worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            
            //you can update progress bar or UI component from the value handled
        }

        private void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                //do something here
                int progress = 0;
                worker.ReportProgress(progress);
            }   
        }*/
        

        private void stylus_move(object sender, StylusEventArgs e)
        {
            Point pos = e.GetPosition(this);
            var point = PointToScreen(pos);
            this.listPoints.Add(pos);

        }
        
        // Changing from  StylusSystemGestureEventArgs to StylusEventArgs
        private void icBox_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
        {
            
            
            /*
            switch (e.SystemGesture) {
                case SystemGesture.Tap:
                    // Get tapping position relative to the screen
                    Point pos = e.GetPosition(this);
                    var point = PointToScreen(pos);
                    
                    // Get current proceess ID
                    var process = Process.GetCurrentProcess();
                    hWnd = process.MainWindowHandle;

                    // Get current style and set windows transparent
                    // After this line all click-though the current windows
                    var extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                    SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);

                    // Perform left click
                    
                    LeftMouseClick((int)point.X, (int)point.Y);
                    Console.WriteLine("Clicking at X: " + point.X + " Y: " + point.Y);

                    // Restore windows
                    SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle);
                    break;
                
                }*/

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
            int lenght = this.listPoints.Count;
            int halfLenght = lenght / 2;
            Console.Write("Clicking Begin");
            // need to check the complete circle first the doing the distance calculation

            var first_point = PointToScreen((Point)this.listPoints[0]);
            var second_point = PointToScreen((Point)this.listPoints[halfLenght]);
            
            int x_coordinate = (int)first_point.X + ((int)second_point.X - (int)first_point.X) / 2;
            int y_coordinate = (int)first_point.Y + ((int)second_point.Y - (int)first_point.Y) / 2;

            var process = Process.GetCurrentProcess();
            hWnd = process.MainWindowHandle;

            var extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);

            LeftMouseClick((int)x_coordinate, (int)y_coordinate);

            SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle);

            IconSelector selector = new IconSelector();

            // Add Dummy Icon
            selector.AddIcon(new Icon(500, 550, 350, 300));

            // Find the best points
            Point select_pt = selector.Select(listPoints);
            Console.Write(select_pt);

            this.listPoints.Clear();
            //LeftMouseClick((int)x_coordinate, (int)y_coordinate);
        }

        
    }
}
