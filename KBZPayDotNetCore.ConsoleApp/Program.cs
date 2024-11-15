// See https://aka.ms/new-console-template for more information
using KPayEfcore.Database.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;

Console.WriteLine("Hello, World!");

/*
 
Kpay

Mobile number

User ID
Full Name
MObile num
balance
pin
bank => deposit / withdraw 


API
-Create User  -t
-Firsst time login -t
-Login -t
-Deposit -t
-Transfer -t
-Withdraw -t
-Transaction history -t
-REciept -t
-
-Phone No Change

-Forget Password

-Reset Password



 */



int GenerateCode() {
    Random rnd = new Random();
    return rnd.Next(123456, 987654);


}

void GenerateOneLogInCode(int id,string code) {

   


    AppDbContext dbContext = new AppDbContext();
    FirstTimeLogin item = new FirstTimeLogin() { 
    
        UserId = id,
        FirstTimeCode = code,
        FirstLoginTime = DateTime.Now,
        IsFirstLogin = true
    
    
    };


    dbContext.FirstTimeLogin.Add(item);
    int result = dbContext.SaveChanges();
   

}
string CreateAccount(User user) { 

    Random rnd = new Random();
    string code = GenerateCode().ToString();

    AppDbContext db = new AppDbContext();
    db.Users.Add(user);
    int result = db.SaveChanges();

    GenerateOneLogInCode(user.UserId,code);
   
    return code;
}

string GetOTP(int id)
{

    string code = GenerateCode().ToString(); // Generate a 6-digit OTP code



    AppDbContext appDb = new AppDbContext();
    var firstTimeRecord = appDb.FirstTimeLogin.Where(x => x.UserId == id).FirstOrDefault();



    if (firstTimeRecord is null)

    {

        Console.WriteLine("No data for FirstTimeLogin");
        return string.Empty;

    }



    bool isfirstTimeLogin = firstTimeRecord.IsFirstLogin;



    OTPCode otpItem = new OTPCode()
    {
        UserId = id,
        OtpCodeValue = code,
        OtpExpiryDate = DateTime.Now.AddHours(24),
        OtpStatus = "active",
        OtpType = "Login"

    };



    if (isfirstTimeLogin)
    {
        appDb.OtpCodes.Add(otpItem);
        Console.WriteLine("FirstTimeLogin True");

    }

    else
    {
        var item = appDb.OtpCodes.AsNoTracking().Where(x => x.UserId == id).FirstOrDefault();

        if (item != null)
        {
            otpItem.OtpId = item.OtpId;
            appDb.OtpCodes.Update(otpItem);
        }

        else
        {
            appDb.OtpCodes.Add(otpItem);
        }


        Console.WriteLine("FirstTimeLogin False");

    }



    int result = appDb.SaveChanges();

    if (result == 0)

    {

        Console.WriteLine("Concurrency issue: No rows affected.");

        return "Concurrency issue: No rows affected.";

    }



    return code;

}

string LoginFirstTime(int id,string code) { 
    
    AppDbContext appDb = new AppDbContext();
    var item = appDb.FirstTimeLogin.AsNoTracking().Where(x=>x.UserId==id && x.FirstTimeCode == code).FirstOrDefault();
    if (item is null)
    {
        Console.WriteLine("No DAta");
        return "No DAta";
    }
    item.IsFirstLogin = false;
    appDb.Entry(item).State = EntityState.Modified;
    int result = appDb.SaveChanges();
    if (result > 0)
    {
        string otpCode = GetOTP(id);
        return otpCode;
    }

    return string.Empty;


}

void setupPin(int id,string pin) { 
    AppDbContext db = new AppDbContext();
    var item =db.Users.AsNoTracking().Where(x => x.UserId == id).FirstOrDefault();
    if (item is null) {
        Console.WriteLine("No DAta");
        return ;

    }
    item.pin = pin;
    
    db.Entry(item).State = EntityState.Modified;
  int result =   db.SaveChanges();


    Console.WriteLine("pin set up");
}

