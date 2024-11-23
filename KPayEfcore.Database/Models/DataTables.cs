using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KPayEfcore.Database.Models;

    [Table("Users")]
    public partial class User_Tbl
    {
        [Key]
    [Column("userId")]
    public int UserId { get; set; }
    //[Required]
    [Column("userName")]
    public string Full_Name { get; set; }
    [Column("balance")]
    public double Balance { get; set; }
    [Column("phoneNumber")]
    public string Phone_Number { get; set; }

    [Column("createdDate")] 
    public DateTime CreatedDate { get; set; }



}

    [Table("Pin")]
    public partial class Pin_tbl
    {
        [Key]
    [Column("userId")]
    public int UserId { get; set; }
    [Column("pinCode")]
    public string PinCode { get; set; } = "";


    }

    [Table("OTPCodes")]
    public partial class OTP_Tbl
    {
        [Key]
    [Column("otpId")]
    public int Otp_Id { get; set; }
    [Column("userId")]
    public int UserId { get; set; }
    [Column("otpCode")]
    public string Otp_Code { get; set; }
    [Column("otpType")]
    public string Type { get; set; }
    [Column("otpExpiryDate")]
    public DateTime Expire_Date { get; set; }


    }

    [Table("FirstTimeLogin")]
    public partial class First_Login_Tbl
    {
   [Key][Column("userId")]
    public int UserId { get; set; }
    [Column("firstTimeCode")] public string Login_code { get; set; } 
    [Column("firstLoginTime")] public DateTime First_Login_Date { get; set; } 
    [Column("isFirstLogin")] public bool Is_LoggedIn { get; set; } = false; 

    }

    [Table("Transactions")]
    public partial class Transaction_Tbl
    {
        [Key]
    [Column("transactionId")]
    public int Transaction_Id { get; set; }
    [Column("senderId")]
    public int senderId { get; set; }
    [Column("receiverId")]
    public int? receiverId { get; set; } = null;
    [Column("amount")]
    public double amount { get; set; }
    [Column("transactionTime")]
    public DateTime Transaction_Date { get; set; }
    [Column("transactionType")]
    public string Transaction_Type { get; set; }
    [Column("notes")]
    public string Notes { get; set; }

    [Column("balanceAfter")]  

    public double BalanceAfter { get; set; }


}

[Table("Receipts")]  
public partial class Receipt
{
    [Column("receiptId")]  
    public int Receipt_Id { get; set; }

    [Column("transactionId")]  
    public int Transaction_Id { get; set; }

    [Column("userId")]  
    public int User_ID { get; set; }

    [Column("receiptContent")]  
    public string ReceiptContent { get; set; } = null!;

    [Column("issuedDate")] 
    public DateTime IssuedDate { get; set; }
}