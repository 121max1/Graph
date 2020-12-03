using GraphVisual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphVisual
{
    class Graph 
    {
        public SortedSet<VertexView> V { get; set; } = new SortedSet<VertexView>();
        public List<EdgeView> E { get; set; } = new List<EdgeView>();

        public static int cntVertix = 1;
        public void AddVertex(VertexView vertex)
        {
            V.Add(vertex);
            cntVertix += 1;
        }
        public void DeleteVertex(VertexView vertex)
        {
            var edgesToDelete = new List<EdgeView>();
            foreach (var edge in E)
            {
                if (edge.V1.Number == vertex.Number || edge.V2.Number == vertex.Number)
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
                if (edge.V1.Number > vertex.Number)
                {
                    edge.V1.Number -= 1;
                }
                if (edge.V2.Number > vertex.Number)
                {
                    edge.V2.Number -= 1;
                }
            }
            V.Remove(vertex);

        }
        public void DeleteEdge(int v1, int v2)
        {
            EdgeView edgeToDelete = null;
            foreach (var edge in E)
            {
                if (edge.V1.Number == v1 && edge.V2.Number == v2)
                {
                    edgeToDelete = edge;
                    E.Remove(edgeToDelete);
                    break;
                }
            }
            if(edgeToDelete.IsOriented == false)
            {
                foreach (var edge in E)
                {
                    if (edge.V1.Number == v2 && edge.V2.Number == v1)
                    {
                        E.Remove(edge);
                        break;
                    }
                }
            }    
        }
        public void AddEdge(EdgeView edge)
        {
            foreach (var e in E)
            {
          
                if (edge.IsOriented == true && e.V1.Number == edge.V1.Number && e.V2.Number == edge.V2.Number
                    || edge.IsOriented == false && (e.V1.Number == edge.V1.Number && e.V2.Number == edge.V2.Number
                    || e.V2.Number == edge.V1.Number && e.V1.Number == edge.V2.Number) )
                {
                    throw new Exception("Edge is already exists");
                }
                else if (edge.IsOriented == true && e.V1.Number == edge.V2.Number && e.V2.Number == edge.V1.Number)
                {
                    E.Add(edge);
                    throw new Exception("Add new nonOriented edge");
                }
            }
            if(edge.IsOriented == false)
            {
                E.Add(new EdgeView() { V1 = edge.V2, V2 = edge.V1, Distance = edge.Distance, IsOriented = edge.IsOriented }) ;
            }
            E.Add(edge);
        }
        public VertexView GetVertexByNumber(int number)
        {
            return V.Where(vert => vert.Number == number).FirstOrDefault();
        }

        public IEnumerable<VertexView> FindАdjacentVertexs(int v_num)
        {
            SortedSet<VertexView> adjacentVertexs = new SortedSet<VertexView>();
            foreach (var e in E)
            {
                if (e.V1.Number == v_num)
                {
                    adjacentVertexs.Add(e.V2);
                }
            }
            return adjacentVertexs;
        }

        public IEnumerable<VertexView> DFSLearningMode(int num_v)
        {
            Stack<VertexView> stack = new Stack<VertexView>();
            List<VertexView> toReturn = new List<VertexView>();
            VertexView visited = V.Where(x => x.Number == num_v).First();
            stack.Push(visited);
            toReturn.Add(visited);
            SortedSet<VertexView> VisitedVertex = new SortedSet<VertexView>();
            VisitedVertex.Add(visited);
            while (stack.Count != 0)
            {
                VertexView s = stack.Pop();
                if (!VisitedVertex.Contains(s))
                {
                    toReturn.Add(s);
                }
                VisitedVertex.Add(s);
                foreach (var vert in FindАdjacentVertexs(s.Number).OrderByDescending(x => x.Number))
                {
                    if (!VisitedVertex.Contains(vert))
                    {
                        stack.Push(vert);

                    }
                }
            }
            return toReturn;
        }
        public IEnumerable<VertexView> DFS(VertexView v)
        {
            Stack<VertexView> stack = new Stack<VertexView>();
            List<VertexView> toReturn = new List<VertexView>();
            VertexView visited = V.Where(x => x.Number == v.Number).First();
            stack.Push(visited);
            toReturn.Add(visited);
            SortedSet<VertexView> VisitedVertex = new SortedSet<VertexView>();
            VisitedVertex.Add(visited);
            while (stack.Count != 0)
            {
                VertexView s = stack.Pop();
                if (!VisitedVertex.Contains(s))
                {
                    toReturn.Add(s);
                }
                VisitedVertex.Add(s);
                foreach (var vert in FindАdjacentVertexs(v.Number).OrderByDescending(x => x.Number))
                {
                    if (!VisitedVertex.Contains(vert))
                    {
                        stack.Push(vert);
          
                    }
                }
            }
            return toReturn;
        }

        public async Task<IEnumerable<VertexView>> DFS(int v_num, Canvas graphCanvas)
        {
            Stack<VertexView> stack = new Stack<VertexView>();
            List<VertexView> toReturn = new List<VertexView>();
            VertexView visited = V.Where(x => x.Number == v_num).First();
            foreach(var elem in graphCanvas.Children)
            {
                if (elem is Ellipse ellipse)
                {
                    if ((int)ellipse.Tag == visited.Number)
                    {
                        ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 140, 0));
                    }
                }
            }
            stack.Push(visited);
            toReturn.Add(visited);
            SortedSet<VertexView> VisitedVertex = new SortedSet<VertexView>();
            VisitedVertex.Add(visited);
            while (stack.Count != 0)
            {
                VertexView s = stack.Pop();
                if (!VisitedVertex.Contains(s))
                {
                    toReturn.Add(s);
                    await Task.Delay(2000);
                    foreach (var elem in graphCanvas.Children)
                    {
                        if (elem is Ellipse ellipse)
                        {
                            if ((int)ellipse.Tag == s.Number)
                            {
                                ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 140, 0));
                            }
                        }
                        else if(elem is Line line)
                        {
                            int v1 = int.Parse(line.Tag.ToString().Split()[0]);
                            int v2 = int.Parse(line.Tag.ToString().Split()[1]);
                            if(toReturn[toReturn.Count-2].Number == v1 && toReturn.Last().Number == v2)
                            {
                                line.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 140));
                            }
                        }
                    }
                   
                }
                VisitedVertex.Add(s);
                foreach (var vert in FindАdjacentVertexs(s.Number).OrderByDescending(x => x.Number))
                {
                    if (!VisitedVertex.Contains(vert))
                    {
                        stack.Push(vert);
                    }
                }
            }
            return toReturn;
        }
        public IEnumerable<VertexView> BFSLearningMode(int v_num)
        {
            Queue<VertexView> queue = new Queue<VertexView>();
            List<VertexView> toReturn = new List<VertexView>();
            VertexView visited = V.Where(x => x.Number == v_num).First();
            queue.Enqueue(visited);
            SortedSet<VertexView> VisitedVertex = new SortedSet<VertexView>();
            VisitedVertex.Add(visited);
            while (queue.Count != 0)
            {
                VertexView cur = queue.Dequeue();
                toReturn.Add(cur);
                foreach (var vert in FindАdjacentVertexs(cur.Number).OrderBy(x => x.Number))
                {
                    if (!VisitedVertex.Contains(vert))
                    {
                        queue.Enqueue(vert);
                        VisitedVertex.Add(vert);
                    }
                }
            }
            return toReturn;
        }
        public async Task<IEnumerable<VertexView>> BFS(int v_num, Canvas graphCanvas)
        {
            Queue<VertexView> queue = new Queue<VertexView>();
            List<VertexView> toReturn = new List<VertexView>();
            VertexView visited = V.Where(x => x.Number == v_num).First();
            foreach (var elem in graphCanvas.Children)
            {
                if (elem is Ellipse ellipse)
                {
                    if ((int)ellipse.Tag == visited.Number)
                    {
                        ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 140, 0));
                    }
                }
            }
            queue.Enqueue(visited);
            toReturn.Add(visited);
            SortedSet<VertexView> VisitedVertex = new SortedSet<VertexView>();
            VisitedVertex.Add(visited);
            while(queue.Count != 0)
            {
                VertexView cur = queue.Dequeue();
                toReturn.Add(cur);
                await Task.Delay(1000);
                foreach (var elem in graphCanvas.Children)
                {
                    if (elem is Ellipse ellipse)
                    {
                        if ((int)ellipse.Tag == cur.Number)
                        {
                            ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 140, 0));
                        }
                    }
                }
                foreach (var vert in FindАdjacentVertexs(cur.Number).OrderBy(x => x.Number))
                {
                    if (!VisitedVertex.Contains(vert))
                    {
                        foreach(var elem in graphCanvas.Children)
                        {
                            if (elem is Ellipse ellipse)
                            {
                                if ((int)ellipse.Tag == cur.Number)
                                {
                                    ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 140, 0));
                                }
                            }
                            else if (elem is Line line)
                            {
                                int v1 = int.Parse(line.Tag.ToString().Split()[0]);
                                int v2 = int.Parse(line.Tag.ToString().Split()[1]);
                                if(cur.Number == v1 && vert.Number == v2)
                                {
                                    line.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 140));
                                }
                            }
                        }
                        await Task.Delay(1000);
                        queue.Enqueue(vert);
                        VisitedVertex.Add(vert);
                    }
                }
            }
            return toReturn;
        }
        public IEnumerable<VertexView> DFSBoruvka(int v)
        {
            Stack<VertexView> stack = new Stack<VertexView>();
            List<VertexView> toReturn = new List<VertexView>();
            VertexView visited = V.Where(x => x.Number == v).First();
            stack.Push(visited);
            toReturn.Add(visited);
            SortedSet<VertexView> VisitedVertex = new SortedSet<VertexView>();
            VisitedVertex.Add(visited);
            while (stack.Count != 0)
            {
                VertexView s = stack.Pop();
                if (!VisitedVertex.Contains(s))
                {
                    toReturn.Add(s);
                }
                VisitedVertex.Add(s);
                foreach (var vert in FindАdjacentVertexs(s.Number).OrderByDescending(x => x.Number))
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
        public async Task<Graph> AlgBoruvka(Canvas graphCanvas, bool isLearnignMode)
        {
            Graph T = new Graph();
            T.V = V;
            if (!isLearnignMode)
            {
                foreach (UIElement elem in graphCanvas.Children)
                {
                    if (elem is Line line)
                    {

                        line.Visibility = Visibility.Hidden;
                    }
                    else if (elem is TextBlock text)
                    {
                        if (text.Tag.ToString().Split().Length == 2)
                        {

                            text.Visibility = Visibility.Hidden;
                        }
                    }

                }
            }
            while (T.FindRelatedComponents().Count() != 1)
            {
                List<UIElement> linesToDraw = new List<UIElement>();
                var components = T.FindRelatedComponents();
                foreach (var relatedComponent in components)
                {
                    EdgeView minEdgeInRelatedComponent = FindMinEdgeInRelatedComponent(relatedComponent, FindEdgesInRalatedComponents(relatedComponent));

                    if (minEdgeInRelatedComponent != null)
                    {
                        if (!isLearnignMode)
                        {
                            foreach (UIElement elem in graphCanvas.Children)
                            {
                                if (elem is Line line)
                                {
                                    int v1 = int.Parse(line.Tag.ToString().Split()[0]);
                                    int v2 = int.Parse(line.Tag.ToString().Split()[1]);
                                    if (minEdgeInRelatedComponent.V1.Number == v1 && minEdgeInRelatedComponent.V2.Number == v2 ||
                                        minEdgeInRelatedComponent.V2.Number == v1 && minEdgeInRelatedComponent.V1.Number == v2)
                                    {
                                        linesToDraw.Add(line);
                                    }
                                }
                                else if (elem is TextBlock text)
                                {
                                    if (text.Tag.ToString().Split().Length == 2)
                                    {
                                        int v1 = int.Parse(text.Tag.ToString().Split()[0]);
                                        int v2 = int.Parse(text.Tag.ToString().Split()[1]);
                                        if (minEdgeInRelatedComponent.V1.Number == v1 && minEdgeInRelatedComponent.V2.Number == v2 ||
                                            minEdgeInRelatedComponent.V2.Number == v1 && minEdgeInRelatedComponent.V1.Number == v2)
                                        {
                                            linesToDraw.Add(text);
                                        }
                                    }
                                }

                            }
                        }
                        T.E.Add(minEdgeInRelatedComponent);
                    }

                }

                if (!isLearnignMode)
                {
                    await Task.Delay(2000);
                    foreach (var line in linesToDraw)
                    {

                        line.Visibility = Visibility.Visible;
                    }
                }

            }
            return T;
        }
            
        

        
        public IEnumerable<EdgeView> FindEdgesInRalatedComponents(IEnumerable<VertexView> component)
        {
            List<EdgeView> toReturn = new List<EdgeView>();
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

        private EdgeView FindMinEdgeInRelatedComponent(IEnumerable<VertexView> component, IEnumerable<EdgeView> edgesInComponent)
        {
            int min = E.Select(x => x.Distance).Max();
            SortedSet<EdgeView> minEdgesInVertexs = new SortedSet<EdgeView>();
            List<EdgeView> minEdges = new List<EdgeView>();
            foreach (var vertex in component)
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
        public IEnumerable<IEnumerable<VertexView>> FindRelatedComponents()
        {
            List<VertexView> relations = new List<VertexView>();
            V.ToList().ForEach(item => relations.Add(new VertexView(item.Number, item.Name)));
            while (relations.Count != 0)
            {
                List<VertexView> comp = new List<VertexView>();
                foreach (var v in DFSBoruvka(relations.First().Number))
                {
                    comp.Add(v);
                }
                #region Mda
                List<VertexView> _toRemove = new List<VertexView>();
                foreach (var vert in relations)
                {
                    foreach (var v in comp)
                    {
                        if (v.Number == vert.Number)
                        {
                            _toRemove.Add(v);
                        }
                    }
                }
                foreach (var ver in _toRemove)
                {
                    relations.RemoveAll(x => x.Number == ver.Number);
                }
                #endregion
                yield return comp;
            }
        }
        private IEnumerable<EdgeView> FindAdjacentEdges(VertexView vertex)
        {
            foreach (var e in E)
            {
                if (e.V1.Number == vertex.Number)
                {
                    yield return e;
                }
            }
        }

    }
}
