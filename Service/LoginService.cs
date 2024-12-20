using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        private const string UrlLogin = "https://actbackendseervices.azurewebsites.net/api/login";

        public LoginService(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<UserWithTokenDTO> LoginAsync(UserLoginDTO login)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(UrlLogin, login);
                response.EnsureSuccessStatusCode();

                var userWithToken = await response.Content.ReadFromJsonAsync<UserWithTokenDTO>();
                if (userWithToken?.Token != null)
                {
                    _tokenService.Token = userWithToken.Token;
                    return userWithToken;
                }
                throw new Exception("Invalid response from server");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                throw;
            }
        }

        public void Logout()
        {
            _tokenService.Token = null;
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
