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
    private readonly AppDbContext db = new AppDbContext(); // Class-level context

    public int GenerateCode()
    {
        Random rnd = new Random();
        return rnd.Next(123456, 987654);
    }

    public void GenerateOneLogInCode(int id, string code)
    {
        FirstTimeLogin item = new FirstTimeLogin()
        {
            UserId = id,
            FirstTimeCode = code,
            FirstLoginTime = DateTime.Now,
            IsFirstLogin = true
        };

        db.FirstTimeLogin.Add(item);
        db.SaveChanges();
    }

    public string CreateAccount(User user)
    {
        string code = GenerateCode().ToString();
        db.Users.Add(user);
        db.SaveChanges();

        GenerateOneLogInCode(user.UserId, code);
        return code;
    }
    public string GetOTP(string number) {

        var item = GetUserByPhoneNumber(number);
        return GetOTP(item.UserId);
    
    }

    public string GetOTP(int id)
    {
        string code = GenerateCode().ToString();
        var firstTimeRecord = db.FirstTimeLogin.FirstOrDefault(x => x.UserId == id);

        if (firstTimeRecord is null)
        {
            Console.WriteLine("No data for FirstTimeLogin");
            return string.Empty;
        }

        OTPCode otpItem = new OTPCode()
        {
            UserId = id,
            OtpCodeValue = code,
            OtpExpiryDate = DateTime.Now.AddHours(24),
            OtpStatus = "active",
            OtpType = "Login"
        };

        if (firstTimeRecord.IsFirstLogin)
        {
            db.OtpCodes.Add(otpItem);
            Console.WriteLine("FirstTimeLogin True");
        }
        else
        {
            var existingOtp = db.OtpCodes.AsNoTracking().FirstOrDefault(x => x.UserId == id);
            if (existingOtp != null)
            {
                otpItem.OtpId = existingOtp.OtpId;
                db.OtpCodes.Update(otpItem);
            }
            else
            {
                db.OtpCodes.Add(otpItem);
            }

            Console.WriteLine("FirstTimeLogin False");
        }

        int result = db.SaveChanges();
        return result > 0 ? code : "Concurrency issue: No rows affected.";
    }

    public string LoginFirstTime(int id, string code,string pin)
    {
        var item = db.FirstTimeLogin.AsNoTracking().FirstOrDefault(x => x.UserId == id && x.FirstTimeCode == code)!;
        //if (item is null)
        //{
        //    //Console.WriteLine("No Data");
        //    return "No User";
        //}

        db.Entry(item).State = EntityState.Detached;
        item.IsFirstLogin = false;
        db.Attach(item);
        
        db.Entry(item).State = EntityState.Modified;


        int result = db.SaveChanges();

        setupPin(id, pin);

        return result > 0 ? GetOTP(id) : string.Empty;
    }

    public void setupPin(int id, string pin)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == id);
        if (user is null)
        {
            //Console.WriteLine("No Data");
            return;
        }
        
        Pincode item = new Pincode() { 
        
        UserId=id,
        PinCode=pin
        };
        db.PinCodes.Add(item);
        //user.pin = pin;
        //db.Entry(user).State = EntityState.Modified;
        db.SaveChanges();
        //Console.WriteLine("Pin set up");
    }

    public string ChangePin(int id, string oldPin, string newPin)
    {
        if (!checkPin(id, oldPin))
        {
            //Console.WriteLine("Pin Error");
            return "Wrong Old Pin Error";
        }

        var pinItem = db.PinCodes.AsNoTracking().FirstOrDefault(x => x.UserId == id);
        //if (user is null)
        //{
        //    Console.WriteLine("No Data");
        //    return;
        //}

        pinItem.PinCode = newPin;
        db.Entry(pinItem).State = EntityState.Modified;
        db.SaveChanges();
       return "Pin changed successful";
    }

    public bool hasUser(int id)
    {
        return db.Users.Any(x => x.UserId == id);
    }

    public User? login(int id, string otp)
    {
        var userExist = db.Users.FirstOrDefault(x => x.UserId == id);
        //if (userExist is null)
        //{
        //    Console.WriteLine("No Data");
        //    return null;
        //}

        var otpItem = db.OtpCodes.AsNoTracking().FirstOrDefault(x => x.UserId == id && x.OtpCodeValue == otp && x.OtpExpiryDate > DateTime.Now && x.OtpType == "Login") ;
        db.Entry(otpItem).State = EntityState.Detached;
        if (otpItem is not null)
        {
            
            otpItem.OtpStatus = "Expired";
           
           
           
        }
        else
        {
            Console.WriteLine("Invalid OTP");
            return null;
        }


        db.Attach(otpItem);
        db.Entry(otpItem).State = EntityState.Modified;
        db.SaveChanges();
        Console.WriteLine("Login Success");
        return userExist;
    }

    public double getBalance(int id)
    {
        return db.Users.Where(x => x.UserId == id).Select(x => x.Balance).FirstOrDefault();
    }

    public bool hasMoreBalance(int id, double amount)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == id);
        bool hasmore = user is not null && user.Balance > amount ;
        return hasmore;
    }

    public void createTransaction(Transaction item)
    {
        db.Add(item);
        db.SaveChanges();
    }

    public bool checkPin(int id, string pin)
    {
        return db.PinCodes.Any(x => x.UserId == id && x.PinCode == pin);
    }

    public string getReceiptContent(Transaction transaction)
    {
        var transactionSummary = new
        {
            transaction.SenderId,
            transaction.ReceiverId,
            transaction.TransactionType,
            transaction.Amount,
            transaction.TransactionTime
        };
        return JsonConvert.SerializeObject(transactionSummary);
    }

    public void createReceipt(Transaction transaction)
    {
        Receipt senderReceipt = new Receipt()
        {
            TransactionId = transaction.TransactionId,
            UserID = transaction.SenderId,
            IssuedDate = transaction.TransactionTime,
            ReceiptContent = getReceiptContent(transaction),
        };
        db.Receipts.Add(senderReceipt);

        if (transaction.ReceiverId is not null)
        {
            Receipt receiverReceipt = new Receipt()
            {
                TransactionId = transaction.TransactionId,
                UserID = Convert.ToInt32(transaction.ReceiverId),
                IssuedDate = transaction.TransactionTime,
                ReceiptContent = getReceiptContent(transaction),
            };
            db.Receipts.Add(receiverReceipt);
        }

        db.SaveChanges();
    }

    public Transaction? BalanceTransfer(int senderId,  int receiverId, double amount, string note)
    {
        //if (senderId == receiverId || !checkPin(senderId, pin) || !hasUser(receiverId) || !hasMoreBalance(senderId, amount))
        //{
        //    Console.WriteLine("Transaction error");
        //    return null;
        //}

        var sender = db.Users.FirstOrDefault(x => x.UserId == senderId)!;
        sender.Balance -= amount;


        //db.Entry(sender).State = EntityState.Modified;

        var receiver = db.Users.FirstOrDefault(x => x.UserId == receiverId)!;
        receiver.Balance += amount;


        //db.Entry(receiver).State = EntityState.Modified;
        //db.Entry(sender).State = EntityState.Detached;
        //db.Entry(receiver).State = EntityState.Detached;




        //db.Attach(sender);
        //db.Attach(receiver);






        int result = db.SaveChanges();
        if (result > 0)
        {
            Transaction transaction = new Transaction()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Amount = amount,
                BalanceAfter = sender.Balance,
                TransactionTime = DateTime.Now,
                Status = "Completed",
                TransactionType = "Transfer",
                Note = note
            };
            createTransaction(transaction);
            createReceipt(transaction);
            return transaction;
        }

        Console.WriteLine("Transfer failed");
        return null;
    }

    public List<Transaction>? getTransactionHistory(int id)
    {
        if (!hasUser(id))
        {
            Console.WriteLine("No Data");
            return null;
        }

        return db.Transactions.Where(x => x.ReceiverId == id || x.SenderId == id).ToList();
    }

    public List<Receipt> getReceiptRecord(int id)
    {
        if (!hasUser(id))
        {
            Console.WriteLine("No Data");
            return null;
        }

        return db.Receipts.Where(x => x.UserID == id).ToList();
    }

    public Transaction depositMoney(int id, double amount, string note)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == id)!;

       // db.Entry(user).State = EntityState.Detached;
        user.Balance += amount;
        //db.Attach(user);


        db.Entry(user).State = EntityState.Modified;
        db.SaveChanges();

        Transaction transaction = new Transaction()
        {
            SenderId = id,
            Amount = amount,
            BalanceAfter = user.Balance,
            TransactionType = "Deposit",
            Status = "Complete",
            TransactionTime = DateTime.Now,
            Note = note
        };

        createTransaction(transaction);
        createReceipt(transaction);
        return transaction;
    }

    public Transaction WithDrawMoney(int id, double amount, string note)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == id);

        //db.Entry(user).State = EntityState.Detached;
        user.Balance -= amount;
        //db.Attach(user);

        db.Entry(user).State = EntityState.Modified;
        db.SaveChanges();

        Transaction transaction = new Transaction()
        {
            SenderId = id,
            Amount = amount,
            BalanceAfter = user.Balance,
            TransactionType = "Withdraw",
            Status = "Complete",
            TransactionTime = DateTime.Now,
            Note = note
        };

        createTransaction(transaction);
        createReceipt(transaction);
        return transaction;
    }

    public string ChangePhoneNumber(int userId, string newPhoneNumber)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == userId)!;
        //if (user is null)
        //{
        //    Console.WriteLine("User not found");
        //    return;
        //}

        user.PhoneNumber = newPhoneNumber;
        db.Entry(user).State = EntityState.Modified;
        db.SaveChanges();
        // Console.WriteLine("Phone number updated successfully.");

        return "Phone number updated successfully.";
    }

    public string ForgetPin(int userId)
    {
        var user = db.Users.FirstOrDefault(x => x.UserId == userId);
        //if (user is null)
        //{
        //    Console.WriteLine("User not found.");
        //    return string.Empty;
        //}

        string resetCode = GenerateCode().ToString();

        OTPCode otpItem = new OTPCode()
        {
            UserId = userId,
            OtpCodeValue = resetCode,
            OtpExpiryDate = DateTime.Now.AddMinutes(10),
            OtpStatus = "Active",
            OtpType = "ResetPin"
        };

        db.OtpCodes.Add(otpItem);
        db.SaveChanges();
        return resetCode;
    }

    public string ResetPin(int userId, string resetCode, string newPin)
    {
        var otpRecord = db.OtpCodes.AsNoTracking().FirstOrDefault(x => x.UserId == userId && x.OtpCodeValue == resetCode && x.OtpExpiryDate > DateTime.Now && x.OtpType == "ResetPin");

        if (otpRecord is null)
        {
           // Console.WriteLine("Invalid or expired reset code.");
            return "Invalid or expired reset code.";
        }

        var pinItem = db.PinCodes.AsNoTracking().FirstOrDefault(x => x.UserId == userId);
        //if (user == null)
        //{
        //    Console.WriteLine("User not found.");
        //    return;
        //}

        pinItem.PinCode = newPin;
        db.Entry(pinItem).State = EntityState.Modified;

        otpRecord.OtpStatus = "Expired";
        db.Entry(otpRecord).State = EntityState.Modified;

        db.SaveChanges();
        //Console.WriteLine("Pin reset successfully.");
        return "Pin reset successfully.";
    }

    public User? GetUserByPhoneNumber(string phoneNumber)
    {   var item = db.Users.Where(x=>x.PhoneNumber == phoneNumber).FirstOrDefault();
        return item;
    }
}

