using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CateringPro.Models;
using CateringPro.Repositories;
using Microsoft.AspNetCore.Authorization;
using CateringPro.Data;

namespace CateringPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryRepository _categoryRepo;

        public CategoriesController(AppDbContext context, ICategoryRepository categoryRepo)
        {
            _context = context;
            _categoryRepo = categoryRepo;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepo.GetAllAsync());
        }
        public async Task<IActionResult> ListItems(string searchcriteria)
        {
            if (string.IsNullOrEmpty(searchcriteria))
            {
                return PartialView(await _context.Categories.ToListAsync());
            }
            return PartialView(await _context.Categories.Where(d => d.Name.Contains(searchcriteria) || d.Description.Contains(searchcriteria)).ToListAsync());

        }
        [HttpPost]
        public async Task<IActionResult> EditModal(int id, [Bind("Id,Code,Name,Price,Description,CategoriesId")] Categories cat)
        {
            if (id != cat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(cat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { res = "OK" });
            }
           
            return PartialView(cat);
        }


        public async Task<IActionResult> EditModal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _context.Categories.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }
            
            return PartialView(cat);
        }
        public IActionResult CreateModal()
        {

            var cat = new Categories();
            if (cat == null)
            {
                return NotFound();
            }
            
            return PartialView("EditModal", cat);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepo.GetByIdAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(categories);
                await _categoryRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepo.GetByIdAsync(id);

            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Categories categories)
        {
            if (id != categories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _categoryRepo.Update(categories);
                    await _categoryRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepo.GetByIdAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _categoryRepo.GetByIdAsync(id);
            _categoryRepo.Remove(categories);
            await _categoryRepo.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool CategoriesExists(int id)
        {
            return _categoryRepo.Exists(id);
        }
    }
}
