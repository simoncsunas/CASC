using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace sLOGISTIC
{
    public partial class fCraneImage : Form
    {
        public string psImageFilePath = string.Empty;

        private int piPicStartX = 0;
        private int piPicEndX = 0;
        
        private int piPicStartY = 0;
        private int piPicEndY = 0;

        public Point piFirstPoint;
        public Point piSecondPoint;

        public fCraneImage()
        {
            InitializeComponent();
        }

#region main
        private void fCraneImage_Load(object sender, EventArgs e)
        {
            psImageFilePath = "C:\\CASC - Copy\\IR\\OR_TEST.bmp";

            gsDisplayImage(pic, psImageFilePath, PictureBoxSizeMode.StretchImage);

            this.WindowState = FormWindowState.Maximized;
        }
        private void fCraneImage_Resize(object sender, EventArgs e)
        {
            piPicEndX = pic.Width;
            piPicEndY = pic.Height;
        }
#endregion

#region button
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            gsClearPicBox();
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            Graphics g = pic.CreateGraphics();
            g.Clear(Color.Blue);
            Pen p = new Pen(Color.Red, 2.0f);

            for (int n = 1; n <= 50; n++)
            {
                g.DrawLine(p, n * (Cursor.Position.X), Cursor.Position.Y - 30.0f, n * (Cursor.Position.X), Cursor.Position.Y + 30.0f);
             }

            
        }
        private void tsbSaveAs_Click(object sender, EventArgs e)
        {
            //
        }
#endregion

#region function/procedure
        public static void gsDisplayImage(PictureBox pImage, string pImageFile, PictureBoxSizeMode pMode)
        {
            try
            {
                pImage.Image = Image.FromFile(pImageFile);
                pImage.SizeMode = pMode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Display Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void gsClearPicBox()
        {
            if (pic.Image != null)
            {
                pic.Image = null;
                pic.Invalidate();

                //+drawing
                //Graphics g = Graphics.FromImage(this.pic.Image);
                //g.Clear(this.pic.BackColor);
            }
        }
#endregion

#region events
        private void pic_DoubleClick(object sender, EventArgs e)
        {
            //
        }
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            //
        }
        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            //
        }
        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            //
        }
        private void pic_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Orange, piFirstPoint, piSecondPoint); 
        }
#endregion

    }
}
