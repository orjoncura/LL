using LLama.Common;
using System.Text;
using LLama;

namespace LL.Extensions
{
    public class Agent
    {
        private static async Task<string> RunGeminiAPI(string input)
        {
            // Replace with your actual Google API key
            string apiKey = "";
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = input } // Pass the text input here
                        }
                    }
                }
            };

            // Serialize the payload to JSON
            string jsonPayload = JSON.SerializeObject(payload);

            // Create an HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                // Create the HTTP content with the JSON payload
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            
                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response
                return await response.Content.ReadAsStringAsync();
            }
        }

        private static async Task<string> RunLocalLlama(string input)
        {
            var parameters = new ModelParams(@"D:\GGUF\7B-chat.gguf")
            {
                GpuLayerCount = 12 // How many layers to offload to GPU. Please adjust it according to your GPU memory.
            };

            using var model = LLamaWeights.LoadFromFile(parameters);
            using var context = model.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            ChatSession session = new(executor);

            InferenceParams inferenceParams = new InferenceParams()
            {
                AntiPrompts = new List<string> { "User:" } // Stop generation once antiprompts appear.
            };

            string output = "";

            await foreach (var text in session.ChatAsync(new ChatHistory.Message(AuthorRole.User, input), inferenceParams))
                output += text;

            return output;
        }

        public static async Task<string> Run(string input)
        {
            return await RunGeminiAPI(input);
        }
    }
}
