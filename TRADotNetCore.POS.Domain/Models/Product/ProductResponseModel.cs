using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRADotNetCore.POS.Database.Models;

namespace TRADotNetCore.POS.Domain.Models.Product
{
    public class ProductResponseModel
    {
        public BaseResponseModel Response {  get; set; }

        public List<ProductViewModel>? productModels { get; set; }

    }

    public class ResultProductResponseModel
    {
      // public BaseResponseModel response { get; set; }

        public List<ProductViewModel>? productModels { get; set; }

    }
}
