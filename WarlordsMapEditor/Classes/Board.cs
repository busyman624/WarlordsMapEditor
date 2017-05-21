using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Board : INotifyPropertyChanged
    {
        public static int? selectedItemIndex=null;
        public static int? selectedSetIndex=null;


        private int _boardRows;
        private int _boardColumns;
        private int mapRows;
        private int mapColumns;
        private Map map;
        private List<Sprite> _sprites = new List<Sprite>();
        private ObservableCollection<MapItem> _boardItems = new ObservableCollection<MapItem>();
        private BrushCategories _brushCategories;

        public int rows
        {
            get { return _boardRows; }
            set
            {
                if (_boardRows != value)
                {

                    _boardRows = value;
                    RaisePropertyChaged("rows");
                }
            }
        }

        public int columns
        {
            get { return _boardColumns; }
            set
            {
                if (_boardColumns != value)
                {

                    _boardColumns = value;
                    RaisePropertyChaged("columns");
                }
            }
        }

        public ObservableCollection<MapItem> boardItemList
        {
            get { return _boardItems; }
            set { _boardItems = value; }
        }

        public BrushCategories brushCategories
        {
            get { return _brushCategories; }
            set { _brushCategories = value; }
        }

        public Board() 
        {
            _boardRows = 20;
            _boardColumns = 20;
            mapRows = 1000;
            mapColumns = 1000;
            Random random = new Random();


            _sprites.Add(new Sprite(Resources.forest, "Forest", 0, "Terrain"));
            _sprites.Add(new Sprite(Resources.grass, "Grass", 1, "Terrain"));
            _sprites.Add(new Sprite(Resources.hills, "Hills", 2, "Terrain"));
            _sprites.Add(new Sprite(Resources.mountains, "Mountains", 3, "Terrain"));
            _sprites.Add(new Sprite(Resources.swamp, "Swamp", 4, "Terrain"));
            _sprites.Add(new Sprite(Resources.water, "Water", 5, "Terrain"));
            _sprites.Add(new Sprite(Resources.roads, "Roads", 6, "Road"));
            _sprites.Add(new Sprite(Resources.bridges, "Bridges", 7, "Road"));

            brushCategories = new BrushCategories(_sprites);

            FileMapProvider mapProvider = new FileMapProvider();
            map=mapProvider.LoadMapFromBytes(_sprites, @"C:\Users\krysz\Repos\K\Warlors\src\Warlords\Assets\Resources\Maps\duel.bytes");
            mapRows = map.rows;
            mapColumns = map.columns;

            for (int r = 0; r < _boardRows; r++)
            {
                for (int c = 0; c < _boardColumns; c++)
                {
                    _boardItems.Add(map.tiles[c+r*mapColumns]);
                }
            }
        }

        //Map Navigation
        public void NavigateLeft()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate - 1 + boardItemList[c + r * columns].Ycoordinate * mapColumns];
                }
            }

        }
        public bool CanNavigateLeft() { return boardItemList[0].Xcoordinate>0; }

        public void NavigateRight()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + 1 + boardItemList[c + r * columns].Ycoordinate * mapColumns];
                }
            }

        }
        public bool CanNavigateRight() { return boardItemList[columns-1].Xcoordinate<mapColumns-1; }

        public void NavigateUp()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + (boardItemList[c + r * columns].Ycoordinate-1) * mapColumns];
                }
            }
        }
        public bool CanNavigateUp() { return boardItemList[0].Ycoordinate > 0; }

        public void NavigateDown()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + (boardItemList[c + r * columns].Ycoordinate + 1) * mapColumns];
                }
            }
        }
        public bool CanNavigateDown() { return boardItemList[columns*rows - 1].Ycoordinate < mapRows - 1; ; }

        public void ZoomIn()
        {
            _boardRows--;
            _boardColumns--;
            _boardItems.Clear();
            for (int r = 0; r < _boardRows; r++)
            {
                for (int c = 0; c < _boardColumns; c++)
                {
                    _boardItems.Add(map.tiles[c + r * mapColumns]);
                }
            }
        }
        public bool CanZoomIn() { return true; }

        public void ZoomOut()
        {
            _boardRows++;
            _boardColumns++;
            _boardItems.Clear();
            for (int r = 0; r < _boardRows; r++)
            {
                for (int c = 0; c < _boardColumns; c++)
                {
                    _boardItems.Add(map.tiles[c + r * mapColumns]);
                }
            }

        }
        public bool CanZoomOut() { return true; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChaged(string info)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
        }



        private ICommand _mapNavigateLeft;
        private ICommand _mapNavigateRight;
        private ICommand _mapNavigateUp;
        private ICommand _mapNavigateDown;

        private ICommand _mapZoomIn;
        private ICommand _mapZoomOut;

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

        public ICommand MapZoomIn
        {
            get
            {
                if (_mapZoomIn == null)
                {
                    _mapZoomIn = new RelayCommand(
                        param => this.ZoomIn(),
                        param => this.CanZoomIn()
                    );
                }
                return _mapZoomIn;
            }
        }

        public ICommand MapZoomOut
        {
            get
            {
                if (_mapZoomOut == null)
                {
                    _mapZoomOut = new RelayCommand(
                        param => this.ZoomOut(),
                        param => this.CanZoomOut()
                    );
                }
                return _mapZoomOut;
            }
        }

    }
}
