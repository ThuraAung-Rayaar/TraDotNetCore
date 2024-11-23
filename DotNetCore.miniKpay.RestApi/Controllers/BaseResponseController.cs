using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KPayEfcore.Database.Models;

namespace DotNetCore.miniKpay.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseResponseController : ControllerBase
    {

        protected IActionResult Excute(object item)
        {

            JObject jobj = JObject.Parse(JsonConvert.SerializeObject(item));

            if (jobj.ContainsKey("Response"))
            {
                BaseResponseModel B_response = new BaseResponseModel();
                B_response = JsonConvert.DeserializeObject<BaseResponseModel>(jobj["Response"]!.ToString())!;



                int code = 0;

                if (B_response.responseType == EnumResponse.Success) { return Ok(item); }

                else if (B_response.responseType == EnumResponse.SystemError)
                {
                    code = (int) EnumResponse.SystemError;
                }
                else if (B_response.responseType == EnumResponse.NotFound)
                {
                    code = (int)EnumResponse.NotFound;
                }
                else if (B_response.responseType == EnumResponse.ValidationError)
                {
                    code = (int)EnumResponse.ValidationError;
                }
                else if (B_response.responseType == EnumResponse.InternalServerError)
                {
                    code = (int)EnumResponse.InternalServerError;
                }
                else if (B_response.responseType == EnumResponse.InsufficientBalance)
                {
                    code = (int)EnumResponse.InsufficientBalance;
                }
                else if (B_response.responseType == EnumResponse.DuplicateEntry)
                {
                    code = (int)EnumResponse.DuplicateEntry;
                }
                else if (B_response.responseType == EnumResponse.ExpiredCode)
                {
                    code = (int)EnumResponse.ExpiredCode;
                }
                else if (B_response.responseType == EnumResponse.IncorrectPin)
                {
                    code = (int)EnumResponse.IncorrectPin;
                }
                else if (B_response.responseType == EnumResponse.IncorrectPhoneNumber)
                {
                    code = (int)EnumResponse.IncorrectPhoneNumber;
                }

                return StatusCode(code, item);



            }

            return StatusCode(501, "Invalid Response Model , Need to implement Response MOdel");
        }

    }
}
