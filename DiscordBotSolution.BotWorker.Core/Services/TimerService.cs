using Discord.WebSocket;
using DiscordBotSolution.BotWorker.Core.DTOs;
using DiscordBotSolution.BotWorker.Core.Interfaces;

namespace DiscordBotSolution.BotWorker.Core.Services;

public class TimerService : ITimerService
{
    private readonly IApiClient _apiClient;
    public TimerService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task StartTimerAsync(BotUserResponse? user, int minutes, ISocketMessageChannel channel)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        await channel.SendMessageAsync($"{user.Username} set timer for {minutes}");

        var botTimerRequest = new BotUserTimerRequest(
                UserId: user.Id,
                Duration: minutes,
                StartTime: DateTime.UtcNow
                );
        await _apiClient.CreateUserTimerAsync(botTimerRequest);
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(minutes));
                await channel.SendMessageAsync($"{user.Username} your time is up!");
                var completedTimer = botTimerRequest with { Completed = true };
                await _apiClient.UpdateUserTimerAsync(completedTimer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Timer Failed{ex}");
            }
        });
    }
}
