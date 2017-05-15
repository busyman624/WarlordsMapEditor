using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Sprite
    {
        private List<BitmapImage> Tiles;
        public List<BitmapImage> ButtonTiles { get; set; }
        public int currentIndex { get; set; }
        public string setName { get; set; }

        public Sprite(Bitmap bmp, string setName)
        {
            Tiles = new List<BitmapImage>();
            ButtonTiles = new List<BitmapImage>();
            for (int i = 0; i < bmp.Width / bmp.Height; i++)
            {
                Bitmap temp_bmp = bmp.Clone(new Rectangle(i*bmp.Height, 0, bmp.Height, bmp.Height), bmp.PixelFormat);
                using (var memory = new MemoryStream())
                {
                    temp_bmp.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    BitmapImage temp_img= new BitmapImage();
                    temp_img.BeginInit();
                    temp_img.StreamSource = memory;
                    temp_img.CacheOption = BitmapCacheOption.OnLoad;
                    temp_img.EndInit();
                    Tiles.Add(temp_img);
                }
            }
            this.setName = setName;
            currentIndex = 0;
            RefreshButtonTiles();
        }

        public void RefreshButtonTiles()
        {
            ButtonTiles.Clear();
            ButtonTiles.Add(Tiles[currentIndex]);
            ButtonTiles.Add(Tiles[currentIndex+1]);
            ButtonTiles.Add(Tiles[currentIndex+2]);
        }

        public int getSize()
        {
            return Tiles.Count();
        }
    }
}
