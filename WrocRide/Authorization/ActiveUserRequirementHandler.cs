using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WrocRide.Services;

namespace WrocRide.Authorization;

public class ActiveUserRequirementHandler : AuthorizationHandler<ActiveUserRequirement>
{
    private readonly IUserContextService _userContextService;
    
    public ActiveUserRequirementHandler(IUserContextService userContextService)
    {
        _userContextService = userContextService;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
    {
        var isActiveClaim = _userContextService.User.FindFirst("IsActive");
        if (isActiveClaim == null)
        {
            return Task.CompletedTask;
        }

        var isActive = bool.Parse(isActiveClaim.Value);
        if (isActive)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}