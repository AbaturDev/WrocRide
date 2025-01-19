using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared.DTOs.Account;

namespace WrocRide.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider; 

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> Login(LoginUserDto dto)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/user/login", dto);

            if(!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Invalid login");
            }


            var token = await responseMessage.Content.ReadAsStringAsync();

            await _localStorageService.SetItemAsync("token", token);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserAuthenticated(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return token;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("token");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUserLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
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
