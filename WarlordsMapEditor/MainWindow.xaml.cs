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
        List<Tile> _tiles = new List<Tile>();

        public Board(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            Random random = new Random();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _tiles.Add(new Tile()
                    {
                        Data = string.Format("Row {0}, Column {1}", r, c),
                        Background = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)))
                    });
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
    }

    public class Tile
    {
        public string Data { get; set; }
        public SolidColorBrush Background { get; set; }
    }

}

