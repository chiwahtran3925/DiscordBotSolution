using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordBotSolution.BotWorker.Core.DTOs;
using DiscordBotSolution.BotWorker.Core.Interfaces;
using DiscordBotSolution.BotWorker.Core.Services;
using Moq;

namespace DiscordBotSolution.Tests;

public class BotWorkerTests
{
    private readonly Mock<IApiClient> _apiClient;
    private readonly Mock<ITimerService> _timerService;
    private readonly Mock<ISocketMessageChannel> _socketMessageChannel;
    public BotWorkerTests()
    {
        _apiClient = new Mock<IApiClient>();
        _timerService = new Mock<ITimerService>();
        _socketMessageChannel = new Mock<ISocketMessageChannel>();
    }

    [Fact]
    public async Task HandleMessageAsync_CreatesUserSendsPING_RecievesPONG()
    {
        //Arrange
        var username = "Chi8899";
        var content = "PING";

        var botUserRequest = new BotUserRequest(        
            Username : username,
            IsBlocked : false
        );
        var botUserResponse = new BotUserResponse(
            Username: username,
            IsBlocked: false,
            Id:1
        );
        _socketMessageChannel.Setup(c => c.SendMessageAsync("PONG",false, null, null, null, null, null, null, null, MessageFlags.None,null));
        _apiClient.Setup(a => a.CreateUserAsync(botUserRequest)).Returns(Task.FromResult<BotUserResponse?>(botUserResponse));

        //Setup
        var discordMessageService = new DiscordMessageService(_apiClient.Object, _timerService.Object);

        //Act
        await discordMessageService.HandleMessageLogicAsync(content, username, _socketMessageChannel.Object);

        //Assert
        _socketMessageChannel.Verify(c => c.SendMessageAsync("PONG",false, null, null, null, null, null, null, null, MessageFlags.None, null), Times.Once);
        _apiClient.Verify(c => c.CreateUserAsync(botUserRequest), Times.Once);
    }


    [Fact]
    public async Task HandleMessageAsync_SkipsCreationSendsPING_RecievesPONG()
    {
        //Arrange
        var username = "Chi8899";
        var content = "PING";

        var botUserResponse = new BotUserResponse(
            Username: username,
            IsBlocked: false,
            Id: 1
        );
        _socketMessageChannel.Setup(c => c.SendMessageAsync("PONG", false, null, null, null, null, null, null, null, MessageFlags.None, null));
        _apiClient.Setup(a => a.GetUserByUsernameAsync(username)).Returns(Task.FromResult<BotUserResponse?>(botUserResponse));

        //Setup
        var discordMessageService = new DiscordMessageService(_apiClient.Object, _timerService.Object);

        //Act
        await discordMessageService.HandleMessageLogicAsync(content, username, _socketMessageChannel.Object);

        //Assert
        _socketMessageChannel.Verify(c => c.SendMessageAsync("PONG", false, null, null, null, null, null, null, null, MessageFlags.None, null), Times.Once);
        _apiClient.Verify(c => c.GetUserByUsernameAsync(username), Times.Once);
    }

    [Fact]
    public async Task HandleMessageAsync_UserBlockedSendsPING_RecievesUserBlocked()
    {
        //Arrange
        var username = "Chi8899";
        var content = "PING";

        var botUserResponse = new BotUserResponse(
            Username: username,
            IsBlocked: true,
            Id: 1
        );
        _socketMessageChannel.Setup(c => c.SendMessageAsync($"{username} is blocked", false, null, null, null, null, null, null, null, MessageFlags.None, null));
        _apiClient.Setup(a => a.GetUserByUsernameAsync(username)).Returns(Task.FromResult<BotUserResponse?>(botUserResponse));

        //Setup
        var discordMessageService = new DiscordMessageService(_apiClient.Object, _timerService.Object);

        //Act
        await discordMessageService.HandleMessageLogicAsync(content, username, _socketMessageChannel.Object);

        //Assert
        _socketMessageChannel.Verify(c => c.SendMessageAsync($"{username} is blocked", false, null, null, null, null, null, null, null, MessageFlags.None, null), Times.Once);
        _apiClient.Verify(c => c.GetUserByUsernameAsync(username), Times.Once);
    }


