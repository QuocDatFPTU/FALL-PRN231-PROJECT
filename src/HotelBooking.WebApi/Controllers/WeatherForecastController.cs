using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers;
[ApiController]
[Route("[action]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet]
    public IActionResult Get()
    {
        var data = System.IO.File.ReadAllText("./demo.json");

        return Ok(data);
    }
    [HttpGet]
    public IActionResult Get2()
    {
        var data = System.IO.File.ReadAllText("./wwwroot/countries.json");

        return Ok(data);
    }

    [HttpGet]
    public IActionResult Get3()
    {
        var data = System.IO.File.ReadAllText("./wwwroot/states.json");

        return Ok(data);
    }
}
