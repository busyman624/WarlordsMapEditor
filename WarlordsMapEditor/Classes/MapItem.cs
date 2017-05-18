using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class MapItem : Item
    {
        private List<Sprite> _sprites;
        public int Xcoordinate;
        public int Ycoordinate;
        public bool isWater;
        public MapItem(int itemIndex, int setIndex, List<Sprite> _sprites, int Xcoordinate, int Ycoordinate)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this._sprites = _sprites;
            this.Xcoordinate = Xcoordinate;
            this.Ycoordinate = Ycoordinate;
            if (_sprites[setIndex].setName == "Water") isWater = true;
            else isWater = false;
            image = _sprites[setIndex].imagesList[itemIndex];
        }

        private BitmapImage combineImages(int setIndex, int itemIndex)
        {
            Bitmap basebmp;
            Bitmap topbmp = _sprites[setIndex].bitmapList[itemIndex];
            Bitmap combined;
            BitmapImage combinedBitmapImage = new BitmapImage();

            //conver base to Bitmap
            using (MemoryStream baseOutStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(image));
                enc.Save(baseOutStream);
                basebmp = new Bitmap(baseOutStream);
            }

             combined= new Bitmap(basebmp.Width, basebmp.Height);

            //combine
            using (Graphics g = Graphics.FromImage(combined))
            {
                int offset = 0;
                g.DrawImage(basebmp, new Rectangle(offset, 0, basebmp.Width, basebmp.Height));
                g.DrawImage(topbmp, new Rectangle(offset, 0, topbmp.Width, topbmp.Height));
            }

            using (var memory = new MemoryStream())
            {
                combined.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                combinedBitmapImage.BeginInit();
                combinedBitmapImage.StreamSource = memory;
                combinedBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                combinedBitmapImage.EndInit();
            }

            return combinedBitmapImage;
        }

        public override void onItemClick()
        {
            if(Board.selectedItemIndex!=null && Board.selectedSetIndex != null)
            {
                itemIndex = (int)Board.selectedItemIndex;
                setIndex = (int)Board.selectedSetIndex;

                if (_sprites[(int)Board.selectedSetIndex].category == "Terrain")
                {
                    image = _sprites[setIndex].imagesList[itemIndex];
                    if (_sprites[setIndex].setName == "Water") isWater = true;
                    else isWater = false;
                }
                else if (!isWater || setIndex == 7) image = combineImages(setIndex, itemIndex);
            }
        }
    }
}
