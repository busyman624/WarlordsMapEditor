using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new Board(10, 10);
        }
    }

    public class Board
    {
        int _rows;
        int _columns;
        int _menuRows=2;
        int _menuColumns=1;
        List<Tile> _tiles = new List<Tile>();
        List<Sprite> _sprites = new List<Sprite>();

        public Board(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            Random random = new Random();

            _sprites.Add(new Sprite(Resources.forest, "Forest"));
            _sprites.Add(new Sprite(Resources.grass, "Grass"));
            _sprites.Add(new Sprite(Resources.hills, "Hills"));
            _sprites.Add(new Sprite(Resources.mountains, "Mountains"));
            _sprites.Add(new Sprite(Resources.swamp, "Swamp"));
            _sprites.Add(new Sprite(Resources.water, "Water"));

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Tile tile =new Tile(string.Format("Row {0}, Column {1}", r, c));
                    ImageBrush brush1 = new ImageBrush();
                    BitmapImage image = new BitmapImage(new Uri("C:\\Users\\krysz\\rico.gif"));
                    brush1.ImageSource = image;
                    tile.Background = brush1;

                    _tiles.Add(tile);
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

        public List<Tile> Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public int MenuRows
        {
            get { return _menuRows; }
            set { _menuRows = value; }
        }

        public int MenuColumns
        {
            get { return _menuColumns; }
            set { _menuColumns = value; }
        }

        public List<Sprite> Menu
        {
            get { return _sprites; }
            set { _sprites = value; }
        }
    }

    public class Tile : Button
    {
        public string Data { get; set; }
        public Tile(string Data)
        {
            this.Data = Data;
            // auto-register any "click" will call our own custom "click" handler
            // which will change the status...  This could also be done to simplify
            // by only changing visibility, but shows how you could apply via other
            // custom properties too.
            Click += MyCustomClick;
        }
        public SolidColorBrush MyBackground { get; set; }

        public void MyCustomClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Data);
        }

    }
}

