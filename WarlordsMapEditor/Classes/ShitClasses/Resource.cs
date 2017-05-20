using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public struct Resource
{
    public string name;
    public float count;

    public Resource(string name, float count)
    {
        this.name = name;
        this.count = count;
    }

}
