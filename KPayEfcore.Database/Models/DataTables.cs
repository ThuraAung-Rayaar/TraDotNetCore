using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KPayEfcore.Database.Models
{
    [Table("Users")]  // Maps the class to the Users table
public partial class User
{
    [Column("userId")]  // Maps UserId property to userId column
    public int UserId { get; set; }

    [Column("phoneNumber")]  // Maps PhoneNumber property to phoneNumber column
    public string PhoneNumber { get; set; } = null!;

    [Column("userName")]  // Maps UserName property to userName column
    public string UserName { get; set; } = null!;

    [Column("balance")]  // Maps Balance property to balance column
    public double Balance { get; set; }

    [Column("createdDate")]  // Maps CreatedDate property to createdDate column
    public DateTime CreatedDate { get; set; }

    [Column("updatedDate")]  // Maps UpdatedDate property to updatedDate column
    public DateTime UpdatedDate { get; set; }

    //[Column("pin")]  // Maps pin property to pin column
    //public string? pin { get; set; } = null;


}

    [Table("Pin")]
    public partial class Pincode { 
    
        [Column("userId")]
        public int UserId { get; set; }
        [Column("pinCode")]
        public string PinCode {  get; set; }


    }


    [Table("FirstTimeLogin")]  // Maps the class to the FirstTimeLogin table
    public partial class FirstTimeLogin
    {
        [Column("firstLoginId")]  // Maps FirstLoginId property to firstLoginId column
        public int FirstLoginId { get; set; }

        [Column("userId")]  // Maps UserId property to userId column
        public int UserId { get; set; }

        [Column("firstTimeCode")]  // Maps FirstTimeCode property to firstTimeCode column
        public string FirstTimeCode { get; set; } = null!;

        [Column("firstLoginTime")]  // Maps FirstLoginTime property to firstLoginTime column
        public DateTime FirstLoginTime { get; set; }

        [Column("isFirstLogin")]  // Maps IsFirstLogin property to isFirstLogin column
        public bool IsFirstLogin { get; set; }
    }

    [Table("OTPCodes")]  // Maps the class to the OTPCodes table
    public partial class OTPCode
    {
        [Column("otpId")]  // Maps the OtpId property to the otpId column
        public int OtpId { get; set; }

        [Column("userId")]  // Maps the UserId property to the userId column
        public int UserId { get; set; }

        [Column("otpCode")]  // Maps the OtpCodeValue property to the otpCode column
        public string? OtpCodeValue { get; set; } = null!;

        [Column("otpExpiryDate")]  // Maps the OtpExpiryDate property to the otpExpiryDate column
        public DateTime OtpExpiryDate { get; set; }

        [Column("otpStatus")]  // Maps the OtpStatus property to the otpStatus column
        public string? OtpStatus { get; set; } = null!;

        [Column("otpType")]
        public string OtpType { get; set; }
    }


    [Table("Transactions")]  // Maps the class to the Transactions table
    public partial class Transaction
    {
        [Column("transactionId")]  // Maps TransactionId property to transactionId column
        public int TransactionId { get; set; }

        [Column("senderId")]  // Maps SenderId property to senderId column
        public int SenderId { get; set; }

        [Column("transactionType")]  // Maps TransactionType property to transactionType column
        public string TransactionType { get; set; } = null!;

        [Column("amount")]  // Maps Amount property to amount column
        public double Amount { get; set; }

        [Column("receiverId")]  // Maps ReceiverId property to receiverId column
        public int? ReceiverId { get; set; } = null;

        [Column("transactionTime")]  // Maps TransactionTime property to transactionTime column
        public DateTime TransactionTime { get; set; }

        [Column("status")]  // Maps Status property to status column
        public string Status { get; set; } = null!;

        [Column("balanceAfter")]  // Maps BalanceAfter property to balanceAfter column
        public double BalanceAfter { get; set; }

        [Column("notes")]
        public string Note { get; set; }

    }

    [Table("Receipts")]  // Maps the class to the Receipts table
    public partial class Receipt
    {
        [Column("receiptId")]  // Maps ReceiptId property to receiptId column
        public int ReceiptId { get; set; }

        [Column("transactionId")]  // Maps TransactionId property to transactionId column
        public int TransactionId { get; set; }

        [Column("userId")]  // Maps UserID property to userId column
        public int UserID { get; set; }

        [Column("receiptContent")]  // Maps ReceiptContent property to receiptContent column
        public string ReceiptContent { get; set; } = null!;

        [Column("issuedDate")]  // Maps IssuedDate property to issuedDate column
        public DateTime IssuedDate { get; set; }
    }
}
