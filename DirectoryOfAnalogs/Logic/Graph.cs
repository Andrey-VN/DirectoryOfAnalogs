using System.Collections.Generic;
using System.Linq;


namespace DirectoryOfAnalogs
{
    /// <summary>
    /// Граф.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Список вершин.
        /// </summary>
        public List<Vertex> Vertices { get; }

        /// <summary>
        /// Список ребер.
        /// </summary>
        public List<Edge> Edges { get; }

        public Graph()
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }
        public void Add(string from, string to, int weight)
        {
            GetVertexInEdge(from, to, out Vertex v1, out Vertex v2);

            AddVertex(v1, v2);                    //добавление вершин в список вершин
            Edges.Add(new Edge(v1, v2, weight));  //добавление вершин в ребро
        }

        private void GetVertexInEdge(string from, string to, out Vertex v1, out Vertex v2)
        {
            v1 = Vertices.FirstOrDefault(p => p.Item == from);
            v2 = Vertices.FirstOrDefault(p => p.Item == to);
            if (v1 == null) { v1 = new Vertex(from); }  //если нет вершин с добавляемыми вершинами, то добавляем
            if (v2 == null) { v2 = new Vertex(to); }
        }

        /// <summary>
        /// Добавление значений в список вершин.
        /// </summary>
        /// <param name="verFrom"></param>
        /// <param name="vertTo"></param>
        private void AddVertex(Vertex verFrom, Vertex vertTo)
        {
            Vertices.Add(verFrom);
            Vertices.Add(vertTo);
        }
        /// <summary>
        /// Проверка на наличие значения в имеюзихся вершинах, если вершин нет, то возвращаем null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Vertex GetVertOrNull(string str)
        {
            foreach (var i in Vertices)
            {
                if (i.Item.Equals(str))
                    return i;
            }
            return null;
        }

        /// <summary>
        /// Метод поиска всех возможных маршрутов искомого аналога.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="numberOfIterations"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<Vertex, Vertex>> BFS(string start, string finish, int numberOfIterations)
        {
            List<Vertex> isVisited = new List<Vertex>();                                                                   //лист с посещенными вершинами
            Dictionary<Vertex, Vertex> pathList = new Dictionary<Vertex, Vertex>();                                        //словарь с найденными товарами - ключ и их аналогов - значение
            Dictionary<string, Dictionary<Vertex, Vertex>> listOut = new Dictionary<string, Dictionary<Vertex, Vertex>>(); //вывод номера маршрута и листа его значения

            if (GetVertOrNull(start) == null || GetVertOrNull(finish) == null)                                             //проверка на наличие в вершинах введенных товаров, если их нет, то будет возвращен пустой лист
                return listOut;

            pathList[GetVertOrNull(start)] = null; 

            int countIteration = 0;  //текущий шаг рекурсии
            int numberRoute = 0;     //номер текущего маршрута

            return PrintAllPathsUtil(GetVertOrNull(start), 
                GetVertOrNull(finish), 
                isVisited, 
                pathList, 
                numberOfIterations, 
                countIteration, ref numberRoute, listOut);
        }
        /// <summary>
        /// Рекурсивный метод поиска аналогов.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="numberOfIterations"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<Vertex, Vertex>> PrintAllPathsUtil(Vertex u, Vertex d,
                               List<Vertex> isVisited,
                               Dictionary<Vertex, Vertex> localPathList,
                               int numberOfIterations,
                               int countIteration,
                               ref int numberRoute,
                               Dictionary<string, Dictionary<Vertex, Vertex>> listOut)
        {

            if (countIteration == numberOfIterations + 1) //если количество рекурсий равно заданному
                return listOut;
            countIteration++;


            if (u.Equals(d) || IsTrustZero(u))   //если аналог найден или аналог равен нулю
            {
                var Dcopy = new Dictionary<Vertex, Vertex>();

                foreach (var e in localPathList) { Dcopy.Add(e.Key, e.Value); }
                numberRoute++;
                listOut.Add("Маршрут " + numberRoute, Dcopy);
                return listOut;
            }

            isVisited.Add(u);   //добавляем исследуемый товар в список просмотренных

            foreach (var i in GetVertex(u))
            {
                if (!isVisited.Contains(i))  //проверка, просмотрен ли товар до этого
                {
                    localPathList[u] = i;
                    PrintAllPathsUtil(i, d, isVisited,
                                      localPathList, numberOfIterations, countIteration, ref numberRoute, listOut);
                    localPathList.Remove(u);   
                }
            }

            isVisited.Remove(u);
            return listOut;
        }
        /// <summary>
        /// Проверка на нулевое доверие.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        private bool IsTrustZero(Vertex vert)
        {
            foreach (var i in Edges)
            {
                if (i.To.Equals(vert) & i.Weight == 0)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Поиск вершин ребра по искомой вершине.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        private List<Vertex> GetVertex(Vertex vertex)
        {
            List<Vertex> vert = new List<Vertex>();
            foreach (var i in Edges)
            {
                if (i.From == vertex)
                    vert.Add(i.To);
            }
            return vert;
        }


    }
}
