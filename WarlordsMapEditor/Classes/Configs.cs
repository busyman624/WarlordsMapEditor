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

        public Configs()
        {
            fractions = DeserializeDefaultConfig<FractionList>(Resources.Fractions).fractions;
            ruinsData = DeserializeDefaultConfig<RuinsDataList>(Resources.Ruins).ruinsData;
        }

        public Configs(int? empty)
        {
            fractions = null;
            ruinsData = null;
        }

        public string SetRuinsConfig()
        {

            Microsoft.Win32.OpenFileDialog ruinsXML = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            ruinsXML.DefaultExt = ".xml";
            ruinsXML.Filter = "(*.xml)|*.xml";
            ruinsXML.Title = "Choose Ruins Configuration File";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> ruinsXMLresult = ruinsXML.ShowDialog();
            if (ruinsXMLresult == true)
            {
                string filename = ruinsXML.FileName;
                ruinsData = DeserializeConfig<RuinsDataList>(filename).ruinsData;
                return ruinsXML.SafeFileName;
            }
            else return null;

        }

        public string SetFractionsConfig()
        {
            Microsoft.Win32.OpenFileDialog fractionsXML = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            fractionsXML.DefaultExt = ".xml";
            fractionsXML.Filter = "(*.xml)|*.xml";
            fractionsXML.Title = "Choose Fractions Configuration File";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> fractionsXMLresult = fractionsXML.ShowDialog();
            if (fractionsXMLresult == true)
            {
                string filename = fractionsXML.FileName;
                fractions = DeserializeConfig<FractionList>(filename).fractions;
                return fractionsXML.SafeFileName;
            }
            else return null;

        }

        private T DeserializeConfig<T>(string path) where T : class
        {
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                return deserializer.Deserialize(reader) as T;
            }
        }

        private T DeserializeDefaultConfig<T>(string xml) where T : class
        {
            using (TextReader reader = new StringReader(xml))
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
