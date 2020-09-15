using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Edge
    {
        public Vertex _v1 { get; set; }
        public Vertex _v2 { get; set; }

        public int _distance { get; set; }
      
        public Edge(Vertex v1, Vertex v2,int distance)
        {
            _v1 = v1;
            _v2 = v2;
            _distance = distance;
        }
    }
}
