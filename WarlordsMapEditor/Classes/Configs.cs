using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WarlordsMapEditor.Properties;

namespace WarlordsMapEditor
{
    public class Configs
    {
        public List<Fraction> fractions = new List<Fraction>();
        public List<RuinsData> ruinsData = new List<RuinsData>();

        public void SetRuinsConfig(string path)
        {
            ruinsData = DeserializeConfig<RuinsDataList>(path).ruinsData;
        }

        public void SetFractionsConfig(string path)
        {
            fractions = DeserializeConfig<FractionList>(path).fractions;
        }

        private T DeserializeConfig<T>(string path) where T : class
        {
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                return deserializer.Deserialize(reader) as T;
            }
        }
    }

    public class RuinsDataList
    {
        [XmlElement("Ruins")]
        public List<RuinsData> ruinsData = new List<RuinsData>();
    }

    public class RuinsData
    {
        [XmlAttribute]
        public string name;

        public List<string> sprites;
        public string action;
        public List<Resource> resourceBonus = new List<Resource>();
        public List<string> enemyUnits = new List<string>();

    }

    public class FractionList
    {
        public List<Fraction> fractions;
        public FractionList() { }
    }

    public class Fraction
    {
        [XmlAttribute]
        public string name;

        [XmlArray]
        [XmlArrayItem(ElementName = "building")]
        public List<string> buildings;
    }
}
