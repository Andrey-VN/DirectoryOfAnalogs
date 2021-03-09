using DirectoryOfAnalogs.Logic;
using System;
using System.Collections;
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
        
        List<Product> products;

        public MainFormOfAnalogs()
        {
            InitializeComponent();

            ///Инициализация связи с БД и извлечение значений в таблицу
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

            //вызов окна для добавления нового объекта
            DialogResult result = addManufacture.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Product product = new Product();

            //ввод новых значений в таблицу
            product.Article1 = addManufacture.textBox1.Text;
            product.Manufacturer1 = addManufacture.textBox2.Text;


            product.Article2 = addManufacture.textBox4.Text;
            product.Manufacturer2 = addManufacture.textBox5.Text;
            product.Trust = (int)addManufacture.numericUpDown1.Value;


            //добавление  и отвправка нововведенного значения в БД и обновление таблицы "грида", сохранение изменений
            db.Products.Add(product);
            db.SaveChanges();
            dataGridView1.Refresh();

            MessageBox.Show("Новый объект добавлен");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //проверка, есть ли в таблице записи
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //удаление выделеной строки по ID после нажатия кнопки
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                //удаление строки из БД и сохранение изменений
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();

                MessageBox.Show("Объект удален");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //проверка, есть ли в таблице записи
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //открытие новой формы при нажатии кнопки обновления строки
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Product product = db.Products.Find(id);

                AddManufacture addManufacture = new AddManufacture();


                //извлечение из таблицы соответствующих значений в поля формы для редактирования
                addManufacture.textBox1.Text = product.Article1;
                addManufacture.textBox2.Text = product.Manufacturer1;

                addManufacture.textBox4.Text = product.Article2;
                addManufacture.textBox5.Text = product.Manufacturer2;
                addManufacture.numericUpDown1.Value = product.Trust;

                DialogResult result = addManufacture.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                //запись новый данных
                product.Article1 = addManufacture.textBox1.Text;
                product.Manufacturer1 = addManufacture.textBox2.Text;


                product.Article2 = addManufacture.textBox4.Text;
                product.Manufacturer2 = addManufacture.textBox5.Text;
                product.Trust = (int)addManufacture.numericUpDown1.Value;


                //сохранение в БД и обновление таблицы                
                db.SaveChanges();
                dataGridView1.Refresh();


                MessageBox.Show("Объект обновлен");


            }
        }
        /// <summary>
        /// Кнопка поиска аналогов товара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Graph<string> graph = new Graph<string>();
            FindAConnection findAConnection = new FindAConnection();
            DialogResult result = findAConnection.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Product product = new Product();
            int Count_1 = 0;
            int Count_2 = 1;
            List<Vertex> vertices = new List<Vertex>();

            //цикл для добавления элементов в узлы, и эти же узлы добавляются по сторонам ребра и их вес, который является "доверие" в таблице
            foreach (var v in db.Products.Local.ToBindingList())
            {
                vertices.Add(new Vertex(СonvertedManufacturer(СonvertedArticle(v.Article1)), СonvertedManufacturer(v.Manufacturer1)));
                vertices.Add(new Vertex(СonvertedManufacturer(СonvertedArticle(v.Article2)), СonvertedManufacturer(v.Manufacturer2)));
                graph.Add(vertices[Count_1], vertices[Count_2], v.Trust);
                Count_1 += 2;
                Count_2 += 2;
            }
            


            // ввод значений в поиск аналогов 
            string Article1 = СonvertedManufacturer((СonvertedArticle(findAConnection.textBox1.Text)));
            string Manufacturer1 = СonvertedManufacturer(findAConnection.textBox2.Text);

            string Article2 = СonvertedManufacturer(СonvertedArticle(findAConnection.textBox3.Text));
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
                //if (start !=null & finish != null)
                //    return;
            }


            #region Условия для результата поисков аналога

            if (finish == null & start == null)
            {
                MessageBox.Show("Искомый и исходный товар отсутствует в таблице");
                return;
            }
            else if (start == null)
            {
                MessageBox.Show("Исходный товар отсутствует в таблице.");
                return;
            }
            else if (finish == null)
            {
                MessageBox.Show("Искомый товар отсутствует в таблице");
                return;
            }


            int Count2 = 1;

            ///метод, который присваивает словарю значение, является поиском аналогов. ключ - маршрут "От", значение - лист узлов, к которым доступен путь поиска.
            ///если искомое значение не найдено за n итераций, то возвращаемое значение приравнивается к null
            Dictionary<Vertex, List<Vertex>> values = graph.Wave(start, finish, Trust);


            ///тернарный оператор показывает, является ли значение словаря маршрута null или имеет конкретное значение
            bool x = values == null ? false : true;

            if (x)
            {
                foreach (var v in values)
                {
                    string column1 = $"Маршрут {Count2}";
                    string column2 = v.Key.ToString() + "-> ";

                    foreach (var i in v.Value)
                    {
                        column2 += i.ToString() + "/ ";
                    }
                    route.dataGridView2.Rows.Add(column1, column2);
                    Count2++;
                }
                route.ShowDialog(findAConnection);
            }
            else
            {
                MessageBox.Show($"Искомый товар \"{finish.Article + " " + finish.Manufacturer}\" не найден за {Trust} шагов.");
            }
            #endregion
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
