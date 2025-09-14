namespace DiscordBotSolution.BotWorker.Core.DTOs;

public record BotUserTimerRequest(int UserId, int Duration, DateTime StartTime, bool Completed = false, int Id = 0);
