using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarlordsMapEditor.Classes.ShitClasses
{
    class UnitInfo
    {
        public string unitName;
        public string unitDisplayName;
        public float range;
        public int experience;
        public int level;
        public Statistics stats;
        public bool overrideStats;
        public StatsModifiers modifiers;

        public UnitInfo(string unitName, string unitDisplayName, float range, int experience, int level, Statistics stats, bool overrideStats, StatsModifiers modifiers)
        {
            this.unitName = unitName;
            this.range = range;
            this.experience = experience;
            this.level = level;
            this.stats = stats;
            this.overrideStats = overrideStats;
            this.unitDisplayName = unitDisplayName;
            this.modifiers = modifiers;
        }
    }

    class UnitContainerInfo
    {
        public int x;
        public int y;
        public int owner;
        public List<UnitInfo> units;

        public UnitContainerInfo(int x, int y, int owner, List<UnitInfo> units)
        {
            this.x = x;
            this.y = y;
            this.owner = owner;
            this.units = units;
        }
    }
}
