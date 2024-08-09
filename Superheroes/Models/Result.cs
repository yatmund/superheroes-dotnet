using System;

namespace Superheroes.Models;

public sealed class Result<T> where T : class, new()
{
    private Result(bool isSuccess, T payload, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        Payload = payload;
    }

    public T Payload { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result<T> Success(T payload) => new(true, payload, Error.None);

    public static Result<T> Failure(Error error) => new(false, default, error);

    public static Result<T> Empty() => new(false, default, null);
}
