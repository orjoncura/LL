using LL.Core.Helpers;
using LL.Core.Models.Arguments;
using LL.Extensions.Models;

namespace LL.Test.Helpers;

public class JsonHelperTest
{
    [Fact]
    public void JsonHelper_ExtractGeminiCandidate()
    {
        string content = File.ReadAllText(@"../../../Files/CandidateExample.txt");

        Candidate? candidate = JsonHelper.Extract<JsonData>(content)?.Candidates?.FirstOrDefault();
        
        Assert.True(candidate != null);
    }
    
    [Fact]
    public void JsonHelper_ExtractSeminarWordsModelList()
    {
        string content = File.ReadAllText(@"../../../Files/SeminarWordsModelListExample.txt");

        List<CourseWordsModel>? seminarWords = JsonHelper.Extract<List<CourseWordsModel>>(content);
        
        Assert.True(seminarWords != null && seminarWords.Any());
    }
}