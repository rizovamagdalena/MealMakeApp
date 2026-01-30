using MealMake.Domain.Domain_Models;
using MealMake.Domain.ViewModels;
using MealMake.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MealMake.Web.Controllers
{
    [Authorize]
    public class CollectionCategoriesController : Controller
    {
        private readonly ICollectionCategoryService _categoryService;

        public CollectionCategoriesController(ICollectionCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: CollectionCategories
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var categories = _categoryService.GetAll(userId);
            return View(categories);
        }

        // GET: CollectionCategories/Details/5
        public IActionResult Details(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = _categoryService.GetById(id,userId);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: CollectionCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CollectionCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CollectionCategoryCreateViewModel collectionCategory)
        {
            if (!ModelState.IsValid)
                return View(collectionCategory);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _categoryService.Add(collectionCategory,userId);
            return RedirectToAction(nameof(Index));
        }

        // GET: CollectionCategories/Edit/5
        public IActionResult Edit(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = _categoryService.GetById(id,userId);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: CollectionCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, CollectionCategory collectionCategory)
        {
            if (id != collectionCategory.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(collectionCategory);

            _categoryService.Update(collectionCategory);
            return RedirectToAction(nameof(Index));
        }

        // GET: CollectionCategories/Delete/5
        public IActionResult Delete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = _categoryService.GetById(id,userId);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: CollectionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _categoryService.DeleteById(id,userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
