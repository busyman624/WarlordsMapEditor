using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarlordsMapEditor.Classes.ShitClasses;

namespace WarlordsMapEditor
{
    public class Map
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

        public void addUnit(int x, int y, int owner)
        {
            //int unitCount = reader.ReadInt32();
            int unitCount = 1;
            List<UnitInfo> unitInfos = new List<UnitInfo>();
            for (int j = 0; j < unitCount; j++)
            {
                string unitName = "Hero";
                string unitDisplayName = "Hero";
                float range = -1;
                int experience = 0;
                int level = 1;
                bool overrideStats = false;
                Statistics stats = new Statistics();

                // Modifiers
                StatsModifiers modifiers = new StatsModifiers();

                unitInfos.Add(new UnitInfo(unitName, unitDisplayName, range, experience, level, stats, overrideStats, modifiers));
            }
            units.Add(new UnitContainerInfo(x, y, owner, unitInfos));
        }

        public Map() { }

        public bool validate(MapObjects mapObjects, Configs configs)
        {
            bool status;
            foreach(string prefab in prefabPath)
            {
                string setName = prefab.Split('_')[0];
                int itemIndex = Int16.Parse(prefab.Split('_')[1]);
                //status = setName == mapObjects.castles.setName.ToLower() && mapObjects.castles.imagesList.Count > itemIndex;
                //if (status) continue;
                //status = mapObjects.ruins.Exists(p => p.setName.ToLower() == setName && p.imagesList.Count > itemIndex);
                //if (status) continue;
                status = mapObjects.roads.Exists(p => p.setName.ToLower() == setName && p.imagesList.Count > itemIndex);
                if (status) continue;
                status = mapObjects.terrains.Exists(p => p.setName.ToLower() == setName && p.imagesList.Count > itemIndex);
                if (!status)
                {
                    MessageBox.Show(prefab+" does not exist in current editor configuration. Update map objects before loading custom map", "Error");
                    return false;
                }
            }
            if (configs.fractions.Count > mapObjects.castles.imagesList.Count)
            {
                MessageBox.Show("Fractions configuration of the map does not match current editor configuration. Update map objects before loading custom map", "Error");
                return false;
            } 
            if (configs.ruinsData.Count > mapObjects.ruins.Count)
            {
                MessageBox.Show("Ruins configuration of the map does not match current editor configuration. Update map objects before loading custom map", "Error");
                return false;
            }
            else
            {
                for (int i = 0; i < configs.ruinsData.Count; i++)
                {
                    if (configs.ruinsData[i].sprites.Count > mapObjects.ruins[i].imagesList.Count)
                    {
                        MessageBox.Show(mapObjects.ruins[i].setName + " has less objects in current editor configuration. Update map objects before loading custom map", "Error");
                        return false;
                    }
                }
            }

            return true;
        }

        public void UpdatePallete(MapObjects mapObjects)
        {
            prefabPath.Clear();
            paletteSize = 0;
            foreach (Sprite sprite in mapObjects.terrains)
            {
                for(int i = 0; i < sprite.imagesList.Count; i++)
                {
                    prefabPath.Add(sprite.setName.ToLower() + "_" + i.ToString());
                }
            }

            foreach (Sprite sprite in mapObjects.roads)
            {
                for (int i = 0; i < sprite.imagesList.Count; i++)
                {
                    prefabPath.Add(sprite.setName.ToLower() + "_" + i.ToString());
                }
            }
            paletteSize = prefabPath.Count;
        }
    }
}
