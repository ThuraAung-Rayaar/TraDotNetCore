using KpayDotNet.Domain.Functions;
using KpayDotNet.Domain.ResponseModels;
using KPayEfcore.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KpayDotNet.Domain.Features;


public class KpayServices
{
    private readonly ServiceHelper _helper = new ServiceHelper();

    #region User  Response

    public async Task<UserResponseModel> CreateWalletUserAsync(User_Tbl user) {
        BaseResponseModel baseResponse;
        string ResCode;
        #region Check Ecisting Phone Number
        bool isExist = await _helper.CheckDuplicatePhoneNum(user.Phone_Number);

        if (isExist) {
            ResCode = EnumResponse.DuplicateEntry.ToString();
            baseResponse = BaseResponseModel.DuplicateEntryError(ResCode, "Phone Number Exist");
            goto Response_Result;
        }
        #endregion



        #region User Creation
        int result = await _helper.Create_UserAsync(user);
        if(result==0)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error Creating user");
            
            goto Response_Result;
        };
        #endregion



        #region Get Firt Time Login Code
        var FTcode =await _helper.Generate_First_TimeCodeAsync(user);

        if (FTcode is null) { 
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.DuplicateEntryError(ResCode, "Error Generating Code");
            goto Response_Result;

        }
        #endregion
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, "Success");
       
        Response_Result:
        return new UserResponseModel{ 
        Response = baseResponse,
        user = user
           
        };

       


    }

    public async Task<UserResponseModel> FirstTimeLoginAsync(string number, string code, string newPin)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        #region Check Phone Number
        var user =await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            goto Response_Result;
        }
        #endregion

        #region Check Login Code
        var checkCode =await _helper.CheckCodeAsync(user.UserId, code);
        if (!checkCode)
        {
            ResCode = EnumResponse.ValidationError.ToString();
            baseResponse = BaseResponseModel.ValidationError(ResCode, "Wrong Code");
            goto Response_Result;
        }
        #endregion

        #region Setup New Pin for User
        int pinResult =await _helper.SetupPinAsync(user, newPin);
        if (pinResult == 0)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error Pin SEtUp");
            goto Response_Result;

        }
        #endregion
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Pin Setup Complete");

    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
       // return "Pin Setup Complete";
    }
    public async Task<UserResponseModel> GetOtpAsync(string number)
    {
        BaseResponseModel baseResponse;
        string ResCode;

        #region Check Phone Number
        var user = await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            goto Response_Result;
        }
        #endregion

        #region Request OTP Code for Login
        var code =await _helper.GetOtpAsync(user, "Login");
        #endregion

        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
       // return code;
    }
    public async Task<UserResponseModel> LoginAsync(string number, string code)
    {
        BaseResponseModel baseResponse;
        string ResCode;

        #region Check Phone Number
        var user =await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            goto Response_Result;
        }
        #endregion

        #region Check OTP Code
        var checkCode = await _helper.CheckOTPAsync(user.UserId, code, "Login");
        if (!checkCode)
        {
            ResCode = EnumResponse.ValidationError.ToString();
            baseResponse = BaseResponseModel.ValidationError(ResCode, "Phone Number not Exist");
            user = null;
            goto Response_Result;
        };
        #endregion

        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
      //  return user;
    }

    public async Task<UserResponseModel> GetBalanceAsync(string number)
    
    {
        BaseResponseModel baseResponse;
        string ResCode;
        #region Get User
        var user =await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
            {
                ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
                baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
                goto Response_Result;
            }
        #endregion

        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
        #endregion
    }

    public async Task<UserResponseModel> ForgetPinAsync(string number)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        #region Get User
        var user = await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            goto Response_Result;
        }
        #endregion
        var resetCode = await _helper.GetOtpAsync(user, "Reset");


        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
        #endregion
        //return resetCode;
    }

    public async Task<UserResponseModel> ResetPinAsync(string number, string resetCode, string newPin)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        #region Get User
        var user = await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            goto Response_Result;
        }
        #endregion

        #region Check OTP
        var checkCode =await _helper.CheckOTPAsync(user.UserId, resetCode, "Reset");
        if (!checkCode) 
            {
                ResCode = EnumResponse.ValidationError.ToString();
                baseResponse = BaseResponseModel.ValidationError(ResCode, "Wrong OTP");
                user = null;
                goto Response_Result;
            };
        #endregion

        
        var pinResult = await _helper.Change_PinAsync(user, newPin);
        if (pinResult == 0)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            user = null;
            goto Response_Result;
        };

        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
        #endregion
       // return "Pin Change Successful";
    }
    public async Task<UserResponseModel> ChangePinAsync(string number, string oldPin, string newPin)
    {
        BaseResponseModel baseResponse;
        string ResCode;


        #region Get User
        var user = await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            goto Response_Result;
        }
        #endregion

        #region PIn Check
        var checkPin = await _helper.CheckPinAsyn(user.UserId, oldPin);
        if (!checkPin)
        {
            ResCode = EnumResponse.IncorrectPin.ToString();
            baseResponse = BaseResponseModel.IncorrectPinError(ResCode, "INcorrect pin");
            user = null;
            goto Response_Result;
        }
        #endregion



        var pinResult = await _helper.Change_PinAsync(user, newPin);
        if (pinResult == 0)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            user = null;
            goto Response_Result;
        };



        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
        #endregion

        //return "Pin Change Successful";
    }
    public async Task<UserResponseModel> ChangePhoneNumberAsync(string oldNumber, string newNumber, string pin)
    {
        BaseResponseModel baseResponse;
        string ResCode;

        #region Get User
        var user = await _helper.GetUserbyPhnumAsync(oldNumber);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            goto Response_Result;
        }
        #endregion


        #region PIn Check
        var checkPin = await _helper.CheckPinAsyn(user.UserId, pin);
        if (!checkPin)
        {
            ResCode = EnumResponse.IncorrectPin.ToString();
            baseResponse = BaseResponseModel.IncorrectPinError(ResCode, "INcorrect PIn");
            user = null;
            goto Response_Result;
        }
        #endregion


        var result = await _helper.ChangePhoneNumberAsync(user.UserId, newNumber);
        if (result == 0)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            user = null;
            goto Response_Result;
        };


        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new UserResponseModel
        {
            Response = baseResponse,
            user = user

        };
        #endregion


        // return "Phone Number Change Successful";
    }

    #endregion

    #region Transaction Response
    public async Task<TransferResponseModel> TransferBalanceAsync(string senderNumber, string receiverNumber, double amount, string pin, string note)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        List<Transaction_Tbl?>? transactionResponse;
        List<Receipt>? receiptRecords=null;
        #region Check Note
        if (string.IsNullOrEmpty(note))
        {
            ResCode = EnumResponse.InputError.ToString();
            baseResponse = BaseResponseModel.InputError(ResCode, "User Input Error");
            transactionResponse= null;
            goto Response_Result;
        }
        #endregion

        #region Check PhoneNumber Duplicate
        if (senderNumber == receiverNumber)
        {
            ResCode = EnumResponse.DuplicateEntry.ToString();
            baseResponse = BaseResponseModel.DuplicateEntryError(ResCode, "Sender and receiver cannot be the same phone number");
            transactionResponse = null;
            goto Response_Result;
        };
        #endregion

        #region Check Sender Validity
        var sender =await _helper.GetUserbyPhnumAsync(senderNumber);
        if (sender is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Sender Phone Number not Exist");
            transactionResponse = null;
            goto Response_Result;
        }

        #endregion

        #region PIn Check
        var checkPin = await _helper.CheckPinAsyn(sender.UserId, pin);
        if (!checkPin)
        {
            ResCode = EnumResponse.IncorrectPin.ToString();
            baseResponse = BaseResponseModel.IncorrectPinError(ResCode, "INcorrect PIn");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region balance CHeck
        var moreBalance = await _helper.MoreBalanceAsync(sender, amount + 10000);
        if (!moreBalance)
        {
            ResCode = EnumResponse.InsufficientBalance.ToString();
            baseResponse = BaseResponseModel.InsufficientBalanceError(ResCode, "Insufficient Balance");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region Check receiver Validity
        var receiver = await _helper.GetUserbyPhnumAsync(receiverNumber);
        if (receiver is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Receiver Phone Number not Exist");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion


       

        

        #region Balance transfer Check

        var resultSender = await _helper.DeductBalanceAsync(sender, amount);

        var resultReceiver = await _helper.AddBalanceAsync(receiver, amount);

        if (resultSender == 0 || resultReceiver == 0)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error Balance Transfer");
            transactionResponse = null;
            goto Response_Result;
        };

        #endregion

        #region Create Transaction History
        var transaction = await _helper.CreateTransactionAsync(sender, receiver, amount, note, "Transfer");
        if (transaction is null)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            transactionResponse = null;
            goto Response_Result;
        }
        transactionResponse = new List<Transaction_Tbl?>() { transaction };
        #endregion

        #region Create Receipt  Records

        var receipts = await _helper.createReceiptAsync(transaction);
        if (receipts is null)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            transactionResponse = null;
            goto Response_Result;
        }
        receiptRecords = new List<Receipt>() ;
        receiptRecords.AddRange(receiptRecords);


        #endregion

        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new TransferResponseModel
        {
            Response = baseResponse,
            TransactionHistory = transactionResponse!
           ,ReceiptRecords = receiptRecords
        };
        #endregion

        //return "Transaction Completed";
    }
    public async Task<TransferResponseModel> DepositMoneyAsync(string senderNumber, double amount, string pin, string note)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        List<Transaction_Tbl?>? transactionResponse;
        List<Receipt>? receiptRecords = null;
        #region Check Note
        if (string.IsNullOrEmpty(note))
        {
            ResCode = EnumResponse.InputError.ToString();
            baseResponse = BaseResponseModel.InputError(ResCode, "User Input Error");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region Check Sender Validity
        var sender = await _helper.GetUserbyPhnumAsync(senderNumber);
        if (sender is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Sender Phone Number not Exist");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region PIn Check
        var checkPin = await _helper.CheckPinAsyn(sender.UserId, pin);
        if (!checkPin)
        {
            ResCode = EnumResponse.IncorrectPin.ToString();
            baseResponse = BaseResponseModel.IncorrectPinError(ResCode, "INcorrect PIn");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region balance CHeck
        var moreBalance = await _helper.MoreBalanceAsync(sender, amount + 10000);
        if (!moreBalance)
        {
            ResCode = EnumResponse.InsufficientBalance.ToString();
            baseResponse = BaseResponseModel.InsufficientBalanceError(ResCode, "Insufficient Balance");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region Balance transfer Check

        var resultSender = await _helper.AddBalanceAsync(sender, amount);

        //var resultReceiver = await _helper.AddBalanceAsync(receiver, amount);

        if (resultSender == 0 )
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error Money Deposit");
            transactionResponse = null;
            goto Response_Result;
        };

        #endregion

        #region Create Transaction History
        var transaction = await _helper.CreateTransactionAsync(sender, amount, note, "Deposit");
        if (transaction is null)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            transactionResponse = null;
            goto Response_Result;
        };
        transactionResponse = new List<Transaction_Tbl?>() { transaction };
        #endregion

        #region Create Receipt  Records

        var receipts = await _helper.createReceiptAsync(transaction);
        if (receipts is null)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            transactionResponse = null;
            goto Response_Result;
        }
        receiptRecords = new List<Receipt>();
        receiptRecords.AddRange(receiptRecords);


        #endregion

        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new TransferResponseModel
        {
            Response = baseResponse,
            TransactionHistory = transactionResponse!
            ,
            ReceiptRecords = receiptRecords
        };
        #endregion
        //return "Money Deposit Successful";
    }

    public async Task<TransferResponseModel> WithdrawMoneyAsync(string senderNumber, double amount, string pin, string note)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        List<Transaction_Tbl?>? transactionResponse;
        List<Receipt>? receiptRecords = null;
        #region Check Note
        if (string.IsNullOrEmpty(note))
        {
            ResCode = EnumResponse.InputError.ToString();
            baseResponse = BaseResponseModel.InputError(ResCode, "User Input Error");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region Check Sender Validity
        var sender = await _helper.GetUserbyPhnumAsync(senderNumber);
        if (sender is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Sender Phone Number not Exist");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region PIn Check
        var checkPin = await _helper.CheckPinAsyn(sender.UserId, pin);
        if (!checkPin)
        {
            ResCode = EnumResponse.IncorrectPin.ToString();
            baseResponse = BaseResponseModel.IncorrectPinError(ResCode, "INcorrect PIn");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        #region Balance transfer Check

        var resultSender = await _helper.DeductBalanceAsync(sender, amount);

        //var resultReceiver = await _helper.AddBalanceAsync(receiver, amount);

        if (resultSender == 0)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error Money Deposit");
            transactionResponse = null;
            goto Response_Result;
        };

        #endregion

        #region Create Transaction History
        var transaction = await _helper.CreateTransactionAsync(sender, amount, note, "Withdraw");
        if (transaction is null)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            transactionResponse = null;
            goto Response_Result;
        };
        transactionResponse = new List<Transaction_Tbl?>() { transaction};
        #endregion

        #region Create Receipt  Records

        var receipts = await _helper.createReceiptAsync(transaction);
        if (receipts is null)
        {
            ResCode = EnumResponse.InternalServerError.ToString();
            baseResponse = BaseResponseModel.InternalServerError(ResCode, "Error internal server");
            transactionResponse = null;
            goto Response_Result;
        }
        receiptRecords = new List<Receipt>();
        receiptRecords.AddRange(receiptRecords);


        #endregion

        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");
    Response_Result:
        return new TransferResponseModel
        {
            Response = baseResponse,
            TransactionHistory = transactionResponse!,
            ReceiptRecords = receiptRecords
        };
    
        #endregion
       // return "Money Withdraw Successful";
    }

    public async Task<TransferResponseModel> GetTransactionHistoryAsync(string number)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        List<Transaction_Tbl?>? transactionResponse; List<Receipt>? receiptRecords = null;
        #region Get User
        var user = await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            transactionResponse=null;
            goto Response_Result;
        }
        #endregion

        #region Get Transaction History
        var transactionList = await _helper.GetTransactionHistoryAsync(user.UserId);
        if (transactionList is null || !transactionList.Any())
        {
            ResCode = EnumResponse.NotFound.ToString();
            baseResponse = BaseResponseModel.NotFoundError(ResCode, "NO Transaction History");
            transactionResponse = null;
            goto Response_Result;
        };

        transactionResponse = transactionList!;
        #endregion

        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");


    Response_Result:
        return new TransferResponseModel
        {
            Response = baseResponse,
            TransactionHistory = transactionResponse
            ,ReceiptRecords = receiptRecords
        };
        #endregion
    }
    public async Task<TransferResponseModel> GetReceiptRecordsAsync(string number)
    {
        BaseResponseModel baseResponse;
        string ResCode;
        List<Transaction_Tbl?>? transactionResponse=null; List<Receipt>? receiptRecords = null;
        #region Get User
        var user = await _helper.GetUserbyPhnumAsync(number);
        if (user is null)
        {
            ResCode = EnumResponse.IncorrectPhoneNumber.ToString();
            baseResponse = BaseResponseModel.IncorrectPhoneNumberError(ResCode, "Phone Number not Exist");
            transactionResponse = null;
            goto Response_Result;
        }
        #endregion

        //not needed Transaction
        /* #region Get Transaction History
         var transactionList = await _helper.GetTransactionHistoryAsync(user.UserId);
         if (transactionList is null || !transactionList.Any())
         {
             ResCode = EnumResponse.NotFound.ToString();
             baseResponse = BaseResponseModel.NotFoundError(ResCode, "NO Transaction History");
             transactionResponse = null;
             goto Response_Result;
         };

         transactionResponse = transactionList!;
         #endregion*/

        #region Get Receiprt Records

        var Receipts = await _helper.GetReceiptRecords(user.UserId);
        if (Receipts is null || Receipts.Count == 0) {
            {
                ResCode = EnumResponse.NotFound.ToString();
                baseResponse = BaseResponseModel.NotFoundError(ResCode, "NO REceipt Records");
                receiptRecords = null;
                goto Response_Result;
            };
        }
        #endregion

        #region Response 
        ResCode = EnumResponse.Success.ToString();
        baseResponse = BaseResponseModel.Success(ResCode, " Success");


    Response_Result:
        return new TransferResponseModel
        {
            Response = baseResponse,
            TransactionHistory = transactionResponse
            ,
            ReceiptRecords = receiptRecords
        };
        #endregion
    }
    #endregion
}


