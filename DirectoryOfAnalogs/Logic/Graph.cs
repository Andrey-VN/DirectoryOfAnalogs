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
        /// 
        public List<Vertex<T>> GetVertexList(Vertex<T> vertex)
        {
            
            var resut = new List<Vertex<T>>();
            foreach(var i in Edges)
            {
                if (i.From.Equals(vertex))
                {
                    resut.Add(i.To);
                }
            }
            return resut;

        }
        /// <summary>
        /// Проверяет, есть ли искомый товар за введенное число итерации
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="numberOfIterations"></param>
        /// <returns></returns>
        public bool Wave(Vertex<T> start, Vertex<T> finish, int numberOfIterations)
        {
            var list = new List<Vertex<T>>() {start };


            for (int i = 0; i < numberOfIterations; i++)
            {
                var vertex = list[i];
                foreach (var v in GetVertexList(vertex))
                {
                    if (!list.Contains(v))
                    {
                        list.Add(v);
                    }
                }
                if(list.Count== i+1)
                {
                    return list.Contains(finish);
                }
            }
            return list.Contains(finish);
        }

    }
}
