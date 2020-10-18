using Graph.AlgDjekstra;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Graph
{
    class Graph
    {
        public SortedSet<Vertex> V { get; set; } = new SortedSet<Vertex>();
        public List<Edge> E { get; set; } = new List<Edge>();
        List<Edge> tranposed_E = new List<Edge>();

        private static int _cntVertix = 0;

        private  Dictionary<string,int> _namesVertex = new Dictionary<string, int>();

        private readonly List<string> _order = new List<string>();
        private readonly List<string> _component = new List<string>();
        private readonly List<string> _isVisited = new List<string>();
        
        public Graph(Graph prev)
        {
            E = new List<Edge>(prev.E.AsEnumerable());
            V = new SortedSet<Vertex>(prev.V.AsEnumerable());
            _namesVertex = new Dictionary<string, int>(prev._namesVertex);
        }
        public Graph()
        {

        }
        public Graph(string name)
        {
            using (StreamReader file = new StreamReader(name, encoding:Encoding.Default))
            { 
                string[] namesVertex = file.ReadLine().Split();
                for (int i = 0; i < namesVertex.Length; i++)
                {
                    AddVertex(namesVertex[i]);
                }
                while(!file.EndOfStream)
                {
                    string[] edgeArr = file.ReadLine().Split();
                    AddEdge(edgeArr[0], edgeArr[1], int.Parse(edgeArr[2]));
                }
                
            }

        }

        public IEnumerable<Vertex> DFS(string v)
        {
            Stack<Vertex> stack = new Stack<Vertex>();
            List<Vertex> toReturn = new List<Vertex>();
            Vertex visited = V.Where(x => x.Number == _namesVertex[v]).First();
            stack.Push(visited);
            toReturn.Add(visited);
            SortedSet<Vertex> VisitedVertex = new SortedSet<Vertex>();
            VisitedVertex.Add(visited);
            while (stack.Count != 0)
            {
                Vertex s = stack.Pop();
                if(!VisitedVertex.Contains(s))
                {
                    toReturn.Add(s);
                }
                VisitedVertex.Add(s);
                foreach (var vert in FindАdjacentVertexs(s.Name).OrderByDescending(x => x.Number))
                {
                    if (!VisitedVertex.Contains(vert))
                    {
                        stack.Push(vert);
                        vert.IsVisited = true;
                    }
                }
            }
            return toReturn;
        }
        private void DFS1(string v)
        {
            _isVisited.Add(v);
            foreach(var vert in FindАdjacentVertexs(v).OrderByDescending(x => x.Number))
            {
                if(!_isVisited.Contains(vert.Name))
                {
                    DFS1(vert.Name);
                }
            }
            _order.Add(v);
        }
        
        private void DFS2(string v)
        {
            _isVisited.Add(v);
            _component.Add(v);
            foreach(var vert in FindTransposedАdjacentVertexs(v).OrderByDescending(x => x.Number))
            {
                if(!_isVisited.Contains(vert.Name))
                {
                    DFS2(vert.Name);
                }
            }
        }
        public IEnumerable<IEnumerable<string>> FindStrongRelatedComponents()
        {
            tranposed_E = Transpose(E);
            ClearFlags();
            foreach (var v in V)
            {
                if (!_isVisited.Contains(v.Name))
                {
                    DFS1(v.Name);
                }
            }
            ClearFlags();
            for (int i = 0; i < V.Count; ++i)
            {
                string v = _order[V.Count - 1 - i];
                if (!_isVisited.Contains(v))
                {
                    DFS2(v);
                    yield return _component;
                    _component.Clear();
                }
            }
        }
        private List<Edge> Transpose(List<Edge> list)
        {
            List<Edge> tranposed_edges = new List<Edge>();
            foreach(var edge in list)
            {
                tranposed_edges.Add(new Edge(new Vertex(edge.V2.Number, edge.V2.Name), new Vertex(edge.V1.Number, edge.V1.Name), edge.Distance));
            }
            return tranposed_edges;
        }
        
        private void ClearFlags()
        {
            _isVisited.Clear();  
        }
        public void Print()
        {
            foreach (var v in V)
            {
                Console.Write(v.Name + " ");
            }
            Console.WriteLine();
            foreach (var e in E)
            {
                Console.WriteLine(e.V1.Name + " " + e.V2.Name + " " + e.Distance);
            }
        }

        public int AddVertex(string Name)
        {
            
            V.Add(new Vertex(_cntVertix, Name));
            _namesVertex.Add(Name, _cntVertix);
            _cntVertix += 1;
            return _cntVertix;
        }

        public void AddEdge(string v1, string v2, int dist)
        {
            foreach (var v in V)
            {
                if (!V.Contains(new Vertex(_namesVertex[v1], v1)) || !V.Contains(new Vertex(_namesVertex[v2], v2)))
                {
                    throw new Exception("Вершины не найдены");
                }
            }
            foreach (var e in E)
            {
                if (e.V1.Number == _namesVertex[v1] && e.V2.Number == _namesVertex[v2])
                {
                    DeleteEdge(v1, v2);
                    E.Add(new Edge(new Vertex(_namesVertex[v1], v1), new Vertex(_namesVertex[v2], v2), dist));
                    return;
                }
            }
            E.Add(new Edge(new Vertex(_namesVertex[v1], v1), new Vertex(_namesVertex[v2], v2), dist));
        }
        
        public void DeleteVertex(string v)
        {
            var edgesToDelete = new List<Edge>();
            foreach (var edge in E)
            {
                if (edge.V1.Number == _namesVertex[v] || edge.V2.Number == _namesVertex[v])
                {
                    edgesToDelete.Add(edge);
                }
            }          
            foreach (var edge in edgesToDelete)
            {
                E.Remove(edge);
            }
            foreach (var edge in E)
            {
                if (edge.V1.Number > _namesVertex[v])
                {
                    edge.V1.Number -= 1;
                } 
                if (edge.V2.Number > _namesVertex[v])
                {
                    edge.V2.Number -= 1;
                }
            }
            V.Remove(new Vertex(_namesVertex[v],v));
            _namesVertex.Remove(v);
        }
        public IEnumerable<Vertex> FindАdjacentVertexs(string v)
        {
            SortedSet<Vertex> adjacentVertexs = new SortedSet<Vertex>();
            foreach(var e in E)
            {
                if(e.V1.Number==_namesVertex[v])
                {
                    adjacentVertexs.Add(e.V2);
                }
            }
            return adjacentVertexs;
        }

        public Vertex FindVertexWithMinDistancees()
        {
            Dictionary<Vertex, int> minDistancesVertex = new Dictionary<Vertex, int>();
            foreach(var vert in V)
            {
                minDistancesVertex.Add(vert, AlgDjekstr.AlgDjekstra(vert, this).Where(item => item.Value != int.MaxValue).Sum(item => item.Value));
            }
            return minDistancesVertex.OrderBy(item => item.Value).First().Key;
        }

        public IEnumerable<KeyValuePair<Vertex, int>> AlgFordBellman(Vertex vertex)
        {
            Dictionary<Vertex, int> disntances = new Dictionary<Vertex, int>();
            foreach (var v in V)
            {
                disntances.Add(v, int.MaxValue/2);
            }
            disntances[disntances.Where(v => v.Key == vertex).First().Key] = 0;

            for(int i = 0; i<V.Count()-1; ++i)
            {
                foreach(var edge in E)
                {
                    var v1 = V.Where(v => v == edge.V1).FirstOrDefault(); 
                    var v2 = V.Where(v => v == edge.V2).FirstOrDefault();
                    if (disntances[v1] < int.MaxValue / 2)
                    {
                        disntances[v2] = Math.Min(disntances[v2], disntances[v1] + edge.Distance);
                    }
                }
            }

            return disntances;
        }
        public IEnumerable<KeyValuePair<Vertex, int>> AlgFordBellmanNegative(Vertex vertex)
        {
            Dictionary<Vertex, int> disntances = new Dictionary<Vertex, int>();
            foreach (var v in V)
            {
                disntances.Add(v, int.MaxValue / 2);
            }

            disntances[disntances.Where(v => v.Key == vertex).First().Key] = 0;

            for (int i = 0; i < V.Count() - 1; ++i)
            {
                foreach (var edge in E)
                {
                    var v1 = V.Where(v => v == edge.V1).FirstOrDefault();
                    var v2 = V.Where(v => v == edge.V2).FirstOrDefault();
                    if(disntances[v1]<int.MaxValue/2)
                    {
                        if (disntances[v2]>disntances[v1]+edge.Distance)
                        {
                            disntances[v2] = Math.Max(-int.MaxValue, disntances[v1] + edge.Distance);
                        }
                    }
                    disntances[v2] = Math.Min(disntances[v2], disntances[v1] + edge.Distance);
                }
            }

            return disntances;
        }
        private int FindEccentricity(Vertex vertex)
        {
            List<int> minDist = new List<int>();
            foreach(var v in V)
            {
                if (v != vertex)
                {
                   minDist.Add(AlgFordBellman(v).Where(x => x.Key == vertex && x.Value != int.MaxValue / 2).FirstOrDefault().Value);
                }
            }
            return minDist.Max();
        }
        private int FindRadius()
        {
            List<int> Eccentricity = new List<int>();
            foreach(var v in V)
            {
                int findEccentricity = FindEccentricity(v);
                if (findEccentricity != 0)
                {
                    Eccentricity.Add(findEccentricity);
                }
            }
            return Eccentricity.Min();
        }
        public IEnumerable<Vertex> FindCenter()
        {
            int radius = FindRadius();
            foreach (var v in V)
            {
                if(FindEccentricity(v) == radius)
                {
                    yield return v;
                }
            }
        }
        private IEnumerable<Vertex> FindTransposedАdjacentVertexs(string v)
        {
            SortedSet<Vertex> adjacentVertexs = new SortedSet<Vertex>();
            foreach (var e in tranposed_E)
            {
                if (e.V1.Number == _namesVertex[v])
                {
                    adjacentVertexs.Add(e.V2);
                }
            }
            return adjacentVertexs;
        }
        public Graph GetdisorientedGraph()
        {
            Graph disorientedGraph = new Graph(this);
            foreach(var e in E)
            {
                if(!E.Contains(new Edge(new Vertex(e.V2.Number,e.V2.Name),new Vertex(e.V1.Number,e.V1.Name),e.Distance)))
                {
                    disorientedGraph.E.Add(new Edge(new Vertex(e.V2.Number, e.V2.Name), new Vertex(e.V1.Number, e.V1.Name), e.Distance));
                }    
            }
            return disorientedGraph;
        }
        public IEnumerable<Vertex> FindNonАdjacentVertexs(string v)
        {
            return V.Where(x => !FindАdjacentVertexs(v).Contains(new Vertex(x.Number, x.Name)) && x.Number!=_namesVertex[v]);
        }
        public void DeleteEdge(string v1, string v2)
        {
            Edge edgeToDelete = null;
            foreach (var edge in E)
            {
                if (edge.V1.Number == _namesVertex[v1] && edge.V2.Number == _namesVertex[v2])
                {
                    edgeToDelete = edge;
                }
            }
            E.Remove(edgeToDelete);
        }
        public IEnumerable<Edge> GetMinDistancesForEachPair()
        {
            foreach(var v in V)
            {
                foreach(var dist in AlgFordBellmanNegative(v))
                {
                    if (dist.Value != 0 && dist.Value < int.MaxValue / 2 - 1000)
                    {
                        yield return new Edge(v, dist.Key, dist.Value);
                    }
                }
            }
        }
        public void WriteMatrix(string name)
        {
            int[,] _matrix = new int[V.Count, V.Count];
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _matrix.GetLength(0); j++)
                {
                    _matrix[i, j] = 0;
                }
            }

            foreach (var edge in E)
            {
                _matrix[edge.V1.Number, edge.V2.Number] = edge.Distance;
            }

            using (StreamWriter writer = new StreamWriter(name,false, encoding: Encoding.Default))
            {
                writer.WriteLine(_matrix.GetLength(0));
                foreach(var v in V)
                {
                    writer.Write(v.Name + " ");
                }
                writer.WriteLine();
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < _matrix.GetLength(0); j++)
                    {
                        writer.Write(_matrix[i, j] + " ");
                    }
                    writer.WriteLine();
                }
                
            }
        }

        private IEnumerable<Edge> FindAdjacentEdges(Vertex vertex)
        {
            foreach(var e in E)
            {
                if (e.V1.Number == vertex.Number)
                {
                    yield return e;
                }
            }
        }
        private Edge FindMinEdgeInRelatedComponent(IEnumerable<Vertex> component, IEnumerable<Edge> edgesInComponent)
        {
            int min = E.Select(x => x.Distance).Max();
            SortedSet <Edge> minEdgesInVertexs = new SortedSet<Edge>();
            List<Edge> minEdges = new List<Edge>();
            foreach(var vertex in component)
            {
                foreach (var edge in FindAdjacentEdges(vertex))
                {
                    if (edge.Distance <= min && !edgesInComponent.Contains(edge))
                    {
                        minEdges.Add(edge);
                    }
                }

            }
            return minEdges.OrderBy(edge => edge.Distance).FirstOrDefault();
        }
        public IEnumerable<Edge> FindEdgesInRalatedComponents(IEnumerable<Vertex> component)
        {
            List<Edge> toReturn = new List<Edge>();
            foreach (var edge in E)
            {
                foreach (var vert1 in component)
                {
                    foreach (var vert2 in component)
                    {

                        if (edge.V1.Number == vert1.Number && edge.V2.Number == vert2.Number)
                        {
                            toReturn.Add(edge);
                        }

                    }
                }
            }
            return toReturn;
        }
        public Graph AlgBoruvka()
        {
            Graph T = new Graph();
            T.V = V;
            T._namesVertex = _namesVertex;

            while(T.FindRelatedComponents().Count()!=1)
            {
                foreach(var relatedComponent in T.FindRelatedComponents())
                {
                    Edge minEdgeInRelatedComponent = FindMinEdgeInRelatedComponent(relatedComponent, FindEdgesInRalatedComponents(relatedComponent));
                    if (minEdgeInRelatedComponent != null)
                    {
                        T.E.Add(minEdgeInRelatedComponent);
                    }
                }
            }
            return T.GetdisorientedGraph();
        }
        public IEnumerable<IEnumerable<Vertex>> FindRelatedComponents()
        {
            List<Vertex> relations = new List<Vertex>();
            V.ToList().ForEach(item => relations.Add(new Vertex(item.Number, item.Name)));
            while(relations.Count!=0)
            {
                List<Vertex> comp = new List<Vertex>();
                foreach(var v in DFS(relations.First().Name))
                {
                    comp.Add(v);
                }
                #region Mda
                List<Vertex> _toRemove = new List<Vertex>();
                foreach(var vert in relations)
                {
                    foreach (var v in comp)
                    {
                       if (v.Number == vert.Number)
                        {
                            _toRemove.Add(v);
                        }
                    }
                }
                foreach(var ver in _toRemove)
                {
                    relations.RemoveAll(x => x.Number == ver.Number);
                }
                #endregion
                yield return comp;
            }    
        }
        public bool IsDisorientedGraph()
        {
            foreach(var e in E)
            {
                if (!E.Contains(new Edge(new Vertex(e.V2.Number, e.V2.Name), new Vertex(e.V1.Number, e.V1.Name), e.Distance)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}