using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRADotNetCore.POS.Database.Models
{
    public class BaseResponseModel
    {

        public string responseCode { get; set; }
        public string responseDscript { get; set; }
        public EnumResponse responseType { get; set; }

        public bool IsSuccess { get; set; }
        public bool IsError { get { return !IsSuccess; } }

        public static BaseResponseModel Success(string respCode, string responseDscript) { 
        
        return new BaseResponseModel {
            IsSuccess = true,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.Success
            
            
        };
        
        }


        public static BaseResponseModel SystemError(string respCode, string responseDscript)
        {

            return new BaseResponseModel
            {
                IsSuccess = false,
                responseCode = respCode,
                responseDscript = responseDscript,
                responseType = EnumResponse.SystemError


            };

        }

        public static BaseResponseModel ValidationError(string respCode, string responseDscript)
        {

            return new BaseResponseModel
            {
                IsSuccess = false,
                responseCode = respCode,
                responseDscript = responseDscript,
                responseType = EnumResponse.ValidationError


            };

        }

        public static BaseResponseModel InternalServerError(string respCode, string responseDscript)
        {

            return new BaseResponseModel
            {
                IsSuccess = false,
                responseCode = respCode,
                responseDscript = responseDscript,
                responseType = EnumResponse.internalServerError


            };

        }

    }


    public enum EnumResponse
    { 
    
        None,
        Success,
        SystemError,
        ValidationError,
        internalServerError,
        
        
        

    
    }
}
