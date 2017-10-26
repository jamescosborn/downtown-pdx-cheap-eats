using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using RestaurantDatabase.Models;
using RestaurantDatabase.ViewModels;

namespace RestaurantDatabase.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        List<Cuisine> model = Cuisine.GetAll();
        return View(model);
      }

      [HttpPost("/cuisines/add")]
      public ActionResult AddCuisine()
      {
        string cuisineName = Request.Form["cuisine-name"];
        Cuisine newCuisine = new Cuisine(cuisineName);
        newCuisine.Save();

        List<Cuisine> model = Cuisine.GetAll();

        return View("Index", model);
      }

      [HttpGet("/cuisines/{id}")]
      public ActionResult CuisineDetails(int id)
      {
        CuisineDetailsModel model = new CuisineDetailsModel(id);
        return View(model);
      }

      [HttpPost("/cuisines/{cuisineId}/restaurants/add")]
      public ActionResult AddRestaurantToCuisine(int cuisineId)
      {
        string restaurantName = Request.Form["restaurant-name"];
        string restaurantAddress = Request.Form["restaurant-address"];
        string restaurantPhone = Request.Form["restaurant-phone"];
        string restaurantWebsite = Request.Form["restaurant-website"];
        string restaurantCost = Request.Form["restaurant-cost"];
        string restaurantRating = Request.Form["restaurant-rating"];

        Restaurant newRestaurant = new Restaurant(restaurantName, restaurantAddress, restaurantPhone, restaurantWebsite, restaurantCost, restaurantRating, cuisineId);
        newRestaurant.Save();

        CuisineDetailsModel model = new CuisineDetailsModel(cuisineId);
        return View("CuisineDetails", model);
      }
    }
}
