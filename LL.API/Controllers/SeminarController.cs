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
    public class SeminarController(ISeminarService seminarService, IAppMonitoringService appMonitoringService) : Controller
    {
        /// <summary>
        /// Pass a list of words and get a seminar in the selected language
        /// </summary>
        /// <param name="seminarRequest">Contains text, LanguageIdFrom (Input language) and LangaugeIdTo (the laguage the words will be translated to)</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(IEnumerable<SeminarViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Create([FromBody] SeminarRequestModel seminarRequest)
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

                appMonitoringService.ExportError(ex, exceptionData);
            }

            return Ok(seminarViewModels);
        }
    }
}
