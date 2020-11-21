using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisual.Models
{
    class EdgeView
    {
        public VertexView V1 { get; set; }

        public VertexView V2 { get; set; }

        public int Distance { get; set; }

        public bool? IsOriented {get;set;}

       
    }
}
