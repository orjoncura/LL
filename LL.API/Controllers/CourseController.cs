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
    public class CourseController(ICourseService seminarService, 
        IWordRepository wordRepository,
        IAppMonitoringService appMonitoringService) : Controller
    {
        /// <summary>
        /// Pass a languageId to get a list of keywords 
        /// </summary>
        /// <param name="languageId">LanguageIdFrom (Input language)</param>
        /// <param name="importance">The importance level of the words we are looking for/param>
        /// <response code="200">A list of keywords for the selected language</response>
        [HttpGet("GetKeyWords")]
        [ProducesResponseType(typeof(List<WordViewModel>), StatusCodes.Status200OK)]
        public ActionResult GetKeyWords([FromQuery] int languageId)
        {
            try
            {
                return Ok(wordRepository.GetKeyWords(languageId));
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();
                
                exceptionData["languageId"] = languageId;

                appMonitoringService.ExportError(ex, exceptionData);
                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppSettings.Status500InternalServerError });
            }
        }
        
        /// <summary>
        /// Pass a list of words and get a seminar in the selected language
        /// </summary>
        /// <param name="courseRequest">Contains text, LanguageIdFrom (Input language) and languageIdTo (the language the words will be translated to)</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(CourseViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult> Create([FromBody] CourseRequestModel courseRequest)
        {
            try
            {
                return Ok(await seminarService.CreateCourse(courseRequest, 1));
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                if (courseRequest != null)
                {
                    exceptionData["Text"] = courseRequest.Text;
                    exceptionData["LanguageIdFrom"] = courseRequest.LanguageFromId;
                    exceptionData["LanguageIdTo"] = courseRequest.LanguageToId;
                }

                appMonitoringService.ExportError(ex, exceptionData);
                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppSettings.Status500InternalServerError });
            }
        }
        
        /// <summary>
        /// Pass a word amd get its details: meanings, definitions etc
        /// </summary>
        /// <param name="definitionRequestModel">Contains text, its translation, LanguageIdFrom (Input language) and languageIdTo (the language the words will be translated to)</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("CreateDefinitions")]
        [ProducesResponseType(typeof(WordViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateDefinitions([FromBody] DefinitionRequestModel definitionRequestModel)
        {
            try
            {
                return Ok(await seminarService.CreateDefinitions(definitionRequestModel, 1));
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                if (definitionRequestModel != null)
                {
                    exceptionData["Text"] = definitionRequestModel.Text;
                    exceptionData["LanguageIdFrom"] = definitionRequestModel.LanguageFromId;
                    exceptionData["LanguageIdTo"] = definitionRequestModel.LanguageToId;
                }
                
                appMonitoringService.ExportError(ex, exceptionData);
                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppSettings.Status500InternalServerError });
            }
        }
        
        /// <summary>
        /// Pass a word and the course information to create exercises for the selected word.
        /// </summary>
        /// <param name="exerciseRequest">Contains Course & Word information</param>
        /// <response code="200">The new exercises</response>
        [HttpPost("CreateExercises")]
        [ProducesResponseType(typeof(List<ExerciseViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateExercises([FromBody] ExerciseRequestModel exerciseRequest)
        {
            try
            {
                return Ok(await seminarService.CreateExercises(exerciseRequest, 1));
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                if (exerciseRequest != null && exerciseRequest.IsValid)
                {
                    exceptionData["CourseId"] = exerciseRequest.CourseId;
                    exceptionData["WordId"] = exerciseRequest.WordId;
                }

                appMonitoringService.ExportError(ex, exceptionData);
                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppSettings.Status500InternalServerError });
            }
        }
        
        /// <summary>
        /// Pass a wordId to get an audio for the selected word.
        /// </summary>
        /// <param name="wordId">The ID of the selected word</param>
        /// <response code="200">A FileStreamResult that represents an audio file</response>
        [HttpPost("StreamAudio")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public ActionResult StreamAudio(int wordId)
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
