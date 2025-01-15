using Microsoft.JSInterop;
using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared.DTOs.Account;

namespace WrocRide.Client.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jSRuntime;

        public AccountService(HttpClient httpClient, IJSRuntime jSRuntime)
        {
            _httpClient = httpClient;
            _jSRuntime = jSRuntime;
        }

        public async Task<string> Login(LoginUserDto dto)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/user/login", dto);

            if(!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Invalid login");
            }

            var token = await responseMessage.Content.ReadAsStringAsync();

            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);

            return token;
        }

        public async Task Logout()
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }

        public async Task Register(RegisterUserDto dto)
        {
            await _httpClient.PostAsJsonAsync("api/user/register", dto);
        }
        
        public async Task RegisterDriver(RegisterDriverDto dto)
        {
            await _httpClient.PostAsJsonAsync("api/user/register-driver", dto);
        }
    }
}
