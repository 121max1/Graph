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
        public Graph ()
	    {
            matrix = null;
	    }
        public Graph (Graph prev)
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
                int[,] a = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    string line = file.ReadLine();
                    string[] mas = line.Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        matrix[i, j] = int.Parse(mas[j]);
                        if(matrix[i,j]!=0)
                        {
                            V.Add(new Vertex(j));
                            E.Add(new Edge(new Vertex(i), new Vertex(j), matrix[i, j]));
                        }
                    }
                }
            }

        }
        private void BuildMatrix()
        {
            matrix = new int[V.Count,V.Count];
            foreach(var edge in E)
            {
                matrix[edge._v1.Number,edge._v2.Number] = edge._distance;
            }
        }
        public void Print()
        {
            foreach(var v in V)
            {
                Console.Write(v.Number+" ");
            }
            Console.WriteLine();
            foreach(var e in E)
            {
                Console.WriteLine(e._v1.Number+ " " + e._v2.Number + " " + e._distance);
            }
        }

        public int AddVertex()
        {
            V.Add(new Vertex(V.Count+1));
            return V.Count+1;
            BuildMatrix();
        }

        public void AddEdge(int v1, int v2, int  dist)
        {
            if(!V.Contains(new Vertex(v1)) || !V.Contains(new Vertex(v2)))
            {
                throw new Exception("Ошибка добавления ребра, не найдены вершины.");
            }
            else
            {
                E.Add(new Edge(new Vertex(v1), new Vertex(v2),dist));
            }
            BuildMatrix();
        }

        public void DeleteVertex(int v)
        {
            foreach(var edge in E)
            {
                if(edge._v1.Number == v || edge._v2.Number == v)
                {
                    E.Remove(edge);
                }
            }
            V.Remove(new Vertex(v));
            BuildMatrix();
        }

        public void DeleteEdge(int v1,int v2)
        {
             foreach(var edge in E)
             {
                if(edge._v1.Number == v1 && edge._v2.Number == v2)
                {
                    E.Remove(edge);
                }
             }
             BuildMatrix();
        }

        public void WriteMatrix(string name)
        {
            using(StreamWriter writer = new StreamWriter(name))
            {
                writer.WriteLine(V.Count);
                for (int i = 0; i < matrix.Length; i++)
			    {
                    for (int j = 0; j < matrix.Length; j++)
			        {
                        writer.Write(matrix[i,j] + " ");
			        }                 
			    }
                writer.WriteLine();
            }
        }
        
    }
}
