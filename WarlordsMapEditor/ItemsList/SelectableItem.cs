using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor.ItemsList
{
    public class SelectableItem
    {
        public BitmapImage image { get; set; }
        public int index;
        private int setIndex;
        public SelectableItem(int index, int setIndex, BitmapImage image)
        {
            this.index = index;
            this.setIndex = setIndex;
            this.image = image;
        }

        public void onClick()
        {
            Console.WriteLine("Hi I'm Item no "+index.ToString()+" of set "+setIndex.ToString());
        }
        public bool CanPerform() { return true; }

        private ICommand _selectableItemClick;

        public ICommand SelectableItemClick
        {
            get
            {
                if (_selectableItemClick == null)
                {
                    _selectableItemClick = new RelayCommand(
                        param => this.onClick(),
                        param => this.CanPerform()
                    );
                }
                return _selectableItemClick;
            }
        }
    }
}
