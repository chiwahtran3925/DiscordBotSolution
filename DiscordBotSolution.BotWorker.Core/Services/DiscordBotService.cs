using DiscordBotSolution.BotWorker.Core.Interfaces;
using Discord;
using Discord.WebSocket;
namespace DiscordBotSolution.BotWorker.Core.Services;

public class DiscordBotService : IDiscordBotService
{
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly DiscordMessageService _discordMessageHandler;
    private readonly string _discordToken;
    public DiscordBotService(string discordToken, DiscordMessageService discordMessageHandler)
    {
        _discordMessageHandler = discordMessageHandler;
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };
        _discordSocketClient = new DiscordSocketClient(config); _discordToken = discordToken;
    }
    public async Task StartAsync()
    {
        _discordSocketClient.MessageReceived += _discordMessageHandler.HandleMessageAsync;
        await _discordSocketClient.LoginAsync(TokenType.Bot, _discordToken);
        await _discordSocketClient.StartAsync();
        await Task.Delay(-1);
    }   
}
