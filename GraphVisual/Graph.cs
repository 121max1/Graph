﻿using GraphVisual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
            // баг
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
    }
}