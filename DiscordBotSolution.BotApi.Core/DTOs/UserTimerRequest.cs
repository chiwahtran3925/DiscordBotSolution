namespace DiscordBotSolution.BotApi.Core.DTOs;

public record UserTimerRequest(int UserId, int Duration, DateTime StartTime, bool Completed = false, int Id = 0);
