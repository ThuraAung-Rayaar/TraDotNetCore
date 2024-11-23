using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPayEfcore.Database.Models;

public class BaseResponseModel
{
    public string responseCode { get; set; }
    public string responseDscript { get; set; }
    public EnumResponse responseType { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsError { get { return !IsSuccess; } }

    public static BaseResponseModel Success(string respCode, string responseDscript)
    {

        return new BaseResponseModel
        {
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
            responseType = EnumResponse.InternalServerError


        };

    }
    public static BaseResponseModel NotFoundError(string respCode, string responseDscript)
    {

        return new BaseResponseModel
        {
            IsSuccess = false,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.NotFound


        };

    }
    public static BaseResponseModel InsufficientBalanceError(string respCode, string responseDscript)
    {
        return new BaseResponseModel
        {
            IsSuccess = false,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.InsufficientBalance
        };
    }

    public static BaseResponseModel DuplicateEntryError(string respCode, string responseDscript)
    {
        return new BaseResponseModel
        {
            IsSuccess = false,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.DuplicateEntry
        };
    }

    public static BaseResponseModel ExpiredCodeError(string respCode, string responseDscript)
    {
        return new BaseResponseModel
        {
            IsSuccess = false,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.ExpiredCode
        };
    }

    public static BaseResponseModel IncorrectPinError(string respCode, string responseDscript)
    {
        return new BaseResponseModel
        {
            IsSuccess = false,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.IncorrectPin
        };
    }

    public static BaseResponseModel IncorrectPhoneNumberError(string respCode, string responseDscript)
    {
        return new BaseResponseModel
        {
            IsSuccess = false,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.IncorrectPhoneNumber
        };
    }
    public static BaseResponseModel InputError(string respCode, string responseDscript)
    {
        return new BaseResponseModel
        {
            IsSuccess = false,
            responseCode = respCode,
            responseDscript = responseDscript,
            responseType = EnumResponse.InputError
        };
    }

}

public enum EnumResponse
{
    None = 0,
    Success = 200,
    NotFound = 404,
    SystemError = 500,
    ValidationError = 400,
    InternalServerError = 500,
    InsufficientBalance = 409,
    DuplicateEntry = 409,
    ExpiredCode = 401,
    IncorrectPin = 401,
    IncorrectPhoneNumber = 400,
    InputError = 400
}
