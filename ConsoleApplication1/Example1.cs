using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LammeSoft.HSMLib;

namespace LammeSoft.HSMSamples
{
    public class Example1 : IExample
    {
        public CSharpGenerator Generator()
        {
            var s0 = new State();
            var s1 = new State();
            var m = new Method("HelloWorld");
            var x = new CSharpGenerator(s0);
            x.Add(new Transition(m, s0));
            return x;
        }
    }
}
