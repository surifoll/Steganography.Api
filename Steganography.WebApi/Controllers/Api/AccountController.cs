using System.Web.Http;

namespace Steganography.WebApi.Controllers.Api
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        [Route("str")]
        public string GetString()
        {
            return "gjvuyihlgkuyg";
        }

        [Route("acbbb")]
        public string GetApi(string abc)
        {
            return "gjvuyihlgkuyg";
        }
    }
}