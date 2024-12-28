using Microsoft.AspNetCore.Authorization;

namespace WrocRide.Authorization;

public class ActiveUserRequirement : IAuthorizationRequirement
{
    public ActiveUserRequirement()
    {
    }
}