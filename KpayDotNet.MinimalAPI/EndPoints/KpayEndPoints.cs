using KpayDotNet.Domain.Features;
using KPayEfcore.Database.Models;

namespace KpayDotNet.MinimalAPI.EndPoints;

public static class KpayEndPoints
{
    public static void MapKPayEndpoints(this IEndpointRouteBuilder app)
    {
        var kpayService = new KpayService();

        // Deposit Endpoint
        app.MapPost("/api/deposit/{number}", (string number, double amount, string pin, string note) =>
        {
            var result = kpayService.Deposit(number, amount, pin, note);
            return Results.Ok( result );
        })
        .WithName("Deposit")
        .WithOpenApi();

        // Withdraw Endpoint
        app.MapPost("/api/withdraw/{number}", (string number, double amount, string pin, string note) =>
        {
            var result = kpayService.Withdraw(number, amount, pin, note);
            return Results.Ok( result );
        })
        .WithName("Withdraw")
        .WithOpenApi();

        // Transfer Endpoint
        app.MapPost("/api/transfer/{fromMobileNo}-to-{toMobileNo}", (string fromMobileNo, string toMobileNo, double amount, string pin, string note) =>
        {
            var result = kpayService.Transfer(fromMobileNo, toMobileNo, amount, pin, note);
            return Results.Ok(  result );
        })
        .WithName("Transfer")
        .WithOpenApi();


        // Transaction History Endpoint
        app.MapGet("/api/transactionHistory/{mobileNo}", (string mobileNo) =>
        {
            var transactions = kpayService.GetTransactionHistory(mobileNo);
            return transactions is not null ? Results.Ok(transactions) : Results.NotFound("User not found or no transactions.");
        })
        .WithName("TransactionHistory")
        .WithOpenApi();
        app.MapGet("/api/ReceiptRecord/{mobileNo}", (string mobileNo) =>
        {
            var ReceiptREcord = kpayService.GetReceiptHistory(mobileNo);
            return ReceiptREcord is not null ? Results.Ok(ReceiptREcord) : Results.NotFound("User not found or no transactions.");
        })
        .WithName("ReceiptREcord")
        .WithOpenApi();

        // Balance Check Endpoint
        app.MapGet("/api/balance/{mobileNo}", (string mobileNo) =>
        {
            var balance = kpayService.GetBalance(mobileNo);
            return Results.Ok(new { Balance = balance });
        })
        .WithName("GetBalance")
        .WithOpenApi();

        // Create Wallet User Endpoint
        app.MapPost("/api/create-wallet-user", (User user) =>
        {
            var result = kpayService.CreateWalletUser(user);
            return Results.Ok($"First Time Login Code:{result}");
        })
        .WithName("CreateWalletUser")
        .WithOpenApi();

        // First Time Login Endpoint
        app.MapGet("/api/first-time-login/PhoneNum={mobileNo}-Code={code}", (string mobileNo, string code,string newpin) =>
        {
            var result = kpayService.FirstTimeLogin(mobileNo, code,newpin);
            return Results.Ok($"OTP code :{result}");
        })
        .WithName("FirstTimeLogin")
        .WithOpenApi();

        // Login via OTP Endpoint
        app.MapGet("/api/login/{mobileNo}", (string mobileNo, string otp) =>
        {
            var user = kpayService.Login(mobileNo, otp);

            return user is null ? Results.NotFound("Wrong Mobile Number Error") : Results.Ok(user);
           // return user;
        })
        .WithName("Login")
        .WithOpenApi();

        app.MapGet("/api/otp/{mobileNo}", (string mobileNo) =>
        {
            var result = kpayService.GetOtp(mobileNo);

            return Results.Ok(result);
            // return user;
        })
       .WithName("GetOTP")
       .WithOpenApi();

        // Change Pin Endpoint
        app.MapPatch("/api/change-pin", (string mobileNo, string oldPin, string newPin) =>
        {
            var result = kpayService.ChangePin(mobileNo, oldPin, newPin);
            return Results.Ok(result);
        })
        .WithName("ChangePin")
        .WithOpenApi();

        // Change Phone Number Endpoint
        app.MapPatch("/api/change-phone-number", (string oldMobileNo, string newMobileNo,string pin) =>
        {
            var result = kpayService.ChangePhoneNumber(oldMobileNo, newMobileNo,pin);
            return Results.Ok(new { Message = result });
        })
        .WithName("ChangePhoneNumber")
        .WithOpenApi();

        // Forget Pin Endpoint
        app.MapGet("/api/forget-pin/{mobileNo}", (string mobileNo) =>
        {
            var result = kpayService.ForgetPin(mobileNo);
            return Results.Ok($"Reset Code:{result}");
        })
        .WithName("ForgetPin")
        .WithOpenApi();

        // Reset Pin Endpoint
        app.MapPatch("/api/reset-pin/{mobileNo}", (string mobileNo, string resetCode, string newPin) =>
        {
            var result = kpayService.ResetPin(mobileNo, resetCode, newPin);
            return Results.Ok(new { Message = result });
        })
        .WithName("ResetPin")
        .WithOpenApi();
    }
}
