using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Board : INotifyPropertyChanged
    {
        public List<MapItem> changedItems=new List<MapItem>();

        private Configs configs;
        private int _boardRows;
        private int _boardColumns;
        private string _mapName;
        private string _mapDescription;
        private ObservableCollection<MapItem> _boardItems = new ObservableCollection<MapItem>();
        public Map map;
        private MiniMap _miniMap;
        private MapObjects mapObjects;
        private BrushCategories _brushCategories;
        FileMapProvider mapProvider = new FileMapProvider();

        public string mapName
        {
            get { return _mapName; }
            set
            {
                if (_mapName != value)
                {

                    _mapName = value;
                    RaisePropertyChaged("mapName");
                }
            }
        }

        public string mapDescription
        {
            get { return _mapDescription; }
            set
            {
                if (_mapDescription != value)
                {

                    _mapDescription = value;
                    RaisePropertyChaged("mapDescription");
                }
            }
        }

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

        public MiniMap miniMap
        {
            get { return _miniMap; }
            set { _miniMap = value; }
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
            configs = new Configs();
            mapObjects = new MapObjects();
            columns = 10;
            rows = 10;

            brushCategories = new BrushCategories(map, mapObjects, configs);
            miniMap = new MiniMap();
            _brushIsClicked = false;
        }

        public void CreateNewMap()
        {
            map = new Map();
            map.name = "";

            var dialog = new NewMapDialog(map);
            dialog.showDialog();

            if (map.name != "")
            {
                changeConfigs();
                mapProvider.CreateNewMap(map, mapObjects, miniMap, brushCategories.selectedBrush, changedItems, configs);
                refresh();
            }
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
                changeConfigs();
                string filename = dlg.FileName;
                map = mapProvider.LoadMapFromBytes(mapObjects, filename, miniMap, brushCategories.selectedBrush, changedItems, configs);
                if (map != null) refresh();
            }
        }

        public void MapSave()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = mapName; // Default file name
            dlg.DefaultExt = ".bytes"; // Default file extension
            dlg.Filter = "(.bytes)|*.bytes"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                map.name = map.name.Substring(0, map.name.LastIndexOf('\\') + 1) + filename.Substring(filename.LastIndexOf('\\')+1).Split('.')[0];
                mapName = map.name.Split('\\')[map.name.Split('\\').Length - 1];
                mapProvider.SaveMapToFile(filename, map, configs);
            }
        }

        public void changeConfigs()
        {
            configs.SetFractionsConfig();
            configs.SetRuinsConfig();
        }

        public void refresh()
        {
            _boardItems.Clear();
            for (int r = 0; r < _boardRows; r++)
            {
                for (int c = 0; c < _boardColumns; c++)
                {
                    _boardItems.Add(map.tiles[c + map.columns*(map.rows-1-r)]);
                }
            }
            mapName = map.name.Split('\\')[map.name.Split('\\').Length - 1];
            mapDescription = map.description;
            brushCategories.updateCategories(map);
            changedItems.Clear();
            brushCategories.selectedBrush.clear();
            miniMap.calculate(map.tiles, map.columns, map.rows, columns, rows);
        }

        public void DeleteClick()
        {
            brushCategories.selectedBrush.setIndex = -1;
            brushCategories.selectedBrush.itemIndex = -1;
            brushCategories.selectedBrush.category = "Delete";
            brushCategories.selectedBrush.clear();
            brushCategories.selectedBrush.update();
        }

        public void BackClick()
        {
            foreach (MapItem item in changedItems)
            {
                map.tiles[item.Xcoordinate + map.columns * item.Ycoordinate].restore(item);
            }
            changedItems.Clear();
            miniMap.calculate(map.tiles, map.columns, map.rows, columns, rows);
        }

        public bool CanMapLoad() { return true; }
        public bool CanCreateNewMap() { return true; }
        public bool CanMapSave() { return map!=null; }

        public bool CanDeleteClick() { return map != null; }
        public bool CanBackClick() { return changedItems.Count>0; }

        //Map Navigation
        public void NavigateLeft()
        {
            miniMap.moveLeft();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate - 1 + boardItemList[c + r * columns].Ycoordinate * map.columns];
                }
            }

        }
        public bool CanNavigateLeft()
        {
            if (boardItemList.Count != 0) return boardItemList[0].Xcoordinate > 0;
            else return false;
        }

        public void NavigateRight()
        {
            miniMap.moveRight();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + 1 + boardItemList[c + r * columns].Ycoordinate * map.columns];
                }
            }

        }
        public bool CanNavigateRight()
        {
            if (boardItemList.Count != 0)  return boardItemList[columns-1].Xcoordinate<map.columns-1;
            else return false;
        }

        public void NavigateUp()
        {
            miniMap.moveUp();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + (boardItemList[c + r * columns].Ycoordinate + 1) * map.columns];
                }
            }
        }
        public bool CanNavigateUp()
        {
            if (boardItemList.Count != 0) return boardItemList[0].Ycoordinate < map.rows - 1;
            else return false;
        }

        public void NavigateDown()
        {
            miniMap.moveDown();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    boardItemList[c + r * columns] = map.tiles[boardItemList[c + r * columns].Xcoordinate + (boardItemList[c + r * columns].Ycoordinate - 1) * map.columns];
                }
            }
        }
        public bool CanNavigateDown()
        {
            if (boardItemList.Count != 0) return boardItemList[columns*rows - 1].Ycoordinate > 0;
            else return false;
        }

        public void ZoomIn()
        {
            rows--;
            columns--;
            miniMap.zoomIn(columns, rows);

            int Xcoordinate = _boardItems[0].Xcoordinate;
            int Ycoordinate = _boardItems[0].Ycoordinate;
            _boardItems.Clear();
            for (int r = map.rows - 1 - Ycoordinate; r < rows + map.rows - 1 - Ycoordinate; r++)
            {
                for (int c = Xcoordinate; c < columns + Xcoordinate; c++)
                {
                    _boardItems.Add(map.tiles[c + map.columns * (map.rows - 1 - r)]);
                }
            }
        }
        public bool CanZoomIn()
        {
            if (boardItemList.Count != 0) return rows >5;
            else return false;
        }

        public void ZoomOut()
        {
            rows++;
            columns++;
            miniMap.zoomOut(columns, rows);

            int Xcoordinate = _boardItems[0].Xcoordinate;
            int Ycoordinate = _boardItems[0].Ycoordinate;
            _boardItems.Clear();
            for (int r = map.rows - 1 - Ycoordinate; r < rows + map.rows - 1 - Ycoordinate; r++)
            {
                for (int c = Xcoordinate; c < columns + Xcoordinate; c++)
                {
                    _boardItems.Add(map.tiles[c + map.columns * (map.rows - 1 - r)]);
                }
            }
        }
        public bool CanZoomOut()
        {
            if (boardItemList.Count != 0) return rows <20 && _boardItems[columns * rows - 1].Xcoordinate<map.columns-1 && _boardItems[columns * rows - 1].Ycoordinate<map.rows-1;
            else return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChaged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public static bool brushDrawStart = false;
        public static bool _brushIsClicked;
        public bool BrushIsClicked
        {
            get
            {
                return _brushIsClicked;
            }
            set
            {
                brushDrawStart = false;
                _brushIsClicked = value;
                RaisePropertyChaged("BrushIsClicked");
            }
        }

        private ICommand _mapLoad;
        private ICommand _mapSave;
        private ICommand _newMap;

        private ICommand _delete;
        private ICommand _back;

        private ICommand _mapNavigateLeft;
        private ICommand _mapNavigateRight;
        private ICommand _mapNavigateUp;
        private ICommand _mapNavigateDown;

        private ICommand _mapZoomIn;
        private ICommand _mapZoomOut;

        public ICommand newMap
        {
            get
            {
                if (_newMap == null)
                {
                    _newMap = new RelayCommand(
                        param => this.CreateNewMap(),
                        param => this.CanCreateNewMap()
                    );
                }
                return _newMap;
            }
        }

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

        public ICommand delete
        {
            get
            {
                if (_delete == null)
                {
                    _delete = new RelayCommand(
                        param => this.DeleteClick(),
                        param => this.CanDeleteClick()
                    );
                }
                return _delete;
            }
        }

        public ICommand back
        {
            get
            {
                if (_back == null)
                {
                    _back = new RelayCommand(
                        param => this.BackClick(),
                        param => this.CanBackClick()
                    );
                }
                return _back;
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
