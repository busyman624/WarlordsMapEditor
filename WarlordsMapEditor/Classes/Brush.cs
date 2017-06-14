using System;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class Brush : Item
    {
        public SelectedBrush selectedBrush;
        public Brush(int itemIndex, int setIndex, BitmapImage image, SelectedBrush selectedBrush)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this.image = image;
            this.selectedBrush = selectedBrush;
        }

        public override void onItemClick()
        {
            Board.selectedItemIndex = itemIndex;
            Board.selectedSetIndex = setIndex;
            selectedBrush.update();
        }
    }
}
