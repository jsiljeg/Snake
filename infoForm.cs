using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public partial class infoForm : Form
    {
        public infoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        Game mygame = new Game(740, 760);
        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            Pen myPen = new Pen(Color.Red);

            e.Graphics.FillRectangle(mygame.bait_skin_color_smjer, 50, 50, mygame.node_length, mygame.node_length);
            e.Graphics.FillRectangle(mygame.bait_body_color_smjer, 50, 50, mygame.node_length - mygame.node_border_length, mygame.node_length - mygame.node_border_length);

            e.Graphics.FillRectangle(mygame.bait_skin_color_inc, 50, 110, mygame.node_length, mygame.node_length);
            e.Graphics.FillRectangle(mygame.bait_body_color_inc, 50, 110, mygame.node_length - mygame.node_border_length, mygame.node_length - mygame.node_border_length);

            e.Graphics.FillRectangle(mygame.bait_skin_color_dec, 50, 170, mygame.node_length, mygame.node_length);
            e.Graphics.FillRectangle(mygame.bait_body_color_dec, 50, 170, mygame.node_length - mygame.node_border_length, mygame.node_length - mygame.node_border_length);

            e.Graphics.FillRectangle(mygame.bait_skin_color_lv, 50, 230, mygame.node_length, mygame.node_length);
            e.Graphics.FillRectangle(mygame.bait_body_color_lv, 50, 230, mygame.node_length - mygame.node_border_length, mygame.node_length - mygame.node_border_length);

            myPen.Dispose();

        } 

    }
}
