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
using System.Linq;
namespace LammeSoft.HSMLib
{
    public class Transition
    {
        private int index = int.MaxValue;
        public Transition(Method m, State from, State to)
        {
            Method = m;
            From = from;
            To = to;            
        }

        public Transition(Method m, State from) : this(m, from, null)
        {           
        }

        public Transition() { }

        public bool IsLocal
        {
            get { return To == null; }
        }

        public Method Method { get; internal set; }
        public State From { get; internal set; }
        public State To { get; internal set; }
        public string Condition { get; set; }
        public string Action { get; set; }
        public int Index { get { return index; } set { index = value; } }



    }
}
