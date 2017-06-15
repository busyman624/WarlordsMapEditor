using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class Brush : Item
    {
        public SelectedBrush selectedBrush;
        public Brush(int itemIndex, int setIndex, string setName, string category, Bitmap bitmap, BitmapImage image, SelectedBrush selectedBrush)
        {
            this.itemIndex = itemIndex;
            this.setIndex = setIndex;
            this.setName = setName;
            this.category = category;
            this.image = image;
            this.bitmap = bitmap;
            this.selectedBrush = selectedBrush;
        }

        public override void onItemClick()
        {
            selectedBrush.change(this);
        }
    }
}
