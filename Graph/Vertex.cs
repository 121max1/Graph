using Graph.AlgDjekstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Vertex : IComparable
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public bool IsVisited { get; set; }

        public Vertex()
        {
            Number = 0;
            Name = "";
        }
        public Vertex(int number, string Name)
        {
            Number = number;
            this.Name = Name;
        }
        public Vertex(int number, string Name, bool IsVisited)
        {
            Number = number;
            this.Name = Name;
            this.IsVisited = IsVisited;
        }

        public Vertex(VertexDjekstra v)
        {
            Number = v.Number;
            Name = v.Name;
            IsVisited = v.IsVisited;
        }
        public int CompareTo(object o)
        {
            Vertex v = o as Vertex;
            if (o != null)
            {
                return this.Number.CompareTo(v.Number);
            }
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
        
        public static bool operator == (Vertex v1, Vertex v2)
        {
            return v1.Number == v2.Number;
        }
        public static bool operator != (Vertex v1, Vertex v2)
        {
            return v1.Number != v2.Number;
        }
    }
}