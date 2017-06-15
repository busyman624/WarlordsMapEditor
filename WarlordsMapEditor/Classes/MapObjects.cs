using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class MapObjects
    {
        public List<Sprite> terrains;
        public List<Sprite> roads;
        public List<Sprite> castles;
        public List<Sprite> ruins;

        public MapObjects()
        {
            terrains = new List<Sprite>();
            roads = new List<Sprite>();
            castles = new List<Sprite>();
            ruins = new List<Sprite>();

            terrains.Add(new Sprite(Resources.forest, "Forests", 0, "Terrains"));
            terrains.Add(new Sprite(Resources.grass, "Grasses", 1, "Terrains"));
            terrains.Add(new Sprite(Resources.hills, "Hills", 2, "Terrains"));
            terrains.Add(new Sprite(Resources.mountains, "Mountains", 3, "Terrains"));
            terrains.Add(new Sprite(Resources.swamp, "Swamps", 4, "Terrains"));
            terrains.Add(new Sprite(Resources.water, "Waters", 5, "Terrains"));
            roads.Add(new Sprite(Resources.roads, "Roads", 0, "Roads"));
            roads.Add(new Sprite(Resources.bridges, "Bridges", 1, "Roads"));
            castles.Add(new Sprite(Resources.castles, "Castles", 0, "Castles"));
            ruins.Add(new Sprite(Resources.ruin, "Ruins", 0, "Ruins"));
            ruins.Add(new Sprite(Resources.temples, "Temples", 1, "Ruins"));
        }

    }
}
