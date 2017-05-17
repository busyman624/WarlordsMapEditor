using System;
using System.Collections.Generic;

namespace WarlordsMapEditor
{
    public class MapItem : Item
    {
        private List<Sprite> _sprites;
        public MapItem(int itemIndex, int setIndex, List<Sprite> _sprites)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this._sprites = _sprites;
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
