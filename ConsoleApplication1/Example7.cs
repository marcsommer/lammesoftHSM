using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LammeSoft.HSMLib;

namespace LammeSoft.HSMSamples
{
    public class Example7 : IExample
    {
        public CSharpGenerator Generator()
        {
            var s0 = new State();
            var s1 = new State() { Parent = s0 };
            var s2 = new State() { Parent = s0 };
            var s3 = new State();
            var m1 = new Method("toto1");
            var m2 = new Method("q2");

            var x = new CSharpGenerator(s1);
            x.Add(new Transition(m1, s1, s2) { Action = "//action", Condition = "1==1" });
            x.Add(new Transition(m1, s2, s1));
            x.Add(new Transition(m2, s0, s3));

            return x;
        }
    }
}
