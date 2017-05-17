using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Board
    {
        public static int? selectedItemIndex=null;
        public static int? selectedSetIndex=null;

        private int _rows;
        private int _columns;
        private List<Sprite> _sprites = new List<Sprite>();
        private List<MapItem> _mapItems = new List<MapItem>();
        private ObservableCollection<MapItem> _boardItems = new ObservableCollection<MapItem>();
        private ObservableCollection<Carousel> _carouselList = new ObservableCollection<Carousel>();

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

            for (int r = 0; r < 2 * rows; r++)
            {
                for (int c = 0; c < 2 * columns; c++)
                {
                    int randomItemSet = random.Next(_sprites.Count);
                    _mapItems.Add(new MapItem(random.Next(_sprites[randomItemSet].imagesList.Count) , randomItemSet , _sprites)); //filling map with some data
                }
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _boardItems.Add(_mapItems[c+r*(columns+1)]);
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
    }
}
