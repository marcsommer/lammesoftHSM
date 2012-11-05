
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
