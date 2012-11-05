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
