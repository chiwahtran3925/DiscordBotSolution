
using DiscordBotSolution.BotApi.Core.DTOs;
using DiscordBotSolution.BotApi.Core.Interfaces;
using DiscordBotSolution.BotApi.Core.Models;

namespace DiscordBotSolution.BotApi.Core.Services;

public class BotService : IBotService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserTimerRepository _userTimerRepository;
    public BotService(IUserRepository userRepository, IUserTimerRepository userTimerRepository)
    {
        _userRepository = userRepository;
        _userTimerRepository = userTimerRepository;
    }

    public async Task<bool> Login(LoginRequest loginRequest)
    {
        if(loginRequest.Username=="Admin" && loginRequest.Password == "Password")
            return await Task.FromResult(true);
        else
            return await Task.FromResult(false);
    }


    public async Task<UserResponse> CreateUser(UserRequest userRequest)
    {
        var userExists = await _userRepository.GetByUsernameAsync(userRequest.Username);
        if (userExists != null)
            throw new InvalidOperationException("User already exists");

        var user = new User()
        {
            Username = userRequest.Username,
            IsBlocked = userRequest.IsBlocked
        };

        await _userRepository.AddAsync(user);

        return new UserResponse(
            Id: user.Id,
            Username : user.Username,
            IsBlocked : user.IsBlocked
            );
    }

    public async Task<bool> UpdateUser(UserRequest userRequest)
    {
        var user = await _userRepository.GetByUsernameAsync(userRequest.Username);
        if (user == null)
            throw new InvalidOperationException("User doesnt exists");

        user.Username = userRequest.Username;
        user.IsBlocked = userRequest.IsBlocked;

        return await _userRepository.UpdateAsync(user);
    }

    public async Task<UserResponse?> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
            return null;

        return new UserResponse( 
            Id: user.Id,
            Username: user.Username,
            IsBlocked: user.IsBlocked);
    }

    public async Task<List<UserResponse>> GetAllUser()
    {
        var users = await _userRepository.GetAllAsync() ?? Enumerable.Empty<User>(); 

        return users.Select(x => new UserResponse(x.Id, x.Username, x.IsBlocked)).ToList();
    }



    public async Task<UserTimerResponse> CreateUserTimer(UserTimerRequest userTimerRequest)
    {
        var userTime = new UserTimer()
        {
            UserId = userTimerRequest.UserId,
            Completed = false,
            Duration = userTimerRequest.Duration,
            StartTime = userTimerRequest.StartTime
        };

        await _userTimerRepository.AddAsync(userTime);

        return new UserTimerResponse(
            Id: userTime.Id,
            UserId: userTime.UserId,
            Completed: userTime.Completed,
            Duration: userTime.Duration,
            StartTime: userTime.StartTime

            );
    }

    public async Task<bool> UpdateUserTimer(UserTimerRequest userTimerRequest)
    {
        var userTimer = await _userTimerRepository.GetByIdAsync(userTimerRequest.Id);
        if (userTimer == null)
            throw new InvalidOperationException("User Timer doesnt exists");

        userTimer.Duration = userTimerRequest.Duration;
        userTimer.UserId = userTimerRequest.UserId;
        userTimer.Completed = userTimerRequest.Completed;
        userTimer.StartTime = userTimerRequest.StartTime;

        return await _userTimerRepository.UpdateAsync(userTimer);
    }
}
