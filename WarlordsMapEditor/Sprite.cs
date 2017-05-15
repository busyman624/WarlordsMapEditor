using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarlordsMapEditor
{
    public class Sprite
    {
        private List<Bitmap> Tiles;
        private Bitmap thumbnail;
        private string setName;

        public Sprite(string imageName)
        {
            var bmp = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("WarlordsMapEditor.Resources"+imageName));
            Tiles = new List<Bitmap>();
            for(int i = 0; i < bmp.Width / bmp.Height; i++)
            {
                Tiles.Add(bmp.Clone(new Rectangle(i*bmp.Height, 0, bmp.Width, bmp.Height), bmp.PixelFormat));
            }
            thumbnail = Tiles[0];
            setName = imageName.Split('.')[0];
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
