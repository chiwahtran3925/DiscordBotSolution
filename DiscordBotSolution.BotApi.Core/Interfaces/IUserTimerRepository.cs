using DiscordBotSolution.BotApi.Core.Models;

namespace DiscordBotSolution.BotApi.Core.Interfaces;

public interface IUserTimerRepository
{
    public Task AddAsync(UserTimer userTimer);

    public Task<UserTimer?> GetByIdAsync(int id);

    public Task<bool> UpdateAsync(UserTimer userTimer);

}
