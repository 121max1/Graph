using GraphVisual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisual
{
    class Graph
    {
        public SortedSet<VertexView> V { get; set; } = new SortedSet<VertexView>();
        public List<EdgeView> E { get; set; } = new List<EdgeView>();


    }
}
