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

    public partial class EditMapDetails : Window
    {
        private bool result;

        public EditMapDetails(string name, string description)
        {
            InitializeComponent();
            if (name != "") Name.Text = name;
            if (name != "") Description.Text = description;
            DataContext = this;
        }
        public bool showDialog()
        {
            ShowDialog();
            return result;
        }

        public void OKClick()
        {
            result = true;
            Close();

        }

        public bool CanOK()
        {
            return Name.Text.Count() > 1 && Description.Text.Count() > 1;
        }

        public void CancelClick()
        {
            result = false;
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
