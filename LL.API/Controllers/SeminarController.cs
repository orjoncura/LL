using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Services;
using LL.Core.Models.Arguments;
using LL.Core.Models.Short;
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
            return Ok(new List<SeminarViewModel>()
            {
                new SeminarViewModel(
                    "Vamos",
                    new List<StatementShort>()
                    {
                        new StatementShort()
                        {
                            OriginalStatement = "¿Vamos por un café",
                            TranslatedStatement = "Shall we go for a coffee?"
                        }
                    },
                    2),
                
                new SeminarViewModel(
                    "Gusta",
                    new List<StatementShort>()
                    {
                        new StatementShort()
                        {
                            OriginalStatement = "¿Que te gusta hacer en tu tiempo libre?",
                            TranslatedStatement = "What do you like to do in your free time?"
                        }
                    },
                    1),
                
                new SeminarViewModel(
                    "Que",
                    new List<StatementShort>()
                    {
                        new StatementShort()
                        {
                            OriginalStatement = "¿Que me recomiendas hacer?",
                            TranslatedStatement = "What do you recommend I do?"
                        }
                    },
                    3),
                
            });
            
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
