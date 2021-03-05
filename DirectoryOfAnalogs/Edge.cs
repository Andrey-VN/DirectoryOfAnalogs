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
    public class Edge<T>
    {
        /// <summary>
        /// Вершина начала ребра.
        /// </summary>
        public Vertex<T> From { get; set; }
        /// <summary>
        /// Вершина конца ребра.
        /// </summary>
        public Vertex<T> To { set; get; }

        public int Weight { get; }

        public Edge(Vertex<T> from, Vertex<T> to, int weight = 1)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"{From}, {To}";
        }
    }
}
