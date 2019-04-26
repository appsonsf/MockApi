using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MdmDataDistributeApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MdmDataDistributeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public ContactsController(
            IMemoryCache cache
            )
        {
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ContactInfo>> GetByIdCardNoAsync([Required]string idCardNo)
        {
            var found = _cache.TryGetValue(idCardNo.Trim(), out ContactInfo result);
            if (!found) return NotFound();
            return result; ;
        }

        [HttpPost]
        public async Task<IActionResult> AddTempContact(ContactInfo model)
        {
            if (model.Id == Guid.Empty) model.Id = Guid.NewGuid();
            if (model.DepartmentId == Guid.Empty) model.DepartmentId = Guid.NewGuid();

            _cache.Set(model.IdCardNo, model, TimeSpan.FromMinutes(5));
            return CreatedAtAction("GetByIdCardNoAsync", model.IdCardNo);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContactInfo>>> GetAllAsync(bool onlyHasUser = true)
        {
            return new List<ContactInfo>();
        }
    }
}
