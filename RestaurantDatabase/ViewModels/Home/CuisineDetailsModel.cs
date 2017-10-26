using System;
using System.Collections.Generic;
using RestaurantDatabase.Models;

namespace RestaurantDatabase.ViewModels
{
  public class CuisineDetailsModel
  {
    public Cuisine CurrentCuisine {get; set;}
    public List<Restaurant> Restaurants {get; set;}

    public CuisineDetailsModel(int currentCuisineId)
    {
      CurrentCuisine = Cuisine.FindById(currentCuisineId);
      Restaurants = CurrentCuisine.GetRestaurants();
    }
  }
}
