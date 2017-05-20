﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class Sprite
    {
        public List<BitmapImage> imagesList;
        public List<Bitmap> bitmapList;

        public int setIndex;
        public string setName { get; set; }

        public string category;

        public Sprite(Bitmap bmp, string setName, int setIndex, string category)
        {
            this.setName = setName;
            this.setIndex = setIndex;
            this.category = category;

            imagesList = new List<BitmapImage>();
            bitmapList = new List<Bitmap>();

            for (int i = 0; i < bmp.Width / bmp.Height; i++)
            {
                Bitmap temp_bmp = bmp.Clone(new Rectangle(i * bmp.Height, 0, bmp.Height, bmp.Height), bmp.PixelFormat);
                temp_bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                using (var memory = new MemoryStream())
                {
                    temp_bmp.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    BitmapImage temp_img = new BitmapImage();
                    temp_img.BeginInit();
                    temp_img.StreamSource = memory;
                    temp_img.CacheOption = BitmapCacheOption.OnLoad;
                    temp_img.EndInit();
                    imagesList.Add(temp_img);
                }
                bitmapList.Add(temp_bmp);
            }
        }
    }
}
