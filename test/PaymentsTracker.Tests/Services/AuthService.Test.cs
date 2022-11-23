using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using PaymentsTracker.Common.Options;
using PaymentsTracker.Models.Models;
using PaymentsTracker.Services.Business;

namespace PaymentsTracker.Tests.Services;

public class AuthServiceTest
{
    private readonly Mock<IOptions<JwtOptions>> _optionsMock = new();

    public AuthServiceTest()
    {
        _optionsMock.Setup(x => x.Value)
            .Returns(() => new JwtOptions()
            {
                SecretKey = Guid.NewGuid().ToString(),
                TokenPeriod = TimeSpan.FromHours(10)
            });
    }

    [Theory]
    [InlineData("admi.n@site.com")]
    [InlineData("a.b.c.d.e@outlook.com")]
    [InlineData("example@site.com")]
    public void NormalizeEmail_ReturnsSameEmail_WhenNotGmail(string email)
    {
        // Arrange
        var authService = new AuthService(null, _optionsMock.Object, null);
        var method = authService.GetType()
            .GetMethod("NormalizeEmail", BindingFlags.NonPublic | BindingFlags.Static);
        if (method is null)
            Assert.Fail("Method 'NormalizeEmail' is not defined.");
        // Act
        var result = method.Invoke(authService, new object?[] { email });
        // Assert
        result.Should()
            .BeOfType<string>()
            .And
            .Be(email);
    }

    [Theory]
    [InlineData("admin@gmail.com", "admin@gmail.com")]
    [InlineData("a.b.c.d.e.f@gmail.com", "abcdef@gmail.com")]
    [InlineData("email.contains_many.dots@gmail.com", "emailcontains_manydots@gmail.com")]
    public void NormalizeEmail_ReturnsEmailWithoutDots_WhenEndsWithGmail(string email, string normalizedEmail)
    {
        // Arrange
        var authService = new AuthService(null, _optionsMock.Object, null);
        var method = authService.GetType()
            .GetMethod("NormalizeEmail", BindingFlags.NonPublic | BindingFlags.Static);
        if (method is null)
            Assert.Fail("Method 'NormalizeEmail' is not defined.");
        // Act
        var result = method.Invoke(authService, new object?[] { email });
        // Assert
        result.Should()
            .BeOfType<string>()
            .And
            .Be(normalizedEmail);
    }


    [Fact]
    public void GenerateToken_ReturnValidToken_WhenValidUserObject()
    {
        // Arrange
        var authService = new AuthService(null, _optionsMock.Object, null);
        var generateTokenMethod = authService.GetType().GetMethod("GenerateToken",
             BindingFlags.NonPublic | BindingFlags.Instance, new []{typeof(User)});
        if (generateTokenMethod is null)
            Assert.Fail("Method 'GenerateToken' is not defined.");
        // Act

        var user = new User()
        {
            UserId = 1
        };

        var token = generateTokenMethod.Invoke(authService, new[] { user });

        // Assertion
        token.Should()
            .BeOfType<string>()
            .And
            .Match((string s) => s.Split(".", StringSplitOptions.None).Length == 3);
    }
}