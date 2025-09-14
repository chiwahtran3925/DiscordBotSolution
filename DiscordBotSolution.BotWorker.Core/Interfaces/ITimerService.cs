using Discord.WebSocket;
using DiscordBotSolution.BotWorker.Core.DTOs;

namespace DiscordBotSolution.BotWorker.Core.Interfaces;
public interface ITimerService
{
    public Task StartTimerAsync(BotUserResponse? user, int minutes, ISocketMessageChannel channel);

}
