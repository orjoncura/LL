using LL.Core.Enums;
using LL.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace LL.Test.Repositories;

public class WordRepositoryTest
{
    private IWordRepository _wordRepository { get; set; }
    
    public WordRepositoryTest()
    {
        _wordRepository = Provider.GetRequiredService<IWordRepository>();
    }
    
    [Fact]
    public async Task Insert_AddRecords()
    {
        // Assert
        Assert.True(_wordRepository.Insert("Hola", (int)LanguageEnum.Spanish, 1).Id > 0);
    } 
    
    [Fact]
    public void TestSecrets()
    {
        // Arrange (Setup secrets - run these in your terminal in the test project directory)
        // dotnet user-secrets init
        // dotnet user-secrets set "MyKey" "MyValue"

        // Act
        var config = new ConfigurationBuilder()
            .AddUserSecrets<WordRepositoryTest>() // Use the test class itself!
            .Build();

        string test = config["DefaultConnection"];
        string secrets = config.GetSection("DefaultConnection").Value;
        // Assert
        Assert.Equal("MyValue", config["DefaultConnection"]);
    }
}