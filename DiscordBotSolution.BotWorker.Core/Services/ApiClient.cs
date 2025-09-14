using DiscordBotSolution.BotWorker.Core.DTOs;
using DiscordBotSolution.BotWorker.Core.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace DiscordBotSolution.BotWorker.Core.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<BotUserResponse?> CreateUserAsync(BotUserRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/bot/createuser", request);
        result.EnsureSuccessStatusCode();
        return await result.Content.ReadFromJsonAsync<BotUserResponse>();
    }

    public async Task<BotUserResponse?> GetUserByUsernameAsync(string username)
    {
        var result = await _httpClient.GetAsync($"api/bot/getuserbyusername/{username}");

        if (result.StatusCode == HttpStatusCode.NotFound)
            return null;

        result.EnsureSuccessStatusCode();
        return await result.Content.ReadFromJsonAsync<BotUserResponse>();
    }


    public async Task<BotUserTimerResponse?> CreateUserTimerAsync(BotUserTimerRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/bot/createusertimer", request);
        result.EnsureSuccessStatusCode();
        return await result.Content.ReadFromJsonAsync<BotUserTimerResponse>();
    }

    public async Task<bool> UpdateUserTimerAsync(BotUserTimerRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/bot/updateusertimer", request);
        result.EnsureSuccessStatusCode();
        return await result.Content.ReadFromJsonAsync<bool>();
    }
}
