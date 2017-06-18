using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class MiniMap : INotifyPropertyChanged
    {
        private double areaWidth = 1000 / 3 - 80;    //window size / 3 - 2* (navigate buttons + margins(buttons + border))
        private double areaHeight = 800 * 2 / 3 - 160;     //window height * 2/3 - 2* (navigate buttons + margins(buttons + border))
        private int multiplier;

        private int mapColumns;
        private int mapRows;
        public int boardColumns;
        public int boardRows;

        private int _currentX = 0;
        private int _currentY = 0;
        private int _miniMapWidth = 0;
        private int _miniMapHeight = 0;
        private int _viewWidth = 0;
        private int _viewHeight = 0;
        private BitmapImage _miniMapImage;
        private Bitmap miniMap;
        List<MapItem> mapItems;

        public int currentX
        {
            get { return _currentX; }
            set
            {
                if (_currentX != value)
                {

                    _currentX = value;
                    RaisePropertyChaged("currentX");
                }
            }
        }

        public int currentY
        {
            get { return _currentY; }
            set
            {
                if (_currentY != value)
                {

                    _currentY = value;
                    RaisePropertyChaged("currentY");
                }
            }
        }

        public int miniMapWidth
        {
            get { return _miniMapWidth; }
            set
            {
                if (_miniMapWidth != value)
                {

                    _miniMapWidth = value;
                    RaisePropertyChaged("miniMapWidth");
                }
            }
        }

        public int miniMapHeight
        {
            get { return _miniMapHeight; }
            set
            {
                if (_miniMapHeight != value)
                {

                    _miniMapHeight = value;
                    RaisePropertyChaged("miniMapHeight");
                }
            }
        }

        public int viewWidth
        {
            get { return _viewWidth; }
            set
            {
                if (_viewWidth != value)
                {

                    _viewWidth = value;
                    RaisePropertyChaged("viewWidth");
                }
            }
        }

        public int viewHeight
        {
            get { return _viewHeight; }
            set
            {
                if (_viewHeight != value)
                {

                    _viewHeight = value;
                    RaisePropertyChaged("viewHeight");
                }
            }
        }

        public BitmapImage miniMapImage
        {
            get { return _miniMapImage; }
            set
            {
                if (_miniMapImage != value)
                {

                    _miniMapImage = value;
                    RaisePropertyChaged("miniMapImage");
                }
            }
        }

        public void calculate(List<MapItem> mapItems, int mapColumns, int mapRows, int boardColumns, int boardRows)
        {
            this.mapItems = mapItems;
            this.mapColumns = mapColumns;
            this.mapRows = mapRows;
            this.boardColumns = boardColumns;
            this.boardRows = boardRows;

            if (areaWidth / mapColumns < areaHeight / mapRows) multiplier = (int)areaWidth / mapColumns;
            else multiplier = (int)areaHeight / mapRows;

            currentX = 0;
            currentY = 0;

            miniMapWidth =  multiplier*mapColumns + 3;
            miniMapHeight = multiplier*mapRows + 3;

            viewWidth = multiplier * boardColumns;
            viewHeight = multiplier * boardRows;

            miniMap = new Bitmap(multiplier * mapColumns, multiplier * mapRows);
            BitmapImage temp = new BitmapImage();
            Graphics g = Graphics.FromImage(miniMap);

            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapColumns; c++)
                {
                    Bitmap temp_bmp = new Bitmap(mapItems[c + mapColumns * (mapRows - 1 - r)].bitmap, new Size(multiplier, multiplier));
                    g.DrawImage(temp_bmp, new Point(c* multiplier, r* multiplier));
                }
            }

            using (var memory = new MemoryStream())
            {
                miniMap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                temp.BeginInit();
                temp.StreamSource = memory;
                temp.CacheOption = BitmapCacheOption.OnLoad;
                temp.EndInit();
            }
            miniMapImage = temp;
        }

        public void refresh(int Xcoordinate, int Ycoordinate)
        {
            BitmapImage temp = new BitmapImage();
            Graphics g = Graphics.FromImage(miniMap);
            Bitmap temp_bmp = new Bitmap(mapItems[Xcoordinate + mapColumns * Ycoordinate].bitmap, new Size(multiplier, multiplier));
            g.DrawImage(temp_bmp, new Point(Xcoordinate* multiplier, (mapRows - 1 - Ycoordinate) * multiplier));
            using (var memory = new MemoryStream())
            {
                miniMap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                temp.BeginInit();
                temp.StreamSource = memory;
                temp.CacheOption = BitmapCacheOption.OnLoad;
                temp.EndInit();
            }
            miniMapImage = temp;
        }

        public void moveLeft()
        {
            currentX -= multiplier;
        }

        public void moveRight()
        {
            currentX += multiplier;
        }

        public void moveUp()
        {
            currentY -= multiplier;
        }

        public void moveDown()
        {
            currentY += multiplier;
        }

        public void zoomIn(int boardColumns, int boardRows)
        {
            viewWidth = multiplier * boardColumns;
            viewHeight = multiplier * boardRows;
        }

        public void zoomOut(int boardColumns, int boardRows)
        {
            viewWidth = multiplier * boardColumns;
            viewHeight = multiplier * boardRows;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChaged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

    }
}
