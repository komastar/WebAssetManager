using AssetWebManager.Data;
using AssetWebManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApiAssetBundleController> _logger;
        private IWebHostEnvironment _env;

        public GameController(ApplicationDbContext context, ILogger<ApiAssetBundleController> logger, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        #region RESTAPI
        // GET: api/Game/GetAll
        [HttpGet]
        public IEnumerable<GameModel> GetAll()
        {
            return _context.GameModel.ToList();
        }

        //  POST api/Game/Create/4
        [HttpPost("{maxUserCount}")]
        public GameModel Create(int maxUserCount)
        {
            var gameCode = MakeGameCode();
            GameModel newGame = new GameModel(gameCode, maxUserCount);

            AddOrUpdate(newGame);

            return newGame;
        }

        //  PUT api/Game/Join/c0de
        [HttpPut("{gamecode}")]
        public bool Join(string gamecode)
        {
            var game = FindGame(gamecode);
            if (null != game)
            {
                if (game.UserCount < game.MaxUserCount)
                {
                    game.UserCount++;
                    AddOrUpdate(game);

                    return true;
                }
            }

            return false;
        }

        // DELETE api/Game/c0de
        [HttpDelete("{gamecode}")]
        public void Delete(string gamecode)
        {
            Remove(gamecode);
        }
        #endregion

        private string MakeGameCode()
        {
            GameModel game;
            string newGameCode;
            do
            {
                var guid = Guid.NewGuid();
                newGameCode = guid.ToString().Substring(0, 4).ToLower();
                game = FindGame(newGameCode);
            } while (null != game);

            return newGameCode;
        }

        private GameModel FindGame(string gamecode)
        {
            return _context.Find<GameModel>(gamecode);
        }

        private void AddOrUpdate(GameModel newGame)
        {
            var find = _context.Find<GameModel>(newGame.GameCode);
            if (null == find)
            {
                _context.Add(newGame);
            }
            else
            {
                _context.Update(newGame);
            }
            _context.SaveChanges();
        }

        private void Remove(string gamecode)
        {
            var game = FindGame(gamecode);
            if (null != game)
            {
                _context.Remove(game);
            }
            _context.SaveChanges();
        }
    }
}
