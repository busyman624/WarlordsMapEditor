using System.Collections.Generic;

namespace WarlordsMapEditor.Classes.ShitClasses
{
    class CastleInfo
    {
        public string prefabName;
        public string castleName;
        public int x;
        public int y;
        public int owner;
        public bool razed;
        public HashSet<string> purchasedBuildings;
        public string producedUnitName;
        public int producedUnitTurnsLeft;
        public string producedUnitDestination;
        public Queue<string> unitQueue;

        public CastleInfo(string prefabName, string castleName, int x, int y, int owner, bool razed, HashSet<string> purchasedBuildings,
                          string producedUnitName, int producedUnitTurnsLeft, string producedUnitDestination, Queue<string> unitQueue)
        {
            this.prefabName = prefabName;
            this.castleName = castleName;
            this.x = x;
            this.y = y;
            this.owner = owner;
            this.razed = razed;
            this.purchasedBuildings = purchasedBuildings;
            this.producedUnitName = producedUnitName;
            this.producedUnitTurnsLeft = producedUnitTurnsLeft;
            this.producedUnitDestination = producedUnitDestination;
            this.unitQueue = unitQueue;
        }
    }
}
