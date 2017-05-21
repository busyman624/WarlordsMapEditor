using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using WarlordsMapEditor.Classes.ShitClasses;

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
                map.startTurn = reader.ReadInt32();  
                map.startPlayer = reader.ReadInt32();  

                int players = reader.ReadInt32();  
                for (int i = 0; i < players; i++)
                {
                    map.playersInTurnOrder.Add(reader.ReadInt32());  
                }

                for (int i = 0; i < players; i++)
                {
                    List<Resource> playerResources = new List<Resource>();
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

                    int resourceCount = reader.ReadInt32();  
                    map.resourceCount.Add(resourceCount);
                    for (int j = 0; j < resourceCount; j++)
                    {
                        playerResources.Add(new Resource(reader.ReadString(), reader.ReadInt32()));
                           
                    }
                    map.resourcesList.Add(playerResources);
                }

                // Prefab palette
                map.paletteSize = reader.ReadInt16();  
                List<string> palette = new List<string>();
                if (map.paletteSize <= 0)
                    throw new IOException("Palette is empty");
                Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");
                for (int i = 0; i < map.paletteSize; i++)  
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
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId));
                                    break;
                                }
                            case "grass":
                                {
                                    setIndex=1;
                                    String sss = palette[prefabId].Split('_')[1];
                                    itemIndex = Int16.Parse(sss);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId)); 
                                    break;
                                }
                            case "hills":
                                {
                                    setIndex = 2;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId));
                                    break;
                                }
                            case "mountains":
                                {
                                    setIndex = 3;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId));
                                    break;
                                }
                            case "swamp":
                                {
                                    setIndex = 4;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId));
                                    break;
                                }
                            case "water":
                                {
                                    setIndex = 5;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId));
                                    break;
                                }
                            case "roads":
                                {
                                    setIndex = 6;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId));
                                    break;
                                }
                            case "bridges":
                                {
                                    setIndex = 7;
                                    itemIndex = Int16.Parse(palette[prefabId].Split('_')[1]);
                                    map.tiles.Add(new MapItem(itemIndex, setIndex, sprites, j, i, prefabId));
                                    break;
                                }
                        }
                    }
                }

                // Overlay tiles
                map.overlayTilesCount = reader.ReadInt32();  
                for (int i = 0; i < map.overlayTilesCount; i++)
                {
                    map.overlayTilesX.Add(reader.ReadInt32());
                    map.overlayTilesY.Add(reader.ReadInt32());
                    map.overlayTilesPrefabId.Add(reader.ReadInt32());

                    int tileID = map.tiles.FindIndex(t => (t.Xcoordinate == map.overlayTilesX[i]) && (t.Ycoordinate == map.overlayTilesY[i]));
                    switch (palette[map.overlayTilesPrefabId[i]].Split('_')[0])
                    {
                        case "roads":
                            {
                                map.tiles[tileID].objectSet = 6;
                                map.tiles[tileID].objectIndex = Int16.Parse(palette[map.overlayTilesPrefabId[i]].Split('_')[1]);
                                map.tiles[tileID].combineImages();
                                break;
                            }
                        case "bridges":
                            {
                                map.tiles[tileID].objectSet = 7;
                                map.tiles[tileID].objectIndex = Int16.Parse(palette[map.overlayTilesPrefabId[i]].Split('_')[1]);
                                map.tiles[tileID].combineImages();
                                break;
                            }
                    }
                }

                // Castles
                int castleCount = reader.ReadInt32();  
                for (int i = 0; i < castleCount; i++)
                {
                    string prefabName = reader.ReadString();  
                    string castleName = reader.ReadString();  
                    int x = reader.ReadInt32();  
                    int y = reader.ReadInt32();  
                    int owner = reader.ReadInt32();  
                    bool razed = reader.ReadBoolean();  
                    int buildings = reader.ReadInt32();  

                    HashSet<string> purchasedBuildings = new HashSet<string>();
                    for (int j = 0; j < buildings; j++)
                    {
                        purchasedBuildings.Add(reader.ReadString());  
                    }

                    string producedUnitName = reader.ReadString();  
                    int producedUnitTurnsLeft = reader.ReadInt32();  
                    string producedUnitDestination = reader.ReadString();  

                    int queuedUnits = reader.ReadInt32();  
                    Queue<string> unitQueue = new Queue<string>();
                    for (int j = 0; j < queuedUnits; j++)
                    {
                        unitQueue.Enqueue(reader.ReadString());  
                    }

                    map.castles.Add(new CastleInfo(prefabName, castleName, x, y, owner, razed, purchasedBuildings,
                        producedUnitName, producedUnitTurnsLeft, producedUnitDestination, unitQueue));
                }

                // Ruins
                int ruinCount = reader.ReadInt32();  
                for (int i = 0; i < ruinCount; i++)
                {
                    string prefabName = reader.ReadString();  
                    int x = reader.ReadInt32();  
                    int y = reader.ReadInt32();  
                    bool isVisited = reader.ReadBoolean();  

                    map.ruins.Add(new RuinsInfo(prefabName, x, y, isVisited));
                }

                // Units
                int unitContainers = reader.ReadInt32();  
                for (int i = 0; i < unitContainers; i++)
                {
                    int x = reader.ReadInt32();  
                    int y = reader.ReadInt32();  
                    int owner = reader.ReadInt32();  
                    int unitCount = reader.ReadInt32();  

                    List<UnitInfo> unitInfos = new List<UnitInfo>();
                    for (int j = 0; j < unitCount; j++)
                    {
                        string unitName = reader.ReadString();  
                        string unitDisplayName = reader.ReadString();  
                        float range = reader.ReadSingle();  
                        int experience = reader.ReadInt32();  
                        int level = reader.ReadInt32();  
                        bool overrideStats = reader.ReadBoolean();  
                        Statistics stats = new Statistics();
                        if (overrideStats)
                        {
                            stats.attack = reader.ReadSingle();  
                            stats.defense = reader.ReadSingle();  
                            stats.power = reader.ReadSingle();  
                            stats.wisdom = reader.ReadSingle();  
                            stats.hp = reader.ReadSingle();  
                            stats.speed = reader.ReadSingle();  
                            stats.damageMin = reader.ReadSingle();  
                            stats.damageMax = reader.ReadSingle();  
                            stats.timeToProduce = reader.ReadSingle();  
                            stats.canShoot = reader.ReadBoolean();  
                            stats.canFly = reader.ReadBoolean();  
                            stats.range = reader.ReadSingle();  
                        }

                        // Modifiers
                        StatsModifiers modifiers = new StatsModifiers();
                        int modifierCount = reader.ReadInt32();  
                        for (int k = 0; k < modifierCount; k++)
                        {
                            string statType = reader.ReadString();  
                            int modifierSubCount = reader.ReadInt32();  
                            for (int m = 0; m < modifierSubCount; m++)
                            {
                                modifiers.addModifier(statType, new StatsModifiersEntry(
                                    reader.ReadString(),
                                    reader.ReadSingle(),
                                    (StatsModifiersEntry.DurationType)reader.ReadInt32(),
                                    reader.ReadInt32(),
                                    (StatsModifiersEntry.ModifierType)reader.ReadInt32()
                                ));
                            }
                        }

                        unitInfos.Add(new UnitInfo(unitName, unitDisplayName, range, experience, level, stats, overrideStats, modifiers));
                    }

                    map.units.Add(new UnitContainerInfo(x, y, owner, unitInfos));
                }   
            }

            return map;
        }

        // Utility method for serializing maps to binary files
        public void SaveMapToFile(string filePath, Map map)
        {
            using (FileStream fs = File.Open(filePath, FileMode.Create))
            {
                byte[] serializedMap = SaveMapToBytes(map);
                fs.Write(serializedMap, 0, serializedMap.Length);
            }
        }

        public byte[] SaveMapToBytes(Map map)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    int columns = map.columns;
                    int rows = map.rows;

                    // Header & metadata
                    writer.Write(headerMagic);
                    writer.Write(columns);
                    writer.Write(rows);
                    writer.Write(map.name);
                    writer.Write(map.description);
                    writer.Write(map.startTurn);
                    writer.Write(map.startPlayer);
                    writer.Write(map.playersInTurnOrder.Count);

                    foreach (int playerId in map.playersInTurnOrder)
                    {
                        writer.Write(playerId);  
                    }
                        
                    for (int i = 0; i < map.playersInTurnOrder.Count; i++)
                    {
                        writer.Write(map.isAi[i]);
                        writer.Write(map.useColor[i]);

                        if (map.useColor[i])
                        {
                            writer.Write(map.r[i]); 
                            writer.Write(map.g[i]);
                            writer.Write(map.b[i]);
                        }
                        writer.Write(map.resourceCount[i]);  
                        foreach (Resource resource in map.resourcesList[i])
                        {
                            writer.Write(resource.name);
                            writer.Write((int)resource.count);
                        }
                    }

                    writer.Write((Int16)map.paletteSize);  
                    for (int i = 0; i < map.paletteSize; i++)
                    {
                        writer.Write(map.prefabPath[i]);  
                    }

                    // Tiles
                    for (int i = 0; i < map.tiles.Count; i++) // map.tiles.Count should be equeal rows * columns
                    {
                        // for (int j = 0; j < columns; j++)
                        // {
                        string tilePalleteName = "";
                        switch(map.tiles[i].setIndex)
                        {
                            case 0:
                                {
                                    tilePalleteName += "forest_" + map.tiles[i].itemIndex.ToString();               
                                    break;
                                }
                            case 1:
                                {
                                    tilePalleteName += "grass_" + map.tiles[i].itemIndex.ToString();
                                    break;
                                }
                            case 2:
                                {
                                    tilePalleteName += "hills_" + map.tiles[i].itemIndex.ToString();
                                    break;
                                }
                            case 3:
                                {
                                    tilePalleteName += "mountains_" + map.tiles[i].itemIndex.ToString();
                                    break;
                                }
                            case 4:
                                {
                                    tilePalleteName += "swamp_" + map.tiles[i].itemIndex.ToString();
                                    break;
                                }
                            case 5:
                                {
                                    tilePalleteName += "water_" + map.tiles[i].itemIndex.ToString();
                                    break;
                                }
                            case 6:
                                {
                                    tilePalleteName += "roads_" + map.tiles[i].itemIndex.ToString();
                                    break;
                                }
                            case 7:
                                {
                                    tilePalleteName += "bridges_" + map.tiles[i].itemIndex.ToString();
                                    break;
                                }
                        }

                        for (int j = 0; j < map.prefabPath.Count; j++) //find tile name in pallete
                        {
                            if (map.prefabPath[j] == tilePalleteName)
                            {
                                writer.Write((Int16)j);  
                                break;
                            }
                        }
                    }

                    List<MapItem> overlayedTiles = map.tiles.FindAll(t => (t.objectSet != null) && (t.objectIndex != null));

                    map.overlayTilesCount = 0;
                    map.overlayTilesX.Clear();
                    map.overlayTilesY.Clear();
                    map.overlayTilesPrefabId.Clear();

                    foreach (MapItem tile in overlayedTiles)
                    {
                        map.overlayTilesCount++;
                        map.overlayTilesX.Add(tile.Xcoordinate);
                        map.overlayTilesY.Add(tile.Ycoordinate);
                        string tilePalleteName = "";
                        switch (tile.objectSet)
                        {
                            case 6:
                                {
                                    tilePalleteName += "roads_" + tile.objectIndex.ToString();
                                    break;
                                }
                            case 7:
                                {
                                    tilePalleteName += "bridges_" + tile.objectIndex.ToString();
                                    break;
                                }
                        }

                        
                        map.overlayTilesPrefabId.Add(map.prefabPath.FindIndex(p => p == tilePalleteName));
                    }

                    // Overlay tiles
                    writer.Write(map.overlayTilesCount);  
                    for (int i = 0; i < map.overlayTilesCount; i++)
                    {
                        writer.Write(map.overlayTilesX[i]);  
                        writer.Write(map.overlayTilesY[i]);  
                        writer.Write(map.overlayTilesPrefabId[i]);  
                    }
           
                    // Castles
                    writer.Write(map.castles.Count);  
                    foreach (CastleInfo castle in map.castles)
                    {
                        writer.Write(castle.prefabName);  
                        writer.Write(castle.castleName);  
                        writer.Write(castle.x);  
                        writer.Write(castle.y);  
                        writer.Write(castle.owner);  
                        writer.Write(castle.razed);  
                        writer.Write(castle.purchasedBuildings.Count);  
                        foreach (string building in castle.purchasedBuildings)
                        {
                            writer.Write(building);  
                        }

                        writer.Write(castle.producedUnitName);  
                        writer.Write(castle.producedUnitTurnsLeft);  
                        writer.Write(castle.producedUnitDestination);  

                        writer.Write(castle.unitQueue.Count);  
                        Queue<string> unitQueue = new Queue<string>(castle.unitQueue);
                        while (unitQueue.Count > 0)
                        {
                            writer.Write(unitQueue.Dequeue());  
                        }
                    }

                    // Ruins
                    writer.Write(map.ruins.Count);  
                    foreach (RuinsInfo ruin in map.ruins)
                    {
                        writer.Write(ruin.prefabName);  
                        writer.Write((int)ruin.x);  
                        writer.Write((int)ruin.y);  
                        writer.Write(ruin.isVisited);  
                    }

                    // Units
                    writer.Write(map.units.Count);  
                    foreach (UnitContainerInfo container in map.units)
                    {
                        writer.Write(container.x);  
                        writer.Write(container.y);  
                        writer.Write(container.owner);  
                        writer.Write(container.units.Count);  

                        foreach (UnitInfo unit in container.units)
                        {
                            writer.Write(unit.unitName);  
                            writer.Write(unit.unitDisplayName);  
                            writer.Write(unit.range);  
                            writer.Write(unit.experience);  
                            writer.Write(unit.level);  
                            writer.Write(unit.overrideStats);  

                            if (unit.overrideStats)
                            {
                                writer.Write(unit.stats.attack);  
                                writer.Write(unit.stats.defense);  
                                writer.Write(unit.stats.power);  
                                writer.Write(unit.stats.wisdom);  
                                writer.Write(unit.stats.hp);  
                                writer.Write(unit.stats.speed);  
                                writer.Write(unit.stats.damageMin);  
                                writer.Write(unit.stats.damageMax);  
                                writer.Write(unit.stats.timeToProduce);  
                                writer.Write(unit.stats.canShoot);  
                                writer.Write(unit.stats.canFly);  
                                writer.Write(unit.stats.range);  
                            }

                            // Modifiers
                            var modifiers = unit.modifiers.modifiers;
                            writer.Write(modifiers.Keys.Count);  
                            foreach (KeyValuePair<string, List<StatsModifiersEntry>> entry in modifiers)
                            {
                                writer.Write(entry.Key);  
                                writer.Write(entry.Value.Count);  
                                foreach (StatsModifiersEntry me in entry.Value)
                                {
                                    writer.Write(me.name);  
                                    writer.Write(me.value);  
                                    writer.Write((int)me.durationType);  
                                    writer.Write(me.duration);  
                                    writer.Write((int)me.type);  
                                }
                            }
                        }
                    }

                    writer.Flush();
                    return ms.ToArray();
                }
            }
        }
    }
}
