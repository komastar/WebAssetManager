using AssetWebManager.Data;
using AssetWebManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Repository
{
    public class GameRepository
    {
        private static ApplicationDbContext db;
        private static GameRepository instance;

        public GameRepository(ApplicationDbContext _context)
        {
            if (null == instance)
            {
                instance = this;
                db = _context;
            }
        }

        #region GameRoom
        public List<GameRoomModel> GetAllGameRoom()
        {
            return GetAllGameRoomAsync().GetAwaiter().GetResult();
        }

        public async Task<List<GameRoomModel>> GetAllGameRoomAsync()
        {
            return await db.GameRoom.ToListAsync();
        }

        public GameRoomModel CreateGameRoom(int maxUserCount)
        {
            return CreateGameRoomAsync(maxUserCount).GetAwaiter().GetResult();
        }

        public async Task<GameRoomModel> CreateGameRoomAsync(int maxUserCount)
        {
            var gameCode = MakeGameCode();
            GameRoomModel newGame = new GameRoomModel(gameCode, maxUserCount);

            newGame = await CreateGameRoomAsync(newGame);

            var gameOwner = CreateGameUser(newGame.Id);
            JoinGameUser(gameOwner.UserId, newGame);
            newGame.OwnerUserId = gameOwner.UserId;
            newGame.UserId = gameOwner.UserId;

            return newGame;
        }

        public async Task<GameRoomModel> CreateGameRoomAsync(GameRoomModel gameRoom)
        {
            await db.AddAsync(gameRoom);
            await db.SaveChangesAsync();

            return gameRoom;
        }

        public GameRoomModel FindGameRoom(string gameCode)
        {
            return FindGameRoomAsync(gameCode).GetAwaiter().GetResult();
        }

        public async Task CreateGameUserAsync(GameUserModel gameUserModel)
        {
            db.Add(gameUserModel);
            await db.SaveChangesAsync();
        }

        public async Task<GameRoomModel> FindGameRoomAsync(string gameCode)
        {
            return await db.FindAsync<GameRoomModel>(gameCode);
        }

        public GameRoomModel JoinGameRoom(string gameCode)
        {
            var gameRoom = FindGameRoom(gameCode);
            if (null != gameRoom)
            {
                if (gameRoom.UserCount < gameRoom.MaxUserCount)
                {
                    gameRoom.UserCount++;
                    db.Update(gameRoom);

                    var gameUser = CreateGameUser(gameRoom.Id);
                    JoinGameUser(gameUser.UserId, gameRoom);
                    gameRoom.UserId = gameUser.UserId;

                    return gameRoom;
                }
            }

            return null;
        }

        public async Task UpdateGameRoomAsync(GameRoomModel gameRoom)
        {
            db.Update(gameRoom);
            await db.SaveChangesAsync();
        }

        public GameRoomModel ExitGameRoom(string gameCode, string userId)
        {
            var gameRoom = FindGameRoom(gameCode);
            if (null != gameRoom)
            {
                gameRoom.UserCount--;
                if (0 < gameRoom.UserCount)
                {
                    db.Update(gameRoom);
                }

                var gameUser = FindGameUser(userId);
                gameUser.GameRoomId = 0;
                db.Update(gameUser);
            }
            else
            {
                return null;
            }

            if (0 >= gameRoom.UserCount)
            {
                DeleteGameRoom(gameRoom);

                return null;
            }
            else
            {
                return gameRoom;
            }
        }

        public async Task UpdateGameUserAsync(GameUserModel gameUserModel)
        {
            db.Add(gameUserModel);
            await db.SaveChangesAsync();
        }

        public async Task DeleteGameRoomAsync(GameRoomModel gameRoom)
        {
            var users = db.GameUser.Where(u => u.GameRoomId == gameRoom.Id);
            db.GameUser.RemoveRange(users);
            db.Remove(gameRoom);
            await db.SaveChangesAsync();
        }

        public void DeleteGameRoom(GameRoomModel gameRoom)
        {
            DeleteGameRoomAsync(gameRoom).GetAwaiter().GetResult();
        }

        public async Task<int> ClearGameRoomAsync()
        {
            int removeCount;
            var gameRooms = await GetAllGameRoomAsync();
            removeCount = gameRooms.Count;
            db.RemoveRange(gameRooms);
            await db.SaveChangesAsync();

            return removeCount;
        }

        public int ClearGameRoom()
        {
            return ClearGameRoomAsync().GetAwaiter().GetResult();
        }

        public async Task ClearGameUserAsync()
        {
            var gameUsers = await db.GameUser.ToListAsync();
            db.RemoveRange(gameUsers);
            await db.SaveChangesAsync();
        }

        public async Task DeleteGameUserAsync(GameUserModel gameUserModel)
        {
            db.GameUser.Remove(gameUserModel);
            await db.SaveChangesAsync();
        }
        #endregion

        #region GameUser
        public async Task<List<GameUserModel>> GetAllGameUserAsync()
        {
            return await db.GameUser.ToListAsync();
        }

        public GameUserModel CreateGameUser(int gameRoomId = 0)
        {
            GameUserModel newGameUser = new GameUserModel
            {
                UserId = MakeGameUserId(),
                GameRoomId = gameRoomId
            };

            db.Add(newGameUser);
            db.SaveChanges();

            return newGameUser;
        }

        public GameUserModel FindGameUser(string userId)
        {
            return FindGameUserAsync(userId).GetAwaiter().GetResult();
        }

        public async Task<GameUserModel> FindGameUserAsync(string userId)
        {
            return await db.GameUser.FirstOrDefaultAsync(u => u.UserId == userId);
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
                game = FindGameRoom(newGameCode);
            } while (null != game);

            return newGameCode;
        }

        private string MakeGameUserId()
        {
            GameUserModel gameUser;
            string userId;
            do
            {
                userId = Guid.NewGuid().ToString();
                gameUser = FindGameUser(userId);

            } while (null != gameUser);

            return userId;
        }

        private GameUserModel JoinGameUser(string userId, GameRoomModel gameRoom)
        {
            var gameUser = FindGameUser(userId);
            gameUser.GameRoomId = gameRoom.Id;
            db.Update(gameUser);
            db.SaveChanges();

            return gameUser;
        }
    }
}
