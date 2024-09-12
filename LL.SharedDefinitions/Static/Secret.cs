using Microsoft.Extensions.Configuration;

namespace LL.SharedDefinitions.Static
{
    public static class Secret
    {
        /// <summary>
        /// Needs to be populated at the start of the project.
        /// </summary>
        public static IConfiguration? Configuration { private get; set; }

        public static string GenimiAPI => Configuration?["GeminiAPI"] ?? string.Empty;
        public static string LLamaModeLocation => Configuration?["LLamaModeLocation"] ?? string.Empty;
    }
}
