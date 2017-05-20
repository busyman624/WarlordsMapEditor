using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarlordsMapEditor
{
    class Map
    {
        public int columns;
        public int rows;
        public List<MapItem> tiles;
        public string name;
        public string description;

        public int startTurn;
        public int startPlayer;
        public List<int> playersInTurnOrder = new List<int>();
        public List<Resource> resources = new List<Resource>();
        public List<bool> isAi = new List<bool>();
        public List<bool> useColor = new List<bool>();
        public List<float> r = new List<float>();
        public List<float> g = new List<float>();
        public List<float> b = new List<float>();
        public int resourceCount;
        public int paletteSize;
        public List<string> prefabPath = new List<string>();


        public Map()
        {

        }
    }
}
