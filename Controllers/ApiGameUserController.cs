using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebManager.Data;
using AssetWebManager.Models;

namespace AssetWebManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiGameUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiGameUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  GET: api/ApiGameUser/Create
        [HttpGet]
        public string Create()
        {
            GameUserModel newUser = new GameUserModel();

            while (true)
            {
                newUser.UserId = Guid.NewGuid().ToString();
                var find = _context.Find<GameUserModel>(newUser.UserId);
                if (null == find)
                {
                    _context.Add(newUser);
                    _context.SaveChanges();
                    break;
                }
            }

            return newUser.UserId;
        }

        [HttpGet]
        public int Clear()
        {
            var allUsers = _context.GameUser.ToList();
            int userCount = allUsers.Count;
            if (0 < userCount)
            {
                _context.GameUser.RemoveRange(allUsers);
                _context.SaveChanges();
            }

            return userCount;
        }

        // GET: api/ApiGameUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameUserModel>>> GetGameUser()
        {
            return await _context.GameUser.ToListAsync();
        }

        // GET: api/ApiGameUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameUserModel>> GetGameUserModel(int id)
        {
            var gameUserModel = await _context.GameUser.FindAsync(id);

            if (gameUserModel == null)
            {
                return NotFound();
            }

            return gameUserModel;
        }

        // PUT: api/ApiGameUser/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameUserModel(int id, GameUserModel gameUserModel)
        {
            if (id != gameUserModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameUserModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameUserModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiGameUser
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GameUserModel>> PostGameUserModel(GameUserModel gameUserModel)
        {
            _context.GameUser.Add(gameUserModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameUserModel", new { id = gameUserModel.Id }, gameUserModel);
        }

        // DELETE: api/ApiGameUser/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameUserModel>> DeleteGameUserModel(int id)
        {
            var gameUserModel = await _context.GameUser.FindAsync(id);
            if (gameUserModel == null)
            {
                return NotFound();
            }

            _context.GameUser.Remove(gameUserModel);
            await _context.SaveChangesAsync();

            return gameUserModel;
        }

        private bool GameUserModelExists(int id)
        {
            return _context.GameUser.Any(e => e.Id == id);
        }
    }
}
