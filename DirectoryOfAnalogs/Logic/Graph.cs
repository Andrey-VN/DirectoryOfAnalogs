using System;
using System.Collections;
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
        List<Vertex> Vertices = new List<Vertex>();

        /// <summary>
        /// Список ребер графа.
        /// </summary>
        List<Edge> Edges = new List<Edge>();

        #endregion

        /// <summary>
        /// Метод добавления вершин ребра товара и его аналога.
        /// </summary>
        /// <param name="from">Товар.</param>
        /// <param name="to">Аналог товара.</param>
        public void Add(Vertex from, Vertex to, int weight)
        {
            var vert = new Edge(from, to, weight);
            Edges.Add(vert);
        }

        /// <summary>
        /// Метод поиска аналога по одному производителю.
        /// </summary>
        /// <param name="vertex">Производитель, по которому идет поиска аналогов.</param>
        /// <returns></returns>
        /// 
        public List<Vertex> GetVertexList(Vertex vertex)
        {
            List<Vertex> resut = new List<Vertex>();

            foreach (var i in Edges)
            {
                if (i.From.ToString() == vertex.ToString())
                {
                    resut.Add(i.To);
                }

            }
            return resut;

        }
        /// <summary>
        /// Метод проверки ребра на нулевое значение "доверия".
        /// </summary>
        /// <param name="vert1">Узел "Из".</param>
        /// <param name="vert2">Узел "В".</param>
        /// <returns></returns>
        public bool TrustZero(Vertex vert1, Vertex vert2)
        {
            foreach (var i in Edges)
            {
                if (i.From.ToString() == vert1.ToString() & i.To.ToString() == vert2.ToString())
                {
                    if (i.Weight == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет, есть ли искомый товар за введенное число итерации
        /// </summary>
        /// <param name="start">Узел исходного товара, с которого начинается поиск.</param>
        /// <param name="finish">Узел искомого значения.</param>
        /// <param name="numberOfIterations">Количество итерации.</param>
        /// <returns></returns>
        public Dictionary<Vertex,List<Vertex>> Wave(Vertex start, Vertex finish, int numberOfIterations)
        {
            Dictionary<Vertex, List<Vertex>> dic = new Dictionary<Vertex, List<Vertex>>();

            List<Vertex> list = new List<Vertex>() { start };
            

            for (int i = 0; i < numberOfIterations; i++)
            {
                List<Vertex> item = new List<Vertex>();
                var vertex = list[i];
                var getVertexList = GetVertexList(vertex);
                foreach (var v in getVertexList)
                {
                    if(!list.Contains(v))
                    {   //условие проверки на нулевое доверие
                        if(!TrustZero(vertex, v))
                        {
                            list.Add(v);
                        }
                        //если искомый товар один и доверие равно нулю, то прерывается итерация и возвращается значение
                        else if (TrustZero(vertex, v) & getVertexList.Count == 1)
                        {
                            item.Add(v);
                            dic.Add(vertex, item);
                            return dic;
                        }
                        item.Add(v);                      
                    }                    
                }
                
                dic.Add(vertex, item);
                //если товар найден
                if(item.Contains(finish))
                {
                    return dic;
                }
                //если товар не найден найден, то возвращает null
                if (list.Count == i + 1)
                {
                    return null;
                }
            }
            return null;
        }

    }
}
