using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class EnrollmentService
    {
        private readonly HttpClient _httpClient;

        private const string UrlEnrollment = "https://actbackendseervices.azurewebsites.net/api/enrollments";

        public EnrollmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Enrollment>> GetEnrollmentAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Enrollment>>(UrlEnrollment);
        }

        public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
        {
            var main = await _httpClient.PostAsJsonAsync(UrlEnrollment, enrollment);
            main.EnsureSuccessStatusCode();
            return await main.Content.ReadFromJsonAsync<Enrollment>();
        }
    }
}
