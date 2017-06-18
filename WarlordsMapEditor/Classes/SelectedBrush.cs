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
    public class SelectedBrush : Item
    {
        public new int? setIndex;
        public new int? itemIndex;

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

        private ObservableCollection<string> _details2;
        public ObservableCollection<string> details2
        {
            get { return _details2; }
            set { _details2 = value; }
        }

        private MapObjects mapObjects;
        private Configs configs;

        public SelectedBrush(MapObjects mapObjects, Configs configs)
        {
            setIndex = null;
            itemIndex = null;
            this.mapObjects = mapObjects;
            this.configs = configs;
            details = new ObservableCollection<string>();
            details2 = new ObservableCollection<string>();
        }

        public void change(Brush brush)
        {
            clear();
            category = brush.category;
            setIndex = brush.setIndex;
            setName = brush.setName;
            itemIndex = brush.itemIndex;
            bitmap = brush.bitmap;
            image = brush.image;
            update();
        }

        public void update()
        {
            if (category != "Delete")
            {
                description = category + ": " + setName + " - " + itemIndex;
                if (setName == "Castles")
                {
                    additionalInfo = "Fraction: " + configs.fractions[(int)itemIndex].name;
                    additionalInfo2 = "Buildings:";
                    foreach (string building in configs.fractions[(int)itemIndex].buildings)
                    {
                        details2.Add(building);
                    }
                }
                int ruinIndex = configs.ruinsData.FindIndex(r => r.name == setName.ToLower());
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
                        details2.Add(resource.count + " " + resource.name);
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
            }


        }

        public void clear()
        {
            setIndex = null;
            itemIndex = null;
            category = null;
            details.Clear();
            details2.Clear();
            additionalInfo = null;
            additionalInfo2 = null;
            description = null;
            image = null;
        }
    }
}
