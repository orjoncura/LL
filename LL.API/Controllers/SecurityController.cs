using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using LL.API.Constants;
using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LL.API.Controllers
{    
    /// <response code="500">There is a bug in the system somewhere.</response>
    [ApiController]
    [AllowAnonymous] 
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
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
        [HttpPost("RegisterUser")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult RegisterUser([FromBody] NewUserModel model)
        {
            bool isRequestCreated = false;
            
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var url = config[AppSettings.BaseUrl] ?? string.Empty;

                if (string.IsNullOrWhiteSpace(url) == false)
                    isRequestCreated = securityService.RegisterUser(model, ipAddress, url);
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
        /// Will check if token is valid and will mark the user as 'verified'.
        /// </summary>
        /// <param name="model"> Contains password and the token that has been generated in CreateUserRequest method</param>
        /// <response code="200">Success status</response>
        [HttpPost("CompleteUserRegistration")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult CompleteUserRegistration([FromBody] ConfirmationModel model)
        {
            bool isCompleted = false;
            
            try
            {
                isCompleted = securityService.CompleteUserRegistration(model);
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

            return Ok(isCompleted);
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
            bool isRequestCreated = false;
            
            try
            {    
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var url = config[AppSettings.BaseUrl] ?? string.Empty;

                if (string.IsNullOrWhiteSpace(url) == false)
                    isRequestCreated = securityService.ResetPassword(email, ipAddress,  url);
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>
                {
                    ["email"] = email
                };

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(isRequestCreated);
        }        
        
        /// <summary>
        /// Will check if token is valid and reset the password for this user.
        /// </summary>
        /// <param name="model">Contains the new password and its confirmation</param>
        /// <response code="200">Success status</response>
        [HttpPost("CompletePasswordReset")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult CompletePasswordReset([FromBody] ConfirmationModel model)
        {
            bool isCompleted = false;
            
            try
            {
                isCompleted = securityService.CompletePasswordReset(model);
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

            return Ok(isCompleted);
        }
    }
}
