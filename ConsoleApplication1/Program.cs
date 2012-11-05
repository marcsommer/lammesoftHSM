/*
 * This file is part of the LammeSoft HSM project.
 *
 * Copyright (C) 2011-2012 Bruno Nogent <bruno.nogent@hotmail.com> 
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301 USA
 */
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
