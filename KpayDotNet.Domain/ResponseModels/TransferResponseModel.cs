using KPayEfcore.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpayDotNet.Domain.ResponseModels
{
    public class TransferResponseModel
    {
        public BaseResponseModel Response { get; set; }
        //public User_Tbl user { get; set; }

        public List<Transaction_Tbl?>? TransactionHistory { get; set; }

        public List<Receipt>? ReceiptRecords { get; set; }
    }

    public class ResultTransferResponseModel
    {
        //public BaseResponseModel Response { get; set; }
        ////public User_Tbl user { get; set; }

        public List<Transaction_Tbl?>? TransactionHistory { get; set; }

        public List<Receipt>? ReceiptRecords { get; set; }
    }
}
