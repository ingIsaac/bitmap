using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace bitmap
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.init();
            Console.ReadKey();
        }

        public void init()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xml_file = directory + "/spells.xml";
            XDocument d_xml = XDocument.Load(xml_file);
            var filters = new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
            List<string> files = new List<string>();
            foreach(var filter in filters)
            {
                files.AddRange(Directory.GetFiles(directory + "\\files", String.Format("*.{0}", filter)));
            }
            Image img = null;
            Bitmap bm_1 = null;
            Bitmap bm_2 = null;
            Bitmap bm_3 = null;
            string t = string.Empty;
            foreach (string x in files)
            {
                Console.WriteLine("Loading... " + Path.GetFileNameWithoutExtension(x).Replace('_', ' '));
                img = Image.FromFile(x);
                bm_1 = new Bitmap(img, 40, 40);
                bm_2 = new Bitmap(img, 40, 40);
                bm_3 = new Bitmap(40, 80);
                ToGrayScale(bm_2);
                using (Graphics b = Graphics.FromImage(bm_3))
                {
                    b.DrawImage(bm_1, new Point(0, 0));
                    b.DrawImage(bm_2, new Point(0, 40));
                    t = getSpellWordsFromXML(d_xml, Path.GetFileNameWithoutExtension(x).Replace('_', ' '));
                    bm_3.Save(directory + "\\output_files\\" + (t != string.Empty ? t : Path.GetFileNameWithoutExtension(x)) + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
                bm_1 = null;
                bm_2 = null;
                bm_3 = null; 
                img = null;
            }
        }

        public string getSpellWordsFromXML(XDocument doc, string spell)
        {
            string r = string.Empty;
            try
            {
                var tc = (from x in doc.Descendants("spells").Elements("instant") where x.Attribute("name").Value == spell select x).Select(x => x.Attribute("words").Value).SingleOrDefault();
                if(tc != null)
                {
                    r = tc;
                }
            }
            catch(Exception e)
            {
                Console.Write(e);
            }
            return r;
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
