using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V2.Controllers
{

    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    [ApiVersion("2.0")]

    public class WordController : ControllerBase
    {
        [HttpGet("", Name = "GetAll")]
        public string GetWord()
        {
            return "Versão 2.0";
        }
    }
}
