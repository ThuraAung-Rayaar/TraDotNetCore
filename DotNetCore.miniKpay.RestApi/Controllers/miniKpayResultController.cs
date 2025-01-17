using KpayDotNet.Domain.Features;
using KPayEfcore.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TRADotNetCore.miniKpay.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class miniKpayResultController : ControllerBase
    {
        private readonly KpayServices _service = new KpayServices();

       
        //[HttpPost("User/result/createWallet")]
        //public async Task<IActionResult> CreateUserWallet(User_Tbl user)
        //{

        //    var result = await _service.CreateWalletUserAsync(user);

        //    return Excute(result);
        //}

    }
}
