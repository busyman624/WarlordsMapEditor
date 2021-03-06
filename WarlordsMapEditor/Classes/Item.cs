﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        public string setName;
        public string category;

        public Bitmap bitmap;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChaged(string info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public virtual void onItemClick(){ }
        public virtual void execute_MouseMoveCommand(MouseEventArgs param) { }
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

        private RelayCommand _MouseMoveCommand;
        public RelayCommand MouseMoveCommand
        {
            get
            {
                if (_MouseMoveCommand == null) return _MouseMoveCommand = new RelayCommand(param => execute_MouseMoveCommand((MouseEventArgs)param));
                return _MouseMoveCommand;
            }
            set { _MouseMoveCommand = value; }
        }

        

    }
}

