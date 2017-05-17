using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarlordsMapEditor.ItemsList;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Board
    {
        public static int? selectedItemIndex=null;
        public static int? selectedSetIndex=null;

        int _rows;
        int _columns;
        ObservableCollection<MapItem> _mapItems = new ObservableCollection<MapItem>();
        List<Sprite> _sprites = new List<Sprite>();
        ObservableCollection<Carousel> _carouselList = new ObservableCollection<Carousel>();

        public Board(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
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

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _mapItems.Add(new MapItem(r, (int)c/2, _sprites)); //filling map with some data
                }
            }
        }

        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        public int Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public ObservableCollection<MapItem> mapItemList
        {
            get { return _mapItems; }
            set { _mapItems = value; }
        }

        public ObservableCollection<Carousel> Carousels
        {
            get { return _carouselList; }
            set { _carouselList = value; }
        }
    }
}
