using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.DTOs;
using System.Net;

namespace OrderManagementAPI.Controllers.Extensions
{
    public static class ApiResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this ApiResponse<T> response)
        {
            return response.Status switch
            {
                HttpStatusCode.OK => new OkObjectResult(response),
                HttpStatusCode.Created => new CreatedResult(string.Empty, response),
                HttpStatusCode.NoContent => new NoContentResult(),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
                HttpStatusCode.NotFound => new NotFoundObjectResult(response),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
                HttpStatusCode.Forbidden => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Forbidden },
                HttpStatusCode.Conflict => new ConflictObjectResult(response),
                HttpStatusCode.InternalServerError => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError },
                _ => new ObjectResult(response) { StatusCode = (int)response.Status }
            };
        }
    }
}
