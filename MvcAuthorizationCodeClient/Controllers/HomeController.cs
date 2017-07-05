using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Thinktecture.IdentityModel.Clients;

namespace MvcAuthorizationCodeClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //AuthorizationCode without owin middleware
            //if (User.Identity.IsAuthenticated)
            //{
            //    var claimsPrincipal = User as ClaimsPrincipal;
            //    return Content(claimsPrincipal.FindFirst("access_token").Value);
            //}

            //var url = "http://localhost:59248/connect/authorize" +
            //          "?client_id=MvcAuthorizationCodeClient" +
            //          "&redirect_uri=http://localhost:55082/Home/AuthorizationCallBack" +
            //          "&response_type=code" +
            //          "&scope=openid+profile" +
            //          "&respone_mode=form_post";


            //return Redirect(url);

            return View();
        }


        //Just for test
        public ActionResult RefreshAccessToken()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            var client = new OAuth2Client(new Uri("http://localhost:59248/connect/token"),
                "MvcAuthorizationCodeClient", "secret");

            var requestResponse = client.RequestAccessTokenRefreshToken(
                claimsPrincipal.FindFirst("refresh_token").Value);

            var manager = HttpContext.GetOwinContext().Authentication;

            var refreshedIdentity = new ClaimsIdentity(User.Identity);

            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("access_token"));
            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("refresh_token"));

            refreshedIdentity.AddClaim(new Claim("access_token",
                requestResponse.AccessToken));

            refreshedIdentity.AddClaim(new Claim("refresh_token",
                requestResponse.RefreshToken));

            manager.AuthenticationResponseGrant =
                new AuthenticationResponseGrant(new ClaimsPrincipal(refreshedIdentity),
                new AuthenticationProperties { IsPersistent = true });

            return Redirect("/Home/Contact");
        }

        //AuthorizationCode without owin middleware
        //public ActionResult AuthorizationCallBack(string code,string state,string error)
        //{
        //    var tokenUrl = "http://localhost:59248/connect/token";

        //    var client = new OAuth2Client(new Uri(tokenUrl), "MvcAuthorizationCodeClient","secret");

        //    var requestResult = client.RequestAccessTokenCode(code,
        //        new Uri("http://localhost:55082/Home/AuthorizationCallBack"));

        //    var claims = new[]
        //    {
        //        new Claim("access_token", requestResult.AccessToken)

        //    };

        //    var identity = new ClaimsIdentity(claims,DefaultAuthenticationTypes.ApplicationCookie);

        //    Request.GetOwinContext().Authentication.SignIn(identity);

        //    return Redirect("/");
        //}


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


        [Authorize]
        public ActionResult Logout()
        {
           Request.GetOwinContext().Authentication.SignOut();
           
           return Redirect("/");
        }
    }
}