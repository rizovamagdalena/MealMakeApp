using MealMake.Domain.Domain_Models;
using MealMake.Domain.Dto;
using MealMake.Domain.ViewModels;
using MealMake.Service.Implementation;
using MealMake.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace MealMake.Web.Controllers
{
    [Authorize]
    public class MealCollectionsController : Controller
    {
        private readonly IMealCollectionService _mealCollectionService;
        private readonly IDataFetchService _dataFetchService;
        private readonly ICollectionCategoryService _categoryService;
        private readonly IMealService _mealService;
        private readonly IActiveCollectionService _activeCollectionService;
        private readonly IArchivedCollectionService _archivedCollectionService;


        public MealCollectionsController(
            IMealCollectionService mealCollectionService,
            IDataFetchService dataFetchService,
            ICollectionCategoryService categoryService ,
            IMealService mealService,
            IActiveCollectionService activeCollectionService,
            IArchivedCollectionService archivedCollectionService)
        {
            _mealCollectionService = mealCollectionService;
            _dataFetchService = dataFetchService;
            _categoryService = categoryService;
            _mealService = mealService;
            _activeCollectionService = activeCollectionService;
            _archivedCollectionService = archivedCollectionService;
        }

        // GET: MealCollections
        public IActionResult Index(string category = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collections = _mealCollectionService.GetAll(userId);

            var categories = _categoryService.GetAll(userId);
            ViewBag.AllCategories = categories;
            ViewBag.SelectedCategory = category;


            if (!string.IsNullOrEmpty(category))
            {
                collections = collections
                    .Where(c => c.Categories.Any(mc => mc.CollectionCategory.Name == category))
                    .ToList();
            }

            return View(collections);
        }

        // GET: MealCollections/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mealCollection = _mealCollectionService.GetById(id.Value, userId);

            if (mealCollection == null) return NotFound();

            return View(mealCollection);
        }

        // GET: MealCollections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MealCollections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MealCollectionCreateViewModel mealCollection)
        {
            if (!ModelState.IsValid)
                return View(mealCollection);

            // This gets the currently logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _mealCollectionService.Add(mealCollection, userId);

            return RedirectToAction(nameof(Index));
        }


        // GET: MealCollections/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mealCollection = _mealCollectionService.GetById(id.Value, userId);

            if (mealCollection == null) return NotFound();

            return View(mealCollection);
        }

        // POST: MealCollections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, MealCollection mealCollection)
        {
            if (id != mealCollection.Id) return NotFound();

            if (!ModelState.IsValid) return View(mealCollection);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _mealCollectionService.Update(mealCollection, userId);

            return RedirectToAction(nameof(Index));
        }

        // GET: MealCollections/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mealCollection = _mealCollectionService.GetById(id.Value, userId);

            if (mealCollection == null) return NotFound();

            return View(mealCollection);
        }

        // POST: MealCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _mealCollectionService.DeleteById(id, userId);
            return RedirectToAction(nameof(Index));
        }



        // ----------------------------
        // GET: Add meals from favorites
        // ----------------------------

        public IActionResult AddFromFavorites(Guid collectionId)
        {
            if (collectionId == Guid.Empty) return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collection = _mealCollectionService.GetById(collectionId, userId);
            if (collection == null) return NotFound();

            var favorites = _mealService.GetUserFavorites(userId);

            var favoriteMeals = favorites.Select(f => new MealViewModel
            {
                Id = f.MealId,
                Name = f.MealName,
                Thumbnail = f.MealThumbnail,
                IsInCollection = _mealCollectionService.IsMealInCollection(collectionId, f.MealId)
            }).ToList();

            var vm = new AddMealFromFavoritesViewModel
            {
                CollectionId = collection.Id,
                CollectionName = collection.Name,
                FavoriteMeals = favoriteMeals
            };

            return View(vm);
        }


        // ----------------------------
        // POST: Add meal from favorites
        // ----------------------------
        [HttpPost]
        public async Task<IActionResult> AddMealToCollection(Guid collectionId, string mealId)
        {
            if (collectionId == Guid.Empty || string.IsNullOrEmpty(mealId))
                return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var meal = await _dataFetchService.GetMealDetailsAsync(mealId, userId);

            if (meal == null)
                return NotFound("Meal not found");

            _mealCollectionService.AddMealToCollection(
                collectionId, 
                new AddMealDto
                {
                    IdMeal = meal.Id,
                    Name = meal.Name,
                    Thumbnail = meal.Thumbnail
                },
                userId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true });


            return RedirectToAction("Details", new { id = collectionId });
        }



        // ----------------------------
        // POST: Remove a meal from a collection
        // ----------------------------
        [HttpPost]
        public IActionResult RemoveMealFromCollection(string mealId, Guid collectionId)
        {
            

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _mealCollectionService.RemoveMealFromCollection(collectionId, mealId, userId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true });

            return RedirectToAction("Details", new { id = collectionId });
        }
        // ----------------------------
        // GET: Home page that shows active collection
        // ----------------------------
        [HttpGet]
        public async Task<IActionResult> HomePage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"UserId: {userId}");

            ViewBag.UserCollections = _mealCollectionService.GetAll(userId);

            var activeCollectionEntry = _activeCollectionService.GetActiveCollection(userId);

            if (activeCollectionEntry == null)
                return View(null); // no active collection

            var activeCollection = _mealCollectionService.GetById(activeCollectionEntry.CollectionId, userId);

            List<IngredientSummaryViewModel> ingredientSummary = new();
            if (activeCollection != null)
            {
                ingredientSummary = await _mealCollectionService.GetIngredientSummaryForCollectionAsync(activeCollection, userId);
                Console.WriteLine($"Ingredient summary count: {ingredientSummary.Count}");
                for (int i = 0; i < ingredientSummary.Count; i++)
                {
                    Console.WriteLine($"Ingredient {i + 1}: {ingredientSummary[i].name} - {ingredientSummary[i].totalAmount}");
                }
            }
            ViewBag.IngredientSummary = ingredientSummary;

            return View(activeCollection);
        }


        // ----------------------------
        // GET: Show AddCategory form
        // ----------------------------
        [HttpGet]
        public IActionResult AddCategory(Guid collectionId)
        {
            if (collectionId == Guid.Empty)
                return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var collection = _mealCollectionService.GetById(collectionId, userId);
            if (collection == null)
                return NotFound();

            var allCategories = _categoryService.GetAll(userId);

            var existingCategories = collection.Categories ?? new List<MealCollectionCategory>();


            var availableCategories = allCategories
                 .Where(c => !existingCategories.Any(cc => cc.CollectionCategoryId == c.Id))
                 .ToList();


            var vm = new AddCategoryViewModel
            {
                CollectionId = collection.Id,
                CollectionName = collection.Name,
                Categories = availableCategories
            };

            return View(vm);
        }

        // ----------------------------
        // POST: AddCategory 
        // ----------------------------
        [HttpPost]
        public IActionResult AddCategory(AddCategoryViewModel vm)
        {
            if (vm.CollectionId == Guid.Empty || vm.SelectedCategoryId == Guid.Empty)
                return BadRequest("Invalid parameters");

            _mealCollectionService.AddCategoryToCollection(vm.CollectionId, vm.SelectedCategoryId);

            return RedirectToAction("Details", new { id = vm.CollectionId });
        }

        // ----------------------------
        //  GET: Remove Category from Collection
        // ----------------------------
        [HttpPost]
        public IActionResult RemoveCategory(Guid collectionCategoryId, Guid collectionId)
        {
            if (collectionCategoryId == Guid.Empty)
                return BadRequest("Invalid parameters");

            _mealCollectionService.RemoveCategoryFromCollection(collectionCategoryId);

            return RedirectToAction("Details", new { id = collectionId });
        }

        // ----------------------------
        // POST: Set a collection as active
        // ----------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetActiveCollection(Guid collectionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
                _activeCollectionService.SetActiveCollection(userId, collectionId);

            return RedirectToAction("HomePage");
        }

        // ----------------------------
        // POST: Archive a collection
        // ----------------------------

        [HttpPost]
        public IActionResult ArchiveActiveCollection()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _activeCollectionService.ArchiveActiveCollection(userId);

            return RedirectToAction("ArchivedCollections");
        }

        // ----------------------------
        // GET: Archived collections
        // ----------------------------
        public IActionResult ArchivedCollections()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var archivedCollections = _archivedCollectionService.GetAllByUser(userId);

            return View(archivedCollections);
        }

        // ----------------------------
        // GET: Archived collections detalis
        // ----------------------------
        public IActionResult ArchivedDetails(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var archived = _archivedCollectionService.GetDetails(id, userId);

            if (archived == null)
                return NotFound();

            return View(archived);
        }

    }
}
