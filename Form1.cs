using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//emgu library
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing.Imaging;

/*
 * The current reason this program is not working is believed to be
 * that the XML file containing the facial recognition data is not
 * formatted correctly. The idea is that the file should be cloned
 * onto a folder ton this PC then dragged into this project rather
 * than being copied and pasted.
 * The Tutorial I've been following is complete except for the
 * current error: https://www.youtube.com/watch?v=zLgIy0o_0Ow
 * The XML file is from the openCV repo which should be cloned:
 * https://github.com/opencv/opencv/tree/master/data/haarcascades
 * I created a clone folder for this under /source/
 */

namespace Face_Detection
{
    public partial class Form1 : Form
    {
        //retrieve face recognition data
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        public Form1() { InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e) { }

        private void btnDetect_Click_1(object sender, EventArgs e)
        {
            //allow user to open a file dialog and select image to scan
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false, Filter = "JPEG|*.jpg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pic.Image = Image.FromFile(ofd.FileName);
                    Bitmap bitmap = new Bitmap(pic.Image);

                    //creating an Image from a bitmap is no longer supported in the latest version of emgu.
                    //instead of downgrading, add Emgu.CV.Bitmap from NuGet then use .ToImage<>() 

                    //NEW METHOD .ToImage ~ Emgu.CV.Bitmap
                    Image<Bgr, byte> greyImage = bitmap.ToImage<Bgr, byte>();

                    Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(greyImage, 1.4, 0);

                    //draw rectangles around detected faces
                    foreach (Rectangle rectangle in rectangles)
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Pen pen = new Pen(Color.Red, 1))
                            {
                                graphics.DrawRectangle(pen, rectangle);
                            }
                        }
                    }
                    pic.Image = bitmap;
                }
            }
        }
    }
}
