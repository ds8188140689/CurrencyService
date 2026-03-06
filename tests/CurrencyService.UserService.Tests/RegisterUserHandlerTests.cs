using CurrencyService.Shared.Domain.Entities;
using CurrencyService.Shared.Infrastructure.Persistence;
using CurrencyService.UserService.Application.Commands.RegisterUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CurrencyService.UserService.Tests;

/// <summary>
/// Тест регистрации пользователей
/// </summary>
public class RegisterUserHandlerTests
{
    private readonly AppDbContext _ctx;
    private readonly Mock<IPasswordHasher<User>> _hasherMock;
    private readonly RegisterUserHandler _handler;

    /// <summary>
    /// Конструктор
    /// </summary>
    public RegisterUserHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _ctx = new AppDbContext(options);
        _hasherMock = new Mock<IPasswordHasher<User>>();
        _hasherMock.Setup(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashed");
        _handler = new RegisterUserHandler(_ctx, _hasherMock.Object);
    }

    /// <summary>
    /// Зарегистрировать пользователя при отсутствии логина в БД
    /// </summary>
    [Fact]
    public async Task Handle_ShouldCreateUser_WhenLoginNotExists()
    {
        var cmd = new RegisterUserCommand("Test", "test_login", "pass123");
        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.True(result.IsSuccess);
        User? user = await _ctx.Users.FindAsync(result.Value);
        Assert.NotNull(user);
        _hasherMock.Verify(h => h.HashPassword(It.IsAny<User>(), "pass123"), Times.Once);
    }

    /// <summary>
    /// Получить ошибку регистрации пользователя при наличии логина в БД
    /// </summary>
    [Fact]
    public async Task Handle_ShouldFail_WhenLoginExists()
    {
        _ctx.Users.Add(new User { Login = "test_login" });
        await _ctx.SaveChangesAsync();

        var cmd = new RegisterUserCommand("Test", "test_login", "pass");
        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("уже существует", result.Error);
    }
}