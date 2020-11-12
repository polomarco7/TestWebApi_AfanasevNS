using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi_AfanasevNS.Models
{
    public class ProductMovements
    {
        public int Id { get; set; }
        public DateTime InsertDateTime { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
