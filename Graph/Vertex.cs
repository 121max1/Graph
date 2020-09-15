using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Vertex:IComparable
    {
        public int number { get; set; }
        public Vertex(int number)
        {
            this.number = number;
        }

        public int CompareTo(object o)
        {
            Vertex v = o as Vertex;
            if (o != null)
            {
                return this.number.CompareTo(v.number);
            }
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }
}
