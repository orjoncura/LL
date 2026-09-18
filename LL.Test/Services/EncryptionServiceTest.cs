using LL.Core.Constants;
using LL.Core.Interfaces.Extensions;
using LL.Core.Model.DataTransferObjects;
using LL.Resources.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LL.Test.Services;

public class EncryptionServiceTest
{
    private readonly IEncryptionService _encryptionService;

    public EncryptionServiceTest()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        // 32 ASCII chars => 16-byte AES key + 16-byte IV
        mockConfiguration.Setup(c => c[Secrets.EncryptionKey])
            .Returns("edTWS52cRCrRB4NDDCwT6mY6dMcWwa3n");

        _encryptionService = new EncryptionService(mockConfiguration.Object);
    }

    [Fact]
    public void HashPassword_AndVerifyPassword_ShouldRoundTrip()
    {
        var hash = _encryptionService.HashPassword("Password123!");

        Assert.False(string.IsNullOrWhiteSpace(hash.Password));
        Assert.False(string.IsNullOrWhiteSpace(hash.Salt));
        Assert.True(_encryptionService.VerifyPassword("Password123!", hash.Password, hash.Salt));
        Assert.False(_encryptionService.VerifyPassword("WrongPassword", hash.Password, hash.Salt));
    }

    [Fact]
    public void Encrypt_AndDecrypt_ShouldRoundTrip()
    {
        const int value = 42;
        var encrypted = _encryptionService.Encrypt(value);

        Assert.False(string.IsNullOrWhiteSpace(encrypted));
        Assert.Equal(value, _encryptionService.Decrypt(encrypted));
    }

    [Fact]
    public void GenerateSecureToken_ShouldReturnNonEmptyToken()
    {
        var token = _encryptionService.GenerateSecureToken();

        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.DoesNotContain("+", token);
        Assert.DoesNotContain("/", token);
        Assert.DoesNotContain("=", token);
    }

    [Fact]
    public void GenerateAuthenticationToken_ShouldReturnJwt()
    {
        var tokenConfig = new TokenConfigModel(
            "aP9vB3kL6mN8qR2tU5xY7zA1cD4fG6hJ9kL2mN5pQ8rS1vW4yZ7xC3vB6nM9qT2",
            "test-issuer",
            "test-audience",
            "60");

        var token = _encryptionService.GenerateAuthenticationToken(1, "user@test.com", tokenConfig);

        Assert.False(string.IsNullOrWhiteSpace(token.Token));
        Assert.False(string.IsNullOrWhiteSpace(token.Expiration));
    }
}
