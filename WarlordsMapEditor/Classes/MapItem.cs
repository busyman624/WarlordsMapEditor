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
        public Bitmap bitmap;
        public bool isWater;
        public int? objectIndex;
        public int? objectSet;

        public MapItem(int itemIndex, int setIndex, List<Sprite> _sprites, int Xcoordinate, int Ycoordinate)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            objectIndex = null;
            objectSet = null;
            this._sprites = _sprites;
            this.Xcoordinate = Xcoordinate;
            this.Ycoordinate = Ycoordinate;
            if (_sprites[setIndex].setName == "Water") isWater = true;
            else isWater = false;
            image = _sprites[setIndex].imagesList[itemIndex];
            bitmap = _sprites[setIndex].bitmapList[itemIndex];
        }

        private BitmapImage combineImages(int setIndex, int itemIndex)
        {
            Bitmap topbmp = _sprites[setIndex].bitmapList[itemIndex];
            Bitmap combined;
            BitmapImage combinedBitmapImage = new BitmapImage();

            combined= new Bitmap(bitmap.Width, bitmap.Height);

            //combine
            using (Graphics g = Graphics.FromImage(combined))
            {
                int offset = 0;
                g.DrawImage(bitmap, new Rectangle(offset, 0, bitmap.Width, bitmap.Height));
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
            Console.WriteLine("Hi I'm button X:" + Xcoordinate.ToString() + " Y: " + Ycoordinate.ToString());
            if(Board.selectedItemIndex!=null && Board.selectedSetIndex != null)
            {
                if (_sprites[(int)Board.selectedSetIndex].category == "Terrain")
                {
                    itemIndex = (int)Board.selectedItemIndex;
                    setIndex = (int)Board.selectedSetIndex;
                    objectIndex = null;
                    objectSet = null;

                    image = _sprites[setIndex].imagesList[itemIndex];
                    bitmap = _sprites[setIndex].bitmapList[itemIndex];
                    if (_sprites[setIndex].setName == "Water") isWater = true;
                    else isWater = false;
                }
                else if (!isWater || Board.selectedSetIndex == 7)
                {
                    objectIndex = Board.selectedItemIndex;
                    objectSet = Board.selectedSetIndex;
                    image = combineImages((int)objectSet, (int)objectIndex);
                }
            }
        }
    }
}
