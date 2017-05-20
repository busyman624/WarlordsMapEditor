using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace WarlordsMapEditor
{
    public enum MapLoadMode
    {
        All,
        Tiles,
        Meta,
    }

    class FileMapProvider
    {
        public FileMapProvider() { }

        public const UInt32 headerMagic = 0x50414d58; // "XMAP"

        public Map LoadMapFromBytes(List<Sprite> sprites, string path, MapLoadMode mode = MapLoadMode.All)
        {

            byte[] serializedMap = File.ReadAllBytes(path);

            Map map = new Map();
            using (BinaryReader reader = new BinaryReader(new MemoryStream(serializedMap)))
            {
                // Header & metadata
                if (reader.ReadUInt32() != headerMagic)
                    throw new IOException("Invalid map header");

                map.columns = reader.ReadInt32();
                map.rows = reader.ReadInt32();
                if (map.columns <= 0 || map.rows <= 0)
                    throw new IOException("Invalid rows/columns values");
                map.tiles = new List<MapItem>();
                map.name = reader.ReadString();
                map.description = reader.ReadString();

                //player shit
                map.startTurn = reader.ReadInt32();
                map.startPlayer = reader.ReadInt32();

                int players = reader.ReadInt32();
                for (int i = 0; i < players; i++)
                    map.playersInTurnOrder.Add(reader.ReadInt32());

                for (int i = 0; i < players; i++)
                {
                    bool isAi = reader.ReadBoolean();
                    bool useColor = reader.ReadBoolean();
                    map.isAi.Add(isAi);
                    map.useColor.Add(useColor);
                    if (useColor)
                    {
                        map.r.Add(reader.ReadSingle());
                        map.g.Add(reader.ReadSingle());
                        map.b.Add(reader.ReadSingle());
                    }

                    map.resourceCount = reader.ReadInt32();
                    for (int j = 0; j < map.resourceCount; j++)
                    {
                        map.resources.Add(new Resource(reader.ReadString(), reader.ReadInt32()));
                    }
                }

                // Prefab palette
                map.paletteSize = reader.ReadInt16();
                List<string> palette = new List<string>();
                if (map.paletteSize <= 0)
                    throw new IOException("Palette is empty");
                Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");
                for (int i = 0; i < map.paletteSize; i++)    //DEBUG - Jak wygladaja sciezki?
                {
                    string prefabPath = reader.ReadString();
                    map.prefabPath.Add(prefabPath);
                    if (!regex.IsMatch(prefabPath))
                        throw new IOException("Invalid prefab path: " + prefabPath);
                    palette.Add(prefabPath);
                }



                // Tiles
                for (int i = 0; i < map.rows; i++)
                {
                    for (int j = 0; j < map.columns; j++)
                    {
                        int prefabId = reader.ReadInt16();
                        int setIndex;
                        int itemIndex;
                        if (prefabId < 0 || prefabId >= map.paletteSize)
                            throw new IOException("Invalid prefab ID");
                        switch (palette[prefabId].Split('_')[0])
                        {
                            case "forest":
                                {
                                    setIndex = 0;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i));
                                    break;
                                }
                            case "grass":
                                {
                                    setIndex=1;
                                    String sss = palette[prefabId].Split('_')[1];
                                    itemIndex = Int16.Parse(sss);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i)); 
                                    break;
                                }
                            case "hills":
                                {
                                    setIndex = 2;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i));
                                    break;
                                }
                            case "mountains":
                                {
                                    setIndex = 3;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i));
                                    break;
                                }
                            case "swamp":
                                {
                                    setIndex = 4;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i));
                                    break;
                                }
                            case "water":
                                {
                                    setIndex = 5;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i));
                                    break;
                                }
                            case "roads":
                                {
                                    setIndex = 6;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i));
                                    break;
                                }
                            case "bridges":
                                {
                                    setIndex = 7;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i));
                                    break;
                                }
                        }

                    }
                }

                return map;

                //    // Overlay tiles
                //    int overlayTiles = reader.ReadInt32();
                //    for (int i = 0; i < overlayTiles; i++)
                //    {
                //        int x = reader.ReadInt32();
                //        int y = reader.ReadInt32();
                //        int prefabId = reader.ReadInt32();
                //        if (prefabId < 0 || prefabId >= paletteSize)
                //            throw new IOException("Invalid prefab ID");
                //        PlaceOverlayTile(map, palette[prefabId], x, y);
                //    }

                //    if (mode != MapLoadMode.Tiles)
                //    {
                //        // Castles
                //        int castleCount = reader.ReadInt32();
                //        for (int i = 0; i < castleCount; i++)
                //        {
                //            string prefabName = reader.ReadString();
                //            string castleName = reader.ReadString();
                //            int x = reader.ReadInt32();
                //            int y = reader.ReadInt32();
                //            int owner = reader.ReadInt32();
                //            bool razed = reader.ReadBoolean();
                //            int buildings = reader.ReadInt32();

                //            HashSet<string> purchasedBuildings = new HashSet<string>();
                //            for (int j = 0; j < buildings; j++)
                //            {
                //                purchasedBuildings.Add(reader.ReadString());
                //            }

                //            string producedUnitName = reader.ReadString();
                //            int producedUnitTurnsLeft = reader.ReadInt32();
                //            string producedUnitDestination = reader.ReadString();

                //            int queuedUnits = reader.ReadInt32();
                //            Queue<string> unitQueue = new Queue<string>();
                //            for (int j = 0; j < queuedUnits; j++)
                //                unitQueue.Enqueue(reader.ReadString());

                //            castles.Add(new CastleInfo(prefabName, castleName, x, y, owner, razed, purchasedBuildings,
                //                producedUnitName, producedUnitTurnsLeft, producedUnitDestination, unitQueue));
                //        }

                //        // Ruins
                //        int ruinCount = reader.ReadInt32();
                //        for (int i = 0; i < ruinCount; i++)
                //        {
                //            string prefabName = reader.ReadString();
                //            int x = reader.ReadInt32();
                //            int y = reader.ReadInt32();
                //            bool isVisited = reader.ReadBoolean();

                //            ruins.Add(new RuinsInfo(prefabName, x, y, isVisited));
                //        }

                //        // Units
                //        int unitContainers = reader.ReadInt32();
                //        for (int i = 0; i < unitContainers; i++)
                //        {
                //            int x = reader.ReadInt32();
                //            int y = reader.ReadInt32();
                //            int owner = reader.ReadInt32();
                //            int unitCount = reader.ReadInt32();

                //            List<UnitInfo> unitInfos = new List<UnitInfo>();
                //            for (int j = 0; j < unitCount; j++)
                //            {
                //                string unitName = reader.ReadString();
                //                string unitDisplayName = reader.ReadString();
                //                float range = reader.ReadSingle();
                //                int experience = reader.ReadInt32();
                //                int level = reader.ReadInt32();
                //                bool overrideStats = reader.ReadBoolean();
                //                Statistics stats = new Statistics();
                //                if (overrideStats)
                //                {
                //                    stats.attack = reader.ReadSingle();
                //                    stats.defense = reader.ReadSingle();
                //                    stats.power = reader.ReadSingle();
                //                    stats.wisdom = reader.ReadSingle();
                //                    stats.hp = reader.ReadSingle();
                //                    stats.speed = reader.ReadSingle();
                //                    stats.damageMin = reader.ReadSingle();
                //                    stats.damageMax = reader.ReadSingle();
                //                    stats.timeToProduce = reader.ReadSingle();
                //                    stats.canShoot = reader.ReadBoolean();
                //                    stats.canFly = reader.ReadBoolean();
                //                    stats.range = reader.ReadSingle();
                //                }

                //                // Modifiers
                //                StatsModifiers modifiers = new StatsModifiers();
                //                int modifierCount = reader.ReadInt32();
                //                for (int k = 0; k < modifierCount; k++)
                //                {
                //                    string statType = reader.ReadString();
                //                    int modifierSubCount = reader.ReadInt32();
                //                    for (int m = 0; m < modifierSubCount; m++)
                //                    {
                //                        modifiers.addModifier(statType, new StatsModifiersEntry(
                //                            reader.ReadString(),
                //                            reader.ReadSingle(),
                //                            (StatsModifiersEntry.DurationType)reader.ReadInt32(),
                //                            reader.ReadInt32(),
                //                            (StatsModifiersEntry.ModifierType)reader.ReadInt32()
                //                        ));
                //                    }
                //                }

                //                unitInfos.Add(new UnitInfo(unitName, unitDisplayName, range, experience, level, stats, overrideStats, modifiers));
                //            }

                //            units.Add(new UnitContainerInfo(x, y, owner, unitInfos));
                //        }
                //    }
                //}

                //return map;
            }
        }
    }
}
