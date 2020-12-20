using AssetWebManager.Data;
using AssetWebManager.Models;
using AssetWebManager.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetWebManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiGameController : ControllerBase
    {
        private readonly GameRepository gameRepo;

        public ApiGameController(ApplicationDbContext context)
        {
            gameRepo = new GameRepository(context);
        }

        #region RESTAPI
        //  GET: api/ApiGame/Get
        [HttpGet("{gamecode}")]
        public ResponseModel Get(string gamecode)
        {
            return new ResponseModel(gameRepo.FindGameRoom(gamecode));
        }

        // GET: api/ApiGame/GetAll
        [HttpGet]
        public IEnumerable<GameRoomModel> GetAll()
        {
            return gameRepo.GetAllGameRoom();
        }

        //  GET: api/ApiGame/Create/4
        [HttpGet("{maxUserCount}")]
        public ResponseModel Create(int maxUserCount)
        {
            var gameRoom = gameRepo.CreateGameRoom(maxUserCount);

            return new ResponseModel(gameRoom);
        }

        //  GET: api/ApiGame/Start/c0de
        [HttpGet("{gamecode}")]
        public ResponseModel Start(string gamecode)
        {
            var gameRoom = gameRepo.StartGame(gamecode);
            if (null != gameRoom)
            {
                return new ResponseModel(gameRoom);
            }

            return new ResponseModel(false);
        }

        //  GET: api/ApiGame/Join/c0de
        [HttpGet("{gamecode}")]
        public ResponseModel Join(string gamecode)
        {
            var gameRoom = gameRepo.JoinGameRoom(gamecode);
            if (null != gameRoom)
            {
                return new ResponseModel(true, gameRoom);
            }

            return new ResponseModel(false);
        }

        // GET: api/ApiGame/Exit/c0de
        [HttpGet("{gamecode}/{userid}")]
        public ResponseModel Exit(string gamecode, string userid)
        {
            var gameRoom = gameRepo.ExitGameRoom(gamecode, userid);
            if (null == gameRoom)
            {
                return new ResponseModel(false);
            }
            else
            {
                return new ResponseModel(gameRoom);
            }
        }

        //  GET: api/ApiGame/Delete/c0de
        [HttpGet("{gamecode}")]
        public bool Delete(string gamecode)
        {
            var gameRoom = gameRepo.FindGameRoom(gamecode);
            if (null != gameRoom)
            {
                gameRepo.DeleteGameRoom(gameRoom);

                return true;
            }
            else
            {
                return false;
            }
        }

        static object _lock = new object();
        static Dictionary<string, Stack<int>> game = new Dictionary<string, Stack<int>>();

        //  GET: api/ApiGame/Round/c0de
        [HttpGet("{gamecode}/{round}")]
        public async Task<bool> Round(string gamecode, int round)
        {
            if (0 == round)
            {
                return false;
            }

            await RoundAsync(gamecode, round);

            return true;
        }

        private async Task RoundAsync(string gamecode, int round)
        {
            lock (_lock)
            {
                if (false == game.ContainsKey(gamecode))
                {
                    game.Add(gamecode, new Stack<int>());
                }

                if (round > game[gamecode].Count)
                {
                    game[gamecode].Push(0);
                }

                int userRound = game[gamecode].Pop();
                userRound++;
                game[gamecode].Push(userRound);
            }

            var gameRoom = gameRepo.FindGameRoom(gamecode);
            while (gameRoom.MaxUserCount > game[gamecode].Peek())
            {
                await Task.Yield();
            }
        }
        #endregion
    }
}
