using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpiceRestaurant.Data;
using SpiceRestaurant.Models;
using SpiceRestaurant.Models.ViewModels;
using SpiceRestaurant.Utility;

namespace SpiceRestaurant.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var subCategory = await _db.SubCategory.Include(s => s.Category).ToListAsync();
            return View(subCategory);
        }

        //GET -CREATE
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await _db.SubCategory
                                            .OrderBy(p => p.Name)
                                            .Select(p => p.Name)
                                            .Distinct()
                                            .ToListAsync()
            };

            return View(model);

        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.CategoryId == model.SubCategory.CategoryId);

                if(doesSubCategoryExists.Count() > 0)
                {
                    StatusMessage = "Error : Sub Category exists under " + doesSubCategoryExists.First().Category.Name + " category. Please use another name.";
                }
                else
                {
                    _db.SubCategory.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

                SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
                {
                    CategoryList = await _db.Category.ToListAsync(),
                    SubCategory = new Models.SubCategory(),
                    SubCategoryList = await _db.SubCategory
                                            .OrderBy(p => p.Name)
                                            .Select(p => p.Name)
                                            .Distinct()
                                            .ToListAsync(),
                    StatusMessage = StatusMessage
                };
                return View(modelVM);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await (from s in _db.SubCategory
                                   where s.CategoryId == id
                                   select s).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }


        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);

            if(subCategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory
                                            .OrderBy(p => p.Name)
                                            .Select(p => p.Name)
                                            .Distinct()
                                            .ToListAsync()
            };

            return View(model);

        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.CategoryId == model.SubCategory.CategoryId);

                if (doesSubCategoryExists.Count() > 0)
                {
                    StatusMessage = "Error : Sub Category exists under " + doesSubCategoryExists.First().Category.Name + " category. Please use another name.";
                }
                else
                {
                    var subCatFromDb = await _db.SubCategory.FindAsync(id);
                    subCatFromDb.Name = model.SubCategory.Name;

                     await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await _db.SubCategory
                                        .OrderBy(p => p.Name)
                                        .Select(p => p.Name)
                                        .Distinct()
                                        .ToListAsync(),
                StatusMessage = StatusMessage
            };
            modelVM.SubCategory.Id = id;
            return View(modelVM);
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategory.FirstOrDefaultAsync(s => s.Id == id);

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(s => s.Name).Select(p => p.Name).
                                            Distinct().ToListAsync()

            };

            return View(model);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);

        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DELETEConfirmed(int id)
        {
            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);
            _db.SubCategory.Remove(subCategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}