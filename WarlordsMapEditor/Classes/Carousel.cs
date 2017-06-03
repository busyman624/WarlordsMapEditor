using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WarlordsMapEditor
{
    public class Carousel
    {
        private Sprite itemSet;
        private string _setName;
        public string setName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        private ObservableCollection<Item> _brushList;
        public ObservableCollection<Item> brushList
        {
            get { return _brushList; }
            set { _brushList = value; }
        }

        public Carousel(Sprite itemSet)
        {
            _brushList = new ObservableCollection<Item>();
            for(int i = 0; i < 3 && itemSet.imagesList.Count-i>0; i++)
            {
                _brushList.Add(new Brush(i, itemSet.setIndex, itemSet.imagesList[i]));
            }
            this.itemSet = itemSet;
            this._setName = itemSet.setName;

        }

        public void SelectableItemsGoLeft()
        {
            _brushList[2] = _brushList[1];
            _brushList[1] = _brushList[0];
            _brushList[0] = new Brush(_brushList[1].itemIndex - 1, itemSet.setIndex, itemSet.imagesList[_brushList[1].itemIndex - 1]);
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
            _brushList[2] = new Brush(_brushList[1].itemIndex + 1, itemSet.setIndex, itemSet.imagesList[_brushList[1].itemIndex + 1]);
        }
        public bool CanSelectableItemsGoRight()
        {
            if (_brushList.Count == 3)
                return _brushList[2].itemIndex != itemSet.imagesList.Count() - 1;
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
