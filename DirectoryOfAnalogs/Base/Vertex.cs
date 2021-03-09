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

        /// <summary>
        /// Артикул узла.
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// Продукт узла.
        /// </summary>
        public string Manufacturer { get; set; }

        public Vertex(string artic, string manuf)
        {
            Article = artic;
            Manufacturer = manuf;
        }

        public override string ToString()
        {
            return Article.ToString() +" " + Manufacturer.ToString();
        }

    }
}
