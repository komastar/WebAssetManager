using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssetWebManager.Data;
using AssetWebManager.Models;

namespace AssetWebManager.Controllers
{
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Content
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContentLock.ToListAsync());
        }

        // GET: Content/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentLockModel = await _context.ContentLock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contentLockModel == null)
            {
                return NotFound();
            }

            return View(contentLockModel);
        }

        // GET: Content/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Content/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentLockModel contentLockModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contentLockModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contentLockModel);
        }

        // GET: Content/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentLockModel = await _context.ContentLock.FindAsync(id);
            if (contentLockModel == null)
            {
                return NotFound();
            }
            return View(contentLockModel);
        }

        // POST: Content/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContentLockModel contentLockModel)
        {
            if (id != contentLockModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentLockModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentLockModelExists(contentLockModel.Id))
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
            return View(contentLockModel);
        }

        // GET: Content/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentLockModel = await _context.ContentLock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contentLockModel == null)
            {
                return NotFound();
            }

            return View(contentLockModel);
        }

        // POST: Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contentLockModel = await _context.ContentLock.FindAsync(id);
            _context.ContentLock.Remove(contentLockModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentLockModelExists(int id)
        {
            return _context.ContentLock.Any(e => e.Id == id);
        }
    }
}
