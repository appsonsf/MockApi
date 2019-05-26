using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace SmsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileCodeController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public MobileCodeController(
            IMemoryCache cache
            )
        {
            _cache = cache;
        }

        [HttpGet]
        public ActionResult<string> GetMobileCode(string mobile)
        {
            if (_cache.TryGetValue(mobile, out string value))
                return value;
            return NotFound();
        }
    }
}