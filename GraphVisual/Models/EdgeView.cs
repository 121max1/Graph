using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisual.Models
{
    class EdgeView
    {
        public VertexView V1 { get; set; }

        public VertexView V2 { get; set; }

        public int Distance { get; set; }

        public bool? IsOriented {get;set;}

        public EdgeView()
        {

        }
        public EdgeView(VertexView v1, VertexView v2, int distance)
        {
            V1 = v1;
            V2 = v2;
            Distance = distance;
        }
        public bool Equals(EdgeView other)
        {
            if (other is null)
                return false;
            return V1.Number == other.V1.Number && V2.Number == other.V2.Number;
        }
    }
}
