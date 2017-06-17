using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class BrushCategories
    {
        public Map map;
        public MapObjects mapObjects;
        public Configs configs;

        private ObservableCollection<Carousel> _terrainCarousels;
        private ObservableCollection<Carousel> _roadCarousels;
        private ObservableCollection<Carousel> _buildingCarousels;

        private List<BitmapImage> _brushCategoryImages;
        public List<BitmapImage> brushCategoryImages
        {
            get { return _brushCategoryImages; }
            set { _brushCategoryImages = value; }
        }

        private ObservableCollection<Carousel> _visibleCarousels;
        public ObservableCollection<Carousel> visibleCarousels
        {
            get { return _visibleCarousels; }
            set { _visibleCarousels = value; }
        }

        private SelectedBrush _selectedBrush;
        public SelectedBrush selectedBrush
        {
            get { return _selectedBrush; }
            set { _selectedBrush = value; }
        }


        public BrushCategories(Map map, MapObjects mapObjects, Configs configs)
        {
            _brushCategoryImages = new List<BitmapImage>();
            _terrainCarousels  = new ObservableCollection<Carousel>();
            _roadCarousels = new ObservableCollection<Carousel>();
            _buildingCarousels = new ObservableCollection<Carousel>();

            _visibleCarousels = new ObservableCollection<Carousel>();
            selectedBrush = new SelectedBrush(mapObjects, configs);

            this.map = map;
            this.mapObjects = mapObjects;
            this.configs = configs;

            updateCategories(map);
        }

        public void updateCategories(Map map)
        {
            this.map = map;
            _brushCategoryImages.Clear();
            _terrainCarousels.Clear();
            _roadCarousels.Clear();
            _buildingCarousels.Clear();
            _visibleCarousels.Clear();

            foreach(Sprite sprite in mapObjects.terrains)
            {
                _terrainCarousels.Add(new Carousel(sprite, sprite.imagesList.Count, selectedBrush));
            }

            foreach(Sprite sprite in mapObjects.roads)
            {
                _roadCarousels.Add(new Carousel(sprite, sprite.imagesList.Count, selectedBrush));
            }

                _buildingCarousels.Add(new Carousel(mapObjects.castles, configs.fractions.Count, selectedBrush));

            foreach (RuinsData ruin in configs.ruinsData)
            {
                _buildingCarousels.Add(new Carousel(mapObjects.ruins.Find(s => s.setName.ToLower() == ruin.name.ToLower()), ruin.sprites.Count, selectedBrush));
            }


            _brushCategoryImages.Add(_terrainCarousels[0].brushList[0].image);
            _brushCategoryImages.Add(_roadCarousels[0].brushList[0].image);
            _brushCategoryImages.Add(_buildingCarousels[0].brushList[0].image);

            if (map != null) map.UpdatePallete(mapObjects);
        }

        public void terrainCategoryClick()
        {
            _visibleCarousels.Clear();
            foreach (Carousel carousel in _terrainCarousels)
                _visibleCarousels.Add(carousel);
        }
        public void roadCategoryClick()
        {
            _visibleCarousels.Clear();
            foreach (Carousel carousel in _roadCarousels)
                _visibleCarousels.Add(carousel);
        }
        public void buildingCategoryClick()
        {
            _visibleCarousels.Clear();
            foreach (Carousel carousel in _buildingCarousels)
                _visibleCarousels.Add(carousel);
        }

        public void addNewBrushesClick()
        {
            var dialog = new AddNewBrushes(mapObjects, configs);
            dialog.showDialog();
            updateCategories(map);
        }

        private ICommand _terrainCategory;
        private ICommand _roadCategory;
        private ICommand _buildingCategory;
        private ICommand _addNewBrushes;

        public ICommand terrainCategory
        {
            get
            {
                if (_terrainCategory == null)
                {
                    _terrainCategory = new RelayCommand(
                        param => terrainCategoryClick(),
                        param => true
                    );
                }
                return _terrainCategory;
            }
        }

        public ICommand roadCategory
        {
            get
            {
                if (_roadCategory == null)
                {
                    _roadCategory = new RelayCommand(
                        param => roadCategoryClick(),
                        param => true
                    );
                }
                return _roadCategory;
            }
        }

        public ICommand buildingCategory
        {
            get
            {
                if (_buildingCategory == null)
                {
                    _buildingCategory = new RelayCommand(
                        param => buildingCategoryClick(),
                        param => true
                    );
                }
                return _buildingCategory;
            }
        }

        public ICommand addNewBrushes
        {
            get
            {
                if (_addNewBrushes == null)
                {
                    _addNewBrushes = new RelayCommand(
                        param => addNewBrushesClick(),
                        param => true
                    );
                }
                return _addNewBrushes;
            }
        }

    }
}