    [Fact]
    public async Task HandleMessageAsync_SendsTIMER_CallsTimer()
    {
        //Arrange
        var username = "Chi8899";
        var content = "TIMER 1";

        var botUserResponse = new BotUserResponse(
            Username: username,
            IsBlocked: false,
            Id: 1
        );
        _timerService.Setup(t => t.StartTimerAsync(botUserResponse, 1, _socketMessageChannel.Object));
        _apiClient.Setup(a => a.GetUserByUsernameAsync(username)).Returns(Task.FromResult<BotUserResponse?>(botUserResponse));

        //Setup
        var discordMessageService = new DiscordMessageService(_apiClient.Object, _timerService.Object);

        //Act
        await discordMessageService.HandleMessageLogicAsync(content, username, _socketMessageChannel.Object);

        //Assert
        _timerService.Verify(c => c.StartTimerAsync(botUserResponse, 1, _socketMessageChannel.Object), Times.Once);
        _apiClient.Verify(c => c.GetUserByUsernameAsync(username), Times.Once);
    }


    [Fact]
    public async Task HandleMessageAsync_SendsTIMER_SkipsTimer()
    {
        //Arrange
        var username = "Chi8899";
        var content = "TIMER";

        var botUserResponse = new BotUserResponse(
            Username: username,
            IsBlocked: false,
            Id: 1
        );

        _apiClient.Setup(a => a.GetUserByUsernameAsync(username)).Returns(Task.FromResult<BotUserResponse?>(botUserResponse));

        //Setup
        var discordMessageService = new DiscordMessageService(_apiClient.Object, _timerService.Object);

        //Act
        await discordMessageService.HandleMessageLogicAsync(content, username, _socketMessageChannel.Object);

        //Assert
        _timerService.Verify(c => c.StartTimerAsync(botUserResponse, 1, _socketMessageChannel.Object), Times.Never);
        _apiClient.Verify(c => c.GetUserByUsernameAsync(username), Times.Once);
    }

    [Fact]
    public async Task StartTimerAsync_StartsTimer_SendsMessage()
    {
        //Arrange
        var username = "Chi8899";
        var minutes = 0;
        var botUserResponse = new BotUserResponse(
            Username: username,
            IsBlocked: false,
            Id: 1
        );

        var botUserTimerRequest = new BotUserTimerRequest(
             UserId: 1,
             StartTime: DateTime.UtcNow,
             Duration: 1
        );

        var botUserTimerResponse = new BotUserTimerResponse(
             UserId: 1,
             StartTime: DateTime.UtcNow,
             Duration: 1,
             Completed: false,
             Id: 1
        );

        _socketMessageChannel.Setup(c => c.SendMessageAsync(It.IsAny<string>(), false, null, null, null, null, null, null, null, MessageFlags.None, null))
             .ReturnsAsync((RestUserMessage?)null);

        _apiClient.Setup(a => a.CreateUserTimerAsync(botUserTimerRequest)).Returns(Task.FromResult<BotUserTimerResponse?>(botUserTimerResponse));
        var completedTimer = botUserTimerRequest with { Completed = true };
        _apiClient.Setup(a => a.UpdateUserTimerAsync(completedTimer)).Returns(Task.FromResult(true));

        //Setup
        var timerService = new TimerService(_apiClient.Object);

        //Act
        await timerService.StartTimerAsync(botUserResponse, minutes, _socketMessageChannel.Object);

        await Task.Delay(50);

        //Assert
        _socketMessageChannel.Verify(c => c.SendMessageAsync(It.IsAny<string>(), false, null, null, null, null, null, null, null, MessageFlags.None, null), Times.Exactly(2));
        _apiClient.Verify(a => a.CreateUserTimerAsync(It.IsAny<BotUserTimerRequest>()), Times.Once);
        _apiClient.Verify(a => a.UpdateUserTimerAsync(It.IsAny<BotUserTimerRequest>()), Times.Once);
    }

    [Fact]
    public async Task StartTimerAsync_ThrowsException_WhenUserNull()
    {
        var minutes = 0;

        //Setup
        var timerService = new TimerService(_apiClient.Object);

        //Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => timerService.StartTimerAsync(null, minutes, _socketMessageChannel.Object));
    }
}
