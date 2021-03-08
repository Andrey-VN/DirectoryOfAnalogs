using DirectoryOfAnalogs.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs
{
    public class Product
    {

        public int Id { get; set; }
        public string Article1 { get; set; }
        public string Manufacturer1 { get; set; }

        public string Article2 { get; set; }
        public string Manufacturer2 { get; set; }
        public int Trust { get; set; }

        public Product()
        {
            
        }
    }
}
