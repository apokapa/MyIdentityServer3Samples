using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Clients;

namespace MvcResourceOwnerClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


      
        [Authorize]
        public async Task<ActionResult> Contact()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer",
                    claimsPrincipal.FindFirst("access_token").Value);

                var values = await client.GetAsync("http://localhost:63357/api/values");

                ViewBag.Message = values.ToString();
   
            }


          

            return View(claimsPrincipal.Claims);
        }


       
    }
}