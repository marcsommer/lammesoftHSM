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
using System.IO;

namespace LammeSoft.HSMLib
{
    public class CSharpGenerator
    {
        #region pseudo
          public string StateNamePattern {get;set;}       
        public  State Startup { get;  set; }  
        public  HSMResults level0, level1;
        public List<State> states = new List<State>();
        public List<string> statesNames = new List<string>();


        
        private void StoreState(State st)
        {
            if (st == null)
                return;
            if (!states.Contains(st))
            {
                states.Add(st);

                if (st.Name == null)
                {
                    int k = 0;
                    while (statesNames.Contains("s" + k))
                    {
                        k++;
                    }
                    st.Name = "s" + k;

                }                
                statesNames.Add(st.Name);                
            }

        }
        






        public void BuildPseudo( IEnumerable<Transition> _userTransitions)
        {           
         
                     
            #region Record States                        
            StoreState(Startup);            
            foreach (var tr in _userTransitions)
            {
                StoreState(tr.From);
                StoreState(tr.To);              
            }            
            foreach (var etat in states.ToList()) // ToList to create a copy of states from transition
            {
                State et = etat;
                while (et.Parent != null)
                {
                    StoreState(et.Parent);
                    et = et.Parent;
                }
            }
            // nomage des etats non deja nomes
            int serieNum = 0;
            while (states.Any(p => String.IsNullOrWhiteSpace(p.Name)))
            {
                var x = states.First(p => String.IsNullOrWhiteSpace(p.Name));
                while (states.Where(p => !String.IsNullOrWhiteSpace(p.Name)).Any(p => p.Name.Equals(string.Format(StateNamePattern, serieNum))))
                    serieNum++;
                x.Name = string.Format(StateNamePattern, serieNum);
            }
            #endregion

            
            
            #region Version non reduite            
            level0 = new HSMResults();
            level0.States = states.ToList(); // copies
            level0.Transitions = _userTransitions.ToList(); // copies
            #endregion



            #region Reduction / Normalisation des transitions

            List<Transition> transitions = new List<Transition>();
            foreach (var st in states)
            {
                List<Transition> stTransitions = new List<Transition>();
                State i = st;
                while (i != null)
                {
                    var transitionsPossibles = _userTransitions.Where(p => p.From.Equals(i)).ToList();
                    var methodesPossibles = transitionsPossibles.Select(p => p.Method).Distinct();

                    List<Method> mthsToAdd = new List<Method>();
                    foreach (var mp in methodesPossibles)
                        if (!stTransitions.Any(p => p.Method.Equals(mp)))
                            mthsToAdd.Add(mp);


                    foreach (var mtiToAdd in mthsToAdd)
                    {
                        stTransitions.AddRange(transitionsPossibles.Where(p => p.Method.Equals(mtiToAdd)).Select(h => new Transition()
                        {
                            From = st,
                            To = h.To,
                            Action = h.Action,
                            Condition = h.Condition,
                            Index = h.Index,
                            Method = h.Method
                        }));
                    }
                    i = i.Parent;
                }
                transitions.AddRange(stTransitions);
            }

            VisitAllStates(transitions, Startup);

            //suppression des states inutiles
            {
                var statesToDelete = states.Where(p => p.Visited == false).ToList();
                while (statesToDelete.Count > 0)
                {
                    //Console.WriteLine("\tsuppresion etat {0}", statesToDelete[0].Label);                    
                    var trToDelete = transitions.Where(p => p.From.Equals(statesToDelete[0]) || (!p.IsLocal && p.To.Equals(statesToDelete[0]))).ToList();
                    while (trToDelete.Count > 0)
                    {
                        transitions.Remove(trToDelete[0]);
                        trToDelete.RemoveAt(0);
                    }
                    states.Remove(statesToDelete[0]);
                    statesToDelete.RemoveAt(0);
                }
            }


            //Numerotation des etats
            for (int i = 0; i < states.Count; i++)
                states[i].UniqueId = i;


            level1 = new HSMResults();
            level1.States = states.ToList(); // copies
            level1.Transitions = transitions.ToList(); // copies

            #endregion
        }

