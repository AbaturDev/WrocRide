namespace WrocRide.Client.Interfaces
{
    public interface IAddBearerTokenService
    {
        Task AddBearerToken(HttpClient httpClient);
    }
}
