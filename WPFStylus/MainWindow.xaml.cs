using MongoDB.Bson;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private int scenarioID = 0;
        private int penID = 0;
        private List<int> scenarios;
        private List<int> pens;    // type of input (pen type, input method)
        private Icon random_icon;

        private Stopwatch timer;
        private double decision_time = 0;

        private DatabaseHandler database = null;

        public MainWindow()
        {
            InitializeComponent();
            icBox.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            scenarios = new List<int>( new int[] { 0, 1, 2, 3, 4, 5 });
            pens = new List<int>(new int[] { 0, 1, 2, 3});

            MessageBoxResult result1 = MessageBox.Show("Is this real session?",
    "Real session", MessageBoxButton.YesNo);
            if (result1 == MessageBoxResult.Yes)
            {
                database = new DatabaseHandler();   
            }
            // database = null

            // initalize timer
            timer = new Stopwatch();

            // randomize method
            Shuffle(pens);
            Shuffle(scenarios);
            Form1 m = new Form1(pens[penID++]);
            m.ShowDialog();
            showScernario(scenarios[scenarioID++]);
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
            random_icon = selector.randomIcon();
            selector.showIcons(icBox);
            random_icon.hideArea(icBox);
            random_icon.showAreaRed(icBox);

            // screen is done, (re)start timer
            timer.Restart();
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

        private void stylus_down(object sender, StylusEventArgs e)
        {
            // stylus is down
            // decision time
            timer.Stop();
            decision_time = timer.ElapsedMilliseconds / 1000d;
            timer.Start();
        }

        private void stylus_move(object sender, StylusEventArgs e)
        {
            Point pos = e.GetPosition(this);
            var point = PointToScreen(pos);
            this.listPoints.Add(pos);
        }

        private void stylus_up(object sender, StylusEventArgs e)
        {
            // stylus is up
            timer.Stop();
            double eclipsed_time = timer.ElapsedMilliseconds / 1000d;

            // Find the best points
            Icon select_icon = selector.Select(listPoints);
            int correct = 0;
            if (select_icon == random_icon)
                correct = 1;

            // if the input is real test -- not training
            if (database != null)
                database.logData(eclipsed_time, decision_time, 
                    scenarios[scenarioID - 1], pens[penID - 1], correct);

            //selector.hideIcons(icBox);
            //select_icon.showArea(icBox);
            //Point select_pt = PointToScreen(select_icon.getMidPoint());
            //Console.Write(select_pt);

            this.listPoints.Clear();
            selector.hideIcons(icBox);
            if (scenarioID >= 6)
            {
                scenarioID = 0;
                Shuffle(scenarios);
                // selector.hideIcons(icBox);
                if (penID >= 4)
                    Application.Current.Shutdown();
                else
                {
                    Form1 m = new Form1(pens[penID++]);
                    m.ShowDialog();
                }
            }
            showScernario(scenarios[scenarioID++]);
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

class DatabaseHandler
{
    //This is my connection string i have assigned the database file address path
    private string connectionKey = "datasource=localhost;port=3306;username=root;password=chayanin";
    private int userID;

    MySqlConnection connection;

    public DatabaseHandler()
    {   
        // connection
        try
        {
            connection = new MySqlConnection(connectionKey);
            connection.Open();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
        // TODO: get user ID here
        // insert dummy user int
        talkToDatabase("insert into open.user (`dummy`) values (8);").Close();
        userID = getMaxId("open.user");
    }

    MySqlDataReader talkToDatabase(string query)
    {
        try
        {

            //command class which will handle the query and connection object. 
            MySqlCommand cmd = new MySqlCommand(query, connection);

            // Here our query will be executed and data saved into the database
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();

            return reader;
        } catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
        return null;
    }

    public void logData(double op_time, double decision_time, int scenario, int input_method, int correctness)
    {  
        talkToDatabase("insert into `open`.`selection` (`operation_time`, `decision_time`, `scenarioID`, `penID`, `right`, `user`) values('" 
            + op_time + "','"
            + decision_time + "','"
            + scenario + "','" 
            + input_method + "','" 
            + correctness + "','"
            + userID
            + "');")
            .Close();
    }

    // get max id from table
    int getMaxId(String tableName)
    {
        MySqlDataReader r = talkToDatabase(String.Format("select MAX(id) from {0}", tableName));
        int max = -1;
        if (r.Read())
        {
            max = r.GetInt32(0);
        }
        r.Close();
        return max;
    }
}
