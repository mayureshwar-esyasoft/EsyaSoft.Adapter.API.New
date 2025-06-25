using EsyaSoft.Adapter.API.EFModels;
using Microsoft.AspNetCore.Mvc;

namespace EsyaSoft.Adapter.API.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly AdapterContext _dbContext;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            AdapterContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<dynamic> Get()
        {
            return _dbContext.ServiceCallLogs.ToList();
        }


       
    }
}
