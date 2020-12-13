using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebManager.Data;
using AssetWebManager.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace AssetWebManager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Game
        public async Task<IActionResult> Index()
        {
            return View(await _context.GameRoom.ToListAsync());
        }

        // GET: Game/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameModel = await _context.GameRoom
                .FirstOrDefaultAsync(m => m.GameCode == id);
            if (gameModel == null)
            {
                return NotFound();
            }

            return View(gameModel);
        }

        // GET: Game/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GameCode,UserCount,MaxUserCount,IsOpen,CreationTime")] GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(gameModel.GameCode))
                {
                    gameModel.GameCode = MakeGameCode();
                }

                gameModel.Validate();

                _context.Add(gameModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameModel);
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameModel = await _context.GameRoom.FindAsync(id);
            if (gameModel == null)
            {
                return NotFound();
            }
            return View(gameModel);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,GameCode,UserCount,MaxUserCount,IsOpen,CreationTime")] GameModel gameModel)
        {
            if (id != gameModel.GameCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameModelExists(gameModel.GameCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gameModel);
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameModel = await _context.GameRoom
                .FirstOrDefaultAsync(m => m.GameCode == id);
            if (gameModel == null)
            {
                return NotFound();
            }

            return View(gameModel);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gameModel = await _context.GameRoom.FindAsync(id);
            _context.GameRoom.Remove(gameModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var games = _context.GameRoom.ToList();
            _context.RemoveRange(games);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool GameModelExists(string id)
        {
            return _context.GameRoom.Any(e => e.GameCode == id);
        }

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
    }
}
