using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class CourseService
    {
        private readonly HttpClient _httpClient;

        private const string UrlCourse = "https://actbackendseervices.azurewebsites.net/api/courses";

        public CourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Course>> GetCourseAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Course>>(UrlCourse);
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Course>($"{UrlCourse}/{id}");
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            var main = await _httpClient.PostAsJsonAsync(UrlCourse, course);
            main.EnsureSuccessStatusCode();
            return await main.Content.ReadFromJsonAsync<Course>();
        }

        public async Task<Course> UpdateCourseAsync(int id, Course course)
        {
            try
            {
                var main = await _httpClient.PutAsJsonAsync($"{UrlCourse}/{id}", course);
                main.EnsureSuccessStatusCode();
                return await main.Content.ReadFromJsonAsync<Course>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error updating category in API.", ex);
            }
        }

        public async Task<Course> DeleteCourseAsync(int id)
        {
            try
            {
                var main = await _httpClient.DeleteAsync($"{UrlCourse}/{id}");
                main.EnsureSuccessStatusCode();
                return await main.Content.ReadFromJsonAsync<Course>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error deleting category with ID {id} from API.", ex);
            }
        }
    }
}
