using KpayDotNet.Domain.Functions;
using KPayEfcore.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KpayDotNet.Domain.Features;


public class KpayService
{
    private readonly ServiceHelper _serviceHelper;

    public KpayService()
    {
        _serviceHelper = new ServiceHelper(); // Initialize the service helper
    }

    // Deposit API
    public string Deposit(string mobileNo, double amount, string pin, string note)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo); // Assume a method that fetches user by phone number
        if (user is null)
        {
            return "User not found";
        }
        if (!_serviceHelper.checkPin(user.UserId, pin))
        {
            return "Invalid pin.";
        }
        if (!_serviceHelper.hasMoreBalance(user.UserId, amount + 10000))
        {
            return "Insufficient balance. Minimum balance of 10,000 MMK required after Deposit.";
        }
        // Call the depositMoney method in ServiceHelper
        var transaction = _serviceHelper.depositMoney(user.UserId, amount, note);

        return transaction != null ? "Deposit successful" : "Deposit failed";
    }

    // Withdraw API
    public string Withdraw(string mobileNo, double amount,string pin,string note)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return "User not found";
        }
        if (!_serviceHelper.checkPin(user.UserId, pin))
        {
            return "Invalid pin.";
        }
        // Ensure that the balance after withdrawal will be at least 10,000 MMK
        if (!_serviceHelper.hasMoreBalance(user.UserId, amount + 10000))
        {
            return "Insufficient balance. Minimum balance of 10,000 MMK required after withdrawal.";
        }

        // Call the WithDrawMoney method in ServiceHelper
        var transaction = _serviceHelper.WithDrawMoney(user.UserId, amount, note);

        return transaction != null ? "Withdraw successful" : "Withdraw failed";
    }

    // Transfer API
    public string Transfer(string fromMobileNo, string toMobileNo, double amount, string pin, string note)
    {
        // Validate sender and receiver are not the same
        if (fromMobileNo == toMobileNo)
        {
            return "Transfer failed: Sender and receiver cannot be the same.";
        }

        var sender = _serviceHelper.GetUserByPhoneNumber(fromMobileNo);
        var receiver = _serviceHelper.GetUserByPhoneNumber(toMobileNo);

        // Check sender and receiver existence
        if (sender is null)
        {
            return "Sender not found.";
        }

        if (receiver is null)
        {
            return "Receiver not found.";
        }

        // Validate pin for the sender
        if (!_serviceHelper.checkPin(sender.UserId, pin))
        {
            return "Invalid pin.";
        }

        // Ensure sender has enough balance
        if (!_serviceHelper.hasMoreBalance(sender.UserId, amount+10000))
        {
            return "Insufficient balance.";
        }

        // Perform the balance transfer
        var transaction = _serviceHelper.BalanceTransfer(sender.UserId,  receiver.UserId, amount, note);

        return transaction != null ? "Transfer successful" : "Transfer failed";
    }

    // Transaction History API
    public List<Transaction>? GetTransactionHistory(string mobileNo)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return null; // No data if user doesn't exist
        }

        // Get transaction history
        return _serviceHelper.getTransactionHistory(user.UserId);
    }

    public List<Receipt>? GetReceiptHistory(string mobileNo)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return null; // No data if user doesn't exist
        }

        // Get transaction history
        var list = _serviceHelper.getReceiptRecord(user.UserId);
        return list;
    }

    // Balance Check API
    public double GetBalance(string mobileNo)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return 0.0; // Return 0 for non-existing users
        }

        return _serviceHelper.getBalance(user.UserId);
    }

    // Create Wallet User API
    public string CreateWalletUser(User user)
    {
        string code = _serviceHelper.CreateAccount(user);
        // return !string.IsNullOrEmpty(code) ? "Account created successfully. Use the code for first login." : "Account creation failed.";
        return code;
    }

    // First Time Login API
    public string FirstTimeLogin(string mobileNo, string code,string pin)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return "User not found";
        }
        var result = _serviceHelper.LoginFirstTime(user.UserId, code, pin);
        // Validate first-time login using the code
        return result;
    }

    // Login API (Using OTP)
    public User? Login(string mobileNo, string otp)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return null; // No data if user doesn't exist
        }
        
        // Attempt login via OTP
        var userItem = _serviceHelper.login(user.UserId, otp);
        return userItem;
    }

    // Change Pin API
    public string ChangePin(string mobileNo, string oldPin, string newPin)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return "User not found";
        }

        // Change pin
       string result = _serviceHelper.ChangePin(user.UserId, oldPin, newPin);
        return result;
    }

    public string GetOtp(string mobileNo)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return "User not found";
        }
        var item = _serviceHelper.GetOTP(mobileNo);
        

        // Change pin
        
        return item;
    }

    // Change Phone Number API
    public string ChangePhoneNumber(string oldMobileNo, string newMobileNo,string pin)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(oldMobileNo);
        if (_serviceHelper.checkPin(user.UserId, pin) ){ 
        
            return "Pin Error";
        }
        if (user is null)
        {
            return "User not found";
        }

        // Change phone number
      string result =   _serviceHelper.ChangePhoneNumber(user.UserId, newMobileNo);
        return result;
    }

    // Forget Password API
    public string ForgetPin(string mobileNo)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return "User not found";
        }

        // Generate and return OTP for password reset
        return _serviceHelper.ForgetPin(user.UserId);
    }

    // Reset Password API
    public string ResetPin(string mobileNo, string resetCode, string newPin)
    {
        var user = _serviceHelper.GetUserByPhoneNumber(mobileNo);
        if (user is null)
        {
            return "User not found";
        }

        // Reset the pin
      string result =   _serviceHelper.ResetPin(user.UserId, resetCode, newPin);
        return result;
    }
}
