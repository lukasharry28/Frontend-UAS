using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazeUTS.Service
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly TokenService _tokenService;

    public CustomAuthStateProvider(TokenService tokenService)
    {
        _tokenService = tokenService;
        _tokenService.OnTokenChanged += AuthenticationStateChanged;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        
        if (_tokenService.IsAuthenticated)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "user"),
            }, "Bearer");
        }

        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
    }

    private void AuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
}