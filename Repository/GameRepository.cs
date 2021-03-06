﻿using AssetWebManager.Data;
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
        private ApplicationDbContext db;

        public GameRepository(ApplicationDbContext _context)
        {
            db = _context;
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

            var gameOwner = JoinGameRoom(newGame.GameCode);
            newGame.OwnerUserId = gameOwner.UserId;
            db.Update(newGame);
            await db.SaveChangesAsync();

            return newGame;
        }

        public string CreateGameUser(string username)
        {
            GameUserModel newUser = new GameUserModel
            {
                Username = username
            };

            db.GameUser.Add(newUser);
            db.SaveChanges();

            return newUser.UserId;
        }

        public string FindGameUserByNameAsync(string username)
        {
            var user = db.GameUser.FirstOrDefault(u => u.Username == username);
            if (null != user)
            {
                return user.UserId;
            }
            else
            {
                return null;
            }
        }

        public GameRoomModel StartGame(string gamecode)
        {
            var gameRoom = FindGameRoom(gamecode);
            if (null != gameRoom)
            {
                if (1 < gameRoom.UserCount)
                {
                    gameRoom.IsOpen = false;
                    gameRoom.MaxUserCount = gameRoom.UserCount;
                    UpdateGameRoom(gameRoom);
                }

                return gameRoom;
            }

            return null;
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
            if (null != gameRoom
                && true == gameRoom.IsOpen)
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

        public void UpdateGameRoom(GameRoomModel gameRoom)
        {
            UpdateGameRoomAsync(gameRoom).GetAwaiter().GetResult();
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
                    var gameUser = FindGameUser(userId);
                    DeleteGameUser(gameUser);

                    var userInRoom = db.GameUser.Where(u => u.GameRoomId == gameRoom.Id).FirstOrDefault();
                    gameRoom.OwnerUserId = userInRoom.UserId;
                    UpdateGameRoom(gameRoom);
                }
                else
                {
                    DeleteGameRoom(gameRoom);
                }
            }

            return gameRoom;
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

        public void DeleteGameUser(GameUserModel gameUser)
        {
            DeleteGameUserAsync(gameUser).GetAwaiter().GetResult();
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
