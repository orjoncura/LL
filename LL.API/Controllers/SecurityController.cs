using LL.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.RegularExpressions;
using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;

namespace LL.API.Controllers
{    
    /// <response code="401">Either your security token is invalid or your permissions do not allow the requested action.</response>
    /// <response code="500">There is a bug in the system somewhere.</response>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SecurityController(IConfiguration config, ISecurityService securityService, IAppMonitoringService appMonitoringService) : Controller
    {
        /// <summary>
        /// Pass username and password and get a security token.
        /// </summary>
        /// <param name="Username">The email address of the user</param>
        /// <param name="Password">The plain password of the user</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("Authenticate")]
        [ProducesResponseType(typeof(IEnumerable<TokenViewModel>), StatusCodes.Status200OK)]
        public ActionResult Authenticate([FromBody] LoginModel loginModel)
        {
            TokenViewModel? tokenViewModel = new TokenViewModel();

            try
            {
                tokenViewModel = securityService.Authenticate(loginModel);
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                if (loginModel != null)
                {
                    exceptionData["Username"] = loginModel.Email;
                }

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(tokenViewModel);
        }

        /// <summary>
        /// Will create a user and a message.
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("CreateUserRequest")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult CreateUserRequest(string email)
        {
            bool isRequestCreated = false;
            
            try
            {
                //Check if the email is in a valid format.
                if (Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                    var attemptsLimit = Convert.ToInt32(config[Secrets.AttemptsLimit]);
                
                    isRequestCreated = securityService.CreateNewUserRequest(email, ipAddress, attemptsLimit);
                }
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                exceptionData["email"] = email;

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(isRequestCreated);
        }
    }
}
