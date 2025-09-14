namespace DiscordBotSolution.BotApi.Core.Models;

public class UserTimer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Duration { get; set; }
    public DateTime StartTime { get; set; }
    public bool Completed { get; set; }
}
