using MealMake.Domain.DomainModels;
using MealMake.Domain.Dto;
using MealMake.Domain.Identity;
using MealMake.Domain.ViewModels;
using MealMake.Service.Implementation;
using MealMake.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace MealMake.Web.Controllers
{
    [Authorize]
    public class MealsController : Controller
    {
        private readonly IMealService _mealService;
        private readonly UserManager<MealMakeApplicationUser> _userManager;
        private readonly IDataFetchService _dataFetchService;
        private readonly IMealCollectionService _mealCollectionService;

        public MealsController(IMealService mealService, UserManager<MealMakeApplicationUser> userManager, IDataFetchService dataFetchService, IMealCollectionService mealColectionService    )
        {
            _mealService = mealService;
            _userManager = userManager;
            _dataFetchService = dataFetchService;
            _mealCollectionService = mealColectionService;
        }


        // ----------------------------
        // GET: /Meals/Search
        // ----------------------------
        [HttpGet]
        public async Task<IActionResult> Search(string? name, string? firstLetter, string? category, string? area, string? ingredient, Guid? collectionId)
        {
            List<MealViewModel> meals = new();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(name))
                meals = await _dataFetchService.SearchMealsByNameAsync(name, userId);
            else if (!string.IsNullOrEmpty(firstLetter))
                meals = await _dataFetchService.ListAllMealsByFirstLetterAsync(firstLetter[0], userId);
            else if (!string.IsNullOrEmpty(category))
                meals = await _dataFetchService.ListAllMealsByCategoryAsync(category, userId);
            else if (!string.IsNullOrEmpty(area))
                meals = await _dataFetchService.ListAllMealsByAreaAsync(area, userId);
            else if (!string.IsNullOrEmpty(ingredient))
                meals = await _dataFetchService.ListAllMealsByIngredientAsync(ingredient, userId);


            if (collectionId != null)
            {
                var map = _mealCollectionService.GetCollectionMealMap(collectionId.Value);

                foreach (var meal in meals)
                {
                    if (map.TryGetValue(meal.Id, out var cmId))
                    {
                        meal.IsInCollection = true;
                        meal.CollectionMealId = cmId;
                    }
                }
            }

            if (collectionId != null)
            {
                var collection = _mealCollectionService.GetById(collectionId.Value, userId);
                ViewBag.CollectionName = collection?.Name ?? "Unknown Collection";
            }


            ViewBag.Categories = await _dataFetchService.ListMealCategoriesAsync();
            ViewBag.Areas = await _dataFetchService.ListAreasAsync();
            ViewBag.CollectionId = collectionId; 

            return View("Index", meals);
        }


        // ----------------------------
        // GET Search page for meals
        // ----------------------------
        public async Task<IActionResult> Index(Guid? collectionId = null)
        {
            ViewBag.Categories = await _dataFetchService.ListMealCategoriesAsync();
            ViewBag.Areas = await _dataFetchService.ListAreasAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (collectionId != null)
            {
                var collection = _mealCollectionService.GetById(collectionId.Value, userId);
                ViewBag.CollectionName = collection?.Name ?? "Unknown Collection";
            }
            return View();
        }

        // ----------------------------
        // GET: Details page
        // ----------------------------
       
        public async Task<IActionResult> Details(string id, Guid? collectionId = null)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mealVM = await _dataFetchService.GetMealDetailsAsync(id, userId);


            if (mealVM == null) return NotFound();

            if (collectionId != null)
            {
                var map = _mealCollectionService.GetCollectionMealMap(collectionId.Value);

                if (map.TryGetValue(mealVM.Id, out var cmId))
                {
                    mealVM.IsInCollection = true;
                    mealVM.CollectionMealId = cmId;
                }
            }

            if (collectionId != null)
            {
                var collection = _mealCollectionService.GetById(collectionId.Value, userId);
                ViewBag.CollectionName = collection?.Name ?? "Unknown Collection";
            }

            ViewBag.CollectionId = collectionId; 

            return View(mealVM);
        }
        // ----------------------------
        // POST: To favorite and unfavorite a meal
        // ----------------------------

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(string mealId, string mealName, string mealThumbnail)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var mealToSave = new MealSave
            {
                mealId = mealId,
                mealName = mealName,
                mealThumbnail = mealThumbnail
            };

            _mealService.ToggleFavorite(mealToSave, userId);

            return Ok();
        }

        // ----------------------------
        // GET: Meals/Favorites
        // ----------------------------
        public async Task<IActionResult> Favorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favoriteMeals = _mealService.GetUserFavorites(userId);

            var model = favoriteMeals.Select(f => new MealViewModel
            {
                Id = f.MealId,
                Name = f.MealName,
                Thumbnail = f.MealThumbnail,
                IsFavorite = true
            }).ToList();

            return View(model);

        }

        // ----------------------------
        // POST: Remove favorite
        // ----------------------------
        [HttpPost]
        public async Task<IActionResult> RemoveFavorite(string mealId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            _mealService.RemoveFavorite(mealId, userId);

            return RedirectToAction(nameof(Favorites));
        }

    }
}
