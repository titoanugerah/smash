using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suscribe_Management_System_Hehe.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            this._logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Dashboard Controller - Index");      
                throw;
            }
        }


    }
}
