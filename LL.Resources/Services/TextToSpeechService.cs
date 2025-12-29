using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using LL.Core.Constants;
using LL.Core.Enums;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using Microsoft.Extensions.Configuration;

namespace LL.Resources.Services;

public class TextToSpeechService(IConfiguration configuration) : ITextToSpeechService
{
    public byte[] CreateAudio(string text, LanguageEnum lang)
    {
        var rawJson = configuration[Secrets.GoogleCredentialsJson] ?? string.Empty;
        
        var credentials = GoogleCredential.FromJson(rawJson);
        var client = new TextToSpeechClientBuilder { Credential = credentials }.Build();
        
        var input = new SynthesisInput
        {
            Text = text
        };

        var voice = new VoiceSelectionParams
        {
            LanguageCode = GetLanguageCode(lang),
            SsmlGender = SsmlVoiceGender.Female
        };

        var config = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Mp3
        };

        var response = client.SynthesizeSpeech(input, voice, config);
        
        string outputPath = "output.mp3";
        
        // Save the audio file
        File.WriteAllBytes(outputPath, response.AudioContent.ToByteArray());
        
        if (File.Exists(outputPath))
        {
            var output = File.ReadAllBytes(outputPath);
            
            File.Delete(outputPath);
            
            return output;
        }
        
        return [];
    }
     
    private string GetLanguageCode(LanguageEnum lang)
    {
        string code = "en-US";
        
        if(lang == LanguageEnum.Spanish)
            code = "es-ES";

        return code;
    }
}