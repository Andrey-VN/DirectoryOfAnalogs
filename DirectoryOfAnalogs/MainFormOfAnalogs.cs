using DirectoryOfAnalogs.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryOfAnalogs
{
    
    public partial class MainFormOfAnalogs : Form
    {
        ProductContext db;
        Route route2 = new Route();
        Graph<string> graph;
        List<Product> products;
        public MainFormOfAnalogs()
        {
            InitializeComponent();

            graph = new Graph<string>();
            db = new ProductContext();

            products = db.Products.ToList();
            db.Products.Load();


            dataGridView1.DataSource = db.Products.Local.ToBindingList();



        }

        private void MainFormOfAnalogs_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddManufacture addManufacture = new AddManufacture();
            DialogResult result = addManufacture.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Product product = new Product();


            product.Article1 = addManufacture.textBox1.Text;
            product.Manufacturer1 = addManufacture.textBox2.Text;
           

            product.Article2 = addManufacture.textBox4.Text;
            product.Manufacturer2 = addManufacture.textBox5.Text;
            product.Trust = (int)addManufacture.numericUpDown1.Value;

            db.Products.Add(product);
            db.SaveChanges();
            dataGridView1.Refresh();

            MessageBox.Show("Новый объект добавлен");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;

                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();

                MessageBox.Show("Объект удален");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Product product = db.Products.Find(id);

                AddManufacture addManufacture = new AddManufacture();

                addManufacture.textBox1.Text = product.Article1;
                addManufacture.textBox2.Text = product.Manufacturer1;            

                addManufacture.textBox4.Text = product.Article2;
                addManufacture.textBox5.Text = product.Manufacturer2;
                addManufacture.numericUpDown1.Value = product.Trust;

                DialogResult result = addManufacture.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                product.Article1 = addManufacture.textBox1.Text;
                product.Manufacturer1 = addManufacture.textBox2.Text;


                product.Article2 = addManufacture.textBox4.Text;
                product.Manufacturer2 = addManufacture.textBox5.Text;
                product.Trust = (int)addManufacture.numericUpDown1.Value;

                db.Products.Add(product);
                db.SaveChanges();
                dataGridView1.Refresh();

                MessageBox.Show("Объект обновлен");

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FindAConnection findAConnection = new FindAConnection();
            DialogResult result = findAConnection.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Product product = new Product();

            //получение списка всех элементов двух таблиц
            List<string> left = new List<string>();
            List<string> right = new List<string>();

            foreach (var v in db.Products.Local.ToBindingList())
            {
                left.Add(v.Article1 + " " + v.Manufacturer1);
                right.Add(v.Article2 + " " + v.Manufacturer2);
            }

            IEnumerable<string> union = left.Union(right);

            //в вершины добавляются только уникальные строки (производитель+артикул)
            List<Vertex<string>> vertex = new List<Vertex<string>>();
            foreach (var i in union)
            {
                vertex.Add(new Vertex<string>(i));
            }

            //добавление строк из таблицы в соответствующие узлы
            Vertex<string> vertexLeft = null;
            Vertex<string> vertexRight = null;
            foreach (var v in db.Products.Local.ToBindingList())
            {
                for (int i = 0; i < vertex.Count; i++)
                {
                    if ((v.Article1 + " " + v.Manufacturer1).Equals(vertex[i].Number))
                    {
                        vertexLeft = vertex[i];
                    }
                    if ((v.Article2 + " " + v.Manufacturer2).Equals(vertex[i].Number))
                    {
                        vertexRight = vertex[i];
                    }
                }
                graph.Add(vertexLeft, vertexRight);
            }

            string Article1 = findAConnection.textBox1.Text;
            string Manufacturer1 = findAConnection.textBox2.Text;

            string Article2 = findAConnection.textBox3.Text;
            string Manufacturer2 = findAConnection.textBox4.Text;

            int Trust = Convert.ToInt32(findAConnection.textBox5.Text);

            Route route = new Route();           

            Vertex<string> start = default;
            Vertex<string> finish = default;
            int count = default;
            int indexStart = 0;


            foreach (var v in vertex)
            {
                if (v.Number == Article1 + " " + Manufacturer1)
                {
                    start = v;
                    indexStart = count;
                }                   
                if (v.Number == Article2 + " " + Manufacturer2)
                    finish = v;
                count++;
            }

            List<Vertex<string>> vvv = new List<Vertex<string>> { start };


            if (graph.Wave(start, finish, Trust) & Trust <= vertex.Count & Trust > 0)
            {
                GetVert(vvv, finish, Trust, route);
                route.ShowDialog(findAConnection);
            }
            else if (finish == null & start == null)
            {
                MessageBox.Show("Искомый и исходный товар отсутствует в таблице");
            }
            else if (start == null)
            {
                MessageBox.Show("Исходный товар отсутствует в таблице.");
            }
            else if (finish == null)
            {
                MessageBox.Show("Искомый товар отсутствует в таблице");
            }
            else
            {
                MessageBox.Show($"Искомый товар \"{finish.Number}\" не найден за {Trust} шагов.");
            }           
        }
        public void GetVert(List<Vertex<string>> vertices, Vertex<string> finish, int iterat, Route route, int count = 1)
        {
            List<Vertex<string>> vertices1 = vertices;
            List<Vertex<string>> vertices2 = new List<Vertex<string>>();
            if (iterat > 0)
            {
                foreach(var i in vertices1)
                {
                    string column1 = $"Маршрут {count}";                   
                    string column2 = i.Number + "-> ";

                    count++;

                    foreach (var v in graph.GetVertexList(i))
                    {
                        column2 += v.Number + "/ ";
                        if(v.Number.Equals(finish.Number))
                        {
                            route.dataGridView2.Rows.Add(column1, column2);
                            return;
                        }
                    }
                    vertices2 = graph.GetVertexList(i);
                    route.dataGridView2.Rows.Add(column1, column2);
                    
                    iterat--;
                    GetVert(vertices2, finish, iterat, route, count);
                }
            }
            else
            {
                return;
            }


        }
        static string СonvertedArticle(string str)
        {
            string result = Regex.Replace(str, @"[^\w]", "");
            return result;
        }
        static string СonvertedManufacturer(string str)
        {
            string result = str.ToUpper();
            return result;
        }
    }
}
