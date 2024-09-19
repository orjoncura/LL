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
    public class SeminarController(ISeminarService seminarService, IAppMonitoring appMonitoring) : Controller
    {
        /// <summary>
        /// Pass a list of words and get a seminat in the selcted language
        /// </summary>
        /// <param name="Words">List of selected words.</param>
        /// <param name="LanguageIdFrom">Input language.</param>
        /// <param name="LangaugeIdTo">the laguage the words will be translated to.</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("CreateSeminar")]
        [ProducesResponseType(typeof(IEnumerable<SeminarViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateSeminar([FromBody] SeminarRequestModel seminarRequest)
        {
            List<SeminarViewModel> seminarViewModels = new List<SeminarViewModel>();

            try
            {
                seminarViewModels = await seminarService.CreateSeminar(seminarRequest, 1);
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                if (seminarRequest != null)
                {
                    exceptionData["Text"] = seminarRequest.Text;
                    exceptionData["LanguageIdFrom"] = seminarRequest.LanguageFromId;
                    exceptionData["LanguageIdTo"] = seminarRequest.LanguageToId;
                }

                appMonitoring.ExportError(ex, exceptionData);
            }

            return Ok(seminarViewModels);
        }
    }
}
