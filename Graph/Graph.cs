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
            using (StreamReader file = new StreamReader(name))
            {
                int n = int.Parse(file.ReadLine());
                matrix = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    V.Add(new Vertex(i));
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
                            E.Add(new Edge(new Vertex(i), new Vertex(j), matrix[i, j]));
                        }
                    }                 
                }
            }

        }
        private void BuildMatrix()
        {
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
                Console.Write(v.Number + " ");
            }
            Console.WriteLine();
            foreach (var e in E)
            {
                Console.WriteLine(e.V1.Number + " " + e.V2.Number + " " + e.Distance);
            }
        }

        public int AddVertex()
        {
            
            V.Add(new Vertex(V.Count));
            matrix = new int[matrix.GetLength(0) + 1,matrix.GetLength(0) + 1];
            BuildMatrix();
            return V.Count-1;
        }

        public void AddEdge(int v1, int v2, int dist)
        {
            if (!V.Contains(new Vertex(v1)) || !V.Contains(new Vertex(v2)))
            {
                throw new Exception("Ошибка добавления ребра, не найдены вершины.");
            }
            else
            {
                E.Add(new Edge(new Vertex(v1), new Vertex(v2), dist));
            }
            BuildMatrix();
        }

        public void DeleteVertex(int v)
        {
            var edgesToDelete = new List<Edge>();
            foreach (var edge in E)
            {
                if (edge.V1.Number == v || edge.V2.Number == v)
                {
                    edgesToDelete.Add(edge);
                }
            }
            foreach (var edge in edgesToDelete)
            {
                E.Remove(edge);
            }
            V.Remove(new Vertex(v));

            BuildMatrix();
        }

        public void DeleteEdge(int v1, int v2)
        {
            Edge edgeToDelete = null;
            foreach (var edge in E)
            {
                if (edge.V1.Number == v1 && edge.V2.Number == v2)
                {
                    edgeToDelete = edge;
                }
            }
            E.Remove(edgeToDelete);
            BuildMatrix();
        }

        public void WriteMatrix(string name)
        {
            using (StreamWriter writer = new StreamWriter(name))
            {
                writer.WriteLine(matrix.GetLength(0));
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