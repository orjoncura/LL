using LL.SharedDefinitions.Models;
using LL.Extensions.Models;
using LL.Extensions.JSON;

namespace LL.Test.Extensions
{
    public class JSONTest
    {
        [Fact]
        public async Task JSON_ExtractJsonData()
        {
            JsonData data = new JsonData();

            string jsonString = "{\n  \"candidates\": [\n    {\n      \"content\": {\n        \"parts\": [\n          {\n            \"text\": \"{\\\"TargetWord\\\": {\\\"OriginalStatement\\\": \\\"Creo\\\", \\\"TranslatedStatement\\\": \\\"I believe\\\"}, \\\"Sentence\\\": [{\\\"OriginalStatement\\\": \\\"Creo que el clima será agradable mañana.\\\", \\\"TranslatedStatement\\\": \\\"I believe the weather will be nice tomorrow.\\\"}, {\\\"OriginalStatement\\\": \\\"Creo que puedo hacer un buen trabajo en este proyecto.\\\", \\\"TranslatedStatement\\\": \\\"I think I can do a good job on this project.\\\"}, {\\\"OriginalStatement\\\": \\\"Creo que es importante ser honesto.\\\", \\\"TranslatedStatement\\\": \\\"I believe it's important to be honest.\\\"}]}\\n\"\n          }\n        ],\n        \"role\": \"model\"\n      },\n      \"finishReason\": \"STOP\",\n      \"index\": 0,\n      \"safetyRatings\": [\n        {\n          \"category\": \"HARM_CATEGORY_SEXUALLY_EXPLICIT\",\n          \"probability\": \"NEGLIGIBLE\"\n        },\n        {\n          \"category\": \"HARM_CATEGORY_HATE_SPEECH\",\n          \"probability\": \"NEGLIGIBLE\"\n        },\n        {\n          ...\n        }\n      ]\n    }\n  ]\n}";
            
            data = JSON.Extract<JsonData>(jsonString);

            Assert.True(data.Candidate.Any());
        }

        [Fact]
        public async Task JSON_ExtractSeminarViewModel()
        {
            SeminarViewModel seminarViewModel = new SeminarViewModel();

            string jsonString = "@{\"TargetWord\": {\"OriginalStatement\": \"Creo\", \"TranslatedStatement\": \"I believe\"}, \"Sentence\": [{\"OriginalStatement\": \"Creo que el sol saldrá mañana.\", \"TranslatedStatement\": \"I believe the sun will rise tomorrow.\"}, {\"OriginalStatement\": \"Creo en el poder de la música.\", \"TranslatedStatement\": \"I believe in the power of music.\"}, {\"OriginalStatement\": \"Creo que podemos hacer una diferencia en el mundo.\", \"TranslatedStatement\": \"I believe we can make a difference in the world.\"}]}\n";

            seminarViewModel = JSON.Extract<SeminarViewModel>(jsonString);

            Assert.True(seminarViewModel.IsValid);
        }
    }   
}