using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class CastleDialog : Window
    {
        public MapItem tile;
        public CastleDialog(MapItem tile)
        {
            InitializeComponent();
            this.tile = tile;
            DataContext = this;
        }
        public void showDialog()
        {
            ShowDialog();
        }

        public void OKClick()
        {
            tile.castleName = CastleName.Text;
            tile.castleOwner = CastleOwner.SelectedIndex - 1;
            Close();

        }

        public bool CanOK()
        {
            return CastleName.Text.Count()>1 && CastleName.Text.Count() <8 && CastleOwner.SelectedIndex > -1;
        }

        public void CancelClick()
        {
            tile.clearTile();
            Close();
        }

        private ICommand _OK;
        private ICommand _Cancel;

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
    }
}
