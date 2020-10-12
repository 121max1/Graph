using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Edge: IEquatable<Edge>
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

        public bool Equals(Edge other)
        {
            if (other is null)
                return false;
            return V1.Number == other.V1.Number && V2.Number == other.V2.Number;
        }
    }
}