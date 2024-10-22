using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
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
        /// <param name="loginModel">Contains the email address and The plain password of the user</param>
        /// <response code="200">The new token</response>
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
        /// <param name="model">Contains the email of the new user and its confirmation</param>
        /// <response code="200">Success status</response>
        [HttpPost("CreateUserRequest")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult CreateUserRequest([FromBody] NewUserModel model)
        {
            bool isRequestCreated = false;
            
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var attemptsLimit = Convert.ToInt32(config[Secrets.AttemptsLimit]);
            
                isRequestCreated = securityService.CreateNewUserRequest(model, ipAddress, attemptsLimit);
                
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();
                
                if(model != null)
                {
                    exceptionData["email"] = model.Email;
                    exceptionData["ConfirmEmail"] = model.ConfirmEmail;
                };

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(isRequestCreated);
        }
        
        /// <summary>
        /// Will send email to the user to create a new password
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <response code="200">Success status</response>
        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult ResetPassword([FromBody] string email)
        {
            bool isVerified = false;
            
            try
            {
                isVerified = securityService.ResetPassword(email);
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>
                {
                    ["email"] = email
                };

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(isVerified);
        }        
        
        /// <summary>
        /// Will check if token is valid and will mark the user as 'verified'.
        /// </summary>
        /// <param name="model"> Contains password and the token that has been generated in CreateUserRequest method</param>
        /// <response code="200">Success status</response>
        [HttpPost("VerifyUser")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult VerifyUser([FromBody] VerifyUserModel model)
        {
            bool isVerified = false;
            
            try
            {
                isVerified = securityService.VerifyUser(model);
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();
                
                if(model != null)
                {
                    exceptionData["IsPasswordEmpty"] = string.IsNullOrWhiteSpace(model.Password);
                    exceptionData["IsConfirmPasswordEmpty"] = string.IsNullOrWhiteSpace(model.ConfirmPassword);
                    exceptionData["Token"] = model.Token;
                };

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(isVerified);
        }        
        
        /// <summary>
        /// Will check if token is valid and will mark the user as 'verified'.
        /// </summary>
        /// <param name="model">Contains the new password and its confirmation</param>
        /// <response code="200">Success status</response>
        [HttpPost("CompletePasswordReset")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult CompletePasswordReset([FromBody] NewPasswordModel model)
        {
            bool hasBeenReset = false;
            
            try
            {
                hasBeenReset = securityService.CompletePasswordReset(model);
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();
                
                if (model != null)
                {
                    exceptionData["IsPasswordEmpty"] = string.IsNullOrWhiteSpace(model.Password);
                    exceptionData["IsConfirmPasswordEmpty"] = string.IsNullOrWhiteSpace(model.ConfirmPassword);
                };

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(hasBeenReset);
        }
    }
}
