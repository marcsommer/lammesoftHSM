using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LammeSoft.HSMLib;

namespace LammeSoft.HSMSamples
{
    public class Example5 : IExample
    {
        public CSharpGenerator Generator()
        {
            var s0 = new State();
            var s1 = new State();
            var s2 = new State();
            var m0 = new Method("calcul1", new Par("int", "a"));
            var x = new CSharpGenerator(s0);
            x.Add(new Transition(m0, s0, s1) { Action = "// default " });
            x.Add(new Transition(m0, s0, s2) { Condition = "a<=2", Action = "// call a<=2", Index = 2 });
            x.Add(new Transition(m0, s0, s1) { Condition = "a==1", Action = "// call a==1", Index = 1 });
            return x;
        }
    }
}
