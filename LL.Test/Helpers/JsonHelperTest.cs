using LL.Core.Helpers;
using LL.Extensions.Models;

namespace LL.Test.Helpers;

public class JsonHelperTest
{
    [Fact]
    public async Task JsonHelper_ExtractGeminiCandidate()
    {
        string content = File.ReadAllText(@"../../../Files/CandidateExample.txt");

        Candidate? candidate = JsonHelper.Extract<Response>(content)?.Candidates?.FirstOrDefault();
        
        Assert.True(candidate != null);
    }
}