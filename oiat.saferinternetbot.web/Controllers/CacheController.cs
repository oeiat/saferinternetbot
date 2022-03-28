using System.Web.Mvc;
using mbit.common.cache;

namespace oiat.saferinternetbot.web.Controllers
{
    public class CacheController : BaseController
    {
        private readonly ICacheService _cache;

        public CacheController(ICacheService cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public ActionResult Invalidate(string redirectUrl)
        {
            _cache.InvalidateAll();
            PushSuccess("Cache leeren", "Cache wurde erfolgreich geleert");
            return Redirect(redirectUrl);
        }
    }
}