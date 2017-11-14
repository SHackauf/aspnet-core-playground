using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_mvc.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return HtmlEncoder.Default.Encode("HomeController");
        }

        public string Action()
        {
            return HtmlEncoder.Default.Encode("HomeController - Action");
        }
    }
}