using System;
using System.Collections.Generic;
using RestaurantDatabase.Models;

namespace RestaurantDatabase.ViewModels
{
  public class CuisineDetailsModel
  {
    public Cuisine CurrentCuisine {get; set;}
    public List<Restaurant> Restaurants {get; set;}
    public Dictionary<int, List<Review>> RestaurantReviews {get; set;} = new Dictionary<int, List<Review>> {};

    public CuisineDetailsModel(int currentCuisineId)
    {
      CurrentCuisine = Cuisine.FindById(currentCuisineId);
      Restaurants = CurrentCuisine.GetRestaurants();

      foreach (Restaurant restaurant in Restaurants)
      {
        RestaurantReviews.Add(restaurant.Id, restaurant.GetReviews());
      }
    }
  }
}
