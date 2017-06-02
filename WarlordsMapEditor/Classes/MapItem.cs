using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
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
        public int palleteId;
        public MiniMap miniMap;

        public MapItem(int itemIndex, int setIndex, List<Sprite> _sprites, int Xcoordinate, int Ycoordinate, int palleteId, MiniMap miniMap)
        {
            this.palleteId = palleteId;
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this.miniMap = miniMap;
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

        public void combineImages()
        {
            Bitmap topbmp = _sprites[(int)objectSet].bitmapList[(int)objectIndex];
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

            bitmap = combined;
            image= combinedBitmapImage;
        }

        public override void execute_MouseMoveCommand(MouseEventArgs param)
        {
            if (Board.brushDrawStart) //is brush selected ?
            {
                changeTile();
            }
        }

        public override void onItemClick()
        {
            changeTile();
            if (Board._brushIsClicked && Board.brushDrawStart == false) //decide if brush drawing starts or ends
            {
                Board.brushDrawStart = true;
            }
            else
            {
                Board.brushDrawStart = false;
            }
        }

        private void changeTile()
        {
            if (Board.selectedItemIndex != null && Board.selectedSetIndex != null)
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
                    combineImages();
                }
                miniMap.refresh(Xcoordinate, Ycoordinate);
            }
        }
    }
}
