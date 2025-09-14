namespace DiscordBotSolution.BotWorker.Core.DTOs;

public record BotUserRequest(string Username, bool IsBlocked, int Id = 0);