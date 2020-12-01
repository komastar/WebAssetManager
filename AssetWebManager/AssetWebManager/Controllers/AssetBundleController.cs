using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssetWebManager.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace AssetWebManager.Data
{
    [Authorize(Roles = "Manager")]
    public class AssetBundleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AssetBundleController> _logger;
        private IWebHostEnvironment _env;

        public AssetBundleController(ApplicationDbContext context, IWebHostEnvironment env, ILogger<AssetBundleController> logger)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        // GET: AssetBundle
        public async Task<IActionResult> Index()
        {
            return View(await _context.AssetBundleModel.ToListAsync());
        }

        // GET: AssetBundle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetBundleModel = await _context.AssetBundleModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetBundleModel == null)
            {
                return NotFound();
            }

            return View(assetBundleModel);
        }

        // GET: AssetBundle/Create
        public IActionResult Create()
        {
            AssetBundleUploadModel assetBundleUploadModel = new AssetBundleUploadModel();
            assetBundleUploadModel.UpdateTime = DateTime.Now;
            assetBundleUploadModel.BundleVersion = "latest";

            return View(assetBundleUploadModel);
        }

        // POST: AssetBundle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AssetBundleUploadModel assetBundleUploadModel)
        {
            AssetBundleModel assetBundleModel = assetBundleUploadModel;
            if (ModelState.IsValid)
            {
                var saveDir = Path.Combine(_env.WebRootPath, assetBundleModel.Project, assetBundleModel.Branch, assetBundleModel.BundleVersion);
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                long size = assetBundleUploadModel.Files.Sum(f => f.Length);
                var filePaths = new List<string>();
                foreach (var formFile in assetBundleUploadModel.Files)
                {
                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = Path.Combine(saveDir, formFile.FileName);

                        filePaths.Add(filePath);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await formFile.CopyToAsync(stream);
                    }
                }

                var find = _context.AssetBundleModel.FirstOrDefault(m => m.Project == assetBundleModel.Project && m.Branch == assetBundleModel.Branch);
                if (null == find)
                {
                    _context.Add(assetBundleModel);
                }
                else
                {
                    find.Name = assetBundleModel.Name;
                    find.UpdateTime = DateTime.Now;
                    _context.Update(find);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(assetBundleModel);
        }

        // GET: AssetBundle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetBundleModel = await _context.AssetBundleModel.FindAsync(id);
            if (assetBundleModel == null)
            {
                return NotFound();
            }
            assetBundleModel.UpdateTime = DateTime.Now;

            return View(assetBundleModel);
        }

        // POST: AssetBundle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] AssetBundleModel assetBundleModel)
        {
            if (id != assetBundleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assetBundleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetBundleModelExists(assetBundleModel.Id))
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
            return View(assetBundleModel);
        }

        // GET: AssetBundle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetBundleModel = await _context.AssetBundleModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetBundleModel == null)
            {
                return NotFound();
            }

            return View(assetBundleModel);
        }

        // POST: AssetBundle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assetBundleModel = await _context.AssetBundleModel.FindAsync(id);
            _context.AssetBundleModel.Remove(assetBundleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetBundleModelExists(int id)
        {
            return _context.AssetBundleModel.Any(e => e.Id == id);
        }
    }
}
