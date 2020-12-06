using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebManager.Data;
using AssetWebManager.Models;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace AssetWebManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAssetBundleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApiAssetBundleController> _logger;
        private IWebHostEnvironment _env;

        public ApiAssetBundleController(ApplicationDbContext context, IWebHostEnvironment env, ILogger<ApiAssetBundleController> logger)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        // GET: api/ApiAssetBundle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetBundleModel>>> GetAssetBundleModel()
        {
            return await _context.AssetBundleModel.ToListAsync();
        }

        // GET: api/ApiAssetBundle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetBundleModel>> GetAssetBundleModel(int id)
        {
            var assetBundleModel = await _context.AssetBundleModel.FindAsync(id);

            if (assetBundleModel == null)
            {
                return NotFound();
            }

            return assetBundleModel;
        }

        // GET: api/ApiAssetBundle/Project/Branch
        // https://localhost:44377/api/ApiAssetBundle/defensquare/release/latest
        [HttpGet("{project}/{branch}")]
        public async Task<ActionResult<AssetBundleModel>> GetAssetBundleModel(string project, string branch)
        {
            var assetBundleModel = await _context.AssetBundleModel.FirstOrDefaultAsync(m => m.Project == project && m.Branch == branch);
            if (assetBundleModel == null)
            {
                return NotFound();
            }

            return assetBundleModel;
        }

        // PUT: api/ApiAssetBundle/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssetBundleModel(int id, AssetBundleModel assetBundleModel)
        {
            if (id != assetBundleModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(assetBundleModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetBundleModelExists(id))
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

        // POST: api/ApiAssetBundle
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostAssetBundleModel([FromForm] AssetBundleUploadModel assetBundleUploadModel)
        {
            AssetBundleModel assetBundleModel = assetBundleUploadModel;
            if (ModelState.IsValid)
            {
                var saveDir = Path.Combine(_env.WebRootPath, assetBundleModel.Project, assetBundleModel.BundleVersion);
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                var filePaths = new List<string>();
                foreach (var formFile in assetBundleUploadModel.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(saveDir, formFile.FileName);

                        filePaths.Add(filePath);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await formFile.CopyToAsync(stream);
                    }
                }

                var find = _context.AssetBundleModel.SingleOrDefault(
                    m => m.Project == assetBundleModel.Project && m.Branch == assetBundleModel.Branch);

                if (null == find)
                {
                    _context.Add(assetBundleModel);
                }
                else
                {
                    find.Name = assetBundleModel.Name;
                    find.Project = assetBundleModel.Project;
                    find.UpdateTime = DateTime.Now;
                    find.BundleVersionCode = assetBundleModel.BundleVersionCode;
                    find.BundleVersion = assetBundleModel.BundleVersion;

                    _context.Update(find);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE: api/ApiAssetBundle/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AssetBundleModel>> DeleteAssetBundleModel(int id)
        {
            var assetBundleModel = await _context.AssetBundleModel.FindAsync(id);
            if (assetBundleModel == null)
            {
                return NotFound();
            }

            _context.AssetBundleModel.Remove(assetBundleModel);
            await _context.SaveChangesAsync();

            return assetBundleModel;
        }

        private bool AssetBundleModelExists(int id)
        {
            return _context.AssetBundleModel.Any(e => e.Id == id);
        }
    }
}
