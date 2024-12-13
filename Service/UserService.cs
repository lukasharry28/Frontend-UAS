using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        private const string UrlUser = "https://actbackendseervices.azurewebsites.net/api/users";
        private const string UrlRole = "https://actbackendseervices.azurewebsites.net/api/roles";
        private const string UrlUserRole = "https://actbackendseervices.azurewebsites.net/api/registeruserrole";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDTO>> GetUserAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDTO>>(UrlUser);
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UserDTO>($"{UrlUser}/{id}");
        }

        public async Task<UserAddDTO> AddUserAsync(UserAddDTO user)
        {
            var main = await _httpClient.PostAsJsonAsync(UrlUser, user);
            main.EnsureSuccessStatusCode();
            return await main.Content.ReadFromJsonAsync<UserAddDTO>();
        }

        public async Task<RoleDTO> GetRoleByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<RoleDTO>($"{UrlRole}/{id}");
        }

        public async Task<RoleAddDTO> AddRoleAsync(RoleAddDTO role)
        {
            var main = await _httpClient.PostAsJsonAsync(UrlRole, role);
            main.EnsureSuccessStatusCode();
            return await main.Content.ReadFromJsonAsync<RoleAddDTO>();
        }

        public async Task<RegisterUserRoleDto> AddUserRoleAsync(RegisterUserRoleDto userRole)
        {
            var main = await _httpClient.PostAsJsonAsync(UrlUserRole, userRole);
            main.EnsureSuccessStatusCode();
            return await main.Content.ReadFromJsonAsync<RegisterUserRoleDto>();
        }
    }
}
