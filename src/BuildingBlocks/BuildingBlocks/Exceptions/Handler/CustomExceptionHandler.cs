using MediatR.Pipeline;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler>logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message:{ExceptionMessage}, time of occurence {time}", exception.Message, DateTime.UtcNow);

            (string Detail, string Title, int StatusCode) detail = exception switch
            {
                InternalServerException =>
                (exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError),
                ValidationException => (exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest),

                BadRequestException => (exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status400BadRequest),

                NotFoundException => (exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode = StatusCodes.Status404NotFound),

                _ => (exception.Message,
                  exception.GetType().Name,
                  context.Response.StatusCode = StatusCodes.Status500InternalServerError),
            };

            var problemDetails = new ProblemDetails
            {
                Detail = detail.Detail,
                Instance = context.Request.Path,
                Status = detail.StatusCode,
                Title = detail.Title,
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);
            if(exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("validationErrors", validationException.Value);
            }
            await context.Response.WriteAsJsonAsync(problemDetails);
            return true;
        }
    }
}
