using System.Drawing;
using System.Windows.Forms;

namespace DirectoryOfAnalogs
{
    public partial class Route : Form
    {
        public Route()
        {
            InitializeComponent();
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);
        }
        /// <summary>
        /// Метод для отображения TabPage в горизонтальном положении.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;
            TabPage _tabPage = tabControl1.TabPages[e.Index];


            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);
            _textBrush = new SolidBrush(Color.Black);

            Font font = tabControl1.Font;

            StringFormat _stringFlags = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(_tabPage.Text, font, _textBrush, _tabBounds, new StringFormat(_stringFlags));

        }
        /// <summary>
        /// Метод отображения в различных TabPage таблиц с маршрутами.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public void TableCallSettings(DataGridView data, int count)
        {
            DataGridViewTextBoxColumn dBox = new DataGridViewTextBoxColumn { HeaderText = "Содержание маршрута" };
            data.Size = tabControl1.Size;
            data.Columns.AddRange(dBox);
            data.RowHeadersVisible = false;
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabControl1.TabPages[count].Controls.Add(data);
        }
    }
}
