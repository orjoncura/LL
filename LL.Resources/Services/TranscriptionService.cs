using System.Diagnostics;
using System.Text;
using LL.Core.Interfaces.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LL.Resources.Services;

public class TranscriptionService() : ITranscriptionService
{  
    public async Task<string> TranscribeFromUrl(string videoUrl)
    {
        var process = new Process();
        process.StartInfo.FileName = "docker";
        process.StartInfo.Arguments = $"run --rm whisper-youtube \"{GetYouTubeShareUrl(videoUrl)}\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        
        // Wait for the process to exit
        await Task.Run(() => process.WaitForExit());
        
        return ExtractTranscript(output);
    }
    
    private static string GetYouTubeShareUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            string videoId = string.Empty;

            // Handle standard YouTube links
            if (uri.Host.Contains("youtube.com"))
            {
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                videoId = query.Get("v") ?? string.Empty;

                // Case 2: https://www.youtube.com/live/VIDEOID
                if (string.IsNullOrEmpty(videoId) && uri.AbsolutePath.StartsWith("/live/"))
                {
                    videoId = uri.AbsolutePath.Split("/live/")[1].Split("/")[0];
                }

                // Case 3: https://www.youtube.com/shorts/VIDEOID
                if (string.IsNullOrEmpty(videoId) && uri.AbsolutePath.StartsWith("/shorts/"))
                {
                    videoId = uri.AbsolutePath.Split("/shorts/")[1].Split("/")[0];
                }

                // Case 4: https://www.youtube.com/embed/VIDEOID
                if (string.IsNullOrEmpty(videoId) && uri.AbsolutePath.StartsWith("/embed/"))
                {
                    videoId = uri.AbsolutePath.Split("/embed/")[1].Split("/")[0];
                }
            }

            // Handle short links
            if (uri.Host.Contains("youtu.be"))
            {
                var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length > 0)
                {
                    videoId = pathParts[0];
                }
            }

            return !string.IsNullOrEmpty(videoId)
                ? $"https://youtu.be/{videoId}"
                : url;
        }
        catch
        {
            return url;
        }
    }
    
    private string ExtractTranscript(string input)
    {
        int start = input.IndexOf("📄 Transcript:");
        
        if (start == -1)
        {
            throw new ArgumentException("The input text does not contain the expected delimiter '📄 Transcript:'", nameof(input));
        }

        // Move past "📄 Transcript:"
        start += "📄 Transcript:".Length;

        // Find the end of the transcript or the end of the string
        int end = input.Length;

        if (end == -1)
        {
            return input.Substring(start);
        }
        
        return input.Substring(start, end - start).Trim();
    }
}