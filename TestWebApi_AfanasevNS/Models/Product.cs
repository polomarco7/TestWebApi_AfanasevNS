using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi_AfanasevNS.Models
{
    public class ProductUom
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UomId { get; set; }
        public ProductUom ProductUom { get; set; }
    }

    public class ProductAll
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Uom { get; set; }
    }

    public class ProductMovements
    {
        public int Id { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public Product Product { get; set; }

    }
}
