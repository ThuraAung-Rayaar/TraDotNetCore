using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;
using TRADotNetCore.POS.Database.Models;

namespace TRADotNetCore.POS.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseResponseController : ControllerBase
    {
        public IActionResult Excute(object item) { 
        
            JObject jobj = JObject.Parse(JsonConvert.SerializeObject(item));

            if (jobj.ContainsKey("Response")) { 
                BaseResponseModel B_response = new BaseResponseModel();
                B_response = JsonConvert.DeserializeObject<BaseResponseModel>(jobj["Response"]!.ToString())!;


               
                int code=0;

                if (B_response.responseType == EnumResponse.Success) { return Ok(item); }

                else if (B_response.responseType == EnumResponse.SystemError)
                {
                    code = 502;
                }
                else if (B_response.responseType == EnumResponse.NotFound)
                {
                    code = 404;
                }
                else if (B_response.responseType == EnumResponse.ValidationError)
                {
                    code = 400; 
                }
                else if (B_response.responseType == EnumResponse.internalServerError)
                {
                    code = 503;
                }


               
                return StatusCode(code,item);


               
            }

            return StatusCode(501, "Invalid Response Model , Need to implement Response MOdel");
        }

    }
}
