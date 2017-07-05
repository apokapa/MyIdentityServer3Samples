using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Thinktecture.IdentityModel.Clients;

namespace MvcResourceOwnerClient.Controllers
{
    public class AccountController : Controller
    {
      

        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login(string username,string password)
        {

            var client = new OAuth2Client(new Uri("http://localhost:59248/connect/token"), "MvcResourceOwnerClient","secret");

            var requestResponse = client.RequestAccessTokenUserName(username,password,"openid profile offline_access read");

            var claims = new[]
            {
                new Claim("access_token", requestResponse.AccessToken),
                new Claim("refresh_token", requestResponse.RefreshToken),


            };

            var claimsIdentity = new ClaimsIdentity(claims,DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(claimsIdentity);

            return Redirect("/Home/Contact");
        }


        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();

            return Redirect("/");
        }

        //Just for test
        public ActionResult RefreshAccessToken()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            var client = new OAuth2Client(new Uri("http://localhost:59248/connect/token"),
                "MvcResourceOwnerClient", "secret");

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


    }
}