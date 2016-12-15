using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace WPFStylus
{
    public partial class RestForm : Form
    {
        private DispatcherTimer t;
        public RestForm()
        {
            InitializeComponent();
            t = new DispatcherTimer();
            t.Tick += new EventHandler(timer_tick); ;
            t.Interval = new TimeSpan(0, 0, 5);
            t.Start();
            
        }

        private void timer_tick(object sender, EventArgs e)
        {
            t.Stop();
            ready_btn.Enabled = true;
        }

        private void ready_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
