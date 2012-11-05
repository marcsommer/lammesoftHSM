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
namespace LammeSoft.HSMLib
{
    public class State
    {
        public State() { }

        private string name;

        public State(string label)
        {
            Name = label;
        }
        public string Name 
        {
            get 
            {
                return name; 
            }

            set 
            {
                name = value; 
            } 
        }
        public State Parent { get; set; }
        internal int UniqueId { get; set; }
        public string EnterAction { get; set; }
        public string ExitAction { get; set; }
        internal bool Visited { get; set; }        
    }
}
