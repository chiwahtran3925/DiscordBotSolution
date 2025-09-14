using DiscordBotSolution.BotApi.Core.DTOs;

namespace DiscordBotSolution.BotApi.Core.Interfaces;

public interface IBotService
{
    public Task<bool> Login(LoginRequest loginRequest);
    public Task<UserResponse> CreateUser(UserRequest userRequest);
    public Task<bool> UpdateUser(UserRequest userRequest);
    public Task<UserResponse?> GetUserByUsername(string username);
    public Task<List<UserResponse>> GetAllUser();
    public Task<UserTimerResponse> CreateUserTimer(UserTimerRequest userTimerRequest);
    public Task<bool> UpdateUserTimer(UserTimerRequest userTimerRequest);
}
