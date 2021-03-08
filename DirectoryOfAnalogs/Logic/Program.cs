using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryOfAnalogs
{
    class Program
    {


            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainFormOfAnalogs());
            }
            //Graph<string> graph = new Graph<string>();

            ////Создание листа со значениями(артикул, производитель, доверие)
            //List<Product> originalProducts = new List<Product>() 
            //{
            //    new Product(1, "KL9", "Knecht", 1),
            //    new Product(2, "0 450 905 030", "Bosch", 1),
            //    new Product(3, "0 450 905 030", "Bosch", 1),
            //    new Product(4, "0 450 905 030", "Bosch", 1),
            //    new Product(5, "0 450 905 030", "Bosch", 1),
            //    new Product(6, "300 823 546", "Hans Pries", 1)


            //};

            //List<Product> analogProducts = new List<Product>()
            //{
            //    new Product(1, "0 450 905 030", "Bosch", 1),
            //    new Product(2, "24073", "Febi", 1),
            //    new Product(3, "12648", "Febi", 1),
            //    new Product(4, "5529", "Nac", 1),
            //    new Product(5, "03310", "Febi", 1),
            //    new Product(6, "17731", "Mapco", 1)

            //};

            ////метод добавления двух таблиц с возвращаемым объединенным листом по индексу
            //var query = from x in originalProducts
            //            join y in analogProducts on x.Id equals y.Id
            //            select new
            //            {
            //                Article1 = x.Article,
            //                Manufacturer1 = x.Manufacturer,
            //                Article2 = y.Article,
            //                Manufacturer2 = y.Manufacturer,
            //                x.Trust
            //            };

            ////получение списка всех элементов двух таблиц
            //List<string> left = new List<string>();
            //List<string> right = new List<string>();
            //foreach (var v in query)
            //{
            //    left.Add(v.Article1 + v.Manufacturer1);
            //    right.Add(v.Article2 + v.Manufacturer2);
            //}
            //IEnumerable<string> union = left.Union(right);



            ////в вершины добавляются только уникальные строки (производитель+артикул)
            //List<Vertex<string>> vertex = new List<Vertex<string>>();
            //foreach (var i in union)
            //{
            //    vertex.Add(new Vertex<string>(i));
            //}


            ////добавление строк из таблицы в соответствующие узлы
            //Vertex<string> vertexLeft = null;
            //Vertex<string> vertexRight = null;
            //foreach (var v in query)
            //{
            //    for (int i = 0; i < vertex.Count; i++)
            //    {
            //        if ((v.Article1 + v.Manufacturer1).Equals(vertex[i].Number))
            //        {
            //            vertexLeft = vertex[i];
            //        }
            //        if ((v.Article2 + v.Manufacturer2).Equals(vertex[i].Number))
            //        {
            //            vertexRight = vertex[i];
            //        }
            //    }
            //    graph.Add(vertexLeft, vertexRight);
            //}



            ////рекурсивное отображение товаров и их аналогов
            //if (graph.Wave(vertex[0], vertex[4], 7))
            //{
            //    for (int i = 0; i < 7; i++)
            //    {
            //        Console.Write($"Маршрут {i+1}: " + vertex[i].Number + "-> ");
            //        foreach (var v in graph.GetVertexList(vertex[i]))
            //        {
            //            Console.Write(v.Number + ", ");
            //        }
            //        Console.WriteLine();
            //    }
            //}
            //else
            //{
            //    Console.WriteLine($"Искомый товар \"{vertex[0].Number + " " + vertex[4].Number}\" не найден за {1} шагов.");
            //}

    }
}
