using System.Collections.Generic;

namespace LammeSoft.HSMLib
{
    public class HSMResults
    {
        public IEnumerable<State> States { get; set; }
        public IEnumerable<Transition> Transitions { get; set; }
        
    }

}
