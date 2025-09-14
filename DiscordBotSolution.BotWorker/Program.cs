using DiscordBotSolution.BotWorker.Core.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var baseUrl = "https://localhost:7091/";
        var discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN")??"";
        var apiClient = new ApiClient(httpClient, baseUrl);
        var timerService = new TimerService(apiClient);
        var discordMessageHandler = new DiscordMessageService(apiClient, timerService);
        var discordBotService = new DiscordBotService(discordToken, discordMessageHandler);
        await discordBotService.StartAsync();
    }
}