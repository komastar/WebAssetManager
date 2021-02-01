using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BigRichApiController : ControllerBase
    {
        [HttpGet]
        public string GetCard()
        {
            return "1";
        }

        [HttpPost]
        [Consumes("application/json")]
        public string SelectCard([FromBody] List<CardModel> card)
        {
            if (ModelState.IsValid)
            {

            }

            return "SUC";
        }

        [HttpPost]
        public string SelectCard2([FromForm] List<CardModel> card)
        {
            return "SUC";
        }
    }

    public enum ESuit
    {
        None = 0,
        Clover,
        Heart,
        Dia,
        Spade,
        Joker,
        Count
    }

    public class CardModel
    {
        public int Number { get; set; }
        public ESuit Suit { get; set; }
    }
}
