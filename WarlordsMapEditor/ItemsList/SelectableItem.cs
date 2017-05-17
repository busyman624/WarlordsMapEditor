using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor.ItemsList
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
            Console.WriteLine("Hi I'm Item no " + itemIndex.ToString() + " of set " + setIndex.ToString());
            Board.selectedItemIndex = itemIndex;
            Board.selectedSetIndex = setIndex;
        }
    }
}
