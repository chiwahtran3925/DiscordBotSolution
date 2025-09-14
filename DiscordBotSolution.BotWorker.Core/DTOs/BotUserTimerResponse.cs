namespace DiscordBotSolution.BotWorker.Core.DTOs;

public record BotUserTimerResponse(int Id, int UserId, int Duration, DateTime StartTime, bool Completed);
