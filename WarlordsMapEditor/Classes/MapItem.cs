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
        public MapObjects mapObjects;
        public SelectedBrush selectedBrush;
        public List<MapItem> changedItems;
        public int Xcoordinate;
        public int Ycoordinate;
        public bool isWater;
        public int? objectIndex;
        public int? objectSet;
        public string objectCategory;
        public MiniMap miniMap;

        public string castleName;
        public int? castleOwner;

        public MapItem(int itemIndex, int setIndex, string setName, string category, MapObjects mapObjects, int Xcoordinate, int Ycoordinate,
            MiniMap miniMap, SelectedBrush selectedBrush, List<MapItem> changedItems)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this.setName = setName;
            this.category = category;
            this.miniMap = miniMap;
            objectIndex = null;
            objectSet = null;
            objectCategory = null;
            castleName = null;
            castleOwner = null;
            this.mapObjects = mapObjects;
            this.selectedBrush = selectedBrush;
            this.changedItems = changedItems;
            this.Xcoordinate = Xcoordinate;
            this.Ycoordinate = Ycoordinate;
            if (setName == "Water") isWater = true;
            else isWater = false;
            image = mapObjects.terrains[setIndex].imagesList[itemIndex];
            bitmap = mapObjects.terrains[setIndex].bitmapList[itemIndex];
        }

        public MapItem() { }

        public MapItem saveInstance()
        {
            MapItem mapItem = new MapItem();
            mapItem.itemIndex = itemIndex;
            mapItem.setIndex = setIndex;
            mapItem.setName = setName;
            mapItem.category = category;
            mapItem.Xcoordinate = Xcoordinate;
            mapItem.Ycoordinate = Ycoordinate;
            mapItem.bitmap = bitmap;
            mapItem.image = image;
            mapItem.isWater = isWater;
            mapItem.objectIndex = objectIndex;
            mapItem.objectSet = objectSet;
            mapItem.objectCategory = objectCategory;
            mapItem.castleName = castleName;
            mapItem.castleOwner = castleOwner;
            return mapItem;
        }

        public void restore(MapItem mapItem)
        {
            itemIndex = mapItem.itemIndex;
            setIndex = mapItem.setIndex;
            setName = mapItem.setName;
            category = mapItem.category;
            Xcoordinate = mapItem.Xcoordinate;
            Ycoordinate = mapItem.Ycoordinate;
            bitmap = mapItem.bitmap;
            image = mapItem.image;
            isWater = mapItem.isWater;
            objectIndex = mapItem.objectIndex;
            objectSet = mapItem.objectSet;
            objectCategory = mapItem.objectCategory;
            castleName = mapItem.castleName;
            castleOwner = mapItem.castleOwner;
        }

        public void combineImages()
        {
            Bitmap topbmp=null;
            switch (objectCategory)
            {
                case "Roads":
                    {
                        topbmp = mapObjects.roads[(int)objectSet].bitmapList[(int)objectIndex];
                        break;
                    }
                case "Castles":
                    {
                        topbmp = mapObjects.castles.bitmapList[(int)objectIndex];
                        break;
                    }
                case "Ruins":
                    {
                        topbmp = mapObjects.ruins[(int)objectSet].bitmapList[(int)objectIndex];
                        break;
                    }
            }
            
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
            if (Board.brushDrawStart && !changedItems.Exists(item => item.Xcoordinate==Xcoordinate && item.Ycoordinate==Ycoordinate)) //is brush selected ?
            {
                changeTile();
            }
        }

        public override void onItemClick()
        {
            if (!Board._brushIsClicked)
            {
                changedItems.Clear();
                changeTile();
            }

            if (Board._brushIsClicked && Board.brushDrawStart == false && (selectedBrush.category == "Terrains" || selectedBrush.category == "Roads" || selectedBrush.category == "Delete")) //decide if brush drawing starts or ends
            {
                Board.brushDrawStart = true;
                changedItems.Clear();
            }
            else
            {
                Board.brushDrawStart = false;
            }
        }

        private void changeTile()
        {
                changedItems.Add(saveInstance());
                if (selectedBrush.setIndex != -1 && selectedBrush.itemIndex != -1)
                {
                    if (selectedBrush.category == "Terrains")
                    {
                        itemIndex = selectedBrush.itemIndex;
                        setIndex = selectedBrush.setIndex;
                        setName = selectedBrush.setName;
                        category= selectedBrush.category;
                        objectIndex = null;
                        objectSet = null;
                        castleName = null;
                        castleOwner = null;

                        image = selectedBrush.image;
                        bitmap = selectedBrush.bitmap;
                        if (selectedBrush.setName == "Water") isWater = true;
                        else isWater = false;
                    }
                    else if (!isWater || selectedBrush.setName == "Bridges")
                    {
                        objectCategory = selectedBrush.category;
                        objectIndex = selectedBrush.itemIndex;
                        objectSet = selectedBrush.setIndex;
                        combineImages();
                    }
                    if (selectedBrush.category == "Castles")
                    {
                        objectCategory = selectedBrush.category;
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

        public void clearTile()
        {
            objectIndex = null;
            objectSet = null;
            castleName = null;
            castleOwner = null;

            image = mapObjects.terrains[setIndex].imagesList[itemIndex];
            bitmap = mapObjects.terrains[setIndex].bitmapList[itemIndex];
        }
    }
}
