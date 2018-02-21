using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02_20180217
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            System.Threading.Thread.Sleep(1);
        }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? UnitsInStock { get; set; }
    }
}
