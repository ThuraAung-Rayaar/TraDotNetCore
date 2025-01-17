using Microsoft.EntityFrameworkCore;
using MiniKpayDotNetCore.MinimalApi.Models;

namespace MiniKpayDotNetCore.MinimalApi.Services;

public  class MiniKpayServices
{
    private readonly AppDbContext dbcontext = new AppDbContext();
    Random random = new Random();
    public string? Generate_First_TimeCode(User_Tbl user)
    {
        string code  = random.Next(123456,987654).ToString();

        First_Login_Tbl first_Login = new First_Login_Tbl() { 
        UserId = user.UserId,
        Login_code = code,
        First_Login_Date = DateTime.Now
        
        
        };
        dbcontext.FirstTimeLogins.Add(first_Login);
       int result =  dbcontext.SaveChanges();
        return result>0? code:null;
    }


    public int Create_User(User_Tbl User) { 
        dbcontext.Users.Add(User);
       int result = dbcontext.SaveChanges();
        return result;
    }

    public string GetOtp(User_Tbl User,string type) { 
        string  code = random.Next(123456, 987654).ToString();

        OTP_Tbl otp = new OTP_Tbl() { 
        
        UserId= User.UserId,
        Otp_Code = code,
        Type = type,
        Expire_Date = DateTime.Now.AddMinutes(10)
        };
        dbcontext.OTP_Codes.Add(otp);
        dbcontext.SaveChanges();


        return code;
    }

    public User_Tbl? GetUserbyPhnum(string number) {

        var user = dbcontext.Users.Where(x => x.Phone_Number == number).FirstOrDefault();
        return user;
    
    }

    public int setupPin(User_Tbl user, string pin)
    {
        


        Pin_tbl item = new Pin_tbl()
        {

            UserId = user.UserId,
            PinCode = pin
        };

        dbcontext.Pin_Codes.Add(item);
        int result = dbcontext.SaveChanges();

        return result;
        
    }

    public int Change_Pin(User_Tbl user, string newpin)
    {
       var pinItem = dbcontext.Pin_Codes.AsNoTracking().Where(x=>x.UserId == user.UserId).FirstOrDefault();


        pinItem.PinCode = newpin;

        dbcontext.Entry(pinItem).State = EntityState.Modified;
        int result = dbcontext.SaveChanges();

        return result;

    }

    public bool CheckCode(int id,string code)
    {
       bool result = dbcontext.FirstTimeLogins.Any(x => x.UserId == id && x.Login_code == code);
        return result;


    }
    public bool CheckOTP(int id, string code, string type) {
        bool result = dbcontext.OTP_Codes.Any(x => x.UserId == id && x.Otp_Code == code && x.Type == type);
        return result;

    }
    public bool CheckPin(int id, string pin)
    {
        bool result = dbcontext.Pin_Codes.Any(x => x.UserId == id && x.PinCode == pin);
        return result;

    }

    public int changePhoneNumber(int id, string number) {


        var user = dbcontext.Users.AsNoTracking().Where(x => x.UserId == id).FirstOrDefault()!;

        user.Phone_Number = number;

        dbcontext.Entry(user).State = EntityState.Modified;
        int result = dbcontext.SaveChanges();
        return result;
    
    }

    public bool MoreBalance(User_Tbl sender, double amount)
    {
        var user = dbcontext.Users.AsNoTracking().Where(x => x.UserId == sender.UserId).FirstOrDefault()!;

        bool result = user.Balance > amount;

        return result;


    }

    public int DeductBalance(User_Tbl sender, double amount)
    {
        var user = dbcontext.Users.AsNoTracking().Where(x => x.UserId == sender.UserId).FirstOrDefault();

        user.Balance -= amount;

        dbcontext.Entry(user).State = EntityState.Modified;
        int result = dbcontext.SaveChanges();

        return result;
    }

    public int AddBalance(User_Tbl sender, double amount)
    {
        var user = dbcontext.Users.AsNoTracking().Where(x => x.UserId == sender.UserId).FirstOrDefault();

        user.Balance += amount;

        dbcontext.Entry(user).State = EntityState.Modified;
        int result = dbcontext.SaveChanges();

        return result;
    }

    public Transaction_Tbl? CreateTransaction(User_Tbl sender, User_Tbl receiver, double Amount, string note,string type)
    {
        Transaction_Tbl transaction = new Transaction_Tbl() { 
        senderId = sender.UserId,
        receiverId = receiver.UserId,
        amount = Amount,
        Notes = note,
        Transaction_Date = DateTime.Now,
        Transaction_Type = type,

        
        
        };

        dbcontext.Transactions.Add(transaction);
       int result =  dbcontext.SaveChanges();
        if (result == 0) { return null; }
        return transaction;

    }
    public Transaction_Tbl? CreateTransaction(User_Tbl sender,  double Amount, string note, string type)
    {
        Transaction_Tbl transaction = new Transaction_Tbl()
        {
            senderId = sender.UserId,
           
            amount = Amount,
            Notes = note,
            Transaction_Date = DateTime.Now,
            Transaction_Type = type,



        };

        dbcontext.Transactions.Add(transaction);
        int result = dbcontext.SaveChanges();
        if (result == 0) { return null; }
        return transaction;

    }

    public List<Transaction_Tbl>? getTransactionHistory(int userId)
    {
        var list = dbcontext.Transactions.Where(x=>x.senderId == userId || x.receiverId == userId).ToList();
        return list;


       
    }
}
