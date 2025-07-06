using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using LL.API.Constants;
using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Model.DataTransferObjects;
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
    public class SecurityController(
        IConfiguration config,
        ISecurityService securityService, 
        IUserRepository userRepository, 
        IAppMonitoringService appMonitoringService,
        TokenConfigModel tokenConfigModel) : BaseController
    {
        /// <summary>
        /// Pass username and password and get a security token.
        /// </summary>
        /// <param name="loginModel">Contains the email address and The plain password of the user</param>
        /// <response code="200">The new token</response>
        [HttpPost("Authenticate")]
        [ProducesResponseType(typeof(TokenViewModel), StatusCodes.Status200OK)]
        public ActionResult Authenticate([FromBody] LoginModel loginModel)
        {
            try
            {
                return Ok(userRepository.GetAuthenticationToken(loginModel, tokenConfigModel));
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                if (loginModel != null)
                {
                    exceptionData["Username"] = loginModel.Email;
                }

                appMonitoringService.ExportError(ex, exceptionData);
                
                return ErrorStatusCode;
            }
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
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var url = config[Secrets.BaseUrl] ?? string.Empty;

                return Ok(securityService.RegisterUser(model, ipAddress, url));
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
                
                return ErrorStatusCode;
            }
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
            try
            {
                return Ok(securityService.CompleteUserRegistration(model));
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
                
                return ErrorStatusCode;
            }
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
            try
            {    
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var url = config[Secrets.BaseUrl] ?? string.Empty;

                return Ok(securityService.ResetPassword(email, ipAddress, url));
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>
                {
                    ["email"] = email
                };

                appMonitoringService.ExportError(ex, exceptionData);
                
                return ErrorStatusCode;
            }
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
            try
            {
                return Ok(securityService.CompletePasswordReset(model));
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
                
                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Will delete the token of the Authorization header.
        /// </summary>
        /// <response code="200">Success message</response>
        [HttpPost("Logout")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult Logout()
        {
            Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(true);
        }
    }
}
