using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Sprite
    {
        private List<Bitmap> Tiles;
        private Bitmap thumbnail;
        private string setName;

        public Sprite(Bitmap bmp, string setName)
        {
            Tiles = new List<Bitmap>();
            for(int i = 0; i < bmp.Width / bmp.Height; i++)
            {
                Tiles.Add(bmp.Clone(new Rectangle(i*bmp.Height, 0, bmp.Height, bmp.Height), bmp.PixelFormat));
            }
            thumbnail = Tiles[0];
            this.setName = setName;
        }

        public Bitmap getTile(int index)
        {
            return Tiles[index];
        }

        public int getSize()
        {
            return Tiles.Count();
        }

        public Bitmap getThumbnail()
        {
            return thumbnail;
        }

        public string getSetName()
        {
            return setName;
        }
    }
}
