using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.ProductModels
{
    public class ProductUpdateDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
    }
}
