using Xunit;
using Moq;
using System.Threading.Tasks;
using Services;
using Repositories;
using Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _mockAuthRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockAuthRepository = new Mock<IAuthRepository>();
            _mockConfiguration = new Mock<IConfiguration>();

            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings:SecretKey", "this_is_a_test_key_that_is_long_enough"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authService = new AuthService(_mockAuthRepository.Object, configuration);
        }

        [Fact]
        public async Task Register_User_ReturnsUser()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Password = "password", Username = "testuser" };
            _mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>())).ReturnsAsync(false);
            _mockAuthRepository.Setup(repo => repo.Register(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _authService.Register(user);

            // Assert
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task Register_UserAlreadyExists_ThrowsException()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Password = "password", Username = "testuser" };
            _mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>())).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _authService.Register(user));
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsUser()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Password = _authService.HashPassword("password"), Username = "testuser" };
            _mockAuthRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _authService.Login("test@example.com", "password");

            // Assert
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task Login_InvalidPassword_ThrowsException()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Password = _authService.HashPassword("password"), Username = "testuser" };
            _mockAuthRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _authService.Login("test@example.com", "wrongpassword"));
        }

        [Fact]
        public async Task GetUserByEmail_ValidEmail_ReturnsUser()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Username = "testuser" };
            _mockAuthRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _authService.GetUserByEmail("test@example.com");

            // Assert
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task GetUserById_ValidId_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = "123", Email = "test@example.com", Username = "testuser" };
            _mockAuthRepository.Setup(repo => repo.GetUserById(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _authService.GetUserById("123");

            // Assert
            Assert.Equal("123", result.Id);
        }

        [Fact]
        public async Task UpdateUser_ValidUser_ReturnsUpdatedUser()
        {
            // Arrange
            var user = new User { Id = "123", Email = "test@example.com", Username = "testuser" };
            _mockAuthRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _authService.UpdateUser(user);

            // Assert
            Assert.Equal("123", result.Id);
        }

        [Fact]
        public async Task DeleteUser_ValidId_ReturnsTrue()
        {
            // Arrange
            _mockAuthRepository.Setup(repo => repo.DeleteUser(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.DeleteUser("123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserExists_ValidEmail_ReturnsTrue()
        {
            // Arrange
            _mockAuthRepository.Setup(repo => repo.UserExists(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.UserExists("test@example.com");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GenerateJwtToken_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new User { Id = "123", Email = "test@example.com" };

            // Act
            var token = _authService.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public void HashPassword_ReturnsHashedPassword()
        {
            // Arrange
            var password = "password";

            // Act
            var hashedPassword = _authService.HashPassword(password);

            // Assert
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "password";
            var hashedPassword = _authService.HashPassword(password);

            // Act
            var result = _authService.VerifyPassword(password, hashedPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var password = "password";
            var hashedPassword = _authService.HashPassword(password);

            // Act
            var result = _authService.VerifyPassword("wrongpassword", hashedPassword);

            // Assert
            Assert.False(result);
        }
    }
}
