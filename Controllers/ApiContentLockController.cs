using AssetWebManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiContentLockController : ControllerBase
    {
        [HttpGet("{version}/{content}")]
        public ResponseModel CheckContent(string version, string content)
        {
            ResponseModel response = new ResponseModel();

            return response;
        }
    }
}
