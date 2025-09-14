using Microsoft.EntityFrameworkCore;
using DiscordBotSolution.BotApi.Core.Interfaces;
using DiscordBotSolution.BotApi.Core.Models;

namespace DiscordBotSolution.BotApi.Core.Repositories;
public class UserRepository : IUserRepository
{
    BotDbContext _dbContext { get; set; }

    public UserRepository(BotDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users.Where(x=>x.Username==username).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var existingUser = await GetByUsernameAsync(user.Username);
        if (existingUser == null)
        {
            return false;
        }
        else {
            existingUser.Username = user.Username;
            existingUser.IsBlocked = user.IsBlocked;
            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
