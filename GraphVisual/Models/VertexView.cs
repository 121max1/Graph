using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisual.Models
{
    class VertexView : IComparable
    {
        public double X { get; set; }
        public double Y { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public static int Radius = 12;

        public bool IsVisited { get; set; }

        public int CompareTo(object obj)
        {
            VertexView v = obj as VertexView;
            if (obj != null)
            {
                return this.Number.CompareTo(v.Number);
            }
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
        public VertexView()
        {

        }
        public VertexView(int number, string Name)
        {
            Number = number;
            this.Name = Name;
        }
        public VertexView(int number, string Name, bool IsVisited)
        {
            Number = number;
            this.Name = Name;
            this.IsVisited = IsVisited;
        }
        public static bool operator == (VertexView v1, VertexView v2)
        {
            return v1.Number == v2.Number;
        }
        public static bool operator != (VertexView v1, VertexView v2)
        {
            return v1.Number != v2.Number;
        }
    }

    
}