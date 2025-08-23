using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
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
    public class CourseController(ICourseService courseService, 
        ICourseRepository courseRepository,
        IModuleRepository  moduleRepository,
        IWordRepository wordRepository,
        IExerciseRepository exerciseRepository,
        IAppMonitoringService appMonitoringService) : BaseController
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
                
                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Pass a list of words and get a seminar in the selected language
        /// </summary>
        /// <param name="courseRequest">Contains text, LanguageIdFrom (Input language) and languageIdTo (the language the words will be translated to)</param>
        /// <response code="200">The new seminar</response>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult> Create([FromBody] CourseRequestModel courseRequest)
        {
            try
            {
                return Ok(await courseService.CreateCourse(courseRequest, UserId));
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
                
                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Pass the Id of a course to get exercises for the selected course.
        /// </summary>
        /// <param name="moduleId">Id of the selected module</param>
        /// <response code="200">The new exercises</response>
        [HttpGet("GetExercisesByModuleId")]
        [ProducesResponseType(typeof(List<ExerciseViewModel>), StatusCodes.Status200OK)]
        public ActionResult GetExercisesByModuleId([FromQuery] string moduleId)
        {
            try
            {
                return Ok(exerciseRepository.GetByModuleId(moduleId));
            }
            catch (Exception ex)
            {
                var exceptionData = new Dictionary<string, object>();

                exceptionData["moduleId"] = moduleId;

                appMonitoringService.ExportError(ex, exceptionData);
                
                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Get a list of courses by the userId found in the claims of the user.
        /// </summary>
        /// <response code="200">The list of courses</response>
        [HttpGet("GetCourses")]
        [ProducesResponseType(typeof(List<CourseViewModel>), StatusCodes.Status200OK)]
        public ActionResult GetCourses()
        {
            try
            {       
                return Ok(courseRepository.GetCoursesByUserId(UserId));
            }
            catch (Exception ex)
            {
                appMonitoringService.ExportError(ex);

                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Get all the modules of a course by course id.
        /// </summary>
        /// <response code="200">The list of courses</response>
        /// <param name="courseId">Id of the selected course</param>
        [HttpGet("GetModulesByCourseId")]
        [ProducesResponseType(typeof(List<ModuleViewModel>), StatusCodes.Status200OK)]
        public ActionResult GetModulesByCourseId(string courseId)
        {
            try
            {       
                return Ok(moduleRepository.GetByCourseId(courseId));
            }
            catch (Exception ex)
            {
                appMonitoringService.ExportError(ex);

                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Mark module as complete and activate next module.
        /// </summary>
        /// <param name="moduleId">Id of the selected module</param>
        /// <response code="200">The list of courses</response>
        [HttpGet("MarkModuleAsComplete")]
        [ProducesResponseType(typeof(List<ModuleViewModel>), StatusCodes.Status200OK)]
        public ActionResult MarkModuleAsComplete(string moduleId)
        {
            try
            {       
                return Ok(moduleRepository.MarkModuleAsComplete(moduleId));
            }
            catch (Exception ex)
            {
                appMonitoringService.ExportError(ex);

                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Get a list of words of a course.
        /// </summary>
        /// <response code="200">The list of words</response>
        [HttpGet("GetCourseWords")]
        [ProducesResponseType(typeof(List<WordViewModel>), StatusCodes.Status200OK)]
        public ActionResult GetCourseWords([FromQuery] string moduleId)
        {
            try
            {       
                return Ok(wordRepository.GetByModuleId(moduleId));
            }
            catch (Exception ex)
            {
                appMonitoringService.ExportError(ex);

                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Delete selected course word by courseId and wordId.
        /// </summary>
        /// <response code="200">Success/fail</response>
        [HttpPost("DeleteCourseWord")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult DeleteCourseWord([FromBody] DeleteCourseWordModel model)
        {
            try
            {       
                return Ok(courseRepository.DeleteCourseWord(model.ModuleId, model.WordId, UserId));
            }
            catch (Exception ex)
            {               
                var exceptionData = new Dictionary<string, object>();

                if (model != null)
                {
                    exceptionData["CourseId"] = model.ModuleId;
                    exceptionData["wordId"] = model.WordId;
                }

                appMonitoringService.ExportError(ex);

                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Delete selected course by id.
        /// </summary>
        /// <response code="200">Success/fail</response>
        [HttpGet("DeleteCourseById")]
        [ProducesResponseType(typeof(List<WordViewModel>), StatusCodes.Status200OK)]
        public ActionResult DeleteCourseById([FromQuery] string courseId)
        {
            try
            {       
                return Ok(courseRepository.DeleteCourseById(courseId, UserId));
            }
            catch (Exception ex)
            {               
                var exceptionData = new Dictionary<string, object>();
                
                exceptionData["courseId"] = courseId;
                
                appMonitoringService.ExportError(ex);

                return ErrorStatusCode;
            }
        }
        
        /// <summary>
        /// Pass a wordId to get an audio for the selected word.
        /// </summary>
        /// <param name="wordId">The ID of the selected word</param>
        /// <response code="200">A FileStreamResult that represents an audio file</response>
        [HttpPost("StreamAudio")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public ActionResult StreamAudio(string wordId)
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
                
                return ErrorStatusCode;
            }
        }
    }
}
