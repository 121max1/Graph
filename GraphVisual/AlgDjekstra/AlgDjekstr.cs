using GraphVisual.Models;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph.AlgDjekstra
{
    class AlgDjekstr
    {
        static public List<EdgeDjekstra> Edges { get; set; }

        static public List<VertexDjekstra> Vertices { get; set; }

        static public Canvas grCanvas { get; set; }

        static private Dictionary<VertexView, int> _distances;
        public AlgDjekstr(GraphVisual.Graph g)
        {
            _distances = new Dictionary<VertexView, int>();


            foreach (var edge in g.E)
            {
                Edges.Add(new EdgeDjekstra(edge.V1, edge.V2, edge.Distance));
            }
            foreach(var vertex in g.V)
            {
                Vertices.Add(new VertexDjekstra(vertex));
            }
        }

        public async static Task<Dictionary<VertexView, int>> AlgDjekstra(int startVert, GraphVisual.Graph g, Canvas graphCanvas)
        {
            VertexView startVertex = g.V.Where(item => item.Number == startVert).First();
;            _distances = new Dictionary<VertexView, int>();
            Edges = new List<EdgeDjekstra>();
            Vertices = new List<VertexDjekstra>();
            grCanvas = graphCanvas;
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
            foreach(var element in grCanvas.Children)
            {
                if(element is TextBlock text)
                {
                    if(text.Tag.ToString().Split().Length==2)
                    {
                        if (text.Tag.ToString().Split()[1] == "Dj")
                        {
                            int vertexNum = int.Parse(text.Tag.ToString().Split()[0]);
                            if (vertexNum == startVertex.Number)
                            {
                                text.Text = "0";
                                text.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
                            }
                        }
                    }
                }
            }    
            await FindMinWays(firstVertex);
            return _distances;
        }

        static private async Task FindMinWays(VertexDjekstra vertex)
        {
            if (Vertices.Where(item => item.IsVisited != true).Count() != 0)
            {
                VertexDjekstra next = await MakeStep(vertex);
                await FindMinWays(next);
            }
        }
        static private  IEnumerable<VertexDjekstra> FindАdjacentVertexs(VertexDjekstra vertex)
        {
            SortedSet<VertexDjekstra> adjacentVertexs = new SortedSet<VertexDjekstra>();
            foreach (var e in Edges)
            {
                if (e.V1.Number == vertex.Number)
                {
                    if (e.V2.IsVisited == false)
                    {
                        adjacentVertexs.Add(e.V2);
                    }
                }
            }
            return adjacentVertexs;
        }

        static private EdgeDjekstra FindEdge(VertexDjekstra v1, VertexDjekstra v2)
        {
            return Edges.Where(item => item.V1.Number == v1.Number && item.V2.Number == v2.Number).FirstOrDefault();
        }

        private static async Task< VertexDjekstra> MakeStep(VertexDjekstra vertex)
        {
            foreach(var v in FindАdjacentVertexs(vertex))
            {
                int CurrentMark = Math.Min(v.CurrentMark, vertex.CurrentMark + FindEdge(vertex, v).Distance);
                Vertices.Where(x => x.Number == v.Number).FirstOrDefault().CurrentMark = CurrentMark;
                await Task.Delay(1000);
                foreach (var element in grCanvas.Children)
                {
                    if (element is Line line)
                    {
                        int v1 = int.Parse(line.Tag.ToString().Split()[0]);
                        int v2 = int.Parse(line.Tag.ToString().Split()[1]);
                        if (vertex.Number == v1 && v.Number == v2)
                        {
                            line.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 140));
                        }
                    }
                    else if (element is TextBlock text)
                    {
                        if (text.Tag.ToString().Split().Length == 2)
                        {
                            if (text.Tag.ToString().Split()[1] == "Dj")
                            {
                                int vertexNum = int.Parse(text.Tag.ToString().Split()[0]);
                                if (vertexNum == v.Number)
                                {
                                    text.Text = CurrentMark.ToString();
                                }
                            }
                        }
                    }
                }

            }
            int min_dist = Vertices.Where(item => item.IsVisited!=true).Select(item => item.CurrentMark).Min();
            VertexDjekstra min_vert = Vertices.Where(item => item.IsVisited != true && item.CurrentMark == min_dist).First();
            Vertices.Where(item => item.IsVisited != true && item.CurrentMark == min_dist).First().IsVisited = true;
            _distances.Add(min_vert, min_dist);
            foreach (var element in grCanvas.Children)
            {
                if (element is TextBlock text)
                {
                    if (text.Tag.ToString().Split().Length == 2)
                    {
                        if (text.Tag.ToString().Split()[1] == "Dj")
                        {
                            int vertexNum = int.Parse(text.Tag.ToString().Split()[0]);
                            if (vertexNum == min_vert.Number)
                            {
                                text.Text = min_dist.ToString();
                                text.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
                            }
                        }
                    }
                }
            }
            return min_vert;
        }
        
        
    }
}
