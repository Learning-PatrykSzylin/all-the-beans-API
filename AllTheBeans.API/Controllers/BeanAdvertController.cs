using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Interfaces;
using AllTheBeans.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AllTheBeans.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeanAdvertController : ControllerBase
    {
        private readonly IBeanAdvertRepository _adRepo;

        public BeanAdvertController(IBeanAdvertRepository adRepo)
        {
            _adRepo = adRepo;
        }

        [HttpPost]
        public async Task<ActionResult> AddAdvert(BeanCreateAdvertDTO req)
        {
            await _adRepo.AddAdvert(req);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<BeanDTO>> GetAdvert(DateTime date)
        {
            var beanAd = await _adRepo.GetAdvert(date);

            if (beanAd == null)
                return NotFound(new { msg = $"No advert found for {date.Date}" });


            return Ok(beanAd);
        }

        [HttpDelete]
        public async Task<ActionResult> ClearAdverts()
        {
            await _adRepo.ClearAdverts();

            return NoContent();
        }
    }
}