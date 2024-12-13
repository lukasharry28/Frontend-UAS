using BlazeUTS.Models;

namespace BlazeUTS.Service
{
    public class InstructorService
    {
        private readonly HttpClient _httpClient;

        private const string UrlInstructor = "https://actbackendseervices.azurewebsites.net/api/instructors";

        public InstructorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Instructor>> GetInstructorAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Instructor>>(UrlInstructor);
        }
    }
}
