using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarlordsMapEditor.Classes.ShitClasses
{
    class RuinsInfo
    {
        public string prefabName;
        public int x;
        public int y;
        public bool isVisited;

        public RuinsInfo(string prefabName, int x, int y, bool isVisited)
        {
            this.prefabName = prefabName;
            this.x = x;
            this.y = y;
            this.isVisited = isVisited;
        }
    }
}