        private static void VisitAllStates(List<Transition> transitions, State s)
        {
            s.Visited = true;
            var tr = transitions.Where(p => p.From.Equals(s) && !p.IsLocal);
            foreach (var ti in tr)
            {
                if (!ti.To.Visited)
                    VisitAllStates(transitions, ti.To);
            }
        }



        #endregion








        

        public string Path { get; set; }        
        public string Nom { get; set; }        
        public string StateIdMemberName { get; set; }                
        public string CustomCode { get; set; }
        public string CustomCtor { get; set; }
        public string Comments { get; set; }
        public string Imports { get; set; }

        public List<Transition> _trs = new List<Transition>();
       
        public CSharpGenerator(State s)
        {
            StateNamePattern = "S{0}";    
            StateIdMemberName = "_s";
            Startup = s;            
        }


        public void Add(Transition t) 
        {
            _trs.Add(t);
        }


        private string Init(State from, State to, string action)
        {
            StringBuilder b = new StringBuilder();

            if (to != null) // CHANGEMENT D'ETAT
            {
                if (from != null && from.ExitAction != null)
                {
                    b.AppendLine(from.ExitAction );
                }

                if (action != null)
                {
                    b.AppendLine(action);
                }

                if (to.EnterAction != null)
                {
                    b.AppendLine(to.EnterAction);
                }



                b.AppendLine(StateIdMemberName + " = " + to.UniqueId + ";");
               


            }
            else // on reste sur l'etat, pas d'entree/sortie, juste l'action a traiter               
            {
                if (action!=null)
                {
                    b.AppendLine(action);
                }
            }



            return b.ToString();
        }

        public Machine<T> GenerateCodeDynamic<T>()
        {

            BuildPseudo(_trs);


            Machine<T> ma = new Machine<T>();


            StringBuilder _builder = new StringBuilder();
            //ctor
            //_builder.AppendLine("public " + ClassName + "()");
            _builder.AppendLine("{");
            _builder.AppendLine(Init(null, Startup, null));
            _builder.AppendLine(CustomCtor);
            _builder.AppendLine("}");


            List<Transition> transitions = level1.Transitions.ToList();
            var methodes = transitions.Select(p => p.Method).Distinct();
            foreach (var m in methodes)
            {
                _builder.AppendLine("public void " + m.Name + "(" + string.Join(",", m.Parameters.Select(o => o.Type + " " + o.Name)) + ")");
                _builder.AppendLine("{"); // start methode


                ClassSwitch<T> sw = new ClassSwitch<T>();
                ma.Add("m" + m.GetHashCode(), sw);



                var mtrs = transitions.Where(p => p.Method.Equals(m));
                var froms = mtrs.Select(p => p.From).Distinct();

                _builder.AppendLine("switch( " + StateIdMemberName + ") {");
                foreach (var fr in froms)
                {
                    var mtos = mtrs.Where(p => p.From.Equals(fr));
                    var mtos2 = new List<Transition>();
                    mtos2.AddRange(mtos.Where(p => !string.IsNullOrWhiteSpace(p.Condition)).OrderBy(p => p.Index)); // cas avec condition
                    var sansConditions = mtos.Where(p => string.IsNullOrWhiteSpace(p.Condition));
                    if (sansConditions.Count() > 1)
                        throw new Exception("Multiples destinations pour sans condition ! : Pas resolvable");
                    mtos2.AddRange(sansConditions); // cas sans condition <=1 !!!!!

                    _builder.AppendLine("case " + fr.UniqueId + " : ");

                    sw.Add(fr.UniqueId, null);

                    #region Block
                    int pass = 0;
                    foreach (var p in mtos2)
                    {
                        if (pass > 0)
                            _builder.AppendLine("else");
                        if (p.Condition != null)
                        {
                            _builder.AppendLine("if ( " + p.Condition + ")");
                            _builder.AppendLine("{");
                            _builder.AppendLine(Init(p.From, p.To, p.Action));
                            _builder.AppendLine("}");
                        }
                        else
                        {
                            _builder.AppendLine("{");
                            _builder.AppendLine(Init(p.From, p.To, p.Action));
                            _builder.AppendLine("}");
                        }
                        pass++;
                    }
                    #endregion
                    _builder.AppendLine("break; ");

                }
                _builder.AppendLine("}"); // end switch






                _builder.AppendLine("}"); // end method
            }


            _builder.AppendLine(CustomCode);

            _builder.AppendLine("}"); // end class
            _builder.AppendLine("}"); // end namespace

            //Console.WriteLine(_builder.ToString());
            //File.WriteAllText(System.IO.Path.Combine(Path, ClassName + ".cs"), _builder.ToString());

            return ma;
        }
           
