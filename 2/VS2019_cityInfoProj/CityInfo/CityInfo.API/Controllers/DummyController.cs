using CityInfo.API.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/testdatabase")]
    public class DummyController : ControllerBase
    {
        private readonly CityInfoContext _ctx;
        //ensure it has access to city info context with constructor injection
        //city info context constructor is hit here and sent into our dummy controller.
        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }
        
        [HttpGet]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
