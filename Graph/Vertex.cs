﻿using System;
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

        public Vertex()
        {
            Number = 0;
            Name = "";
        }
        public Vertex(int number)
        {
            this.Number = number;
            Name = "";
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
    }
}