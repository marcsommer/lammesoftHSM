using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LammeSoft.HSMLib;

namespace LammeSoft.HSMSamples
{
    public class Example4 : IExample
    {
        public CSharpGenerator Generator()
        {
            var s0 = new State();
            var s1 = new State();
            var m0 = new Method("calcul1", new Par("int", "a"));
            var x = new CSharpGenerator(s0);
            x.Add(new Transition(m0, s0, s1) { Condition = "a==0", Action = "// call" });

            return x;
        }
    }
}
