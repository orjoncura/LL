using LL.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
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
    public class SecurityController(ISecurityService securityService, IAppMonitoring appMonitoring) : Controller
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
                    exceptionData["Username"] = loginModel.Username;
                }

                appMonitoring.ExportError(ex, exceptionData);
            }

            return Ok(tokenViewModel);
        }
    }
}
