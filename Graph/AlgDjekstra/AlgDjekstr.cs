using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graph.AlgDjekstra
{
    class AlgDjekstr
    {
        static public List<EdgeDjekstra> Edges { get; set; }

        static public List<VertexDjekstra> Vertices { get; set; }

        static private Dictionary<Vertex, int> _distances;
        public AlgDjekstr(Graph g)
        {
            _distances = new Dictionary<Vertex, int>();


            foreach (var edge in g.E)
            {
                Edges.Add(new EdgeDjekstra(edge.V1, edge.V2, edge.Distance));
            }
            foreach(var vertex in g.V)
            {
                Vertices.Add(new VertexDjekstra(vertex));
            }
        }

        public static Dictionary<Vertex, int> AlgDjekstra(Vertex startVertex, Graph g)
        {
            _distances = new Dictionary<Vertex, int>();
            Edges = new List<EdgeDjekstra>();
            Vertices = new List<VertexDjekstra>();
            foreach (var edge in g.E)
            {
                Edges.Add(new EdgeDjekstra(edge.V1, edge.V2, edge.Distance));
            }
            foreach (var vertex in g.V)
            {
                Vertices.Add(new VertexDjekstra(vertex));
            }
            VertexDjekstra firstVertex = Vertices.Where(item => item.Number == startVertex.Number).First();
            firstVertex.IsVisited = true;
            firstVertex.CurrentMark = 0;
            _distances.Add(startVertex, 0);
            FindMinWays(firstVertex);
            return _distances;
        }

        static private void FindMinWays(VertexDjekstra vertex)
        {
            if (Vertices.Where(item => item.IsVisited != true).Count() != 0)
            {
                VertexDjekstra next = MakeStep(vertex);
                FindMinWays(next);
            }
        }
        static private  IEnumerable<VertexDjekstra> FindАdjacentVertexs(VertexDjekstra vertex)
        {
            SortedSet<VertexDjekstra> adjacentVertexs = new SortedSet<VertexDjekstra>();
            foreach (var e in Edges)
            {
                if (e.V1.Number == vertex.Number)
                {
                    adjacentVertexs.Add(e.V2);
                }
            }
            return adjacentVertexs;
        }

        static private EdgeDjekstra FindEdge(VertexDjekstra v1, VertexDjekstra v2)
        {
            return Edges.Where(item => item.V1.Number == v1.Number && item.V2.Number == v2.Number).FirstOrDefault();
        }

        private static VertexDjekstra MakeStep(VertexDjekstra vertex)
        {
            foreach(var v in FindАdjacentVertexs(vertex))
            {
                int CurrentMark = Math.Min(v.CurrentMark, vertex.CurrentMark + FindEdge(vertex, v).Distance);
                Vertices.Where(x => x.Number == v.Number).FirstOrDefault().CurrentMark = CurrentMark;
            }
            int min_dist = Vertices.Where(item => item.IsVisited!=true).Select(item => item.CurrentMark).Min();
            VertexDjekstra min_vert = Vertices.Where(item => item.IsVisited != true && item.CurrentMark == min_dist).First();
            min_vert.IsVisited = true;
            _distances.Add(min_vert, min_dist);
            return min_vert;
        }
        
        
    }
}
