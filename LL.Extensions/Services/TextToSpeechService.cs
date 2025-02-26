using System.Diagnostics;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;

namespace LL.Extensions.Services;

public class TextToSpeechService : ITextToSpeechService
{
    public byte[] CreateAudio(string text, LanguageEnum lang)
    {
        string virtualEnvPath = "/home/orjoncura/my_tts_env"; // Path to your virtual environment
        string ttsCommandPath = Path.Combine(virtualEnvPath, "bin", "tts"); // Path to the TTS executable

        // Check if the TTS executable exists
        if (!File.Exists(ttsCommandPath))
            return [];
        
        // Text, model name, and output path parameters
        string modelName = GetModelPath(lang); 
        string outputPath = text.Trim().Replace(" ", "_") + ".wav";

        //This models fails with short text.
        if (text.Length == 1)
            text += text;
        
        // Construct the TTS command
        var command = new ProcessStartInfo
        {
            FileName = ttsCommandPath,
            Arguments = $"--text \"{text}\" --model_name {modelName} --out_path {outputPath}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // Start the process
        using (var process = Process.Start(command))
        {
            if (process == null)
                return [];
            
            // Read output and error (if any)
            //string output = process.StandardOutput.ReadToEnd();
            //string error = process.StandardError.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();
        }

        // Verify if the audio file was created
        if (File.Exists(outputPath))
        {
            var output = File.ReadAllBytes(outputPath);
            
            File.Delete(outputPath);
            
            return output;
        }
        
        return [];
    }
     
    private string GetModelPath(LanguageEnum lang)
    {
        string path = "tts_models/en/ljspeech/tacotron2-DDC";
        
        if(lang == LanguageEnum.Spanish)
            path = "tts_models/es/mai/tacotron2-DDC";

        return path;
    }
}