        public  void GenerateCode()
        {

            BuildPseudo(_trs);



            int idx = Nom.LastIndexOf('.');
            string NameSpaceName = Nom.Substring(0, idx);
            string ClassName = Nom.Substring(idx + 1);


            StringBuilder _builder = new StringBuilder();
                
            _builder.AppendLine(Imports);
            
            _builder.AppendLine("namespace "+ NameSpaceName);
            _builder.AppendLine("{"); // begin namespace
            _builder.AppendLine("public partial class " + ClassName         ); // notify, partial !
            _builder.AppendLine("{"); // end namespace

            // state id value
            _builder.AppendLine("private int " + StateIdMemberName+";");                             


            //ctor
            _builder.AppendLine("public " + ClassName + "()");
            _builder.AppendLine("{");
            _builder.AppendLine(Init(null, Startup, null));
            _builder.AppendLine(CustomCtor);
            _builder.AppendLine("}");


            List<Transition> transitions = level1.Transitions.ToList();
            var methodes = transitions.Select(p => p.Method).Distinct();
            foreach (var m in methodes)
            {
                _builder.AppendLine("public void " + m.Name + "(" +     string.Join(",", m.Parameters.Select(o=> o.Type+ " " + o.Name ))      + ")");
                _builder.AppendLine("{"); // start methode
                var mtrs = transitions.Where(p => p.Method.Equals(m));
                var froms = mtrs.Select(p => p.From).Distinct();
            
                _builder.AppendLine("switch( " + StateIdMemberName + ") {");
                foreach (var fr in froms)
                {                    
                    var mtos = mtrs.Where(p => p.From.Equals(fr));
                    var mtos2 = new List<Transition>();
                    mtos2.AddRange(mtos.Where(p => !string.IsNullOrWhiteSpace(p.Condition)).OrderBy(p => p.Index)); // cas avec condition
                    var sansConditions = mtos.Where(p => string.IsNullOrWhiteSpace(p.Condition));
                    if (sansConditions.Count() > 1)
                        throw new Exception("Multiples destinations pour sans condition ! : Pas resolvable");
                    mtos2.AddRange(sansConditions); // cas sans condition <=1 !!!!!
                                                             
                        _builder.AppendLine("case " + fr.UniqueId + " : ");
                        

                        #region Block 
                        int pass = 0;
                        foreach (var p in mtos2)
                        {
                            if (pass > 0)
                                _builder.AppendLine("else");
                            if (p.Condition != null)
                            {
                                _builder.AppendLine("if ( " + p.Condition + ")");
                                _builder.AppendLine("{");
                                _builder.AppendLine(Init(p.From, p.To, p.Action));
                                _builder.AppendLine("}");
                            }
                            else
                            {
                                _builder.AppendLine("{");
                                _builder.AppendLine(Init(p.From, p.To, p.Action));
                                _builder.AppendLine("}");
                            }
                            pass++;
                        }                         
                        #endregion
                        _builder.AppendLine("break; ");
                   
                }
                _builder.AppendLine("}"); // end switch
                _builder.AppendLine("}"); // end method
            }


            _builder.AppendLine(CustomCode);

            _builder.AppendLine("}"); // end class
            _builder.AppendLine("}"); // end namespace

            Console.WriteLine(_builder.ToString());
            File.WriteAllText(System.IO.Path.Combine( Path, ClassName+".cs"), _builder.ToString());
                    
        }


        

        public string NewProperty(string type, string name)
        {
            string s = "";
            s += "private " + type + " _" + name + ";\n";
            s += "public " + type + " " + name + " { get {return _" + name + ";} set { _" + name + "=value;        } }";
            return s;

        }

       
    }
}
