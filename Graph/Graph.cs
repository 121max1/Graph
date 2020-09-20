using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Graph
    {
        SortedSet<Vertex> V { get; set; } = new SortedSet<Vertex>();
        List<Edge> E { get; set; } = new List<Edge>();

        private int[,] matrix;

        private readonly Dictionary<string,int> namesVertex = new Dictionary<string, int>();
        public Graph()
        {
            matrix = null;
        }
        public Graph(Graph prev)
        {
            V = prev.V;
            E = prev.E;
        }
        public Graph(string name) //конструктор внешнего класса
        {
            using (StreamReader file = new StreamReader(name, encoding:Encoding.Default))
            {
                int n = int.Parse(file.ReadLine());
                matrix = new int[n, n];
                string[] namesVertex = file.ReadLine().Split();
                for (int i = 0; i < n; i++)
                {
                    this.namesVertex.Add(namesVertex[i],i);
                }
                    for (int i = 0; i < n; i++)
                {
                    V.Add(new Vertex(i,namesVertex[i]));
                }
                for (int i = 0; i < n; i++)
                {
                    string line = file.ReadLine();
                    string[] mas = line.Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        matrix[i, j] = int.Parse(mas[j]);
                        if (matrix[i, j] != 0)
                        {
                            E.Add(new Edge(new Vertex(i, namesVertex[i]), new Vertex(j,namesVertex[j]), matrix[i, j]));
                        }
                    }                 
                }
            }

        }
        private void BuildMatrix()
        {
            matrix = new int[V.Count, V.Count];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    matrix[i, j] = 0;
                }
            }
            
            foreach (var edge in E)
            {
                matrix[edge.V1.Number, edge.V2.Number] = edge.Distance;
            }
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
            
            V.Add(new Vertex(V.Count,Name));
            matrix = new int[matrix.GetLength(0) + 1,matrix.GetLength(0) + 1];
            BuildMatrix();
            return V.Count-1;
        }

        public void AddEdge(string v1, string v2, int dist)
        {
            if (!V.Contains(new Vertex(namesVertex[v1],v1)) || !V.Contains(new Vertex(namesVertex[v2],v2)))
            {
                throw new Exception("Ошибка добавления ребра, не найдены вершины.");
            }
            else
            {
                E.Add(new Edge(new Vertex(namesVertex[v1], v1), new Vertex(namesVertex[v2], v2), dist));
            }
            BuildMatrix();
        }

        public void DeleteVertex(string v)
        {
            var edgesToDelete = new List<Edge>();
            foreach (var edge in E)
            {
                if (edge.V1.Number == namesVertex[v] || edge.V2.Number == namesVertex[v])
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
                if (edge.V1.Number > namesVertex[v])
                {
                    edge.V1.Number -= 1;
                }
                if (edge.V2.Number > namesVertex[v])
                {
                    edge.V2.Number -= 1;
                }
            }
            V.Remove(new Vertex(namesVertex[v],v));
            namesVertex.Remove(v);
            BuildMatrix();
        }

        public void DeleteEdge(string v1, string v2)
        {
            Edge edgeToDelete = null;
            foreach (var edge in E)
            {
                if (edge.V1.Number == namesVertex[v1] && edge.V2.Number == namesVertex[v2])
                {
                    edgeToDelete = edge;
                }
            }
            E.Remove(edgeToDelete);
            BuildMatrix();
        }

        public void WriteMatrix(string name)
        {
            using (StreamWriter writer = new StreamWriter(name,false,encoding:Encoding.Default))
            {
                writer.WriteLine(matrix.GetLength(0));
                foreach(var v in V)
                {
                    writer.Write(v.Name + " ");
                }
                writer.WriteLine();
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        writer.Write(matrix[i, j] + " ");
                    }
                    writer.WriteLine();
                }
                
            }
        }

    }
}