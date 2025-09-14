using Microsoft.EntityFrameworkCore;
using DiscordBotSolution.BotApi.Core.Interfaces;
using DiscordBotSolution.BotApi.Core.Models;

namespace DiscordBotSolution.BotApi.Core.Repositories;
public class UserTimerRepository : IUserTimerRepository
{
    BotDbContext _dbContext { get; set; }

    public UserTimerRepository(BotDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task AddAsync(UserTimer userTimer)
    {
        await _dbContext.UserTimers.AddAsync(userTimer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserTimer?> GetByIdAsync(int id)
    {
        return await _dbContext.UserTimers.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(UserTimer userTimer)
    {
        var existingUserTimer = await GetByIdAsync(userTimer.Id);
        if (existingUserTimer == null)
        {
            return false;
        }
        else {
            userTimer.Duration = userTimer.Duration;
            userTimer.Completed = userTimer.Completed;
            userTimer.StartTime = userTimer.StartTime;
            userTimer.UserId = userTimer.UserId;
            _dbContext.UserTimers.Update(userTimer);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
