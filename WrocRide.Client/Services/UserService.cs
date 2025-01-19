using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared.DTOs.User;

namespace WrocRide.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IAddBearerTokenService _addBearerTokenService;

        public UserService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
        {
            _httpClient = httpClient;
            _addBearerTokenService = addBearerTokenService;
        }

        public async Task AddCredits(AddCreditsDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);

            await _httpClient.PutAsJsonAsync<AddCreditsDto>("api/me/balance", dto);
        }

        public async Task DeactivateAccount()
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);

            //await _httpClient.PutAsync("api/me/deactivate-account");
        }

        public async Task<UserDto> GetUser()
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);

            var result = await _httpClient.GetFromJsonAsync<UserDto>("api/me");

            if (result == null)
            {
                throw new Exception("Not found");
            }

            return result;
        }

        public async Task UpdateProfile(UpdateUserDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);

            await _httpClient.PutAsJsonAsync<UpdateUserDto>("api/me", dto);
        }
    }
}
