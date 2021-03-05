using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    public class Product
    {

        public string Article { get; set; }
        public string Manufacturer { get; set; }
        public int Trust { get; set; }

        public Product(string article, string manufacturer, int trust)
        {
            Article = article;
            Manufacturer = manufacturer;
            Trust = trust;
        }
        static string СonvertedArticle(string str)
        {
            string result = Regex.Replace(str, @"[^\w]", "");
            return result;
        }
        static string СonvertedManufacturer(string str)
        {
            string result = str.ToLower();
            return result;
        }
    }
}
