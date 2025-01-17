using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniKpayDotNetCore.MinimalApi.Models;

[Table("Users")]
public partial class User_Tbl
{
    [Key]
    public int UserId { get; set; }
    //[Required]
    public string Full_Name { get; set; }
    public double Balance { get; set; } 

    public string Phone_Number  { get; set; }
    




}

[Table("Pin")]
public partial class Pin_tbl {
    [Key]
    public int UserId { get; set; }
    public string PinCode { get; set; } = "";


}

[Table("OTPCodes")]
public partial class OTP_Tbl {
    [Key]
    public int Otp_Id { get; set; }
    public int UserId { get; set; }

    public string Otp_Code { get; set; }

    public string Type { get; set; }

    public DateTime Expire_Date { get; set; }


}

[Table("FirstTimeLogin")]
public partial class First_Login_Tbl {
    [Key]
    public int UserId { get; set; }

    public string Login_code { get; set; }
    public DateTime First_Login_Date { get; set; } 

    public bool Is_LoggedIn { get; set; } =false;

}

[Table("Transactions")]
public partial class Transaction_Tbl {
    [Key]
    public int Transaction_Id { get; set; }
    public int senderId { get; set; }
    public int? receiverId { get; set; } = null;
    
    public double amount { get; set; }

    public DateTime Transaction_Date { get; set; }

    public string Transaction_Type { get; set; }

    public string Notes { get; set; }



}

