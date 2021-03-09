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
            if (dataGridView1.SelectedRows.Count > 0)
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
            using (ProductContext db1 = new ProductContext())
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int index = dataGridView1.SelectedRows[0].Index;
                    int id = 0;
                    bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                    if (converted == false)
                        return;

                    Product product = db1.Products.Find(id);

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

                    db1.SaveChanges();
                    

                    MessageBox.Show("Объект обновлен");

                }
                dataGridView1.Refresh();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FindAConnection findAConnection = new FindAConnection();
            DialogResult result = findAConnection.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Product product = new Product();

            List<Vertex> vertices = new List<Vertex>();


            //В цикле производится добавление элементов таблицы в список узлов, все элементы по окончанию цикла в листе узлов уникальны!!!
            foreach (var v in db.Products.Local.ToBindingList())
            {
                var v2 = vertices.FirstOrDefault(p => p.ToString().Equals(v.Article2 + v.Manufacturer2));
                if (v2 == null)
                    vertices.Add(new Vertex(СonvertedArticle(v.Article2), СonvertedManufacturer(v.Manufacturer2), v.Trust));
                var v1 = vertices.FirstOrDefault(p => p.ToString().Equals(v.Article1 + v.Manufacturer1));
                if (v1 == null)
                    vertices.Add(new Vertex(СonvertedArticle(v.Article1), СonvertedManufacturer(v.Manufacturer1), 2));
            }


            ////добавление узлов в соответствующие им вершины графа
            Vertex vertexLeft = null;
            Vertex vertexRight = null;
            foreach (var v in db.Products.Local.ToBindingList())
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    if ((vertices[i].Article + vertices[i].Manufacturer).Equals(СonvertedArticle(v.Article1) + СonvertedManufacturer(v.Manufacturer1)))
                    {
                        vertexLeft = vertices[i];
                    }
                    if ((vertices[i].Article + vertices[i].Manufacturer).Equals(СonvertedArticle(v.Article2) + СonvertedManufacturer(v.Manufacturer2)))
                    {
                        vertexRight = vertices[i];
                    }
                }
                graph.Add(vertexLeft, vertexRight);
            }

            // ввод значений в поиск аналогов 
            string Article1 = СonvertedArticle(findAConnection.textBox1.Text);
            string Manufacturer1 = СonvertedManufacturer(findAConnection.textBox2.Text);

            string Article2 = СonvertedArticle(findAConnection.textBox3.Text);
            string Manufacturer2 = СonvertedManufacturer(findAConnection.textBox4.Text);

            int Trust = (int)findAConnection.numericUpDown1.Value;

            Route route = new Route();

            Vertex start = default;
            Vertex finish = default;

            // поиск узлов по введенным значениям 
            foreach (var v in vertices)
            {
                if (v.Article + v.Manufacturer == Article1 + Manufacturer1)
                    start = v;
                if (v.Article + v.Manufacturer == Article2 + Manufacturer2)
                    finish = v;
            }

            List<Vertex> vvv = new List<Vertex> { start };

            

            
            #region Условия для результата поисков аналога
            if (graph.Wave(start, finish, Trust) & Trust <= vertices.Count & Trust > 0)
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
                MessageBox.Show($"Искомый товар \"{finish.Article +" "+ finish.Manufacturer}\" не найден за {Trust} шагов.");
            }
            #endregion
        }
        /// <summary>
        /// Метод отображения поиска аналогов в "гриде"
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="finish"></param>
        /// <param name="iterat"></param>
        /// <param name="route"></param>
        /// <param name="count"></param>
        public void GetVert(List<Vertex> vertices, Vertex finish, int iterat, Route route, int count = 1)
        {
            List<Vertex> vertices1 = vertices;
            List<Vertex> vertices2 = new List<Vertex>();
            if (iterat > 0)
            {
                foreach (var i in vertices1)
                {
                    string column1 = $"Маршрут {count}";
                    string column2 = i.Article + " " + i.Manufacturer + "-> ";

                    count++;

                    foreach (var v in graph.GetVertexList(i))
                    {

                        if (v.Trust != 0)
                        {
                            if ((v.Article + " " + v.Manufacturer).Equals(finish.Article + " " + finish.Manufacturer))
                            {
                                column2 += v.Article + " " + v.Manufacturer + "/ ";
                                route.dataGridView2.Rows.Add(column1, column2);
                                return;
                            }
                            vertices2.Add(v);
                            column2 += v.Article + " " + v.Manufacturer + "/ ";
                        }
                            
                    }
                    
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
        /// <summary>
        /// Метод отображения артикула без символов-разделителей
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string СonvertedArticle(string str)
        {
            string result = Regex.Replace(str, @"[^\w]", "");
            return result;
        }

        /// <summary>
        /// Метот отображения строки в верхнем регистре
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string СonvertedManufacturer(string str)
        {
            string result = str.ToUpper();
            return result;
        }
    }
}
