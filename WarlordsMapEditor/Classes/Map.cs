using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarlordsMapEditor.Classes.ShitClasses;

namespace WarlordsMapEditor
{
    class Map
    {
        public int columns;
        public int rows;
        public List<MapItem> tiles;
        public string name;
        public string description;
        public List<int> overlayTilesX = new List<int>();
        public List<int> overlayTilesY = new List<int>();
        public List<int> overlayTilesPrefabId = new List<int>();

        public int startTurn;
        public int startPlayer;
        public List<int> playersInTurnOrder = new List<int>();
        public List<bool> isAi = new List<bool>();
        public List<bool> useColor = new List<bool>();
        public List<float> r = new List<float>();
        public List<float> g = new List<float>();
        public List<float> b = new List<float>();
        public List<int> resourceCount = new List<int>();
        public List<List<Resource>> resourcesList = new List<List<Resource>>();
        public int paletteSize;
        public List<string> prefabPath = new List<string>();
        public int overlayTilesCount;
        public List<CastleInfo> castles = new List<CastleInfo>();
        public List<RuinsInfo> ruins = new List<RuinsInfo>();
        public List<UnitContainerInfo> units = new List<UnitContainerInfo>();


        public Map()
        {

        }
    }
}