void ChangePin(int id, string oldPin,string newPin)
{
    if (checkPin(id, oldPin))
    {

        Console.WriteLine("Pin Error");
        return;
    }

    AppDbContext db = new AppDbContext();
    var item = db.Users.AsNoTracking().Where(x => x.UserId == id).FirstOrDefault();
    if (item is null)
    {
        Console.WriteLine("No DAta");
        return;

    }
    item.pin = newPin;

    db.Entry(item).State = EntityState.Modified;
    int result = db.SaveChanges();


    Console.WriteLine("pin Changed");
}

bool hasUser(int id) {

    AppDbContext appDb=new AppDbContext();
    var item = appDb.Users.Where(x=>x.UserId == id).FirstOrDefault();
    return item is null ? false : true;
   
}




User? login(int id,string otp)
{

    AppDbContext db = new AppDbContext();
    var userExist = db.Users.Where(x => x.UserId == id).FirstOrDefault();
    if (userExist is null)
    {
        Console.WriteLine("No Data 2");
        return null;

    }
    var Otpitem = db.OtpCodes.AsNoTracking().Where(x => x.UserId == id && x.OtpCodeValue == otp && x.OtpExpiryDate > DateTime.Now).FirstOrDefault();


    if (Otpitem is not null)
    {
        Otpitem.OtpStatus = "Expired";
        db.Entry(Otpitem).State = EntityState.Modified;
        db.SaveChanges();
    }


    if (Otpitem is null)
    {
        Console.WriteLine("Fail / Invalid OTP");
        return null;

    }
    Console.WriteLine("Login Success");

    return userExist;
}

double getBalance(int id) {
    AppDbContext appDb = new AppDbContext();
    double balance = appDb.Users.Where(x => x.UserId == id ).FirstOrDefault().Balance;
    return balance;
}

bool hasMoreBalance(int id,double amount)
{

    AppDbContext appDb = new AppDbContext();
    // bool hasMore = appDb.Users.Select(x => x.UserId == id && (x.Balance - amount)>=10000).FirstOrDefault();
    var item = appDb.Users.Where(x => x.UserId == id).FirstOrDefault();
    if(item is null)return false;
    bool hasMore = item.Balance >amount && (item.Balance - amount)>1000;
    return hasMore;
}
void createTransaction(Transaction item) { 


    AppDbContext appDb = new AppDbContext();
    
    appDb.Add(item);
    appDb.SaveChanges();

}


bool checkPin(int id,string pin) { 

        AppDbContext db = new AppDbContext();
        var check  = db.Users.Select(x=>x.UserId == id && x.pin == pin).FirstOrDefault();

        return check;

}

string getReceiptContent(Transaction transaction) {

    var transactionSummary = new {
        transaction.SenderId, 
        transaction.ReceiverId,
        transaction.TransactionType,
        transaction.Amount,
        transaction.TransactionTime };

    string json = JsonConvert.SerializeObject(transactionSummary, Formatting.Indented);
    return json;

}

void createReceipt(Transaction transaction) {
    AppDbContext db = new AppDbContext();

    Receipt senderReceipt = new Receipt()
        {
            TransactionId = transaction.TransactionId,
            UserID = transaction.SenderId,
            IssuedDate = transaction.TransactionTime,
            ReceiptContent = getReceiptContent(transaction),

            //
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

                //
            };
        db.Receipts.Add(receiverReceipt);
    }
    
    db.SaveChanges();


}

