using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace CateringPro.Controllers
{
    public class PicturesController : Controller
    {
        private readonly AppDbContext _context;


        public PicturesController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetPicture(int? id)
        {
            var pict = await _context.Pictures.FindAsync(id);
            if (!id.HasValue || pict == null || pict.PictureData == null || pict.PictureData.Length == 0) {
                string nophotofile= $"{Directory.GetCurrentDirectory()}{@"\wwwroot\images\nophoto.jpg"}";
                using (Image<Rgba32> image = Image.Load<Rgba32>(nophotofile))
                {
                    image.Mutate(x => x
                         .Resize(40, 40));
                    using (MemoryStream ms = new MemoryStream()) {
                        image.SaveAsJpeg(ms);
                        return File(ms.GetBuffer(), "image/jpeg");
                    }
                    //image.Save("bar.jpg"); // Automatic encoder selected based on extension.
                }
               // return File(System.IO.File.ReadAllBytes(nophotofile), "image/jpeg");
            }
           return File(pict.PictureData, "image/jpeg");
        }
        // GET: Pictures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pictures.ToListAsync());
        }

        // GET: Pictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictures = await _context.Pictures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictures == null)
            {
                return NotFound();
            }

            return View(pictures);
        }

        // GET: Pictures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PictureData")] Pictures pictures)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pictures);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pictures);
        }

        // GET: Pictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictures = await _context.Pictures.FindAsync(id);
            if (pictures == null)
            {
                return NotFound();
            }
            return View(pictures);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PictureData")] Pictures pictures)
        {
            if (id != pictures.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pictures);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PicturesExists(pictures.Id))
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
            return View(pictures);
        }

        // GET: Pictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictures = await _context.Pictures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictures == null)
            {
                return NotFound();
            }

            return View(pictures);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pictures = await _context.Pictures.FindAsync(id);
            _context.Pictures.Remove(pictures);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PicturesExists(int id)
        {
            return _context.Pictures.Any(e => e.Id == id);
        }
    }
}
