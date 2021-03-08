using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryOfAnalogs.Logic
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("DbConnectionString")
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
