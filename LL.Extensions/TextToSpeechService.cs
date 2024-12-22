using System.Diagnostics;
using System.Reflection;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;

namespace LL.Extensions;

public class TextToSpeechService : ITextToSpeechService
{
    public byte[] CreateAudio(string text, int langId)
    {
        string scriptPath = string.Empty;
        
        // Get the resource stream
        using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LL.Extensions.Scripts.TextToSpeech.py"))
        {
            if (resourceStream == null)
                throw new Exception("Resource not found.");

            scriptPath = Path.GetTempFileName();

            // Save resource to file
            using (var fileStream = new FileStream(scriptPath, FileMode.Create, FileAccess.Write))
            {
                resourceStream.CopyTo(fileStream);
            }
        }
        
        // Verify the script exists
        if (!File.Exists(scriptPath))
        {
            Console.WriteLine($"Script not found: {scriptPath}");
            return [];
        }
        
        string outputPath = text.Trim().Replace(" ", "_") + ".wav";
        string pythonPath = "/usr/bin/python3"; // Adjust if needed
        
        // Create the process
        Process process = new Process();
        process.StartInfo.FileName = pythonPath;
        process.StartInfo.Arguments = $"{scriptPath} {text} {GetModelPath(langId)} {outputPath}";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;

        // Capture the process output
        process.Start();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
            return [];

        // Verify if the audio file was created
        if (File.Exists(outputPath))
            return File.ReadAllBytes(outputPath);
        
        return [];
    }
    
    private string GetModelPath(int id)
    {
        string path = "/tts_models/en/ljspeech/tacotron2-DDC";
        
        if(id == (int)LanguageEnum.Spanish)
            path = "/tts_models/es/mai/tacotron2-DDC";

        return path;
    }
}