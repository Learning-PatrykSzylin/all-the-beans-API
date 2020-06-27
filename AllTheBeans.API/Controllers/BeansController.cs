using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Interfaces;
using AllTheBeans.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AllTheBeans.API.Controllers
{
    // Inherit from ControllerBase as we don't need to support Views()
    [Route("api/[controller]")]
    [ApiController]
    public class BeansController : ControllerBase
    {
        private readonly IBeanRepository _beanRepo;

        public BeansController(IBeanRepository beanRepo)
        {
            _beanRepo = beanRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<BeanDTO>>> GetAllBeans()
        {
            return Ok(await _beanRepo.GetAllBeans());
        }

        [HttpGet("{id}", Name ="GetBeanById")]
        public async Task<ActionResult<BeanDTO>> GetBeanById(int id)
        {
            return Ok(await _beanRepo.GetBeanById(id));
        }

        [HttpPost]
        public async Task<ActionResult> AddBeanToDb(BeanCreateDTO bean)
        {
            // TODO: Add check to see if table exists before pushing into it
            //       Create table and push if not
            _beanRepo.AddBean(bean);

            // TODO: Return CreatedAtRoute to the client
            // This way, the client will receive an URL to the resource they just created for validation
            return NoContent();
        }


    }
}