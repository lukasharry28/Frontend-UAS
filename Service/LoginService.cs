using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        private const string UrlLogin = "https://actbackendseervices.azurewebsites.net/api/login";

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserWithTokenDTO> LoginAsync(UserLoginDTO login)
        {
            try
            {
                // Kirim POST request ke API
                var response = await _httpClient.PostAsJsonAsync(UrlLogin, login);

                // Pastikan respons sukses
                response.EnsureSuccessStatusCode();

                // Baca respons JSON ke UserWithTokenDTO
                var userWithToken = await response.Content.ReadFromJsonAsync<UserWithTokenDTO>();

                if (userWithToken == null)
                {
                    throw new Exception("Respons API kosong atau tidak sesuai.");
                }

                return userWithToken;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
                throw; // Opsional: Anda bisa menangani ulang atau melempar error kembali
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw;
            }
        }
    }
}
