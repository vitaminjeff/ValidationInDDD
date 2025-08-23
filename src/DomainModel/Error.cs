using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace DomainModel;

public sealed class Error : ValueObject
{
    private const string Separator = "||";
        
    public string Code { get; }
    public string Message { get; }

    internal Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }

    public string Serialize()
    {
        return $"{Code}{Separator}{Message}";
    }

    public static Error Deserialize(string serialized)
    {
        if (serialized == "A non-empty request body is required.")
            return Errors.General.ValueIsRequired();
            
        string[] data = serialized.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

        if (data.Length < 2)
            throw new Exception($"Invalid error serialization: '{serialized}'");

        return new Error(data[0], data[1]);
    }
}