using MealMake.Domain.Domain_Models;
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

namespace MealMake.Web.Controllers
{
    [Authorize]  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataFetchService _dataFetchService;
        private readonly IMealService _mealService;
        private readonly UserManager<MealMakeApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            IDataFetchService dataFetchService,
            IMealService mealService,
            UserManager<MealMakeApplicationUser> userManager)
        {
            _logger = logger;
            _dataFetchService = dataFetchService;
            _mealService = mealService;
            _userManager = userManager;
        }




        // GET: Home/Index
        public async Task<IActionResult> Index()
        {

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SetActivePlan(Guid selectedPlanId)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    //if (user == null) return Unauthorized();


        //    await _userManager.UpdateAsync(user);

        //    return RedirectToAction("Index");
        //}



    }
}
