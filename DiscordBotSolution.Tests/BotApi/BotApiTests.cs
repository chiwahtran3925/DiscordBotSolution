using DiscordBotSolution.BotApi.Core.DTOs;
using DiscordBotSolution.BotApi.Core.Interfaces;
using DiscordBotSolution.BotApi.Core.Models;
using DiscordBotSolution.BotApi.Core.Services;
using Moq;

namespace DiscordBotSolution.Tests.BotApi;

public class BotApiTests
{
    private Mock<IUserRepository> _userRepository;
    private Mock<IUserTimerRepository> _mockUserTimerRepository;
    public BotApiTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _mockUserTimerRepository = new Mock<IUserTimerRepository>();

    }

    [Fact]
    public async Task Login_ReturnsTrue_WhenUserLoggedIn()
    {
        //Arrange
        var loginRequest = new LoginRequest(
            Username:"Admin",
            Password:"Password"
        );

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act
        var result = await botService.Login(loginRequest);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Login_ReturnsFalse_WhenUserLoggedIn()
    {
        //Arrange
        var loginRequest = new LoginRequest(
            Username: "Chi8899",
            Password: "12345678"
        );

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act
        var result = await botService.Login(loginRequest);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateUser_ReturnsUser_WhenuCreated()
    {
        //Arrange
        var userRequest = new UserRequest(
            Username:"Chi8899",
            IsBlocked:false
        );

        var user = new User()
        {
            Id = 1,
            Username = "Chi8899",
            IsBlocked = false
        };

        var userResponse = new UserResponse(
            Id: user.Id,
            Username: user.Username,
            IsBlocked: user.IsBlocked
            );

        _userRepository.Setup(c => c.AddAsync(user)).Returns(Task.FromResult(userResponse));

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act
        var result = await botService.CreateUser(userRequest);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(result.Username,userResponse.Username);
        Assert.False(result.IsBlocked);
    }

    [Fact]
    public async Task CreateUser_ThrowsException_WhenUserExist()
    {
        //Arrange
        var userRequest = new UserRequest(
            Username: "Chi8899",
            IsBlocked: false
        );

        var existingUser = new User()
        {
            Username = "Chi8899",
            IsBlocked = false
        };

        _userRepository.Setup(c => c.GetByUsernameAsync(userRequest.Username)).ReturnsAsync(existingUser);

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(()=>botService.CreateUser(userRequest));    
    }

    [Fact]
    public async Task UpdateUser_ReturnsTrue_WhenUpdated()
    {
        //Arrange
        var userRequest = new UserRequest(
            Username: "Chi8899",
            IsBlocked: true
        );

        var user = new User()
        {
            Username = "Chi8899",
            IsBlocked = false
        };

        _userRepository.Setup(c => c.GetByUsernameAsync(userRequest.Username)).ReturnsAsync(user);
        _userRepository.Setup(c => c.UpdateAsync(user)).ReturnsAsync(true);

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act
        var result = await botService.UpdateUser(userRequest);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUser_ThrowsException_WhenuUserNotExist()
    {
        //Arrange
        var userRequest = new UserRequest(
            Username: "Chi88992",
            IsBlocked: true
        );

        _userRepository.Setup(c => c.GetByUsernameAsync(userRequest.Username)).ReturnsAsync((User?)null); ;

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => botService.UpdateUser(userRequest));
    }


    [Fact]
    public async Task GetUserByUsername_ReturnsUser_WhenExists()
    {
        //Arrange
        var username = "Chi8899";

        var user = new User()
        {
            Username = "Chi8899",
            IsBlocked = false
        };

        _userRepository.Setup(c => c.GetByUsernameAsync(username)).ReturnsAsync(user);

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act
        var result = await botService.GetUserByUsername(username);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(result.Username, username);
        Assert.False(result.IsBlocked);
    }

    [Fact]
    public async Task GetUserByUsername_IsNull_WhenUserNotExists()
    {
        //Arrange
        var username = "Chi8899";
        _userRepository.Setup(c => c.GetByUsernameAsync(username)).ReturnsAsync((User?)null);

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        var result = await botService.GetUserByUsername(username);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUser_ReturnsUsers_WhenUsersExists()
    {
        //Arrange
        var users = new List<User>()
        {
            new User(){ Id=1,Username="Test1", IsBlocked=false },
            new User(){ Id=2,Username="Test2", IsBlocked=false }
        };

        _userRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(users);

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        var result = await botService.GetAllUser();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllUser_ReturnsNoUsers_WhenNoUsersExists()
    {
        //Arrange
        _userRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(new List<User>());

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        var result = await botService.GetAllUser();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateUserTimer_ReturnsUserTimers_WhenUserTimerCreated()
    {
        //Arrange
        var usersTimeRequest = new UserTimerRequest(
            UserId:1, 
            Duration: 1, 
            StartTime:DateTime.UtcNow
        );

        var usersTime = new UserTimer()
        {
            Completed = false,
            Duration = 1,
            Id = 1,
            StartTime = DateTime.UtcNow,
            UserId = 1
        };

        _mockUserTimerRepository.Setup(c => c.AddAsync(usersTime));

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        var result = await botService.CreateUserTimer(usersTimeRequest);
        Assert.NotNull(result);
        Assert.False(result.Completed);
    }

    [Fact]
    public async Task UpdateUserTimer_ReturnsTrue_WhenUserTimerUpdated()
    {
        //Arrange
        var usersTimeRequest = new UserTimerRequest(
            Id:1,
            UserId: 1,
            Duration: 1,
            StartTime: DateTime.UtcNow,
            Completed:false
        );

        var usersTime = new UserTimer()
        {
            Completed = false,
            Duration = 1,
            Id = 1,
            StartTime = DateTime.UtcNow,
            UserId = 1
        };

        _mockUserTimerRepository.Setup(c => c.GetByIdAsync(usersTimeRequest.Id)).ReturnsAsync(usersTime);
        usersTime.Completed = true;
        _mockUserTimerRepository.Setup(c => c.UpdateAsync(usersTime)).ReturnsAsync(true);

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        var result = await botService.UpdateUserTimer(usersTimeRequest);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUserTimer_ThrowsException_WhenNoUserTimerExists()
    {
        //Arrange
        var usersTimeRequest = new UserTimerRequest(
            Id: 1,
            UserId: 1,
            Duration: 1,
            StartTime: DateTime.UtcNow,
            Completed: false
        );

        _mockUserTimerRepository.Setup(c => c.GetByIdAsync(usersTimeRequest.Id)).ReturnsAsync((UserTimer?)null);

        //Setup
        var botService = new BotService(_userRepository.Object, _mockUserTimerRepository.Object);

        //Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(()=>botService.UpdateUserTimer(usersTimeRequest));
    }

}
