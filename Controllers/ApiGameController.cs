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
    public class ApiGameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApiAssetBundleController> _logger;
        private IWebHostEnvironment _env;

        public ApiGameController(ApplicationDbContext context, ILogger<ApiAssetBundleController> logger, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        #region RESTAPI
        //  GET: api/ApiGame/Get
        [HttpGet("{gamecode}")]
        public ResponseModel Get(string gamecode)
        {
            return new ResponseModel(FindGame(gamecode));
        }

        // GET: api/ApiGame/GetAll
        [HttpGet]
        public IEnumerable<GameRoomModel> GetAll()
        {
            return _context.GameRoom.ToList();
        }

        //  GET: api/ApiGame/Create/4
        [HttpGet("{maxUserCount}")]
        public ResponseModel Create(int maxUserCount)
        {
            var gameCode = MakeGameCode();
            GameRoomModel newGame = new GameRoomModel(gameCode, maxUserCount);

            AddOrUpdate(newGame);

            return new ResponseModel(newGame);
        }

        //  GET: api/ApiGame/Join/c0de
        [HttpGet("{gamecode}")]
        public ResponseModel Join(string gamecode)
        {
            var game = FindGame(gamecode);
            if (null != game)
            {
                if (game.UserCount < game.MaxUserCount)
                {
                    game.UserCount++;
                    AddOrUpdate(game);

                    return new ResponseModel(true, game);
                }
            }

            return new ResponseModel(false);
        }

        // GET: api/ApiGame/Exit/c0de
        [HttpGet("{gamecode}")]
        public ResponseModel Exit(string gamecode)
        {
            var game = FindGame(gamecode);
            if (null == game)
            {
                return new ResponseModel(false);
            }

            game.UserCount--;
            if (0 < game.UserCount)
            {
                AddOrUpdate(game);

                return new ResponseModel(game);
            }
            else
            {
                return new ResponseModel(Remove(gamecode));
            }
        }

        //  GET: api/ApiGame/Round/c0de
        [HttpGet("{gamecode}/{time}")]
        public async Task<ResponseModel> Round(string gamecode, int time)
        {
            ResponseModel response = new ResponseModel(true);

            await Task.Delay(time);

            return response;
        }
        #endregion

        private string MakeGameCode()
        {
            GameRoomModel game;
            string newGameCode;
            do
            {
                var guid = Guid.NewGuid();
                newGameCode = guid.ToString().Substring(0, 4).ToLower();
                game = FindGame(newGameCode);
            } while (null != game);

            return newGameCode;
        }

        private GameRoomModel FindGame(string gamecode)
        {
            return _context.Find<GameRoomModel>(gamecode);
        }

        private void AddOrUpdate(GameRoomModel newGame)
        {
            var find = _context.Find<GameRoomModel>(newGame.GameCode);
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

        private bool Remove(string gamecode)
        {
            var game = FindGame(gamecode);
            if (null != game)
            {
                _context.Remove(game);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
