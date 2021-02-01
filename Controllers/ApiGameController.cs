using AssetWebManager.Data;
using AssetWebManager.Models;
using AssetWebManager.Repository;
using Microsoft.AspNetCore.Mvc;
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
        private readonly GameRepository gameRepo;
        private ApplicationDbContext context;

        public ApiGameController(ApplicationDbContext context)
        {
            gameRepo = new GameRepository(context);
            this.context = context;
        }

        #region RESTAPI
        [HttpGet("{gamecode}")]
        public GameRoomModel Find(string gamecode)
        {
            return gameRepo.FindGameRoom(gamecode);
        }

        [HttpGet]
        public IEnumerable<GameRoomModel> GetAll()
        {
            return gameRepo.GetAllGameRoom();
        }

        [HttpGet("{maxUserCount}")]
        public GameRoomModel Create(int maxUserCount)
        {
            return gameRepo.CreateGameRoom(maxUserCount);
        }

        [HttpGet("{username}")]
        public string NewUser(string username)
        {
            string userId = gameRepo.FindGameUserByNameAsync(username);
            if (null == userId)
            {
                return gameRepo.CreateGameUser(username);
            }
            else
            {
                return "";
            }
        }

        [HttpGet("{gamecode}")]
        public GameRoomModel Start(string gamecode)
        {
            return gameRepo.StartGame(gamecode);
        }

        [HttpGet("{gamecode}")]
        public GameRoomModel Join(string gamecode)
        {
            return gameRepo.JoinGameRoom(gamecode);
        }

        [HttpGet("{gamecode}/{userid}")]
        public GameRoomModel Exit(string gamecode, string userid)
        {
            return gameRepo.ExitGameRoom(gamecode, userid);
        }

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

        static object endLock = new object();
        static Dictionary<string, List<GameResultModel>> gameResult = new Dictionary<string, List<GameResultModel>>();
        [HttpGet("{gamecode}/{userid}/{score}")]
        public async Task<List<GameResultModel>> End(string gamecode, string userid, int score)
        {
            lock (endLock)
            {
                if (false == gameResult.ContainsKey(gamecode))
                {
                    gameResult.Add(gamecode, new List<GameResultModel>());
                }

                gameResult[gamecode].Add(new GameResultModel() { UserId = userid, Score = score });
            }

            var game = gameRepo.FindGameRoom(gamecode);
            while (game.MaxUserCount != gameResult[gamecode].Count)
            {
                await Task.Yield();
            }

            return gameResult[gamecode];
        }

        static object _lock = new object();
        static Dictionary<string, List<RoundModel>> rounds = new Dictionary<string, List<RoundModel>>();

        [HttpGet("{gamecode}/{round}")]
        public async Task<RoundModel> Round(string gamecode, int round)
        {
            lock (_lock)
            {
                if (false == rounds.ContainsKey(gamecode))
                {
                    rounds.Add(gamecode, new List<RoundModel>());
                }

                if (round + 1 > rounds[gamecode].Count)
                {
                    rounds[gamecode].Add(new RoundModel(round));
                }

                RoundModel userRound = rounds[gamecode][round];
                userRound.ReadyCount++;
            }

            List<DiceModel> dice101 = context.Dice.Where(d => d.DiceId == 101).ToList();
            List<DiceModel> dice201 = context.Dice.Where(d => d.DiceId == 201).ToList();
            RoundModel roundData = rounds[gamecode][round];
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                int randomIndex = r.Next(0, dice101.Count);
                roundData.Dices.Add(dice101[randomIndex].RouteId);
            }

            roundData.Dices.Add(dice201[r.Next(0, dice201.Count)].RouteId);

            var gameRoom = gameRepo.FindGameRoom(gamecode);
            while (gameRoom.UserCount > roundData.ReadyCount)
            {
                await Task.Yield();
            }

            return roundData;
        }
        #endregion
    }
}
