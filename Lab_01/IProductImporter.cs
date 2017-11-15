using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_01
{
    public interface IProductImporter
    {
        ProductInfo Process(ProductInfo product, CancellationToken token);
        void Save(Task<ProductInfo> t);
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public override string ToString()
        {
            return $"Id: {Id}, Nombre: {Name}";
        }
    }
}
