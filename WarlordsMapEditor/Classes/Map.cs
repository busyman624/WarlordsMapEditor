using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarlordsMapEditor.Classes.ImportedClasses;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

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
            string[] resources=null;
            List<MapResource> neededResources = new List<MapResource>();
            foreach(string prefab in prefabPath)
            {
                string setName = prefab.Split('_')[0];
                int itemIndex = Int16.Parse(prefab.Split('_')[1]);
                status = mapObjects.roads.Exists(p => p.setName.ToLower() == setName && p.imagesList.Count > itemIndex);
                if (status) continue;
                status = mapObjects.terrains.Exists(p => p.setName.ToLower() == setName && p.imagesList.Count > itemIndex);
                if (!status) status = neededResources.Any(nr => nr.setName == setName);
                if (!status)
                {
                    if (resources == null) resources = LoadResources();
                    if (resources == null)
                    {
                        MessageBox.Show("Please choose valid resources directory", "Error");
                        return false;
                    }
                    foreach (string r in resources)
                    {
                        string resourceName=r.Split('\\').Last().Split('.').First();
                        if (resourceName == setName && !neededResources.Any(nr => nr.setName == setName))
                        {
                            int category = 0;
                            foreach (int id in overlayTilesPrefabId)
                            {
                                if (prefabPath[id].Split('_')[0] == setName)
                                {
                                    category = 1;
                                    break;
                                }
                            }
                            neededResources.Add(new MapResource(r, setName, category));
                            status = true;
                            break;
                        }
                    }
                    if (!status)
                    {
                        MessageBox.Show(setName + " does not exist in choosen resources directory.", "Error");
                        return false;
                    }

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
            if (neededResources.Count > 0) AddResources(mapObjects, neededResources);

            return true;
        }

        public string[] LoadResources()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
            {
                return Directory.GetFiles(dlg.SelectedPath);
            }
            else return null;
        }

        public void AddResources(MapObjects mapObjects, List<MapResource> resources)
        {
            foreach(MapResource resource in resources)
            {
                    Bitmap rawBitmap = new Bitmap(resource.path);
                    Bitmap bitmap = new Bitmap(rawBitmap, new Size(rawBitmap.Width / rawBitmap.Height * 40, 40));
                    switch (resource.category)
                    {
                        case 0:
                            {
                                mapObjects.terrains.Add(new Sprite(bitmap, resource.setName, mapObjects.terrains.Count, "Terrains"));
                                break;
                            }
                        case 1:
                            {
                                mapObjects.roads.Add(new Sprite(bitmap, resource.setName, mapObjects.roads.Count, "Roads"));
                                break;
                            }
                        //case 2:
                        //    {
                        //        mapObjects.castles.Merge(bitmap);
                        //        configs.fractions = tempConfigs.fractions;
                        //        break;
                        //    }
                        //case 3:
                        //    {
                        //        mapObjects.ruins.Add(new Sprite(bitmap, SetName.Text, mapObjects.ruins.Count, "Ruins"));
                        //        configs.ruinsData = tempConfigs.ruinsData;
                        //        break;
                        //    }
                    }
            }
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

    public class MapResource
    {
        public string path;
        public string setName;
        public int category;

        public MapResource(string path, string setName, int category)
        {
            this.path = path;
            this.setName = setName;
            this.category = category;
        }
    }
}
