using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LammeSoft.HSMLib;
using System.IO;

namespace LammeSoft.HSMSamples
{
    class Program
    {
        private static string dot = @"C:\Program Files (x86)\Graphviz 2.28\bin\dot.exe";

        public static void Run(IExample ex, int numero)
        {


            string path = @"c:\lammesoftHSM_Examples";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            CSharpGenerator x = ex.Generator();
            x.Nom = "lammesoftHSM.examples.ExampleEx"+numero;
            x.Path = path;
            x.GenerateCode();
            GraphViz.ToGraphviz(dot, x.Nom, x.Path, x.level1, true, x.Startup, 10);            
        }

        static void Main(string[] args)
        {
            List<IExample> gens = new List<IExample>();
            gens.Add(new Example1());
            gens.Add(new Example2());
            gens.Add(new Example3());
            gens.Add(new Example4());
            gens.Add(new Example5());
            gens.Add(new Example6());
            gens.Add(new Example7());       
            int i = 1;
            foreach (var g in gens)
            {
                Run(g, i++);
            }
        }
    }
}
