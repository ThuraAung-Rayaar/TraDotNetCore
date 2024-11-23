using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRADotNetCore.POS.Database.Models;

namespace TRADotNetCore.POS.Domain.Models.Product;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsError { get {return !IsSuccess; } }
    public bool IsValidationError { get { return ResponseType == EnumResponse.ValidationError; } }
    public bool IsNotFound { get { return ResponseType == EnumResponse.NotFound; } }
    public bool IsSystemError { get { return ResponseType == EnumResponse.SystemError; } }
    public bool IsInternalServerError { get { return ResponseType == EnumResponse.internalServerError; } }

    public string Message { get;set; }
    public T? Data { get; set; }

    private EnumResponse ResponseType { get; set; }


    public static Result<T> Success(T item, string message = "Success.")
    {

        return new Result<T>
        {
            IsSuccess = true,
           Message = message,
            ResponseType = EnumResponse.Success,
           Data = item


        };

    }


    public static Result<T> SystemError(string message, T? data= default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.SystemError,
            Data = data
        };
    }

    public static Result<T> ValidationError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.ValidationError,
            Data = data
        };
    }

    public static Result<T> InternalServerError(string message, T? data = default    )
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.internalServerError,
            Data = data
        };
    }

    public static Result<T> NotFoundError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.NotFound,
            Data = data
        };

    }
}


public enum EnumResponse
{

    None,
    Success,
    NotFound,
    SystemError,
    ValidationError,
    internalServerError,





}
