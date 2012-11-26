using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LammeSoft.HSMLib
{
    public class Machine<T>
    {
        public int state = 0;

        Dictionary<string, Op<T>> dic = new Dictionary<string, Op<T>>();

        public void Build(T self)
        {
            //ClassIf<T> test_etat1 = new ClassIf<T>();
            //test_etat1.Test = (x, p) => { return state == 0; };
            //test_etat1.d1 = () =>
            //{
            //    ClassIf<T> test1 = new ClassIf<T>();
            //    test1.Test = (x, p) => { Console.WriteLine("condition"); return true; };
            //    test1.d1 = () => { Console.WriteLine("cas 1"); };
            //    test1.d2 = () => { Console.WriteLine("cas 2"); };
            //    test1.Run(self);
            //    state = 1;
            //};
            //test_etat1.d2 = () => { };
            //dic.Add("m1", test_etat1);
        }
        public void Add(string name, Op<T> op/*, params object[] param*/)
        {
            dic.Add(name, op);
        }


        public void Action(T self, string name, params object[] param)
        {
            dic[name].Run(self, param);
        }
    }
    public interface Op<T>
    {
        void Run(T t, params object[] par);
    }

    public class ClassSwitch<T> : Op<T>
    {
        private Dictionary<int, Op<T>> dic = new Dictionary<int, Op<T>>();
        public void Add(int state, Op<T> op)
        {
            dic.Add(state, op);
        }
        public void Run(T t, params object[] par)
        {

        }
    }


    public class ClassIf<T> : Op<T>
    {
        public Func<T, object[], bool> Test { get; set; }
        public Action d1, d2;
        public void Run(T t, params object[] par)
        {
            if (Test(t, par))
            {
                d1();
            }
            else
            {
                d2();
            }
        }
    }

    public class MySwitch
    {



    }
}
