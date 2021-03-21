using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    /// <summary>
    /// Ребро графа.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Вершина начала ребра(Исходный товар).
        /// </summary>
        public Vertex From { get; set; }
        /// <summary>
        /// Вершина конца ребра(Искомый товар).
        /// </summary>
        public Vertex To { set; get; }
        /// <summary>
        /// Вершина конца ребра(Доверие).
        /// </summary>
        public int Weight { get; }

        public Edge(Vertex from, Vertex to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        
    }
}
