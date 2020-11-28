using GraphVisual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Graph.AlgDjekstra
{
    class VertexDjekstra : VertexView
    {
        public int CurrentMark { get; set; }

        //public VertexDjekstra(int number, string Name): base(number,Name)
        //{
        //    CurrentMark = int.MaxValue;
        //}
        //public VertexDjekstra(int number, string Name, bool visited) : base(number, Name, visited)
        //{
        //    CurrentMark = int.MaxValue;
        //}

        public VertexDjekstra(VertexView v)
        {
            Name = v.Name;
            IsVisited = v.IsVisited;
            Number = v.Number;
            CurrentMark = int.MaxValue / 2;
            X = v.X;
            Y = v.Y;
        }

        public VertexDjekstra()
        {
            CurrentMark = int.MaxValue / 2;
        }
    }
}
