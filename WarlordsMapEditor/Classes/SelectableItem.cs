using System;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class SelectableItem : Item
    {
        public SelectableItem(int itemIndex, int setIndex, BitmapImage image)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this.image = image;
        }

        public override void onItemClick()
        {
            Board.selectedItemIndex = itemIndex;
            Board.selectedSetIndex = setIndex;
        }
    }
}
