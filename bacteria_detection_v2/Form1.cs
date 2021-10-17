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
            mask2.Save("mask_default.png");
            Mat kernel1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Ellipse, new Size(50, 50), new Point(25,25));


            CvInvoke.MorphologyEx(mask2, mask2, 0, kernel1, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(1.0));
            mask2.Save("mask_morphologyex.png");
            Mat distanceTransofrm = new Mat();
            CvInvoke.DistanceTransform(mask2, distanceTransofrm, null, Emgu.CV.CvEnum.DistType.L2, 3);
            distanceTransofrm.Save("1distancet1.png");
            CvInvoke.Normalize(distanceTransofrm, distanceTransofrm, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);
            distanceTransofrm.Save("2distancet2.png");
            var markers = distanceTransofrm.ToImage<Gray, byte>()
                .ThresholdBinary(new Gray(50), new Gray(255));
            markers.Save("3markers3.png");
            CvInvoke.ConnectedComponents(markers, markers);
            var finalMarkers = markers.Convert<Gray, Int32>();
            markers.Save("4finalmarkers4.png");
            CvInvoke.Watershed(img, finalMarkers);

            finalMarkers.Save("5markers5.png");

            Image<Gray, byte> boundaries = finalMarkers.Convert<byte>(delegate (Int32 x)
            {
                return (byte)(x == -1 ? 255 : 0);
            });
            boundaries.Save("6boundaries6.png");


            boundaries._Dilate(1);
            boundaries.Save("7boundaries7.png");
            img.SetValue(new Bgr(0, 0, 255), boundaries);
            pictureBox1.Image = img.ToBitmap();
            pictureBox2.Image = mask2.ToBitmap();
            //mask2.Save("./mask.png");
            img.Save("result.png");
            mask2.Save("mask.png");
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
