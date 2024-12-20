using System.Net.Http.Headers;
using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        private const string UrlCategory = "https://actbackendseervices.azurewebsites.net/api/categories";

        public CategoryService(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<List<Category>> GetCategoryAsync()
        {
            try
            {
                // Set token before making the request
                if (_tokenService.IsAuthenticated)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", _tokenService.Token);
                }

                var response = await _httpClient.GetAsync(UrlCategory);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Session expired. Please login again.");
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Category>>();
            }
            catch (UnauthorizedAccessException)
            {
                _tokenService.Token = null; // Clear invalid token
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Category>($"{UrlCategory}/{id}");
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var main = await _httpClient.PostAsJsonAsync(UrlCategory, category);
            main.EnsureSuccessStatusCode();
            return await main.Content.ReadFromJsonAsync<Category>();
        }

        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            try
            {
                var main = await _httpClient.PutAsJsonAsync($"{UrlCategory}/{id}", category);
                main.EnsureSuccessStatusCode();
                return await main.Content.ReadFromJsonAsync<Category>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error updating category in API.", ex);
            }
        }

        public async Task<Category> DeleteCategoryAsync(int id)
        {
            try
            {
                var main = await _httpClient.DeleteAsync($"{UrlCategory}/{id}");
                main.EnsureSuccessStatusCode();
                return await main.Content.ReadFromJsonAsync<Category>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error deleting category with ID {id} from API.", ex);
            }
        }
    }
}
