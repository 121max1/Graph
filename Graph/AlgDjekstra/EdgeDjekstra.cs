using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.AlgDjekstra
{
    class EdgeDjekstra: IEquatable<EdgeDjekstra>
    {
        public VertexDjekstra V1 { get; set; }
        public VertexDjekstra V2 { get; set; }


        public int Distance { get; set; }

        public EdgeDjekstra(VertexDjekstra v1, VertexDjekstra v2, int distance)
        {
            V1 = v1;
            V2 = v2;
            Distance = distance;
        }
        public EdgeDjekstra(Vertex v1, Vertex v2, int distance)
        {
            V1 = new VertexDjekstra(v1.Number,v1.Name,v1.IsVisited);
            V2 = new VertexDjekstra(v2.Number, v2.Name, v2.IsVisited);
            Distance = distance;
        }

        public bool Equals(EdgeDjekstra other)
        {
            if (other is null)
                return false;
            return V1.Number == other.V1.Number && V2.Number == other.V2.Number;
        }
    }
}
