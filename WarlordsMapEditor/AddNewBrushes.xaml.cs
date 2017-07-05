using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WarlordsMapEditor
{
    /// <summary>
    /// Interaction logic for AddNewBrushes.xaml
    /// </summary>
    public partial class AddNewBrushes : Window, INotifyPropertyChanged
    {
        public MapObjects mapObjects;
        public Configs configs;
        private Configs tempConfigs=new Configs(null);
        private Bitmap bitmap;
        private bool configAdded;
        public AddNewBrushes(MapObjects mapObjects, Configs configs)
        {
            InitializeComponent();
            this.mapObjects = mapObjects;
            this.configs = configs;
            DataContext = this;
        }

        public void showDialog()
        {
            ShowDialog();
        }

        public void OKClick()
        {
            switch (Category.SelectedIndex)
            {
                case 0:
                    {
                        mapObjects.terrains.Add(new Sprite(bitmap, SetName.Text, mapObjects.terrains.Count, "Terrains"));
                        break;
                    }
                case 1:
                    {
                        mapObjects.roads.Add(new Sprite(bitmap, SetName.Text, mapObjects.roads.Count, "Roads"));
                        break;
                    }
                case 2:
                    {
                        mapObjects.castles.Merge(bitmap);
                        configs.fractions = tempConfigs.fractions;
                        break;
                    }
                case 3:
                    {
                        mapObjects.ruins.Add(new Sprite(bitmap, SetName.Text, mapObjects.ruins.Count, "Ruins"));
                        configs.ruinsData = tempConfigs.ruinsData;
                        break;
                    }
            }
            Close();
        }

        public bool CanOK()
        {
            if (Category.SelectedIndex==2 || Category.SelectedIndex == 3)
            {
                return SetName.Text.Count() > 1 && bitmap != null && configAdded;
            }
            else
            {
                return SetName.Text.Count() > 1 && Category.SelectedIndex > -1 && bitmap != null;
            }
        }

        public void CancelClick()
        {
            Close();
        }

        public bool canAddFile() { return Category.SelectedIndex > -1; }

        public void addFileClick()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "(*.png)|*.png";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Bitmap rawBitmap = new Bitmap(dlg.FileName);
                if (rawBitmap.Width % 40 == 0 && rawBitmap.Height % 40 == 0)
                {
                    bitmap = new Bitmap(rawBitmap, new System.Drawing.Size(rawBitmap.Width / rawBitmap.Height * 40, 40));
                    fileStatus = bitmap.Width/bitmap.Height + " items selected";
                    SetName.Text = dlg.SafeFileName.Split('.').First();
                }
                else
                {
                    fileStatus = "Error, please retry";
                }
            }
        }

        public bool canAddConfig() { return (Category.SelectedIndex == 2 || Category.SelectedIndex == 3) && bitmap != null; }

        public void addConfigClick()
        {
            if (Category.SelectedIndex == 2)
            {
                configStatus = tempConfigs.SetFractionsConfig();
                if (configStatus == null || tempConfigs.fractions.Count != mapObjects.castles.imagesList.Count + bitmap.Width / bitmap.Height) configAdded = false;
                else configAdded = true;
            }
            else
            {
                configStatus = tempConfigs.SetRuinsConfig();
                if (configStatus == null || tempConfigs.ruinsData.Count != mapObjects.ruins.Count + 1 ||
                    tempConfigs.ruinsData[tempConfigs.ruinsData.Count - 1].sprites.Count != bitmap.Width / bitmap.Height) configAdded = false;
                else
                {
                    SetName.Text = tempConfigs.ruinsData[tempConfigs.ruinsData.Count - 1].name;
                    configAdded = true;
                } 
            }
            if (!configAdded) configStatus= "Error, please retry";
        }

        private string _fileStatus;

        public string fileStatus
        {
            get { return _fileStatus; }
            set
            {
                if (_fileStatus != value)
                {

                    _fileStatus = value;
                    RaisePropertyChaged("fileStatus");
                }
            }
        }

        private string _configStatus;

        public string configStatus
        {
            get { return _configStatus; }
            set
            {
                if (_configStatus != value)
                {

                    _configStatus = value;
                    RaisePropertyChaged("configStatus");
                }
            }
        }

        private ICommand _OK;
        private ICommand _Cancel;
        private ICommand _addFile;
        private ICommand _addConfig;

        public ICommand OK
        {
            get
            {
                if (_OK == null)
                {
                    _OK = new RelayCommand(
                        param => this.OKClick(),
                        param => this.CanOK()
                    );
                }
                return _OK;
            }
        }

        public ICommand Cancel
        {
            get
            {
                if (_Cancel == null)
                {
                    _Cancel = new RelayCommand(
                        param => this.CancelClick(),
                        param => true
                    );
                }
                return _Cancel;
            }
        }

        public ICommand addFile
        {
            get
            {
                if (_addFile == null)
                {
                    _addFile = new RelayCommand(
                        param => this.addFileClick(),
                        param => this.canAddFile()
                    );
                }
                return _addFile;
            }
        }

        public ICommand addConfig
        {
            get
            {
                if (_addConfig == null)
                {
                    _addConfig = new RelayCommand(
                        param => this.addConfigClick(),
                        param => this.canAddConfig()
                    );
                }
                return _addConfig;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChaged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
