using LL.Core.Interfaces;
using LL.SharedDefinitions.Model;
using LL.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LL.API.Controllers
{
    /// <response code="401">Either your security token is invalid or your permissions do not allow the requested action.</response>
    /// <response code="500">There is a bug in the system somewhere.</response>
    [ApiController]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SeminarController(IChatService chatService) : Controller
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
            SeminarViewModel seminarViewModel = new SeminarViewModel();

            try
            {
                seminarViewModel = await chatService.CreateSeminar(seminarRequest);
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                if (seminarRequest != null)
                {
                    exceptionData["Words"] = seminarRequest.Words;
                    exceptionData["LanguageIdFrom"] = seminarRequest.LanguageIdFrom;
                    exceptionData["LangaugeIdTo"] = seminarRequest.LangaugeIdTo;
                }

                Error.Export(ex, exceptionData);
            }

            return Ok(seminarViewModel);
        }
    }
}
