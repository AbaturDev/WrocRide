using Blazored.LocalStorage;
using System.Net.Http.Headers;
using WrocRide.Client.Interfaces;

namespace WrocRide.Client.Services
{
    public class AddBearerTokenService : IAddBearerTokenService
    {
        private readonly ILocalStorageService _localStorageService;
        public AddBearerTokenService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public async Task AddBearerToken(HttpClient httpClient)
        {
            if(await _localStorageService.ContainKeyAsync("token"))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsync<string>("token"));
            }
        }
    }
}
