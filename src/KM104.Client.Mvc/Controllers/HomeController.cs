using KM104.Client.Mvc.Models;
using KM104.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KM104.Client.Mvc.Controllers
{
    public class HomeController(ILogger<HomeController> logger,
        IHttpClientFactory httpClientFactory) : Controller
    {
        private HttpClient ApiClient => httpClientFactory.CreateClient("api");
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherForecast(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(500, cancellationToken);
                var response = await ApiClient.GetAsync("/weatherforecast", cancellationToken);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>(cancellationToken: cancellationToken);
                return Json(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while making an HTTP request to the REST API");
                return Json(Array.Empty<WeatherForecast>());
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
