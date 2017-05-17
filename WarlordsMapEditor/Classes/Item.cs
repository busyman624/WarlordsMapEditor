using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WarlordsMapEditor
{
    public class Item : INotifyPropertyChanged
    {
        public int itemIndex;
        public int setIndex;

        private BitmapImage _image;
        public BitmapImage image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    
                    _image = value;
                    NotifyPropertyChanged("image");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public virtual void onItemClick(){ }
        public virtual bool CanPerform() { return true; }

        private ICommand _ItemClick;

        public ICommand ItemClick
        {
            get
            {
                if (_ItemClick == null)
                {
                    _ItemClick = new RelayCommand(
                        param => this.onItemClick(),
                        param => this.CanPerform()
                    );
                }
                return _ItemClick;
            }
        }
    }
}

