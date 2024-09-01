using LLama.Common;
using LLama;

namespace LL.Extensions
{
    public class Agent
    {
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
            return await RunLocalLlama(input);
        }
    }
}