Transaction? BalanceTransfer(int senderId,string pin,int receiverId,double amount,string note) {

    if(senderId == receiverId)
    {

        Console.WriteLine("Error Transaction");
        return null;
    }
    if(checkPin(senderId,pin))
    {

        Console.WriteLine("Pin Error");
        return null;
    }
    if (!hasUser(receiverId)) {

        Console.WriteLine("No Receiver");    
        return null;
    }
    if (!hasMoreBalance(senderId,amount)) {

        Console.WriteLine("Not Enough MOney");
        return null;

    }

   AppDbContext appDb = new AppDbContext();
    var sender = appDb.Users.AsNoTracking().Where(x=>x.UserId==senderId).FirstOrDefault();
    var reciver = appDb.Users.AsNoTracking().Where(x => x.UserId == receiverId).FirstOrDefault();

    sender.Balance -= amount;
    appDb.Entry(sender).State = EntityState.Modified;
   

    reciver.Balance += amount;
    appDb.Entry(reciver).State = EntityState.Modified;

    int result = appDb.SaveChanges();

    double balanceAfter = sender.Balance;

    // NEED TO WRITE TRANSACTION AND RECEIPT FOR RCEIVER
    if (result > 0)
    {
        Transaction transactionHistory = new Transaction()
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Amount = amount,
            BalanceAfter = balanceAfter,
            TransactionTime = DateTime.Now,
            Status = "Completed",
            TransactionType = "Transfer",
            Note = note

        };
        createTransaction(transactionHistory);
        createReceipt(transactionHistory);

        Console.WriteLine("Transfer Complete");
        return transactionHistory;
    }
    else { Console.WriteLine("Transfer INComplete");
    return null ;
    }

    

    

}


List<Transaction> getTransactionHistory(int id) {

    if (!hasUser(id)) {

        Console.WriteLine("No DAta");
        return null;
    }


    AppDbContext db = new AppDbContext();
    var itemList = db.Transactions.Where(x => x.ReceiverId == id || x.SenderId == id).ToList();
    Console.WriteLine("This work");
    
    return itemList; 
}



List<Receipt> getReceiptRecord(int id) {

    if (!hasUser(id))
    {

        Console.WriteLine("No DAta");
        return null;
    }


    var db = new AppDbContext();
    var itemList = db.Receipts.Where(x=>x.UserID == id).ToList();

    return itemList;




}


