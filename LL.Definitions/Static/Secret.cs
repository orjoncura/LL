using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.SharedDefinitions.Static
{
    public static class Secret
    {
        public static IConfiguration? Configuration { private get; set; }

        public static string GenimiAPI => Configuration?["GeminiAPI"] ?? string.Empty;
    }
}
