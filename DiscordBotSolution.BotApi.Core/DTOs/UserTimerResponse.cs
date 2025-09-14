namespace DiscordBotSolution.BotApi.Core.DTOs;

public record UserTimerResponse(int Id, int UserId, int Duration, DateTime StartTime, bool Completed);
