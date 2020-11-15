using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisual.Models
{
    class VertexView
    {
        public double X { get; set; }
        public double Y { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public static int Radius = 10;
    }
}