using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    /// <summary>
    /// Вершина графа №Артикул+производитель".
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vertex
    {
        public string Item { get; }

        public Vertex(string item)
        {
            Item = item;
        }
    }
}
