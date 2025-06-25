using EsyaSoft.Adapter.API.EFModels;
using Microsoft.AspNetCore.Mvc;

namespace EsyaSoft.Adapter.API.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AdapterContext _dbContext;
        private readonly ILogger<RegisterController> _logger;
        private readonly ILogger<RegisterCreateConfirmationController> _confirmationLogger;

        private readonly string SectionHeader = "EndPoints";
        private readonly string key = "RegisterCreate";
        private static readonly HttpClient httpClient = new HttpClient();

        public RegisterController(ILogger<RegisterController> logger, AdapterContext dbContext,
            IConfiguration configuration, ILogger<RegisterCreateConfirmationController> confirmationLogger)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;

            _confirmationLogger = confirmationLogger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
