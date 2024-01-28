using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Shop.Web
{
    public class SecurityAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Cookies.Keys.Contains("Security"))
            {
                var isAuthorized = CheckAuthorizationFromExternalAPI(context.HttpContext.Request.Cookies["Security"].ToString());
                if (!isAuthorized)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
                context.Result = new UnauthorizedResult(); // veya RedirectToAction gibi başka bir yanıt
        }

        private bool CheckAuthorizationFromExternalAPI(string cookie)
        {
            var handler = new HttpClientHandler();
            var cookieContainer = new CookieContainer();
            handler.CookieContainer = cookieContainer;
            var uri = new Uri("https://localhost:44350/WeatherForecast"); // Cookie'nin ilişkilendirileceği URL
            cookieContainer.Add(uri, new Cookie(".AspNetCore.Identity.Application", cookie)); // Cookie adı ve değeri

            using (var client = new HttpClient(handler))
            {
                var response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
