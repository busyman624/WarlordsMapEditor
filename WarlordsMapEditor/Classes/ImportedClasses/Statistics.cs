using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarlordsMapEditor.Classes.ImportedClasses
{
    public struct Statistics
    {
        public float attack;
        public float defense;
        public float power;
        public float wisdom;
        public float hp;
        public float speed;
        public float damageMin;
        public float damageMax;
        public float timeToProduce;
        public bool canShoot;
        public bool canFly;
        public float range;
    }

    public struct StatsModifiersEntry
    {
        public enum DurationType { battles, turns, forever };
        public enum ModifierType { percent, unit };// percent: attack + 50% / unit: attack + 5

        public string name;
        public float value;
        public DurationType durationType;
        public int duration;
        public ModifierType type;

        public StatsModifiersEntry(string name, float value, DurationType durationType, int duration, ModifierType type)
        {
            this.name = name;
            this.value = value;
            this.durationType = durationType;
            this.duration = duration;
            this.type = type;
        }
    }

    public class StatsModifiers
    {
        public Dictionary<string, List<StatsModifiersEntry>> modifiers = new Dictionary<string, List<StatsModifiersEntry>>();

        public void addModifier(string statType, StatsModifiersEntry entry)
        {
            if (!modifiers.ContainsKey(statType))
            {
                modifiers.Add(statType, new List<StatsModifiersEntry>());
            }
            removeModifier(statType, entry.name); //override modifier with the same name
            modifiers[statType].Add(entry);
        }

        public void removeModifier(string statType, string name)
        {
            if (!modifiers.ContainsKey(statType))
            {
                return;
            }
            modifiers[statType].RemoveAll(s => s.name == name);
        }

        //public Statistics applyModifiers(Statistics stats)
        //{
        //    foreach (string statType in modifiers.Keys)
        //    {
        //        float percentModifier = 0;
        //        float unitModifier = 0;
        //        foreach (StatsModifiersEntry entry in modifiers[statType])
        //        {
        //            if (entry.type == StatsModifiersEntry.ModifierType.percent)
        //                percentModifier += entry.value;
        //            else if (entry.type == StatsModifiersEntry.ModifierType.unit)
        //                unitModifier += entry.value;
        //        }
        //        float computedValue = (float)typeof(Statistics).GetField(statType, BindingFlags.Public | BindingFlags.Instance).GetValue(stats);
        //        computedValue += unitModifier;
        //        computedValue *= 1 + percentModifier;
        //        object statsModified = stats;
        //        typeof(Statistics).GetField(statType, BindingFlags.Public | BindingFlags.Instance).SetValue(statsModified, computedValue);
        //        stats = (Statistics)statsModified;
        //    }
        //    return stats;
        //}

        //public void WasInBattle()
        //{
        //    foreach (string statType in modifiers.Keys)
        //    {
        //        for (int i = 0; i < modifiers[statType].Count; i++)
        //        {
        //            StatsModifiersEntry entry = modifiers[statType][i];
        //            if (entry.durationType == StatsModifiersEntry.DurationType.battles)
        //            {
        //                entry.duration--;
        //                modifiers[statType][i] = entry;
        //                if (entry.duration <= 0)
        //                {
        //                    removeModifier(statType, entry.name);
        //                }
        //            }
        //        }
        //    }
        //}

        //public bool ModifierExist(string statType, string name)
        //{
        //    if (modifiers.ContainsKey(statType))
        //    {
        //        foreach (StatsModifiersEntry entry in modifiers[statType])
        //        {
        //            if (entry.name == name) return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
