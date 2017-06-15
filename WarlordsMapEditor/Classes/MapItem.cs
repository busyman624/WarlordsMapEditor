using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
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

        public string castleName;
        public int? castleOwner;

        public MapItem(int itemIndex, int setIndex, List<Sprite> _sprites, int Xcoordinate, int Ycoordinate, int palleteId, MiniMap miniMap)
        {
            this.palleteId = palleteId;
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this.miniMap = miniMap;
            objectIndex = null;
            objectSet = null;
            castleName = null;
            castleOwner = null;
            this._sprites = _sprites;
            this.Xcoordinate = Xcoordinate;
            this.Ycoordinate = Ycoordinate;
            if (_sprites[setIndex].setName == "Water") isWater = true;
            else isWater = false;
            image = _sprites[setIndex].imagesList[itemIndex];
            bitmap = _sprites[setIndex].bitmapList[itemIndex];
        }

        public MapItem() { }

        public MapItem saveInstance()
        {
            MapItem mapItem = new MapItem();
            mapItem.itemIndex = itemIndex;
            mapItem.setIndex = setIndex;
            mapItem.Xcoordinate = Xcoordinate;
            mapItem.Ycoordinate = Ycoordinate;
            mapItem.bitmap = bitmap;
            mapItem.image = image;
            mapItem.isWater = isWater;
            mapItem.objectIndex = objectIndex;
            mapItem.objectSet = objectSet;
            mapItem.castleName = castleName;
            mapItem.castleOwner = castleOwner;
            return mapItem;
        }

        public void restore(MapItem mapItem)
        {
            itemIndex = mapItem.itemIndex;
            setIndex = mapItem.setIndex;
            Xcoordinate = mapItem.Xcoordinate;
            Ycoordinate = mapItem.Ycoordinate;
            bitmap = mapItem.bitmap;
            image = mapItem.image;
            isWater = mapItem.isWater;
            objectIndex = mapItem.objectIndex;
            objectSet = mapItem.objectSet;
            castleName = mapItem.castleName;
            castleOwner = mapItem.castleOwner;
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
            if (Board.brushDrawStart && !Board.changedItems.Exists(item => item.Xcoordinate==Xcoordinate && item.Ycoordinate==Ycoordinate)) //is brush selected ?
            {
                changeTile();
            }
        }

        public override void onItemClick()
        {
            if (!Board._brushIsClicked)
            {
                Board.changedItems.Clear();
                changeTile();
            }

            if (Board._brushIsClicked && Board.brushDrawStart == false &&Board.selectedSetIndex<8) //decide if brush drawing starts or ends
            {
                Board.brushDrawStart = true;
                Board.changedItems.Clear();
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
                Board.changedItems.Add(saveInstance());
                if (Board.selectedItemIndex != -1 && Board.selectedSetIndex != -1)
                {
                    if (_sprites[(int)Board.selectedSetIndex].category == "Terrain")
                    {
                        itemIndex = (int)Board.selectedItemIndex;
                        setIndex = (int)Board.selectedSetIndex;
                        objectIndex = null;
                        objectSet = null;
                        castleName = null;
                        castleOwner = null;

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
                    if (Board.selectedSetIndex == 8)
                    {
                        var dialog = new CastleDialog(this);
                        dialog.showDialog();
                    }
                }
                else
                {
                    clearTile();
                }
                miniMap.refresh(Xcoordinate, Ycoordinate);
            }
        }

        public void clearTile()
        {
            objectIndex = null;
            objectSet = null;
            castleName = null;
            castleOwner = null;

            image = _sprites[setIndex].imagesList[itemIndex];
            bitmap = _sprites[setIndex].bitmapList[itemIndex];
        }
    }
}
