using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace bitmap
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.init();
        }

        public void init()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] files = Directory.GetFiles(directory + "/files");
            foreach(string x in files)
            {
                Image img = Image.FromFile(x);
                Bitmap bm_1 = new Bitmap(img, 40, 40);
                Bitmap bm_2 = new Bitmap(img, 40, 40);
                ToGrayScale(bm_2);
                using (Graphics grfx = Graphics.FromImage(bm_1))
                {
                    grfx.DrawImage(bm_2, 0, 40);
                    bm_1.Save(directory + "/output_files", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        public void ToGrayScale(Bitmap Bmp)
        {
            int rgb;
            Color c;

            for (int y = 0; y < Bmp.Height; y++)
            {
                for (int x = 0; x < Bmp.Width; x++)
                {
                    c = Bmp.GetPixel(x, y);
                    rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                    Bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            }
        }
    }
}
