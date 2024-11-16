using KpayDotNet.Domain.Features;
using KPayEfcore.Database.Models;

namespace KpayDotNet.MinimalAPI.EndPoints;

public static class KpayEndPoints
{
    public static void MapKPayEndpoints(this IEndpointRouteBuilder app)
    {
        KpayServices _services = new KpayServices();

        app.MapPost("/api/create-wallet-user", (User_Tbl user) => {

            int result = _services.Create_User(user);
            var code = _services.Generate_First_TimeCode(user);

            return (result > 0 && code is not null) ? Results.Ok(code) : Results.BadRequest("Error User Entry");



        }).WithName("Create_Wallet_User").WithOpenApi();



        app.MapPost("/api/first-time-login/{number}", (string number, string code, string newPin) => {

            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");

            var checkCode = _services.CheckCode(user.UserId, code);
            if (!checkCode) return Results.NotFound("Login Code not found");

            int pinResult = _services.setupPin(user, newPin);
            if (pinResult == 0) return Results.BadRequest("Error Pin Set up");


            return Results.Ok("Pin Steup Complete");



        }).WithName("First_Time_Login").WithOpenApi();

        app.MapGet("/api/GetOTP/{number}", (string number) => {

            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");

            var code = _services.GetOtp(user, "Login");

            return Results.Ok($"OTP code :{code}");


        }).WithName("Get_OTP").WithOpenApi();


        app.MapGet("/api/login/{number}", (string number, string code) => {

            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");

            var checkCode = _services.CheckOTP(user.UserId, code, "Login");
            if (!checkCode) return Results.NotFound("OTP Code not found");




            return Results.Ok(user);



        }).WithName("Login").WithOpenApi();

        app.MapGet("/api/balance/get/{number}", (string number) => {

            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");






            return Results.Ok($" BAlance :{user.Balance}");



        }).WithName("Balance").WithOpenApi();

        app.MapGet("/api/pin/forget/{number}", (string number) => {
            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");


            var resetCode = _services.GetOtp(user, "Reset");

            return Results.Ok($"Pin Reset Code:{resetCode}");


        }).WithName("Forget Pin").WithOpenApi();

        app.MapPatch("/api/pin/reset/{number}", (string number, string resetcode, string newPin) => {
            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");

            var checkCode = _services.CheckOTP(user.UserId, resetcode, "Reset");
            if (!checkCode) return Results.NotFound("OTP Code not found");

            var pinResult = _services.Change_Pin(user, newPin);
            if (pinResult == 0) return Results.BadRequest(" Pin Changing Error");



            return Results.Ok($"Pin Change successful");


        }).WithName("reset Pin").WithOpenApi();

        app.MapPatch("/api/pin/change/{number}", (string number, string oldPin, string newPin) => {

            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");

            var checkpin = _services.CheckPin(user.UserId, oldPin);
            if (!checkpin) return Results.BadRequest("Wrong Pin Error");

            var pinResult = _services.Change_Pin(user, newPin);
            if (pinResult == 0) return Results.BadRequest(" Pin Changing Error");

            return Results.Ok("Pin Change Success");

        }).WithName("Change Pin").WithOpenApi();

        app.MapPatch("/api/Phone-number/change/{number}", (string oldNumber, string newNumber, string pin) => {

            var user = _services.GetUserbyPhnum(oldNumber);
            if (user is null) return Results.NotFound("Phone num not found");

            var checkpin = _services.CheckPin(user.UserId, pin);
            if (!checkpin) return Results.BadRequest("Wrong Pin Error");

            var Result = _services.changePhoneNumber(user.UserId, newNumber);
            if (Result == 0) return Results.BadRequest(" Pin Changing Error");

            return Results.Ok("Phone NUmber Change Success");

        }).WithName("Change Phone Number").WithOpenApi();

        app.MapPost("/api/balance/transfer/{senderNumber}/to/{receiverNumber}",
            (string senderNumber, string receiverNumber, double amount, string pin, string note) => {

                if (senderNumber == receiverNumber) Results.BadRequest("Same Phone NUmber");

                var sender = _services.GetUserbyPhnum(senderNumber);
                if (sender is null) return Results.NotFound("sender num not found");

                var receiver = _services.GetUserbyPhnum(receiverNumber);
                if (receiver is null) return Results.NotFound("receiver num not found");


                var checkpin = _services.CheckPin(sender.UserId, pin);
                if (!checkpin) return Results.BadRequest("Wrong Pin Error");

                var moreBalance = _services.MoreBalance(sender, amount + 10000);
                if (!moreBalance) return Results.BadRequest("Balance not Enough");

                var resultSender = _services.DeductBalance(sender, amount);

                var resultReceiver = _services.AddBalance(sender, amount);

                if (resultSender == 0 || resultReceiver == 0) return Results.BadRequest("Error Transaction");

                var transaction = _services.CreateTransaction(sender, receiver, amount, note, "Transfer");
                if (transaction is null) return Results.BadRequest("Error Creating Transaction history");

                return Results.Ok("Transaction Completed");

            })
            .WithName("Transfer").WithOpenApi();

        app.MapPost("/api/balance/Deposit/bank/from/{receiverNumber}",
            (string senderNumber, double amount, string pin, string note) => {

                //if (senderNumber == receiverNumber) Results.BadRequest("Same Phone NUmber");

                var sender = _services.GetUserbyPhnum(senderNumber);
                if (sender is null) return Results.NotFound("sender num not found");

                //var receiver = _services.GetUserbyPhnum(receiverNumber);
                //if (receiver is null) return Results.NotFound("receiver num not found");


                var checkpin = _services.CheckPin(sender.UserId, pin);
                if (!checkpin) return Results.BadRequest("Wrong Pin Error");

                var moreBalance = _services.MoreBalance(sender, amount + 10000);
                if (!moreBalance) return Results.BadRequest("Balance not Enough");

                var resultSender = _services.DeductBalance(sender, amount);

                // var resultReceiver = _services.AddBalance(sender, amount);

                if (resultSender == 0) return Results.BadRequest("Error Deposit");

                var transaction = _services.CreateTransaction(sender, amount, note, "Deposit");
                if (transaction is null) return Results.BadRequest("Error Creating Transaction history");

                return Results.Ok("Money withdraw successful");

            })
            .WithName("Deposit").WithOpenApi();

        app.MapPost("/api/balance/Withdraw/Bank/to/{senderNumber}",
            (string senderNumber, double amount, string pin, string note) => {

                //if (senderNumber == receiverNumber) Results.BadRequest("Same Phone NUmber");

                var sender = _services.GetUserbyPhnum(senderNumber);
                if (sender is null) return Results.NotFound("sender num not found");

                //var receiver = _services.GetUserbyPhnum(receiverNumber);
                //if (receiver is null) return Results.NotFound("receiver num not found");


                var checkpin = _services.CheckPin(sender.UserId, pin);
                if (!checkpin) return Results.BadRequest("Wrong Pin Error");

                //var moreBalance = _services.MoreBalance(sender, amount + 10000);
                //if (!moreBalance) return Results.BadRequest("Balance not Enough");

                var resultSender = _services.AddBalance(sender, amount);

                // var resultReceiver = _services.AddBalance(sender, amount);

                if (resultSender == 0) return Results.BadRequest("Error Withdraw");

                var transaction = _services.CreateTransaction(sender, amount, note, "Withdraw");
                if (transaction is null) return Results.BadRequest("Error Creating Transaction history");

                return Results.Ok("Withdraw Money from bank");

            })
            .WithName("Withdraw").WithOpenApi();

        app.MapGet("/api/Transaction-history/{number}", (string number) => {

            var user = _services.GetUserbyPhnum(number);
            if (user is null) return Results.NotFound("Phone num not found");

            var transactionList = _services.getTransactionHistory(user.UserId);
            if (transactionList is null) return Results.NoContent();

            return Results.Ok(transactionList);


        }).WithName("Transaction_History").WithOpenApi();



    }
}
