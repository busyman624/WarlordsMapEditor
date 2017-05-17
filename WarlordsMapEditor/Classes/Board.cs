﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Board
    {
        public static int? selectedItemIndex=null;
        public static int? selectedSetIndex=null;

        

        private int _boardRows;
        private int _boardColumns;
        private int mapRows;
        private int mapColumns;
        private List<Sprite> _sprites = new List<Sprite>();
        private List<MapItem> _mapItems = new List<MapItem>();
        private ObservableCollection<MapItem> _boardItems = new ObservableCollection<MapItem>();
        private ObservableCollection<Carousel> _carouselList = new ObservableCollection<Carousel>();

        public Board(int rows, int columns)
        {
            _boardRows = rows;
            _boardColumns = columns;
            mapRows = 2 * Rows;
            mapColumns = 2 * Columns;
            Random random = new Random();


            _sprites.Add(new Sprite(Resources.forest, "Forest", 0));
            _sprites.Add(new Sprite(Resources.grass, "Grass", 1));
            _sprites.Add(new Sprite(Resources.hills, "Hills", 2));
            _sprites.Add(new Sprite(Resources.mountains, "Mountains", 3));
            _sprites.Add(new Sprite(Resources.swamp, "Swamp", 4));
            _sprites.Add(new Sprite(Resources.water, "Water", 5));

            foreach (Sprite sprite in _sprites)
            {
                _carouselList.Add(new Carousel(sprite));
            }

            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapColumns; c++)
                {
                    int randomItemSet = random.Next(_sprites.Count);
                    _mapItems.Add(new MapItem(random.Next(_sprites[randomItemSet].imagesList.Count) , randomItemSet , _sprites, c, r)); //filling map with some data
                }
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _boardItems.Add(_mapItems[c+r*mapColumns]);
                }
            }
        }

        public int Rows
        {
            get { return _boardRows; }
            set { _boardRows = value; }
        }

        public int Columns
        {
            get { return _boardColumns; }
            set { _boardColumns = value; }
        }

        public ObservableCollection<MapItem> boardItemList
        {
            get { return _boardItems; }
            set { _boardItems = value; }
        }

        public ObservableCollection<Carousel> Carousels
        {
            get { return _carouselList; }
            set { _carouselList = value; }
        }


        //Map Navigation
        public void NavigateLeft()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    boardItemList[c + r * Columns] = _mapItems[boardItemList[c + r * Columns].Xcoordinate - 1 + boardItemList[c + r * Columns].Ycoordinate * mapColumns];
                }
            }

        }
        public bool CanNavigateLeft() { return boardItemList[0].Xcoordinate>0; }

        public void NavigateRight()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    boardItemList[c + r * Columns] = _mapItems[boardItemList[c + r * Columns].Xcoordinate + 1 + boardItemList[c + r * Columns].Ycoordinate * mapColumns];
                }
            }

        }
        public bool CanNavigateRight() { return boardItemList[Columns-1].Xcoordinate<mapColumns-1; }

        public void NavigateUp()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    boardItemList[c + r * Columns] = _mapItems[boardItemList[c + r * Columns].Xcoordinate + (boardItemList[c + r * Columns].Ycoordinate-1) * mapColumns];
                }
            }
        }
        public bool CanNavigateUp() { return boardItemList[0].Ycoordinate > 0; }

        public void NavigateDown()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    boardItemList[c + r * Columns] = _mapItems[boardItemList[c + r * Columns].Xcoordinate + (boardItemList[c + r * Columns].Ycoordinate + 1) * mapColumns];
                }
            }
        }
        public bool CanNavigateDown() { return boardItemList[Columns*Rows - 1].Ycoordinate < mapRows - 1; ; }



        private ICommand _mapNavigateLeft;
        private ICommand _mapNavigateRight;
        private ICommand _mapNavigateUp;
        private ICommand _mapNavigateDown;

        public ICommand MapNavigateLeft
        {
            get
            {
                if (_mapNavigateLeft == null)
                {
                    _mapNavigateLeft = new RelayCommand(
                        param => this.NavigateLeft(),
                        param => this.CanNavigateLeft()
                    );
                }
                return _mapNavigateLeft;
            }
        }

        public ICommand MapNavigateRight
        {
            get
            {
                if (_mapNavigateRight == null)
                {
                    _mapNavigateRight = new RelayCommand(
                        param => this.NavigateRight(),
                        param => this.CanNavigateRight()
                    );
                }
                return _mapNavigateRight;
            }
        }

        public ICommand MapNavigateUp
        {
            get
            {
                if (_mapNavigateUp == null)
                {
                    _mapNavigateUp = new RelayCommand(
                        param => this.NavigateUp(),
                        param => this.CanNavigateUp()
                    );
                }
                return _mapNavigateUp;
            }
        }

        public ICommand MapNavigateDown
        {
            get
            {
                if (_mapNavigateDown == null)
                {
                    _mapNavigateDown = new RelayCommand(
                        param => this.NavigateDown(),
                        param => this.CanNavigateDown()
                    );
                }
                return _mapNavigateDown;
            }
        }

    }
}