using LL.Core.Helpers;
using LL.Core.Models.Arguments;
using LL.Core.Models.ViewModels;
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
    public void JsonHelper_ExtractExerciseViewModel()
    {
        string content = File.ReadAllText(@"../../../Files/ExerciseViewModelJSON.txt");

        Candidate? candidate = JsonHelper.Extract<JsonData>(content)?.Candidates?.FirstOrDefault();
        string output = candidate?.Content?.Parts?.Select(x => x.Text).Aggregate((x, y) => x + " " + y) ?? string.Empty;

        List<ExerciseViewModel>? exercises = JsonHelper.Extract<List<ExerciseViewModel>>(output)?.ToList();
        
        Assert.True(exercises != null && exercises.Any());
    }
    
    [Fact]
    public void JsonHelper_ExtractCourseWordsModelList()
    {
        string content = File.ReadAllText(@"../../../Files/CourseWordsModelExample.txt");

        List<CourseWordsModel>? seminarWords = JsonHelper.Extract<List<CourseWordsModel>>(content);
        
        Assert.True(seminarWords != null && seminarWords.Any());
    }
}