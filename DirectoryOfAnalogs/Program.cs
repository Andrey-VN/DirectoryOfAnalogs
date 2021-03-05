using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<Product> graph = new Graph<Product>();

            //Создание листа со значениями(артикул, производитель, доверие)
            List<Product> products = new List<Product>() 
            {
                new Product("KL9", "Knecht", 1),
                new Product("0 450 905 030", "Bosch", 1),
                new Product("24073", "Febi", 1),
                new Product("12648", "Febi", 1),
                new Product("5529", "Nac", 1),
                new Product("03310", "Febi", 1),
                new Product("300 823 546", "Hans Pries", 1),
                new Product("17731", "Mapco", 1)

            };

            //Создание листа с вершинами графа, которые имеют зеначения
            List<Vertex<Product>> vertex = new List<Vertex<Product>>()
            {
                {new Vertex<Product>(products[0])},
                {new Vertex<Product>(products[1])},
                {new Vertex<Product>(products[2])},
                {new Vertex<Product>(products[3])},
                {new Vertex<Product>(products[4])},
                {new Vertex<Product>(products[5])},
                {new Vertex<Product>(products[6])},
                {new Vertex<Product>(products[7])},

            };



            //Добавление вершин графа в ребра(ребро между продуктом и его аналогом)
            graph.Add(vertex[0], vertex[1]);
            graph.Add(vertex[1], vertex[2]);
            graph.Add(vertex[1], vertex[3]);
            graph.Add(vertex[1], vertex[4]);
            graph.Add(vertex[1], vertex[5]);
            graph.Add(vertex[6], vertex[7]);


            Vertex<Product> start = vertex[0];
            Vertex<Product> finish = vertex[5];
            int iterat = 5;

            if (graph.Wave(start, finish, iterat))
            {

                for (int i = 0; i < iterat; i++)
                {

                    Console.Write(vertex[i].Number.Article + " " + vertex[i].Number.Manufacturer + "-> ");
                    foreach (var v in graph.GetVertexList(vertex[i]))
                    {
                        if(i<iterat)
                            Console.Write(v.Number.Article + " " + v.Number.Manufacturer + ", "); 
                        else
                            Console.Write(v.Number.Article + " " + v.Number.Manufacturer);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Искомый товар \"{finish.Number.Article +" " + finish.Number.Manufacturer}\" не найден за {1} шагов.");
            }
            
            Console.Read();
        }


    }
}
