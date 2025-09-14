using Discord.WebSocket;

namespace DiscordBotSolution.BotWorker.Core.Interfaces;

public interface IDiscordMessageService
{
    public Task HandleMessageAsync(SocketMessage socketMessage);
    public Task HandleMessageLogicAsync(string content, string username, ISocketMessageChannel channel);

}
