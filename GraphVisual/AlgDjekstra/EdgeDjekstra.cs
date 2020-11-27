using GraphVisual.Models;
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
        public EdgeDjekstra(VertexView v1, VertexView v2, int distance)
        {
            V1 = new VertexDjekstra()
            {
                IsVisited = v1.IsVisited,
                Number = v1.Number,
                Name = v1.Name
            };
            V2 = new VertexDjekstra()
            {
                IsVisited = v2.IsVisited,
                Number = v2.Number,
                Name = v2.Name
            };
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
