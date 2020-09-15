using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Graph
    {
        public SortedSet<Vertex> V { get; set; } = new SortedSet<Vertex>();
        public List<Edge> E { get; set; } = new List<Edge>();

        private int[,] matrix;
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
        public void Print()
        {
            foreach(var v in V)
            {
                Console.Write(v+" ");
            }
            Console.WriteLine();
            foreach(var e in E)
            {
                Console.WriteLine(e._v1+ " " + e._v2 + " " + e._distance);
            }
        }
        
    }
}
