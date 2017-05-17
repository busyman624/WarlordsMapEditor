﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor.ItemsList
{
    public class Carousel
    {
        private Sprite itemSet;
        private ObservableCollection<SelectableItem> _selectableItems;
        public ObservableCollection<SelectableItem> selectableItemList
        {
            get { return _selectableItems; }
            set { _selectableItems = value; }
        }

        public Carousel(Sprite itemSet)
        {
            _selectableItems = new ObservableCollection<SelectableItem>();
            for(int i = 0; i < 3; i++)
            {
                _selectableItems.Add(new SelectableItem(i, itemSet.setIndex, itemSet.imagesList[i]));
            }
            this.itemSet = itemSet;

        }

        public void SelectableItemsGoLeft()
        {
            Console.WriteLine("Hi I'm left arrow of set " + itemSet.setIndex.ToString());
            _selectableItems[2] = _selectableItems[1];
            _selectableItems[1] = _selectableItems[0];
            _selectableItems[0] = new SelectableItem(_selectableItems[1].itemIndex - 1, itemSet.setIndex, itemSet.imagesList[_selectableItems[1].itemIndex - 1]);
        }
        public bool CanSelectableItemsGoLeft()
        {
            return _selectableItems[0].itemIndex != 0;
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
            Console.WriteLine("Hi I'm right arrow of set " + itemSet.setIndex.ToString());
            _selectableItems[0] = _selectableItems[1];
            _selectableItems[1] = _selectableItems[2];
            _selectableItems[2] = new SelectableItem(_selectableItems[1].itemIndex + 1, itemSet.setIndex, itemSet.imagesList[_selectableItems[1].itemIndex + 1]);
        }
        public bool CanSelectableItemsGoRight()
        {
            return _selectableItems[2].itemIndex != itemSet.imagesList.Count()-1;
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
