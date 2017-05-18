using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class BrushCategories
    {
        private ObservableCollection<Carousel> _terrainCarousels;
        private ObservableCollection<Carousel> _roadCarousels;

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


        public BrushCategories(List<Sprite> spriteList)
        {
            _brushCategoryImages = new List<BitmapImage>();
            _terrainCarousels  = new ObservableCollection<Carousel>();
            _roadCarousels = new ObservableCollection<Carousel>();

            _visibleCarousels = new ObservableCollection<Carousel>();

            foreach (Sprite sprite in spriteList)
            {
                switch (sprite.category)
                {
                    case "Terrain": _terrainCarousels.Add(new Carousel(sprite)); break;
                    case "Road": _roadCarousels.Add(new Carousel(sprite)); break;
                }
            }

            _brushCategoryImages.Add(_terrainCarousels[0].brushList[0].image);
            _brushCategoryImages.Add(_roadCarousels[0].brushList[0].image);
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

        private ICommand _terrainCategory;
        private ICommand _roadCategory;

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
    }
}
