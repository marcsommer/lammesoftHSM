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
