using KPayEfcore.Database.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KpayDotNet.Domain.Functions;

public class ServiceHelper
{

    private readonly AppDbContext dbcontext = new AppDbContext();
    Random random = new Random();
    public async Task<string?> Generate_First_TimeCodeAsync(User_Tbl user)
    {
        string code = random.Next(123456, 987654).ToString();

        First_Login_Tbl first_Login = new First_Login_Tbl()
        {
            UserId = user.UserId,
            Login_code = code,
            First_Login_Date = DateTime.Now


        };
        dbcontext.FirstTimeLogins.Add(first_Login);
        int result = await dbcontext.SaveChangesAsync();
        return result > 0 ? code : null;
    }

    public async Task<bool> CheckDuplicatePhoneNum(string phone_Number)
    {
        var isExist =await dbcontext.Users.AnyAsync(x=>x.Phone_Number == phone_Number);
        return isExist;
    }
    public async Task<int> Create_UserAsync(User_Tbl user)
    {
        dbcontext.Users.Add(user);
        return await dbcontext.SaveChangesAsync();
    }

    public async Task<string?> GetOtpAsync(User_Tbl user, string type)
    {
        string code = random.Next(123456, 987654).ToString();

        OTP_Tbl otp = new OTP_Tbl()
        {
            UserId = user.UserId,
            Otp_Code = code,
            Type = type,
            Expire_Date = DateTime.Now.AddMinutes(10)
        };

        dbcontext.OTP_Codes.Add(otp);
        int result = await dbcontext.SaveChangesAsync();
        return result > 0 ? code : null;
    }

    public async Task<User_Tbl?> GetUserbyPhnumAsync(string number)
    {

        var user = await dbcontext.Users.Where(x => x.Phone_Number == number).FirstOrDefaultAsync();
        return user;

    }

   public async Task<int> SetupPinAsync(User_Tbl user, string pin)
    {
        Pin_tbl item = new Pin_tbl()
        {
            UserId = user.UserId,
            PinCode = pin
        };

        dbcontext.Pin_Codes.Add(item);
        int result = await dbcontext.SaveChangesAsync();
        return result;
    }

    public async Task<int> Change_PinAsync(User_Tbl user, string newpin)
    {
        var pinItem = dbcontext.Pin_Codes.AsNoTracking().Where(x => x.UserId == user.UserId).FirstOrDefault();


        pinItem.PinCode = newpin;

        dbcontext.Entry(pinItem).State = EntityState.Modified;
        int result = await dbcontext.SaveChangesAsync();

        return result;

    }

    public async Task<bool> CheckCodeAsync(int id, string code)
    {
        bool result = await dbcontext.FirstTimeLogins.AnyAsync(x => x.UserId == id && x.Login_code == code);
        return result;


    }
    public async Task<bool> CheckOTPAsync(int id, string code, string type)
    {
        bool result = await dbcontext.OTP_Codes.AnyAsync(x => x.UserId == id && x.Otp_Code == code && x.Type == type);
        return result;

    }
    public async Task<bool> CheckPinAsyn(int id, string pin)
    {
        bool result = await dbcontext.Pin_Codes.AnyAsync(x => x.UserId == id && x.PinCode == pin);
        return result;

    }

    public async Task<int> ChangePhoneNumberAsync(int id, string number)
    {


        var user = await dbcontext.Users.AsNoTracking().Where(x => x.UserId == id).FirstOrDefaultAsync()!;

        user.Phone_Number = number;

        dbcontext.Entry(user).State = EntityState.Modified;
        int result = await dbcontext.SaveChangesAsync();
        return result;

    }

    public async Task<bool> MoreBalanceAsync(User_Tbl sender, double amount)
    {
        var user = await dbcontext.Users.FirstOrDefaultAsync(x => x.UserId == sender.UserId);

        bool result = user!.Balance > amount;

        return result;


    }

    public async Task<int> DeductBalanceAsync(User_Tbl sender, double amount)
    {
        var user = await dbcontext.Users.AsNoTracking().Where(x => x.UserId == sender.UserId).FirstOrDefaultAsync();

        user.Balance -= amount;

        dbcontext.Entry(user).State = EntityState.Modified;
        int result = await dbcontext.SaveChangesAsync();

        return result;
    }

    public async Task<int> AddBalanceAsync(User_Tbl sender, double amount)
    {
        var user = await dbcontext.Users.AsNoTracking().Where(x => x.UserId == sender.UserId).FirstOrDefaultAsync();

        user.Balance += amount;

        dbcontext.Entry(user).State = EntityState.Modified;
        int result = await dbcontext.SaveChangesAsync();

        return result;
    }

    public async Task<Transaction_Tbl?> CreateTransactionAsync(User_Tbl sender, User_Tbl receiver, double Amount, string note, string type)
    {
        Transaction_Tbl transaction = new Transaction_Tbl()
        {
            senderId = sender.UserId,
            receiverId = receiver.UserId,
            amount = Amount,
            Notes = note,
            Transaction_Date = DateTime.Now,
            Transaction_Type = type,



        };

        dbcontext.Transactions.Add(transaction);
        int result = await dbcontext.SaveChangesAsync();
        if (result == 0) { return null; }
        return transaction;

    }
    public async Task<Transaction_Tbl?> CreateTransactionAsync(User_Tbl sender, double Amount, string note, string type)
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
        int result = await dbcontext.SaveChangesAsync();
        if (result == 0) { return null; }
        return transaction;

    }

    public async Task<List<Transaction_Tbl>> GetTransactionHistoryAsync(int userId)
    {
        var list = await dbcontext.Transactions.Where(x => x.senderId == userId || x.receiverId == userId).ToListAsync();
        return list;



    }

    public string getReceiptContent(Transaction_Tbl transaction)
    {
        var transactionSummary = new
        {
            transaction.senderId,
            transaction.receiverId,
            transaction.Transaction_Type,
            transaction.amount,
            transaction.Transaction_Date
        };
        return JsonConvert.SerializeObject(transactionSummary);
    }
    public async Task<List<Receipt?>?> createReceiptAsync(Transaction_Tbl transaction)
    {   
        var ReceiptList =   new List<Receipt?>();
        Receipt senderReceipt = new Receipt()
        {
            Transaction_Id = transaction.Transaction_Id,
            User_ID = transaction.senderId,
            IssuedDate = transaction.Transaction_Date,
            ReceiptContent = getReceiptContent(transaction),
        };
        dbcontext.ReceiptRecords.Add(senderReceipt);
        ReceiptList.Add(senderReceipt);
        if (transaction.receiverId is not null)
        {
            Receipt receiverReceipt = new Receipt()
            {
                Transaction_Id = transaction.Transaction_Id,
                User_ID = Convert.ToInt32(transaction.receiverId),
                IssuedDate = transaction.Transaction_Date,
                ReceiptContent = getReceiptContent(transaction),
            };
            dbcontext.ReceiptRecords.Add(receiverReceipt);
            ReceiptList.Add(receiverReceipt);
        }

        int result = await dbcontext.SaveChangesAsync();

        if (result == 0) { return null; }
        return ReceiptList;
    }

    public async Task<List<Receipt?>?> GetReceiptRecords(int id)
    {
        var ReceiptList =await dbcontext.ReceiptRecords.Where(x=>x.User_ID==id).ToListAsync();
        if(ReceiptList is null) { return null; }
        return ReceiptList!;


    }
}

