using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArarararWeb.DataModels;

namespace ArarararWeb.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly MyDBContext _context;

        public FileUploadController(MyDBContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
              return _context.Files != null ? 
                          View(await _context.Files.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Files'  is null.");
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Files == null)
            {
                return NotFound();
            }

            var fileItem = await _context.Files
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileItem == null)
            {
                return NotFound();
            }

            return View(fileItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,Content,UploadDate")] FileItem fileItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fileItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fileItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Files == null)
            {
                return NotFound();
            }

            var fileItem = await _context.Files.FindAsync(id);
            if (fileItem == null)
            {
                return NotFound();
            }
            return View(fileItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FileName,Content,UploadDate")] FileItem fileItem)
        {
            if (id != fileItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileItemExists(fileItem.Id))
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
            return View(fileItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Files == null)
            {
                return NotFound();
            }

            var fileItem = await _context.Files
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileItem == null)
            {
                return NotFound();
            }

            return View(fileItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Files == null)
            {
                return Problem("Entity set 'MyDBContext.Files'  is null.");
            }
            var fileItem = await _context.Files.FindAsync(id);
            if (fileItem != null)
            {
                _context.Files.Remove(fileItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileItemExists(int id)
        {
          return (_context.Files?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFilePost(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    var fileItem = new FileItem
                    {
                        FileName = file.FileName,
                        Content = memoryStream.ToArray(),
                        UploadDate = DateTime.Now
                    };

                    _context.Files.Add(fileItem);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("UploadFileSuccess");
            }
            else
            {
                return RedirectToAction("UploadFileError");
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is not selected");

            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    byte[] fileBytes = ms.ToArray();
                    var dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
                        .UseSqlite("Data Source=database.db;Cache=Shared")
                             .Options;

                    using (var context = new MyDBContext(dbContextOptions))
                    {
                        context.Files.Add(new FileItem
                        {
                            FileName = file.FileName,
                            Content = fileBytes,
                            UploadDate = DateTime.Now
                        });
                        await context.SaveChangesAsync();
                    }

                    return RedirectToAction("UploadSuccess");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        public IActionResult UploadSuccess()
        {
            return View("UploadSuccess");
        }
    }
}
