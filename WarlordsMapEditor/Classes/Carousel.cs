using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class Carousel
    {
        private Sprite itemSet;
        private string _setName;
        private int itemCount;
        public string setName
        {
            get { return _setName; }
            set { _setName = value; }
        }

        public SelectedBrush selectedBrush;

        private ObservableCollection<Item> _brushList;
        public ObservableCollection<Item> brushList
        {
            get { return _brushList; }
            set { _brushList = value; }
        }

        public Carousel(Sprite itemSet, int itemCount, SelectedBrush selectedBrush)
        {
            this.itemCount = itemCount;
            _brushList = new ObservableCollection<Item>();
            
            for (int i = 0; i < 3 && itemCount-i>0; i++)
            {
                _brushList.Add(new Brush(i, itemSet.setIndex, itemSet.setName, itemSet.category, itemSet.bitmapList[i], itemSet.imagesList[i], selectedBrush));
            }
            this.itemSet = itemSet;
            this._setName = itemSet.setName;
            this.selectedBrush = selectedBrush;

        }

        public void SelectableItemsGoLeft()
        {
            _brushList[2] = _brushList[1];
            _brushList[1] = _brushList[0];
            _brushList[0] = new Brush(_brushList[1].itemIndex - 1, itemSet.setIndex, itemSet.setName, itemSet.category, itemSet.bitmapList[_brushList[1].itemIndex - 1], itemSet.imagesList[_brushList[1].itemIndex - 1], selectedBrush);
        }
        public bool CanSelectableItemsGoLeft()
        {
            if (_brushList.Count == 3)
                return _brushList[0].itemIndex != 0;
            else return false;
        }

        private ICommand _carouselLeftArrowClick;

        public ICommand CarouselLeftArrowClick
        {
            get
            {
                if (_carouselLeftArrowClick == null)
                {
                    _carouselLeftArrowClick = new RelayCommand(
                        param => this.SelectableItemsGoLeft(),
                        param => this.CanSelectableItemsGoLeft()
                    );
                }
                return _carouselLeftArrowClick;
            }
        }


        public void SelectableItemsGoRight()
        {
            _brushList[0] = _brushList[1];
            _brushList[1] = _brushList[2];
            _brushList[2] = new Brush(_brushList[1].itemIndex + 1, itemSet.setIndex, itemSet.setName, itemSet.category, itemSet.bitmapList[_brushList[1].itemIndex + 1], itemSet.imagesList[_brushList[1].itemIndex + 1], selectedBrush);
        }
        public bool CanSelectableItemsGoRight()
        {
            if (_brushList.Count == 3)
                return _brushList[2].itemIndex != itemCount - 1;
            else return false;
        }

        private ICommand _carouselRightArrowClick;

        public ICommand CarouselRightArrowClick
        {
            get
            {
                if (_carouselRightArrowClick == null)
                {
                    _carouselRightArrowClick = new RelayCommand(
                        param => this.SelectableItemsGoRight(),
                        param => this.CanSelectableItemsGoRight()
                    );
                }
                return _carouselRightArrowClick;
            }
        }
    }
}
