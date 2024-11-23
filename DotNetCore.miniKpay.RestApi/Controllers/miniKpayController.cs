using KpayDotNet.Domain.Features;
using KpayDotNet.Domain.ResponseModels;
using KPayEfcore.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.miniKpay.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class miniKpayController : BaseResponseController
{ private readonly KpayServices _service = new KpayServices();

    #region Http Request for user
    [HttpPost("User/createWallet")]
    public async Task<IActionResult> CreateUserWallet(User_Tbl user) {

        var result = await _service.CreateWalletUserAsync(user);
        
        return Excute(result);
    }
    [HttpPost("User/firstTimeLogin/{number}")]
    public async Task<IActionResult> FirstTimeLogin(string number, string code, string newPin)
    {
        var result = await _service.FirstTimeLoginAsync(number, code, newPin);
        return Excute(result);
    }

    [HttpGet("User/GetOTP/{number}")]
    public async Task<IActionResult> GetOTP(string number)
    {
        var result = await _service.GetOtpAsync(number);
        return Excute(result);
    }

    [HttpGet("User/login/{number}")]
    public async Task<IActionResult> Login(string number, string code)
    {
        var result = await _service.LoginAsync(number, code);
        return Excute(result);
    }

    [HttpGet("User/balance/{number}")]
    public async Task<IActionResult> GetBalance(string number)
    {
        var result = await _service.GetBalanceAsync(number);
        return Excute(result);
    }

    [HttpGet("User/forgetPin/{number}")]
    public async Task<IActionResult> ForgetPin(string number)
    {
        var result = await _service.ForgetPinAsync(number);
        return Excute(result);
    }

    [HttpPatch("User/resetPin/{number}")]
    public async Task<IActionResult> ResetPin(string number, string resetCode, string newPin)
    {
        var result = await _service.ResetPinAsync(number, resetCode, newPin);
        return Excute(result);
    }

    [HttpPatch("User/changePin/{number}")]
    public async Task<IActionResult> ChangePin(string number, string oldPin, string newPin)
    {
        var result = await _service.ChangePinAsync(number, oldPin, newPin);
        return Excute(result);
    }

    [HttpPatch("User/changePhoneNumber/{number}")]
    public async Task<IActionResult> ChangePhoneNumber(string oldNumber, string newNumber, string pin)
    {
        var result = await _service.ChangePhoneNumberAsync(oldNumber, newNumber, pin);
        return Excute(result);
    }
    #endregion

    #region Http REquest for Transaction
    [HttpPost("balance/transfer/{senderNumber}/to/{receiverNumber}")]
    public async Task<IActionResult> TransferBalance(string senderNumber, string receiverNumber, double amount, string pin, string note)
    {
       

        var result = await _service.TransferBalanceAsync(senderNumber, receiverNumber, amount, pin, note);
        return Excute(result);
    }

    [HttpPost("balance/deposit/bank/from/{senderNumber}")]
    public async Task<IActionResult> DepositMoney(string senderNumber, double amount, string pin, string note)
    {
        var result = await _service.DepositMoneyAsync(senderNumber, amount, pin, note);
        return Excute(result);
    }

    [HttpPost("balance/withdraw/bank/to/{senderNumber}")]
    public async Task<IActionResult> WithdrawMoney(string senderNumber, double amount, string pin, string note)
    {
        var result = await _service.WithdrawMoneyAsync(senderNumber, amount, pin, note);
        return Excute(result);
    }

    [HttpGet("transaction-history/{number}")]
    public async Task<IActionResult> GetTransactionHistory(string number)
    {
        var result = await _service.GetTransactionHistoryAsync(number);
        return Excute(result);
    }

    [HttpGet("Receipt-records/{number}")]
    public async Task<IActionResult> GetReceiptRecords(string number)
    {
        var result = await _service.GetReceiptRecordsAsync(number);
        return Excute(result);
    }
    #endregion
}
