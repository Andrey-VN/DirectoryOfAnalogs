using DirectoryOfAnalogs.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DirectoryOfAnalogs
{

    public partial class MainFormOfAnalogs : Form
    {
        readonly ProductContext db;      

        public MainFormOfAnalogs()
        {
            InitializeComponent();

            //настройки таблицы при инициализации
            dataGridView1.RowHeadersVisible = false; 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ///Инициализация связи с БД и извлечение значений в таблицу
            db = new ProductContext();
            db.Products.Load();
            dataGridView1.DataSource = db.Products.Local.ToBindingList();
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
            AddValues(addManufacture, product);

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
                if (IsLineSelection(out int id) == false)
                    return;
                Product product = db.Products.Find(id);

                //удаление строки из БД и сохранение изменений
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
                if (IsLineSelection(out int id) == false)
                    return;

                Product product = db.Products.Find(id);

                AddManufacture addManufacture = new AddManufacture();
                //извлечение из таблицы соответствующих значений в поля формы для редактирования
                ExtractFromTable(product, addManufacture);

                DialogResult result = addManufacture.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                //запись новый данных
                AddValues(addManufacture, product);
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
            Graph graph = new Graph();
            FindAConnection findAConnection = new FindAConnection();
            DialogResult result = findAConnection.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            //цикл для добавления элементов в узлы, и эти же узлы добавляются по сторонам ребра и их вес, который является "доверие" в таблице
            foreach (var v in db.Products.Local.ToBindingList())
            {
                graph.Add(GetConvertedItem(v.Article1, v.Manufacturer1), GetConvertedItem(v.Article2, v.Manufacturer2), v.Trust);
            }

            Route route = new Route();   //создание экземпляра окна с отображением маршрутов с аналогами

            //Метод поиска аналогов, если возвращаемый список пуст, то товар либо не найден, либо его нет в таблице
            Dictionary<string, Dictionary<Vertex, Vertex>> valKey = graph.BFS(GetConvertedItem(findAConnection.textBox1.Text, findAConnection.textBox2.Text), 
                GetConvertedItem(findAConnection.textBox3.Text, findAConnection.textBox4.Text),
                (int)findAConnection.numericUpDown1.Value);

            ConclusionOfAnalogsInTheTable(findAConnection, route, valKey);
        }


        /// <summary>
        /// Метод вызова и отображения резальтатов поиска аналогов.
        /// </summary>
        /// <param name="findAConnection"></param>
        /// <param name="route"></param>
        /// <param name="valKey"></param>
        private static void ConclusionOfAnalogsInTheTable(FindAConnection findAConnection, Route route, Dictionary<string, Dictionary<Vertex, Vertex>> valKey)
        {
            int count = 0;
            if (valKey.Count != 0)
            {
                foreach (var i in valKey)
                {
                    DataGridView data = new DataGridView(); //экземпляр для отображения маршрутов в таблице
                    route.tabControl1.TabPages.Add(i.Key);  //добавление название вкладки текущего маршрута
                    route.TableCallSettings(data, count);
                    
                    foreach (var j in i.Value)
                    {
                        data.Rows.Add(j.Key.Item + " -> " + j.Value.Item);  //добавление в таблицу шагов текущего маршрута
                    }
                    count++;
                }
                route.ShowDialog(findAConnection);    
            }
            else
            {
                MessageBox.Show($"Искомый товар «{GetConvertedItem(findAConnection.textBox3.Text, findAConnection.textBox4.Text)}» " +
                    $"не найден за «{(int)findAConnection.numericUpDown1.Value}» шагов");
            }
        }
        /// <summary>
        /// Метод отображения артикула без символов-разделителей и в верхнем регистре
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string СonvertedArticle(string str)
        {
            string result = Regex.Replace(str, @"[^\w]", "");
            return СonvertedManufacturer(result);
        }

        /// <summary>
        /// Метот отображения строки в верхнем регистре
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string СonvertedManufacturer(string str)
        {
            string result = str.ToUpper();
            return result;
        }
        private bool IsLineSelection(out int id)
        {
            int index = dataGridView1.SelectedRows[0].Index;
            return int.TryParse(dataGridView1[0, index].Value.ToString(), out id);
        }
        private static void ExtractFromTable(Product product, AddManufacture addManufacture)
        {
            addManufacture.textBox1.Text = product.Article1;
            addManufacture.textBox2.Text = product.Manufacturer1;

            addManufacture.textBox4.Text = product.Article2;
            addManufacture.textBox5.Text = product.Manufacturer2;
            addManufacture.numericUpDown1.Value = product.Trust;
        }

        private static void AddValues(AddManufacture addManufacture, Product product)
        {
            product.Article1 = addManufacture.textBox1.Text;
            product.Manufacturer1 = addManufacture.textBox2.Text;


            product.Article2 = addManufacture.textBox4.Text;
            product.Manufacturer2 = addManufacture.textBox5.Text;
            product.Trust = (int)addManufacture.numericUpDown1.Value;
        }
        /// <summary>
        /// Возвращение строкового значения артикул + продукт.
        /// </summary>
        /// <param name="article"></param>
        /// <param name="manufacturer"></param>
        /// <returns></returns>
        private static string GetConvertedItem(string article, string manufacturer)
        {
            return СonvertedArticle(article) + "  " + СonvertedManufacturer(manufacturer);
        }
    }
}
