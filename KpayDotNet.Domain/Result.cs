using KPayEfcore.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpayDotNet.Domain;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsError => !IsSuccess;
    public bool IsValidationError => ResponseType == EnumResponse.ValidationError;
    public bool IsNotFound => ResponseType == EnumResponse.NotFound;
    public bool IsSystemError => ResponseType == EnumResponse.SystemError;
    public bool IsInternalServerError => ResponseType == EnumResponse.InternalServerError;
    public bool IsInputError => ResponseType == EnumResponse.InputError;
    public bool IsDuplicateEntry => ResponseType == EnumResponse.DuplicateEntry;
    public bool IsInsufficientBalance => ResponseType == EnumResponse.InsufficientBalance;
    public bool IsExpiredCode => ResponseType == EnumResponse.ExpiredCode;
    public bool IsIncorrectPin => ResponseType == EnumResponse.IncorrectPin;
    public bool IsIncorrectPhoneNumber => ResponseType == EnumResponse.IncorrectPhoneNumber;

    public string Message { get; set; }
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

    public static Result<T> SystemError(string message, T? data = default)
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

    public static Result<T> InternalServerError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.InternalServerError,
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

    public static Result<T> InputError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.InputError,
            Data = data
        };
    }

    public static Result<T> DuplicateEntryError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.DuplicateEntry,
            Data = data
        };
    }

    public static Result<T> InsufficientBalanceError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.InsufficientBalance,
            Data = data
        };
    }

    public static Result<T> ExpiredCodeError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.ExpiredCode,
            Data = data
        };
    }

    public static Result<T> IncorrectPinError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.IncorrectPin,
            Data = data
        };
    }

    public static Result<T> IncorrectPhoneNumberError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ResponseType = EnumResponse.IncorrectPhoneNumber,
            Data = data
        };
    }
}

