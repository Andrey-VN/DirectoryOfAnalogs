using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    /// <summary>
    /// Граф.
    /// </summary>
    public class Graph<T>
    {
        
        #region
        /// <summary>
        /// Список вершин графа.
        /// </summary>
        List<Vertex<T>> Vertices = new List<Vertex<T>>();

        /// <summary>
        /// Список ребер графа.
        /// </summary>
        List<Edge<T>> Edges = new List<Edge<T>>();

        /// <summary>
        /// Количество вершин.
        /// </summary>
        public int VertexCount => Vertices.Count;

        /// <summary>
        /// Количество ребер.
        /// </summary>
        public int EdgeCount => Edges.Count;
        #endregion

        /// <summary>
        /// Метод добавления вершин ребра товара и его аналога.
        /// </summary>
        /// <param name="from">Товар.</param>
        /// <param name="to">Аналог товара.</param>
        public void Add(Vertex<T> from, Vertex<T> to)
        {
            var vert = new Edge<T>(from, to);
            Edges.Add(vert);
        }

        /// <summary>
        /// Метод поиска аналога по одному производителю;
        /// </summary>
        /// <param name="vertex">Производитель, по которому идет поиска аналогов.</param>
        /// <returns></returns>
        public List<Vertex<T>> GetVertexList(Vertex<T> vertex)
        {
            var resut = new List<Vertex<T>>();
            foreach(var i in Edges)
            {
                if(i.From == vertex)
                {
                    resut.Add(i.To);
                }
            }
            return resut;

        }

        public List<Vertex<T>> WaveList(Vertex<T> start, Vertex<T> finish, int numberOfIterations)
        {
            var list = new List<Vertex<T>>();

            list.Add(start);

            for (int i = 0; i < numberOfIterations; i++)
            {
                Vertex<T> vertex = list[i];

                //Console.Write(vertex.Number + " " + vertex.Number + "-> ");
                foreach (var v in GetVertexList(vertex))
                {
                    if (!list.Contains(v))
                    {
                        list.Add(v);                      
                    }
                }
                return list;
            }
            return list;
        }

        public bool Wave(Vertex<T> start, Vertex<T> finish, int numberOfIterations)
        {
            List<Vertex<T>> list = new List<Vertex<T>>();

            list.Add(start);

            for(int i = 0; i < numberOfIterations; i++)
            {
                Vertex<T> vertex = list[i];
                
                //Console.Write(vertex.Number + " " + vertex.Number + "-> ");
                foreach (var v in GetVertexList(vertex))
                {
                    if(!list.Contains(v))
                    {
                        list.Add(v);
                        //Console.Write(v.Number + " " + v.Number + ", ");
                    }
                }
            }
            return list.Contains(finish);
        }

    }
}
