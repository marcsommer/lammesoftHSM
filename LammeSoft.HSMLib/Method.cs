using System.Linq;
using System.Collections.Generic;

namespace LammeSoft.HSMLib
{

    public class Par
    {
        public Par(string t, string n) { Type = t; Name = n; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class Method 
    {            
        public string Name { get; internal set; }       
        public List<Par> Parameters { get; internal set; }              
        public Method(string name, params Par[] parameters)
        {
            Name = name;
            Parameters = parameters.ToList();             
        }                       
    }
}
