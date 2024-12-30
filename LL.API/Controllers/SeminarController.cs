using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using LL.API.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
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
    public class SeminarController(ISeminarService seminarService, 
        IWordRepository wordRepository,
        IAppMonitoringService appMonitoringService) : Controller
    {
        /// <summary>
        /// Pass a list of words and get a seminar in the selected language
        /// </summary>
        /// <param name="seminarRequest">Contains text, LanguageIdFrom (Input language) and LangaugeIdTo (the laguage the words will be translated to)</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SeminarViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult> Create([FromBody] SeminarRequestModel seminarRequest)
        {
            try
            {
                return Ok(await seminarService.CreateSeminar(seminarRequest, 1));
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
                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppSettings.Status500InternalServerError });
            }
        }
        
        [HttpPost("StreamAudio")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> StreamAudio(int wordId)
        {
            try
            {
                return new FileStreamResult(wordRepository.GetFileStreamById(wordId), "audio/wav");
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                exceptionData["wordId"] = wordId;

                appMonitoringService.ExportError(ex, exceptionData);
                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppSettings.Status500InternalServerError });
            }
        }
    }
}
