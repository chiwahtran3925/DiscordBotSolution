using Discord.WebSocket;
using DiscordBotSolution.BotWorker.Core.DTOs;
using DiscordBotSolution.BotWorker.Core.Interfaces;

namespace DiscordBotSolution.BotWorker.Core.Services;

public class DiscordMessageService: IDiscordMessageService
{
    private readonly IApiClient _apiClient;
    private readonly ITimerService _timerService;
    public DiscordMessageService(IApiClient apiClient, ITimerService timerService)
    {
        _apiClient = apiClient;
        _timerService = timerService;
    }

    public async Task HandleMessageLogicAsync(string content, string username, ISocketMessageChannel channel)
    {
        var user = await _apiClient.GetUserByUsernameAsync(username);
        if (user == null)
        {
            var userRequest = new BotUserRequest(Username: username, IsBlocked: false);
            user = await _apiClient.CreateUserAsync(userRequest);
        }
        if (user != null && user.IsBlocked)
        {
            await channel.SendMessageAsync($"{user.Username} is blocked");
        }
        else if (content == "PING")
        {
            await channel.SendMessageAsync("PONG");

        }
        else if (content.StartsWith("TIMER"))
        {
            var parts = content.Split(' ');
            if (parts.Length == 2)
            {
                int.TryParse(parts[1], out int minutes);
                await _timerService.StartTimerAsync(user, minutes, channel);
            }
        }
    }

    public async Task HandleMessageAsync(SocketMessage socketMessage)
    {
        if (socketMessage.Author.IsBot) return;

        var content = socketMessage.Content.ToUpper().Trim();
        var username = socketMessage.Author.Username;

        await HandleMessageLogicAsync(content, username, socketMessage.Channel);
    }
}
