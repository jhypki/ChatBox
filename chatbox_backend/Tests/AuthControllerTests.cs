using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Models;
using Controllers;

namespace Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_User_ReturnsOkResultWithToken()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Password = "password", Username = "testuser" };
            _mockAuthService.Setup(service => service.HashPassword(It.IsAny<string>())).Returns("hashedpassword");
            _mockAuthService.Setup(service => service.Register(It.IsAny<User>())).ReturnsAsync(user);
            _mockAuthService.Setup(service => service.GenerateJwtToken(It.IsAny<User>())).Returns("token");

            // Act
            var result = await _controller.Register(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value as dynamic;
            Assert.Equal("test@example.com", returnValue.user.Email);
            Assert.Equal("token", returnValue.token);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsOkResultWithToken()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "test@example.com", Password = "password" };
            var user = new User { Email = "test@example.com", Username = "testuser" };
            _mockAuthService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _mockAuthService.Setup(service => service.GenerateJwtToken(It.IsAny<User>())).Returns("token");

            // Act
            var result = await _controller.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value as dynamic;
            Assert.Equal("test@example.com", returnValue.Email);
            Assert.Equal("token", returnValue.token);
        }

        //     [Fact]
        //     public async Task GetUserByEmail_ValidEmail_ReturnsOkResult()
        //     {
        //         // Arrange
        //         var user = new User { Email = "test@example.com", Username = "testuser" };
        //         _mockAuthService.Setup(service => service.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(user);

        //         // Act
        //         var result = await _controller.GetUserByEmail("test@example.com");

        //         // Assert
        //         var okResult = Assert.IsType<OkObjectResult>(result);
        //         var returnValue = okResult.Value as User;
        //         Assert.NotNull(returnValue);
        //         Assert.Equal("test@example.com", returnValue.Email);
        //     }

        //     [Fact]
        //     public async Task GetUserById_ValidId_ReturnsOkResult()
        //     {
        //         // Arrange
        //         var user = new User { Id = "123", Email = "test@example.com", Username = "testuser" };
        //         _mockAuthService.Setup(service => service.GetUserById(It.IsAny<string>())).ReturnsAsync(user);

        //         // Act
        //         var result = await _controller.GetUserById("123");

        //         // Assert
        //         var okResult = Assert.IsType<OkObjectResult>(result);
        //         var returnValue = okResult.Value as User;
        //         Assert.NotNull(returnValue);
        //         Assert.Equal("123", returnValue.Id);
        //     }

        //     [Fact]
        //     public async Task UpdateUser_ValidUser_ReturnsOkResult()
        //     {
        //         // Arrange
        //         var user = new User { Id = "123", Email = "test@example.com", Username = "testuser" };
        //         _mockAuthService.Setup(service => service.UpdateUser(It.IsAny<User>())).ReturnsAsync(user);

        //         // Act
        //         var result = await _controller.UpdateUser(user);

        //         // Assert
        //         var okResult = Assert.IsType<OkObjectResult>(result);
        //         var returnValue = okResult.Value as User;
        //         Assert.NotNull(returnValue);
        //         Assert.Equal("123", returnValue.Id);
        //     }

        //     [Fact]
        //     public async Task DeleteUser_ValidId_ReturnsOkResult()
        //     {
        //         // Arrange
        //         _mockAuthService.Setup(service => service.DeleteUser(It.IsAny<string>())).ReturnsAsync(true);

        //         // Act
        //         var result = await _controller.DeleteUser("123");

        //         // Assert
        //         var okResult = Assert.IsType<OkObjectResult>(result);
        //         Assert.True((bool)okResult.Value);
        //     }
    }
}
