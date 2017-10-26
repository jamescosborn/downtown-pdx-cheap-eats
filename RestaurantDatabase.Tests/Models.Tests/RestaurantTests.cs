using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantDatabase.Models;

namespace RestaurantDatabase.Models.Tests
{
  [TestClass]
  public class RestaurantTest : IDisposable
  {
    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "server=localhost; user id=root; password=root; port=8889; database=best_restaurants_test;";
    }
    public void Dispose()
    {
      Restaurant.ClearAll();
    }
    [TestMethod]
    public void GetAll_DatabaseIsEmptyAtFirst_0()
    {
      List<Restaurant> restaurants = Restaurant.GetAll();
      int result = restaurants.Count;

      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void HasSamePropertiesAs_BothHaveSameProperties_True()
    {
      Restaurant restaurant1 = new Restaurant("Gugino's");
      Restaurant restaurant2 = new Restaurant("Gugino's");

      bool result = restaurant1.HasSamePropertiesAs(restaurant2);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void HasSamePropertiesAs_BothDontHaveSameProperties_False()
    {
      Restaurant restaurant1 = new Restaurant("Gugino's");
      Restaurant restaurant2 = new Restaurant("Pepino's");

      bool result = restaurant1.HasSamePropertiesAs(restaurant2);

      Assert.AreEqual(false, result);
    }
    [TestMethod]
    public void Save_SavesRestaurantToDatabase_RestaurantSaved()
    {
      Restaurant localRestaurant = new Restaurant("Gugino's");
      localRestaurant.Save();
      Restaurant databaseRestaurant = Restaurant.GetAll()[0];

      bool result = localRestaurant.HasSamePropertiesAs(databaseRestaurant);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void Save_SavesMultipleRestaurantsToDatabase_RestaurantsSaved()
    {
      Restaurant localRestaurant1 = new Restaurant("Gugino's");
      localRestaurant1.Save();
      Restaurant localRestaurant2 = new Restaurant("Pepino's");
      localRestaurant2.Save();
      Restaurant databaseRestaurant1 = Restaurant.GetAll()[0];
      Restaurant databaseRestaurant2 = Restaurant.GetAll()[1];

      bool result =
        localRestaurant1.HasSamePropertiesAs(databaseRestaurant1) &&
        localRestaurant2.HasSamePropertiesAs(databaseRestaurant2);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void FindById_GetsSpecificRestaurantFromDatabase_Restaurant()
    {
      Restaurant localRestaurant = new Restaurant("Pepino's");
      localRestaurant.Save();
      Restaurant databaseRestaurant = Restaurant.FindById(localRestaurant.Id);

      bool result = localRestaurant.HasSamePropertiesAs(databaseRestaurant);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void FindById_GetsSpecificRestaurantWithMultipleFieldsFromDatabase_Restaurant()
    {
      Restaurant localRestaurant = new Restaurant("Pepino's", "123 Four St.", "555-5555", "www.pepinos.com", "$$$$", "4.7");
      localRestaurant.Save();
      Restaurant databaseRestaurant = Restaurant.FindById(localRestaurant.Id);

      bool result = localRestaurant.HasSamePropertiesAs(databaseRestaurant);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void Update_UpdateRestaurantInDatabase_RestaurantWithNewInfo()
    {
      Restaurant initialRestaurant = new Restaurant("Pepino's", 0);
      initialRestaurant.Save();
      Restaurant newRestaurant = new Restaurant("Gugino's", 0, initialRestaurant.Id);
      initialRestaurant.Update(newRestaurant);
      Restaurant updatedRestaurant = Restaurant.FindById(initialRestaurant.Id);

      bool result = updatedRestaurant.HasSamePropertiesAs(newRestaurant);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void Update_UpdateMultipleFieldsRestaurantInDatabase_RestaurantWithNewInfo()
    {
      Restaurant initialRestaurant = new Restaurant("Pepino's", "123 Four St.", "555-5555", "www.pepinos.com", "$$$$", "4.7", 0);
      initialRestaurant.Save();
      Restaurant newRestaurant = new Restaurant("Not Pepino's", "A different St.", "444-4444", "www.notpepinos.com", "$$$", "4.2", 0, initialRestaurant.Id);
      initialRestaurant.Update(newRestaurant);
      Restaurant updatedRestaurant = Restaurant.FindById(initialRestaurant.Id);

      bool result = updatedRestaurant.HasSamePropertiesAs(newRestaurant);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void DestroyById_RemovesEntryFromDatabase_EntryRemoved()
    {
      Restaurant restaurant1 = new Restaurant("Pepino's");
      restaurant1.Save();
      Restaurant restaurant2 = new Restaurant("Not Pepino's");
      restaurant2.Save();
      Restaurant restaurant3 = new Restaurant("Maybe Pepino's");
      restaurant3.Save();
      Restaurant.DestroyById(restaurant2.Id);
      List<Restaurant> remainingRestaurants = Restaurant.GetAll();

      bool result = (
        restaurant1.HasSamePropertiesAs(remainingRestaurants[0]) &&
        restaurant3.HasSamePropertiesAs(remainingRestaurants[1])
      );

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void GetReviews_ReviewsAreStoredUnderCorrectCuisineType_Reviews()
    {
      Restaurant restaurant1 = new Restaurant("Pepino's");
      restaurant1.Save();
      Restaurant restaurant2 = new Restaurant("Not Pepino's");
      restaurant2.Save();
      Review localReview1 = new Review("This is very Italian", "ItalianGuy2453", restaurant1.Id);
      localReview1.Save();
      Review localReview2 = new Review("This is very French", "NotItalianGuy2453", restaurant1.Id);
      localReview2.Save();
      Review localReview3 = new Review("This is not very Mexican", "TheRealPepino", restaurant2.Id);
      localReview3.Save();
      Review localReview4 = new Review("This is not very Pepino", "NotFakePepino", restaurant2.Id);
      localReview4.Save();
      List<Review> reviewsInRestaurant = restaurant1.GetReviews();

      bool result = (
        reviewsInRestaurant[0].HasSamePropertiesAs(localReview1) &&
        reviewsInRestaurant[1].HasSamePropertiesAs(localReview2)
      );

      Assert.AreEqual(true, result);
    }
  }
}
