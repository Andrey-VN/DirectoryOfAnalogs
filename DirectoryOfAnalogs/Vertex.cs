using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    /// <summary>
    /// Вершина графа.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vertex<T>
    {
        /// <summary>
        /// Значение вершины графа.
        /// </summary>
        public T Number { get; set; }

        public Vertex(T number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
