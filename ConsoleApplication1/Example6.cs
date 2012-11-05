using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LammeSoft.HSMLib;

namespace LammeSoft.HSMSamples
{
    public class Example6 : IExample
    {
        public CSharpGenerator Generator()
        {
            var s0 = new State();
            var s1 = new State() { Parent = s0 };
            var s2 = new State() { Parent = s0 };
            var s3 = new State();

            var m0 = new Method("m0");
            var m1 = new Method("m1");
            var x = new CSharpGenerator(s1);

            x.Add(new Transition(m0, s1, s2));
            x.Add(new Transition(m0, s2, s1));
            x.Add(new Transition(m1, s0, s3));
            return x;
        }
    }
}
