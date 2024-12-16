using System.Diagnostics;
using System.Reflection;
using LL.Core.Enums;
using LL.Core.Interfaces.Extensions;

namespace LL.Extensions;

public class TextToSpeechService : ITextToSpeechService
{
    public void CreateAudio(string text, int langId)
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
            return;
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
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine("TTS Errors:");
            Console.WriteLine(error);
        }

        // Verify if the audio file was created
        if (File.Exists(outputPath))
        {
            Console.WriteLine($"Audio file successfully generated at: {outputPath}");

            // Read the audio file as bytes (for API or streaming)
            byte[] audioBytes = File.ReadAllBytes(outputPath);
            Console.WriteLine($"Audio file read successfully. Size: {audioBytes.Length} bytes.");
            
            // Optional: Serve or stream the audio
            // Example: Copy file to shared location
            string sharedPath = "/output_es.wav";
            File.Copy(outputPath, sharedPath, overwrite: true);
            Console.WriteLine($"Audio file copied to: {sharedPath}");
        }
        else
        {
            Console.WriteLine("Failed to generate audio file.");
        }
    }
    
    private string GetModelPath(int id)
    {
        string path = "/tts_models/en/ljspeech/tacotron2-DDC";
        
        if(id == (int)LanguageEnum.Spanish)
            path = "/tts_models/es/mai/tacotron2-DDC";

        return path;
    }
}