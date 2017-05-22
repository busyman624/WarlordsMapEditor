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
        private int mapColumns;
        private int mapRows;

        private int _currentX = 0;
        private int _currentY = 0;
        private int _miniMapWidth = 0;
        private int _miniMapHeight = 0;
        private BitmapImage _miniMapImage;

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

        public void calculate(int mapColumns, int mapRows)
        {
            this.mapColumns = mapColumns;
            this.mapRows = mapRows;

            currentX = 0;
            currentY = 0;

            miniMapWidth =  mapColumns + 3;
            miniMapHeight =  mapRows + 3;

        }

        public void refresh(List<MapItem> mapItems)
        {
            Bitmap miniMap= new Bitmap(mapColumns, mapRows);
            BitmapImage temp = new BitmapImage();
            Graphics g = Graphics.FromImage(miniMap);

            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapColumns; c++)
                {
                    Bitmap temp_bmp = new Bitmap(mapItems[c + r * mapColumns].bitmap, new Size(1, 1));
                    g.DrawImage(temp_bmp, new Point(c, r));
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChaged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

    }
}
