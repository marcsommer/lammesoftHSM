using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LammeSoft.HSMLib
{
    public class GraphViz
    {        
       

        public static void ToGraphviz(string DotPath, string name, string path, HSMResults items, bool showHeritages, State startup, int level,
            State stateToHighlight = null)
        {

            StringBuilder b = new StringBuilder();
            b.AppendLine("digraph G {");
            b.AppendLine("rankdir= LR;");

            b.AppendLine("node [shape=plaintext];");

            foreach (var st in items.States)
            {
                bool isStitHilightTarget = false;
                if(stateToHighlight!=null)
                {
                    isStitHilightTarget = items.Transitions.Any(p => p.From.Equals(stateToHighlight) && p.To != null && p.To.Equals(st));
                }
                //bool non_accessible = isStitHilightTarget || (stateToHighlight != null && st.Equals(stateToHighlight));
                //bool accessible = true;
                
                
                //b.AppendLine(st.Label + " [fontcolor="+(  non_accessible==false?"gray":"black"   )+", style=filled, fillcolor=" + (highLoght != null && st.Equals(highLoght) ? "red" : (isStitHilightTarget ==true? "orange" : "white")) + "]; ");


                string foreColor = (stateToHighlight != null && st.Equals(stateToHighlight) ? "#" + Colors.CurrentStateForeColor : (isStitHilightTarget == true ? "#" + Colors.AccessibleStatesForeColor : "#" + Colors.InAccessibleStatesForeColor));
                string backColor = (stateToHighlight != null && st.Equals(stateToHighlight) ? "#" + Colors.CurrentStateBackColor : (isStitHilightTarget == true ? "#" + Colors.AccessibleStatesBackColor : "#" + Colors.InAccessibleStatesBackColor));


                b.AppendLine(st.Name + " [label=<<TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"5\"><TR><TD CELLPADDING=\"5\" BGCOLOR=\"" + backColor + "\" ><FONT FACE=\"Arial\" POINT-SIZE=\"12\" COLOR=\"" +foreColor + "\">" + st.Name + "</FONT></TD></TR></TABLE>>]; ");

            }
            //if (highLoght!=null)
            //    b.AppendLine(highLoght.Label + " [style=filled, fillcolor=yellow] ");


            b.AppendLine("_ [shape=point];");
            b.AppendLine("_-> " + startup.Name + ";");

            foreach (var t in items.Transitions)
            {               
                var edgeLabel = t.Method.Name + "(" + string.Join(",", t.Method.Parameters.Select(j => j.Name)) + ")";   

                if (level > 0)
                    edgeLabel += (t.Condition != null ? "\\n[" + t.Condition + "]" : "");
                if (level > 1)
                    edgeLabel += (t.Action != null ? "\\n(" + t.Action + ")" : "");


                if (t.IsLocal)
                {
                    b.AppendLine(t.From.Name + " -> " + t.From.Name + " " + "[label=\"" + edgeLabel + "\",style=dotted,color=\"magenta\"];");
                }
                else
                {
                    if (stateToHighlight!=null && t.From.Name.Equals(stateToHighlight.Name))
                    {
                        b.AppendLine(t.From.Name + " -> " + t.To.Name + " " + "[label=\"" + edgeLabel + "\",color=\"#"+Colors.OperationPossibleColor+"\", fontcolor =\"#"+Colors.OperationPossibleColor+"\"];");

                    }
                    else
                    {
                        b.AppendLine(t.From.Name + " -> " + t.To.Name + " " + "[label=\"" + edgeLabel + "\",color=\"#" + Colors.OperationImpossibleColor + "\", fontcolor =\"#" + Colors.OperationImpossibleColor + "\"];");
                    }
                    
                }
            }

            if (showHeritages)
            {
                foreach (var e in items.States)
                {
                    State ei = e;
                    if (ei.Parent != null)
                        b.AppendLine(ei.Name + " -> " + ei.Parent.Name + " " + "[color=\"red\"];");
                }
            }
            b.AppendLine("}");

            string dotfile = path + @"\" + name +    (stateToHighlight!=null ? "_"+stateToHighlight.Name:"")          +   ".dot";            
            File.WriteAllText(dotfile, b.ToString());

            ProcessStartInfo pi = new ProcessStartInfo(DotPath, " -Tpng -O " + dotfile);
            pi.UseShellExecute = false;
            Process proc = Process.Start(pi);
            proc.WaitForExit();

        }


    }
}
