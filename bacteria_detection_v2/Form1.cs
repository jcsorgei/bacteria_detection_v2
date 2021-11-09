using Emgu.CV;
using Emgu.CV.CvEnum;
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
            var markers = new Bitmap(pictureBox1.Image)
                .ToImage<Gray, byte>()
                .ThresholdBinary(new Gray(180), new Gray(255));
            markers.Save("_01_mask_default.png");
            Mat kernel1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Ellipse, new Size(51, 51), new Point(25, 25));


            CvInvoke.MorphologyEx(markers, markers, MorphOp.Erode, kernel1, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, new MCvScalar(1.0));
            markers.Save("_02_markers_morphologyex.png");
            /*Mat distanceTransofrm = new Mat();
            CvInvoke.DistanceTransform(mask2, distanceTransofrm, null, Emgu.CV.CvEnum.DistType.L2, 3);
            distanceTransofrm.Save("1distancet1.png");
            CvInvoke.Normalize(distanceTransofrm, distanceTransofrm, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);
            distanceTransofrm.Save("2distancet2.png");*/
            /*var markers = distanceTransofrm.ToImage<Gray, byte>()
                .ThresholdBinary(new Gray(50), new Gray(255));
            markers.Save("3markers3.png");*/

            var labels = new Image<Gray, Int32>(markers.Size);
            labels.Save("_04_labels.pgm");
            img.Save("_04a_img.tif");
            //Mat plLabel = new Mat();
            markers.Data[10, 10,0] = 255;
            CvInvoke.ConnectedComponents(markers, labels);
            //plLabel.ConvertTo(labels, Emgu.CV.CvEnum.DepthType.Cv32F);
            //Image<Gray, byte> finalMarkers = labels.Convert<Gray, byte>();
            //markers.Save("4finalmarkers4.png");
            //labels.ThresholdBinary(new Gray(1), new Gray(255));
            //labels.Convert<Gray, Int32>();

            CvInvoke.Watershed(img, labels);

            Image<Gray, Byte> umat = labels.Convert<Gray, Byte>();

            
            umat.Save("_03_segmented.png");

           /* Image<Gray, byte> boundaries = labels.Convert<byte>(delegate (Int32 x)
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
            mask2.Save("mask.png");*/
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
