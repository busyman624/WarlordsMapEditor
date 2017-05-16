using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Sprite
    {
        public List<BitmapImage> image;

        public int setIndex;
        public string setName { get; set; }

        public Sprite(Bitmap bmp, string setName, int setIndex)
        {
            image = new List<BitmapImage>();
            for (int i = 0; i < bmp.Width / bmp.Height; i++)
            {
                Bitmap temp_bmp = bmp.Clone(new Rectangle(i * bmp.Height, 0, bmp.Height, bmp.Height), bmp.PixelFormat);
                using (var memory = new MemoryStream())
                {
                    temp_bmp.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    BitmapImage temp_img = new BitmapImage();
                    temp_img.BeginInit();
                    temp_img.StreamSource = memory;
                    temp_img.CacheOption = BitmapCacheOption.OnLoad;
                    temp_img.EndInit();
                    image.Add(temp_img);
                }
            }
            this.setName = setName;
            this.setIndex = setIndex;
        }
    }
}
