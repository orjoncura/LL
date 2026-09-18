using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Model.DataTransferObjects;
using LL.Resources.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LL.Test.Services;

public class TranscriptionServiceTest
{
    [Fact]
    public async Task TranscribeFromUrl_WithoutGemini_UsesTranscriptionApiAndThrowsOnFailure()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c[Secrets.GeminiAPI]).Returns(string.Empty);
        mockConfiguration.Setup(c => c[Secrets.TranscriptionApi])
            .Returns("http://127.0.0.1:1/transcribe");

        var service = new TranscriptionService(mockConfiguration.Object);

        await Assert.ThrowsAnyAsync<Exception>(() =>
            service.TranscribeFromUrl("https://www.youtube.com/watch?v=dQw4w9WgXcQ"));
    }

    [Fact]
    public async Task TranscribeFromUrl_WithInvalidGeminiKey_ShouldThrow()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c[Secrets.GeminiAPI]).Returns("invalid-gemini-key");

        var service = new TranscriptionService(mockConfiguration.Object);

        await Assert.ThrowsAnyAsync<Exception>(() =>
            service.TranscribeFromUrl("https://www.youtube.com/watch?v=dQw4w9WgXcQ"));
    }
}

public class AgentServiceTest
{
    [Fact]
    public async Task Run_WithoutGemini_UsesOllamaAndThrowsWhenUnavailable()
    {
        var service = new AgentService(new AgentModel(string.Empty, "http://127.0.0.1:1", "qwen2.5:14b", "qwen2.5:1.5b", "online"));

        await Assert.ThrowsAnyAsync<Exception>(() => service.Run("hello"));
    }

    [Fact]
    public async Task Run_WithInvalidGeminiKey_ShouldThrow()
    {
        var service = new AgentService(new AgentModel("invalid-gemini-key", string.Empty, "qwen2.5:14b", "qwen2.5:1.5b", "online"));

        await Assert.ThrowsAnyAsync<Exception>(() => service.Run("hello"));
    }
}

public class TextToSpeechServiceTest
{
    [Fact]
    public void CreateAudio_WithoutCredentials_ShouldThrow()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c[Secrets.GoogleCredentialsJson]).Returns((string?)null);

        var service = new TextToSpeechService(mockConfiguration.Object);

        Assert.ThrowsAny<Exception>(() =>
            service.CreateAudio("hola", LL.Core.Enums.LanguageEnum.Spanish));
    }
}
