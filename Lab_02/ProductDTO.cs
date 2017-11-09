using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_02
{
    class ProductDTO
    {
        public ProductDTO()
        {
            Thread.Sleep(1);
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public Nullable<decimal> UnitsInStock { get; set; }
    }
}
