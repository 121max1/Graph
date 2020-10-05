using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Edge:IComparable
    {
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }


        public int Distance { get; set; }

        public Edge(Vertex v1, Vertex v2, int distance)
        {
            V1 = v1;
            V2 = v2;
            Distance = distance;
        }

        public int CompareTo(object obj)
        {
            Edge v = obj as Edge;
            if (obj != null)
            {
                return this.Distance.CompareTo(v.Distance);
            }
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }
}