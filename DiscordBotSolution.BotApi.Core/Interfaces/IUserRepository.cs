using DiscordBotSolution.BotApi.Core.Models;

namespace DiscordBotSolution.BotApi.Core.Interfaces;

public interface IUserRepository
{
    public Task AddAsync(User user);

    public Task<IEnumerable<User>> GetAllAsync();

    public Task<User?> GetByUsernameAsync(string username);

    public Task<bool> UpdateAsync(User user);

}
