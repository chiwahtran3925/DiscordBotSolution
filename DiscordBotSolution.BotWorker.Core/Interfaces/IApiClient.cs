using DiscordBotSolution.BotWorker.Core.DTOs;

namespace DiscordBotSolution.BotWorker.Core.Interfaces;

public interface IApiClient
{
    Task<BotUserResponse?> CreateUserAsync(BotUserRequest request);
    Task<BotUserResponse?> GetUserByUsernameAsync(string username);
    Task<BotUserTimerResponse?> CreateUserTimerAsync(BotUserTimerRequest request);
    Task<bool> UpdateUserTimerAsync(BotUserTimerRequest request);
}