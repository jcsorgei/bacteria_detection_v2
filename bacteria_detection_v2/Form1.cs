using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bacteria_detection_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var img = new Bitmap(pictureBox1.Image)
                .ToImage<Bgr, byte>();
            var mask2 = new Bitmap(pictureBox1.Image)
                .ToImage<Gray, byte>()
                .ThresholdBinary(new Gray(180), new Gray(255));
            Mat distanceTransofrm = new Mat();
            CvInvoke.DistanceTransform(mask2, distanceTransofrm, null, Emgu.CV.CvEnum.DistType.L2, 3);
            CvInvoke.Normalize(distanceTransofrm, distanceTransofrm, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);
            var markers = distanceTransofrm.ToImage<Gray, byte>()
                .ThresholdBinary(new Gray(50), new Gray(255));
            CvInvoke.ConnectedComponents(markers, markers);
            var finalMarkers = markers.Convert<Gray, Int32>();

            CvInvoke.Watershed(img, finalMarkers);

            Image<Gray, byte> boundaries = finalMarkers.Convert<byte>(delegate (Int32 x)
            {
                return (byte)(x == -1 ? 255 : 0);
            });

            boundaries._Dilate(1);
            img.SetValue(new Bgr(0, 255, 0), boundaries);
            pictureBox1.Image = img.ToBitmap();
            pictureBox2.Image = mask2.ToBitmap();
            //mask2.Save("./mask.png");
        }

        private void watershedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void segmentationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
