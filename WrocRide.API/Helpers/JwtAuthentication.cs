namespace WrocRide.API.Helpers
{
    public class JwtAuthentication
    {
        public required string Key { get; set; }
        public required int Expires { get; set; }
        public required string Issuer { get; set; }
    }
}
