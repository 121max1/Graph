using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Graph.AlgDjekstra
{
    class VertexDjekstra : Vertex
    {
        public int CurrentMark { get; set; }

        public VertexDjekstra(int number, string Name): base(number,Name)
        {
            CurrentMark = int.MaxValue;
        }
        public VertexDjekstra(int number, string Name, bool visited) : base(number, Name, visited)
        {
            CurrentMark = int.MaxValue;
        }

        public VertexDjekstra(Vertex v)
        {
            Name = v.Name;
            IsVisited = v.IsVisited;
            Number = v.Number;
            CurrentMark = int.MaxValue/2;
        }
    }
}
