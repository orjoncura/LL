using System.Text;
using Google.GenAI;
using Google.GenAI.Types;
using LL.Core.Constants;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using Microsoft.Extensions.Configuration;

namespace LL.Resources.Services;

public class TranscriptionService(IConfiguration configuration) : ITranscriptionService
{
    public async Task<string> TranscribeFromUrl(string videoUrl)
    {
        var geminiKey = configuration[Secrets.GeminiAPI] ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(geminiKey))
            return await TranscribeWithGemini(videoUrl, geminiKey);

        return await TranscribeWithWhisperApi(videoUrl);
    }

    private static async Task<string> TranscribeWithGemini(string videoUrl, string apiKey)
    {
        var client = new Client(apiKey: apiKey);

        var contents = new List<Content>
        {
            new()
            {
                Role = "user",
                Parts =
                [
                    new Part
                    {
                        FileData = new FileData
                        {
                            FileUri = videoUrl,
                            MimeType = "video/mp4"
                        }
                    },
                    new Part
                    {
                        Text =
                            "Transcribe all spoken audio from this video verbatim. " +
                            "Return only the transcript text with no commentary, titles, or timestamps. " +
                            "Preserve the original spoken language."
                    }
                ]
            }
        };

        var response = await client.Models.GenerateContentAsync(
            model: "gemini-2.5-flash",
            contents: contents);

        var candidate = response.Candidates?.FirstOrDefault();
        var transcript = candidate?.Content?.Parts?
            .Select(p => p.Text)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Aggregate(string.Empty, (current, next) =>
                string.IsNullOrEmpty(current) ? next! : current + " " + next)
            .Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(transcript))
            throw new Exception("Gemini returned an empty transcription.");

        return transcript;
    }

    private async Task<string> TranscribeWithWhisperApi(string videoUrl)
    {
        var api = configuration[Secrets.TranscriptionApi] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(api))
            throw new Exception("No Gemini API key or TranscriptionAPI configured.");

        var client = new HttpClient();
        var requestBody = new { url = videoUrl };
        var json = JsonHelper.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(api, content);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();

        var errorJson = await response.Content.ReadAsStringAsync();
        throw new Exception(errorJson);
    }
}
