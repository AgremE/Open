using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace WPFStylus
{
    public partial class Form1 : Form
    {
        public Form1(int id)
        {
            InitializeComponent();
            showPicture(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPicture(int id)
        {
            switch (id)
            {
                case 0:
                    pictureBox1.Image = Properties.Resources.ipad_pen;
                    pictureBox2.Image = Properties.Resources.circle;
                    break;
                case 1:
                    pictureBox1.Image = Properties.Resources.ipad_pen;
                    pictureBox2.Image = Properties.Resources.dot;
                    break;
                case 2:
                    pictureBox1.Image = Properties.Resources.surface_pen;
                    pictureBox2.Image = Properties.Resources.circle;
                    break;
                case 3:
                    pictureBox1.Image = Properties.Resources.surface_pen;
                    pictureBox2.Image = Properties.Resources.dot;
                    break;
            }
        }
    }
}
