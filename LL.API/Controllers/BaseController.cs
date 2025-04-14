using System.Security.Claims;
using LL.API.Constants;
using Microsoft.AspNetCore.Mvc;

namespace LL.API.Controllers;

public class BaseController : Controller
{
    protected int UserId
    {
        get
        {
            string userIdString = User.FindFirstValue("UserId");
            
            // Handle case where UserId is not found; perhaps throw an exception or return a default value.
            if (string.IsNullOrEmpty(userIdString))
                throw new InvalidOperationException("UserId claim not found.");
            
            if (int.TryParse(userIdString, out int userId))
                return userId;
            
            throw new InvalidCastException($"The UserId '{userIdString}' cannot be converted to an integer.");
        }
    }
    
    protected ActionResult ErrorStatusCode => StatusCode(StatusCodes.Status500InternalServerError, new { message = AppSettings.Status500InternalServerError });
}