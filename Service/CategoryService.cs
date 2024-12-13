using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        private const string UrlCategory = "https://actbackendseervices.azurewebsites.net/api/categories";

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Category>> GetCategoryAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Category>>(UrlCategory);
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
