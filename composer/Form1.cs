using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace composer
{
    public partial class Form1 : Form
    {
        private Form Preview;

        public Form1()
        {
            InitializeComponent();
            Preview = new Form();
            Preview.Text = "Preview";
            Preview.FormClosed += Preview_FormClosed;
        }

        private void Preview_FormClosed(object sender, EventArgs e)
        {
            Preview.Dispose();
            Preview = new Form();
            Preview.Text = "Preview";
            Preview.FormClosed += Preview_FormClosed;
        }

        private Bitmap CreateBitmap(string FileName)
        {
            Bitmap tmpBmp = Bitmap.FromFile(FileName) as Bitmap;
            Bitmap srcBmp = new Bitmap(tmpBmp);
            tmpBmp.Dispose();
            return srcBmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            dialog.Filter = "圖片格式|*.bmp;*.png";
            dialog.CheckFileExists = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
                Bitmap srcBmp = CreateBitmap(textBox1.Text);
                Preview.Size = srcBmp.Size;
                Preview.BackgroundImage = srcBmp;
                Preview.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            dialog.Filter = "圖片格式|*.bmp;*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
                Bitmap srcBmp = CreateBitmap(textBox1.Text);
                Bitmap dstBmp = Bitmap.FromFile(textBox2.Text) as Bitmap;
                BitmapData srcData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadWrite, srcBmp.PixelFormat);
                BitmapData dstData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadOnly, dstBmp.PixelFormat);
                byte[] srcArray = new byte[srcData.Stride * srcData.Height];
                byte[] dstArray = new byte[dstData.Stride * dstData.Height];
                System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcArray, 0, srcArray.Length);
                System.Runtime.InteropServices.Marshal.Copy(dstData.Scan0, dstArray, 0, dstArray.Length);
                for (int i = 0; i < srcData.Height; i++)
                    for (int j = 0; j < srcData.Stride; j++)
                        srcArray[i * srcData.Stride + j] += dstArray[i * srcData.Stride + j];
                System.Runtime.InteropServices.Marshal.Copy(srcArray, 0, srcData.Scan0, srcArray.Length);
                srcBmp.UnlockBits(srcData);
                dstBmp.UnlockBits(dstData);
                dstBmp.Dispose();
                Preview.BackgroundImage = srcBmp;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Preview.BackgroundImage.Save(textBox2.Text, ImageFormat.Png);
        }
    }
}
