namespace DiscordBotSolution.BotApi.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public bool IsBlocked { get; set; }

}