Transaction depositMoney(int id, double amount,string note) { 
    
    AppDbContext db = new AppDbContext();
    var user =db.Users.AsNoTracking().Where(x => x.UserId == id).FirstOrDefault();

    user.Balance -= amount;
    db.Entry(user).State = EntityState.Modified;
    db.SaveChanges();

    

    Transaction transaction = new Transaction() { 
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

    Console.WriteLine("Deposit money");
    return transaction;
}

Transaction WithDrawMoney(int id, double amount, string note)
{

    AppDbContext db = new AppDbContext();
    var user = db.Users.AsNoTracking().Where(x => x.UserId == id).FirstOrDefault();

    user.Balance += amount;
    db.Entry(user).State = EntityState.Modified;
    db.SaveChanges();



    Transaction transaction = new Transaction()
    {
        SenderId = id,
        Amount = amount,
        BalanceAfter = user.Balance,
        TransactionType = "Withdraw",
        Status = "Complete",
        TransactionTime = DateTime.Now, Note = note

    };

    createTransaction(transaction);
    createReceipt(transaction);

    Console.WriteLine("Withdraw money");
    return transaction;
}

void ChangePhoneNumber(int userId, string newPhoneNumber)
{
    using AppDbContext db = new AppDbContext();
    
    var user = db.Users.FirstOrDefault(x => x.UserId == userId);

    if (user is null)
    {
        Console.WriteLine("User not found");
        return;
    }

   
    user.PhoneNumber = newPhoneNumber;

    
    db.Entry(user).State = EntityState.Modified;
    int result = db.SaveChanges();

    if (result > 0)
    {
        Console.WriteLine("Phone number updated successfully.");
    }
    else
    {
        Console.WriteLine("Error updating phone number.");
    }
}
string ForgetPin(int userId)
{
    using AppDbContext db = new AppDbContext();
    // Check if user exists
    var user = db.Users.FirstOrDefault(x => x.UserId == userId);

    if (user is null)
    {
        Console.WriteLine("User not found.");
        return string.Empty;
    }

    
    string resetCode = GenerateCode().ToString(); 

    
    OTPCode otpItem = new OTPCode()
    {
        UserId = userId,
        OtpCodeValue = resetCode,
        OtpExpiryDate = DateTime.Now.AddMinutes(10), 
        OtpStatus = "Active",
        OtpType="ResetPin"
    };

    
    db.OtpCodes.Add(otpItem);
    int result = db.SaveChanges();

    if (result > 0)
    {
        
        Console.WriteLine($"Password reset code sent to the user's phone number: {resetCode}");
        return resetCode;
    }
    else
    {
        Console.WriteLine("Error generating reset code.");
        return string.Empty;
    }
}

void ResetPin(int userId, string resetCode, string newPin)
{
    using AppDbContext db = new AppDbContext();
    
    var otpRecord = db.OtpCodes.FirstOrDefault(x => x.UserId == userId && x.OtpCodeValue == resetCode && x.OtpExpiryDate > DateTime.Now && x.OtpType=="ResetPin");

    if (otpRecord is null)
    {
        Console.WriteLine("Invalid or expired reset code.");
        return;
    }

    // Find the user
    var user = db.Users.FirstOrDefault(x => x.UserId == userId);

    if (user == null)
    {
        Console.WriteLine("User not found.");
        return;
    }

    // Update the user's password
    user.pin = newPin; // Assuming you have a 'Password' column in the Users table
    db.Entry(user).State = EntityState.Modified;

    // Mark the OTP as used
    otpRecord.OtpStatus = "Expired";
    db.Entry(otpRecord).State = EntityState.Modified;

    // Save changes to both the user and OTP
    int result = db.SaveChanges();

    if (result > 0)
    {
        Console.WriteLine("Password reset successfully.");
    }
    else
    {
        Console.WriteLine("Error resetting the password.");
    }
}



var tracList2 = getTransactionHistory(1);


DevEntension.printList(tracList2);

//DevEntension.printList<Users>(DevEntension.showUsers());
//DevEntension.printList<Transaction>(DevEntension.showTransaction());
//DevEntension.printList<Receipt>(DevEntension.showReceipt());
//int userid = 3;

//string newOTP = LoginFirstTime(userid, DevEntension.getFirstCode(userid));

//var item = login(userid, newOTP);

//Console.WriteLine(item);

//Console.WriteLine(getBalance(item.UserId));

//DevEntension.PrintObjectProperties<Users>(item);
//Console.WriteLine(hasUser(2));
//var tracc = depositMoney(1, 10000);
//var tracc2 = WithDrawMoney(3, 55555);
//var tracc =BalanceTransfer(1, 2, 5000);
//DevEntension.PrintObjectProperties<Transaction>(tracc);


static class DevEntension {

    public static List<User> showUsers()
    {

        AppDbContext db = new AppDbContext();
        var item = db.Users.ToList();

        return item;
    }
    public static List<Transaction> showTransaction()
    {

        AppDbContext db = new AppDbContext();
        var item = db.Transactions.ToList();

        return item;
    }
    public static List<Receipt> showReceipt()
    {

        AppDbContext db = new AppDbContext();
        var item = db.Receipts.ToList();

        return item;
    }
    public static string getOtpCode(int id) { 
    
        AppDbContext db = new AppDbContext();
        var item =db.OtpCodes.Where(x=>x.UserId ==id).FirstOrDefault();

        return item.OtpCodeValue;
    }
    public static string getFirstCode(int id)
    {

        AppDbContext db = new AppDbContext();
        var item = db.FirstTimeLogin.Where(x => x.UserId == id).FirstOrDefault();

        return item.FirstTimeCode;
    }

    public static int  getUserId(string element)
    {

        AppDbContext db = new AppDbContext();
        var item = db.Users.Where(x => x.UserName == element&& x.PhoneNumber == element).FirstOrDefault();

        return item.UserId;
    }
    public static void printList<T>(List<T> list) {

        foreach (var item in list) { 
        
            PrintObjectProperties<T>(item);
        
        }
    
    }


    public static void PrintObjectProperties<T>(T obj)
    {
        foreach (var propertyInfo in typeof(T).GetProperties())
        {
            var value = propertyInfo.GetValue(obj);
            Console.WriteLine($"{propertyInfo.Name}: {value}");
        }
        Console.WriteLine("------------------------------");
    }

}

