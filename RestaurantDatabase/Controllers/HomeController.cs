using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using RestaurantDatabase.Models;

namespace RestaurantDatabase.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }
    }
}
