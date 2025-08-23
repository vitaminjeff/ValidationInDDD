using System.Net;
using CSharpFunctionalExtensions;
using DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace Api;

[ApiController]
public class ApplicationController : ControllerBase
{
    protected new IActionResult Ok(object result = null)
    {
        return new EnvelopeResult(Envelope.Ok(result), HttpStatusCode.OK);
    }

    protected IActionResult NotFound(Error error, string invalidField = null)
    {
        return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.NotFound);
    }

    protected IActionResult Error(Error error, string invalidField = null)
    {
        return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.BadRequest);
    }

    protected IActionResult FromResult<T>(Result<T, Error> result)
    {
        if (result.IsSuccess)
            return Ok();

        return Error(result.Error);
    }
}