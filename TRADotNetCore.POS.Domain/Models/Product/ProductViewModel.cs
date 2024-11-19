using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRADotNetCore.POS.Domain.Models.Product
{
    public class ProductViewModel
    {
        public string productCode {  get; set; }
        public string productName { get; set; }
       
        public string productCategoryName { get; set; }
        public decimal price { get; set; }
        public int productCount { get; set; }

        public DateTime lastStocked { get; set; }
    }
}
