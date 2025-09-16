
using Microsoft.AspNetCore.Mvc;

namespace TransportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Hello from WeatherForecast API!" });
        }
    }
}
