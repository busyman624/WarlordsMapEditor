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
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class SelectedBrush : INotifyPropertyChanged
    {
        private BitmapImage _image;

        public BitmapImage image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    RaisePropertyChaged("image");
                }
            }
        }

        private string _description;

        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChaged("description");
                }
            }
        }

        private string _additionalInfo;

        public string additionalInfo
        {
            get { return _additionalInfo; }
            set
            {
                if (_additionalInfo != value)
                {
                    _additionalInfo = value;
                    RaisePropertyChaged("additionalInfo");
                }
            }
        }

        private string _additionalInfo2;

        public string additionalInfo2
        {
            get { return _additionalInfo2; }
            set
            {
                if (_additionalInfo2 != value)
                {
                    _additionalInfo2 = value;
                    RaisePropertyChaged("additionalInfo2");
                }
            }
        }

        private ObservableCollection<string> _details;
        public ObservableCollection<string> details
        {
            get { return _details; }
            set { _details = value; }
        }

        private ObservableCollection<string> _rewards;
        public ObservableCollection<string> rewards
        {
            get { return _rewards; }
            set { _rewards = value; }
        }

        private List<Sprite> sprites;
        private Configs configs;

        public SelectedBrush(List<Sprite> sprites, Configs configs)
        {
            this.sprites = sprites;
            this.configs = configs;
            details = new ObservableCollection<string>();
            rewards = new ObservableCollection<string>();
        }

        public void update()
        {
            clear();
            if (Board.selectedSetIndex != -1 && Board.selectedItemIndex != -1)
            {
                image = sprites[(int)Board.selectedSetIndex].imagesList[(int)Board.selectedItemIndex];
                description = sprites[(int)Board.selectedSetIndex].category + ": " + sprites[(int)Board.selectedSetIndex].setName + " - " + (int)Board.selectedItemIndex;
                if (sprites[(int)Board.selectedSetIndex].setName == "Castle")
                {
                    additionalInfo = "Buildings:";
                    foreach (string building in configs.fractions[(int)Board.selectedItemIndex].buildings)
                    {
                        details.Add(building);
                    }
                }
                int ruinIndex = configs.ruinsData.FindIndex(r => r.name == sprites[(int)Board.selectedSetIndex].setName.ToLower());
                if (ruinIndex != -1)
                {
                    additionalInfo = "Enemy Units:";
                    additionalInfo2 = "Rewards:";
                    foreach (string unit in configs.ruinsData[ruinIndex].enemyUnits)
                    {
                        details.Add(unit);
                    }
                    foreach (Resource resource in configs.ruinsData[ruinIndex].resourceBonus)
                    {
                        rewards.Add(resource.count + " " + resource.name);
                    }
                }
            }
            else
            {
                Bitmap bmp = Resources.cross;
                using (var memory = new MemoryStream())
                {
                    bmp.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    BitmapImage temp_img = new BitmapImage();
                    temp_img.BeginInit();
                    temp_img.StreamSource = memory;
                    temp_img.CacheOption = BitmapCacheOption.OnLoad;
                    temp_img.EndInit();
                    image = temp_img;
                }
                description = "Delete";
                additionalInfo = "Click on roads or buildings to delete them";
            }


        }

        public void clear()
        {
            details.Clear();
            rewards.Clear();
            additionalInfo = null;
            additionalInfo2 = null;
            description = null;
            image = null;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChaged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
