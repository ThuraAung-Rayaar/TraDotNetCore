using KPayEfcore.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpayDotNet.Domain.ResponseModels
{
    public class UserResponseModel
    {
        public BaseResponseModel Response { get; set; }
        public User_Tbl? user { get; set; }

    }

    public class ResultUserResponseModel
    {
      //  public BaseResponseModel Response { get; set; }
        public User_Tbl? user { get; set; }

    }
}
