using Microsoft.AspNetCore.Mvc;
using HospitalAppointmentSystem.Models;
using System.Text.Json;

namespace HospitalAppointmentSystem.Controllers
{
    // Handles requests for displaying external API posts.
    public class PostsController : Controller
    {
        // Creates an asynchronous action to retrieve posts.
        public async Task<IActionResult> Index()
        {
            // Creates an HTTP client to send the web request.
            using var client = new HttpClient();

            // Sends a GET request and waits for the API response.
            var response = await client.GetAsync(
                "https://jsonplaceholder.typicode.com/posts");

            // Checks whether the API request was successful.
            response.EnsureSuccessStatusCode();

            // Reads the response body as a string asynchronously.
            var json = await response.Content.ReadAsStringAsync();

            // Converts the JSON response into a list of Post objects.
            var posts = JsonSerializer.Deserialize<List<Post>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            // Sends the retrieved posts to the view.
            return View(posts ?? new List<Post>());
        }
    }
}