using System.Diagnostics;
using System.Globalization;
using System.Reflection;
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
     
    public byte[] CreateAudio1(string text, LanguageEnum lang)
    {
        text = text.ToLower().Trim();
        
        // Get the resource stream
        using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LL.Extensions.Scripts.TextToSpeech.py"))
        {
            if (resourceStream == null)
                throw new Exception("Resource not found.");

            string tempFilePath = Path.GetTempFileName();
            using (FileStream tempFile = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                resourceStream.CopyTo(tempFile);
            }
            
            string outputPath = text.Trim().Replace(" ", "_") + ".wav";
            string pythonPath = "/usr/bin/python3"; // Adjust if needed
        
            // Create the process
            Process process = new Process();
            process.StartInfo.FileName = pythonPath;
            process.StartInfo.Arguments = $"{tempFilePath} {text} {GetModelPath(lang)} {outputPath}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            
            // Capture the process output
            process.Start();
            string error = process.StandardError.ReadToEnd();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
                return [];

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
            
            // Verify if the audio file was created
            if (File.Exists(outputPath))
                return File.ReadAllBytes(outputPath);
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