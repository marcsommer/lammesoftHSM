﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LammeSoft.HSMLib;

namespace LammeSoft.HSMSamples
{
    public class Example3 : IExample
    {
        public CSharpGenerator Generator()
        {
            var s0 = new State();
            var s1 = new State();
            var m0 = new Method("f");
            var x = new CSharpGenerator(s0);
            x.Add(new Transition(m0, s0, s1));
            x.Add(new Transition(m0, s1, s0));
            return x;
        }
    }
}
