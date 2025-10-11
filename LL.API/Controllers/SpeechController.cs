using System.Net.Mime;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LL.API.Controllers;

/// <response code="401">Either your security token is invalid
/// or your permissions do not allow the requested action.</response>
/// <response code="500">There is a bug in the system somewhere.</response>
[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class SpeechController(
    IWordRepository wordRepository,
    ITranscriptionService transcriptionService,
    IAppMonitoringService appMonitoringService) : BaseController
{
    /// <summary>
    /// Pass a wordId to get an audio for the selected word.
    /// </summary>
    /// <param name="wordId">The ID of the selected word</param>
    /// <response code="200">A FileStreamResult that represents an audio file</response>
    [HttpPost("TextToSpeech")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    public ActionResult TextToSpeech(string wordId)
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
    
    /// <summary>
    /// Pass a URL to a video online and get a text based on the video.
    /// </summary>
    /// <param name="url">The URL of the video the system will transcribe</param>
    /// <response code="200">The transcription text of the video</response>
    [HttpGet("SpeechToText")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    public ActionResult SpeechToText(string url)
    {
        try
        {
            return Ok(transcriptionService.TranscribeFromUrl(url));
        }
        catch (Exception ex)
        {
            var exceptionData = new Dictionary<string, object>();

            exceptionData["url"] = url;

            appMonitoringService.ExportError(ex, exceptionData);
                
            return ErrorStatusCode;
        }
    }
}