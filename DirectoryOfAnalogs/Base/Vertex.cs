using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    /// <summary>
    /// Вершина графа.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vertex
    {
        /// <summary>
        /// Значение вершины графа.
        /// </summary>
        
        public string Article { get; set; }
        public string Manufacturer { get; set; }
        public int Trust { get; set; }

        public Vertex(string artic, string manuf, int trust )
        {
            Article = artic;
            Manufacturer = manuf;
            Trust = trust;
        }

        public override string ToString()
        {
            return Article.ToString() + Manufacturer.ToString();
        }

    }
}
