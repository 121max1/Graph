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
    }
}
