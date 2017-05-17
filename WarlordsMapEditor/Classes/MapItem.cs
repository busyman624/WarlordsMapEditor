using System;
using System.Collections.Generic;

namespace WarlordsMapEditor
{
    public class MapItem : Item
    {
        private List<Sprite> _sprites;
        public int Xcoordinate;
        public int Ycoordinate;
        public MapItem(int itemIndex, int setIndex, List<Sprite> _sprites, int Xcoordinate, int Ycoordinate)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this._sprites = _sprites;
            this.Xcoordinate = Xcoordinate;
            this.Ycoordinate = Ycoordinate;
            image = _sprites[setIndex].imagesList[itemIndex];
        }

        public override void onItemClick()
        {
            if(Board.selectedItemIndex!=null && Board.selectedSetIndex != null)
            {
                itemIndex = (int)Board.selectedItemIndex;
                setIndex = (int)Board.selectedSetIndex;
                image = _sprites[setIndex].imagesList[itemIndex];
            }
        }
    }
}
