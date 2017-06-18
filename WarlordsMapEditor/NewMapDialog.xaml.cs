using System;
using System.Collections.Generic;
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
    public partial class NewMapDialog : Window
    {
        public Map map;
        public NewMapDialog(Map map)
        {
            InitializeComponent();
            this.map = map;
            DataContext = this;
        }
        public void showDialog()
        {
            ShowDialog();
        }

        public void OKClick()
        {
            map.rows = Int32.Parse(Rows.Text);
            map.columns = Int32.Parse(Columns.Text); 
            map.name = mapName.Text;
            Close();
        }

        public bool CanOK()
        {
            return Rows.Text.Count() > 1 && Columns.Text.Count() > 1 && mapName.Text.Count() > 2;
        }

        public void CancelClick()
        {
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
