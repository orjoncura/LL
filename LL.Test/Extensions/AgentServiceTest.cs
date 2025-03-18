using LL.Core.Enums;
using LL.Core.Factories;
using LL.Core.Helpers;
using LL.Core.Interfaces.Extensions;
using LL.Core.Models.Arguments;

namespace LL.Test.Extensions;

public class AgentServiceTest
{
    private IAgentService _agentService { get; set; }
    
    public AgentServiceTest()
    {
        _agentService = Provider.GetRequiredService<IAgentService>();
    }
        
    [Fact]
    public async Task Run_RunAgent_Success()
    {                  
        string rankingPrompt = PromptFactory.CreateCourseWordsPrompt(
            "Creo en los milagros desde que te vi", 
            (int)LanguageEnum.Spanish, 
            (int)LanguageEnum.English);

        List<CourseWordsModel> words = JsonHelper.Extract<List<CourseWordsModel>>(await _agentService.Run(rankingPrompt));
        
        Assert.True(words.Count == 8);
    } 
}