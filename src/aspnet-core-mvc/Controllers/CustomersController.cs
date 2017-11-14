using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace de.playground.aspnet.core.mvc.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Customers"] = new string [] 
            {
                "Customer1",
                "Customer2",
                "Customer3",
                "Customer4"
            };

            return View();
        }
    }
}
