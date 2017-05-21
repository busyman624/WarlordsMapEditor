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
        
        private int _boardRows=10;
        private int _boardColumns=10;
        private ObservableCollection<MapItem> _boardItems = new ObservableCollection<MapItem>();
        private Map map;
        private List<Sprite> _sprites = new List<Sprite>();
        private BrushCategories _brushCategories;
        FileMapProvider mapProvider;

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
            _sprites.Add(new Sprite(Resources.forest, "Forest", 0, "Terrain"));
            _sprites.Add(new Sprite(Resources.grass, "Grass", 1, "Terrain"));
            _sprites.Add(new Sprite(Resources.hills, "Hills", 2, "Terrain"));
            _sprites.Add(new Sprite(Resources.mountains, "Mountains", 3, "Terrain"));
            _sprites.Add(new Sprite(Resources.swamp, "Swamp", 4, "Terrain"));
            _sprites.Add(new Sprite(Resources.water, "Water", 5, "Terrain"));
            _sprites.Add(new Sprite(Resources.roads, "Roads", 6, "Road"));
            _sprites.Add(new Sprite(Resources.bridges, "Bridges", 7, "Road"));

            brushCategories = new BrushCategories(_sprites);

            mapProvider = new FileMapProvider();
            map=mapProvider.LoadMapFromBytes(_sprites, @"C:\Users\Maciej\Desktop\Warlords\grupowy_21\src\Warlords\Assets\Resources\Maps\duel.bytes");
            refresh();
        }

        public void MapLoad()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".byte";
            dlg.Filter = "(*.bytes)|*.bytes";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog(); 
            if (result == true)
            {
                string filename = dlg.FileName;
                map = mapProvider.LoadMapFromBytes(_sprites, filename);
                refresh();
            }
        }

        public void MapSave()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Map"; // Default file name
            dlg.DefaultExt = ".bytes"; // Default file extension
            dlg.Filter = "(.bytes)|*.bytes"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                mapProvider.SaveMapToFile(filename, map);
            }
        }

        public void refresh()
        {
            _boardItems.Clear();
            for (int r = 0; r < _boardRows; r++)
            {
                for (int c = 0; c < _boardColumns; c++)
                {
                    _boardItems.Add(map.tiles[c + r * map.columns]);
                }
            }
        }

        public bool CanMapLoad() { return true; }
        public virtual bool CanMapSave() { return true; }

        //Map Navigation
        public void NavigateLeft()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate - 1 + boardItemList[c + r * columns].Ycoordinate * map.columns];
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
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + 1 + boardItemList[c + r * columns].Ycoordinate * map.columns];
                }
            }

        }
        public bool CanNavigateRight() { return boardItemList[columns-1].Xcoordinate<map.columns-1; }

        public void NavigateUp()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + (boardItemList[c + r * columns].Ycoordinate-1) * map.columns];
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
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + (boardItemList[c + r * columns].Ycoordinate + 1) * map.columns];
                }
            }
        }
        public bool CanNavigateDown() { return boardItemList[columns*rows - 1].Ycoordinate < map.rows - 1; ; }

        public void ZoomIn()
        {
            rows--;
            columns--;
            int Xcoordinate = _boardItems[0].Xcoordinate;
            int Ycoordinate = _boardItems[0].Ycoordinate;
            _boardItems.Clear();
            for (int r = Ycoordinate; r < rows + Ycoordinate; r++)
            {
                for (int c = Xcoordinate; c < columns + Xcoordinate; c++)
                {
                    _boardItems.Add(map.tiles[c + r * map.columns]);
                }
            }
        }
        public bool CanZoomIn() { return rows>5; }

        public void ZoomOut()
        {
            rows++;
            columns++;
            int Xcoordinate = _boardItems[0].Xcoordinate;
            int Ycoordinate = _boardItems[0].Ycoordinate;
            _boardItems.Clear();
            for (int r = Ycoordinate; r < rows + Ycoordinate; r++)
            {
                for (int c = Xcoordinate; c < columns+ Xcoordinate; c++)
                {
                    _boardItems.Add(map.tiles[c + r * map.columns]);
                }
            }

        }
        public bool CanZoomOut() { return rows<20 && _boardItems[columns * rows - 1].Xcoordinate<map.columns-1 && _boardItems[columns * rows - 1].Ycoordinate<map.rows-1; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChaged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private ICommand _mapLoad;
        private ICommand _mapSave;

        private ICommand _mapNavigateLeft;
        private ICommand _mapNavigateRight;
        private ICommand _mapNavigateUp;
        private ICommand _mapNavigateDown;

        private ICommand _mapZoomIn;
        private ICommand _mapZoomOut;

        public ICommand mapLoad
        {
            get
            {
                if (_mapLoad == null)
                {
                    _mapLoad = new RelayCommand(
                        param => this.MapLoad(),
                        param => this.CanMapLoad()
                    );
                }
                return _mapLoad;
            }
        }

        public ICommand mapSave
        {
            get
            {
                if (_mapSave == null)
                {
                    _mapSave = new RelayCommand(
                        param => this.MapSave(),
                        param => this.CanMapSave()
                    );
                }
                return _mapSave;
            }
        }

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
