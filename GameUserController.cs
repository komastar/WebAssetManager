using AssetWebManager.Data;
using AssetWebManager.Models;
using AssetWebManager.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AssetWebManager
{
    public class GameUserController : Controller
    {
        private readonly GameRepository gameRepo;

        public GameUserController(ApplicationDbContext context)
        {
            gameRepo = new GameRepository(context);
        }

        // GET: GameUser
        public async Task<IActionResult> Index()
        {
            return View(await gameRepo.GetAllGameUserAsync());
        }

        // GET: GameUser/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameUserModel = await gameRepo.FindGameUserAsync(id);
            if (gameUserModel == null)
            {
                return NotFound();
            }

            return View(gameUserModel);
        }

        // GET: GameUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,GameRoomId")] GameUserModel gameUserModel)
        {
            if (ModelState.IsValid)
            {
                await gameRepo.CreateGameUserAsync(gameUserModel);
                return RedirectToAction(nameof(Index));
            }
            return View(gameUserModel);
        }

        // GET: GameUser/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameUserModel = await gameRepo.FindGameUserAsync(id);
            if (gameUserModel == null)
            {
                return NotFound();
            }
            return View(gameUserModel);
        }

        // POST: GameUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserId,GameRoomId")] GameUserModel gameUserModel)
        {
            if (id != gameUserModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await gameRepo.UpdateGameUserAsync(gameUserModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (null == gameRepo.FindGameUser(gameUserModel.UserId))
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
            return View(gameUserModel);
        }

        // GET: GameUser/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameUserModel = await gameRepo.FindGameUserAsync(id);
            if (gameUserModel == null)
            {
                return NotFound();
            }

            return View(gameUserModel);
        }

        // POST: GameUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gameUserModel = await gameRepo.FindGameUserAsync(id);
            await gameRepo.DeleteGameUserAsync(gameUserModel);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Clear()
        {
            await gameRepo.ClearGameUserAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
