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
using System.IO;

namespace composer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Preview preview = new Preview();
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = FileDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = FileDialog.FileName;
                var srcBmp = Bitmap.FromFile(textBox1.Text) as Bitmap;
                var dstBmp = Bitmap.FromFile(textBox2.Text) as Bitmap;
                var srcData = srcBmp.LockBits(new Rectangle(new Point(), srcBmp.Size), ImageLockMode.ReadWrite, srcBmp.PixelFormat);
                var dstData = dstBmp.LockBits(new Rectangle(new Point(), dstBmp.Size), ImageLockMode.ReadWrite, dstBmp.PixelFormat);
                var srcArray = new byte [srcData.Stride * srcData.Height];
                var dstArray = srcArray.Clone() as byte[];
                System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcArray, 0, srcArray.Length);
                System.Runtime.InteropServices.Marshal.Copy(dstData.Scan0, dstArray, 0, dstArray.Length);
                for (int i = 0; i < srcArray.Length; i++)
                    srcArray[i] += dstArray[i];
                System.Runtime.InteropServices.Marshal.Copy(srcArray, 0, srcData.Scan0, srcArray.Length);
                srcBmp.UnlockBits(srcData);
                dstBmp.UnlockBits(dstData);
                dstBmp.Dispose();
                preview.BackgroundImage = srcBmp;
                preview.Size = srcBmp.Size;
                preview.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            preview.BackgroundImage.Save(textBox2.Text, ImageFormat.Png);
        }
    }
}
