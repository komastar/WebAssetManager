using AssetWebManager.Data;
using AssetWebManager.Models;
using AssetWebManager.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AssetWebManager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class GameController : Controller
    {
        private readonly GameRepository gameRepo;

        public GameController(ApplicationDbContext context)
        {
            gameRepo = new GameRepository(context);
        }

        // GET: Game
        public async Task<IActionResult> Index()
        {
            return View(await gameRepo.GetAllGameRoomAsync());
        }

        // GET: Game/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameModel = await gameRepo.FindGameRoomAsync(id);
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
        public async Task<IActionResult> Create([Bind("Id,GameCode,UserCount,MaxUserCount,IsOpen,CreationTime")] GameRoomModel gameRoom)
        {
            if (ModelState.IsValid)
            {
                gameRoom.Validate();
                await gameRepo.CreateGameRoomAsync(gameRoom.MaxUserCount);

                return RedirectToAction(nameof(Index));
            }
            return View(gameRoom);
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameModel = await gameRepo.FindGameRoomAsync(id);
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,GameCode,UserCount,MaxUserCount,IsOpen,CreationTime")] GameRoomModel gameRoom)
        {
            if (id != gameRoom.GameCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await gameRepo.UpdateGameRoomAsync(gameRoom);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (null == gameRepo.FindGameRoom(gameRoom.GameCode))
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
            return View(gameRoom);
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameModel = await gameRepo.FindGameRoomAsync(id);
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
            var gameModel = await gameRepo.FindGameRoomAsync(id);
            await gameRepo.DeleteGameRoomAsync(gameModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            await gameRepo.ClearGameRoomAsync();

            return RedirectToAction("Index");
        }
    }
}
