using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantDatabase.Models;

namespace RestaurantDatabase.Models.Tests
{
  [TestClass]
  public class CuisineTests : IDisposable
  {
    public CuisineTests()
    {
      DBConfiguration.ConnectionString = "server=localhost; user id=root; password=root; port=8889; database=best_restaurants_test;";
    }
    public void Dispose()
    {
      Restaurant.ClearAll();
      Cuisine.ClearAll();
    }

    [TestMethod]
    public void GetAll_DatabaseIsEmptyAtFirst_0()
    {
      int result = Cuisine.GetAll().Count;

      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void HasSamePropertiesAs_BothHaveSameProperties_True()
    {
      Cuisine cuisine1 = new Cuisine("Italian");
      Cuisine cuisine2 = new Cuisine("Italian");

      bool result = cuisine1.HasSamePropertiesAs(cuisine2);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void HasSamePropertiesAs_BothDontHaveSameProperties_False()
    {
      Cuisine cuisine1 = new Cuisine("Italian");
      Cuisine cuisine2 = new Cuisine("French");

      bool result = cuisine1.HasSamePropertiesAs(cuisine2);

      Assert.AreEqual(false, result);
    }
    [TestMethod]
    public void Save_SavesCuisineToDatabase_CuisineSaved()
    {
      Cuisine localCuisine = new Cuisine("Italian");
      localCuisine.Save();
      Cuisine databaseCuisine = Cuisine.GetAll()[0];

      bool result = localCuisine.HasSamePropertiesAs(databaseCuisine);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void Save_SavesMultipleCuisinesToDatabase_CuisinesSaved()
    {
      Cuisine localCuisine1 = new Cuisine("Italian");
      localCuisine1.Save();
      Cuisine localCuisine2 = new Cuisine("Chinese");
      localCuisine2.Save();
      Cuisine databaseCuisine1 = Cuisine.GetAll()[0];
      Cuisine databaseCuisine2 = Cuisine.GetAll()[1];

      bool result =
        localCuisine1.HasSamePropertiesAs(databaseCuisine1) &&
        localCuisine2.HasSamePropertiesAs(databaseCuisine2);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void FindById_GetsSpecificCuisineFromDatabase_Cuisine()
    {
      Cuisine localCuisine = new Cuisine("Italian");
      localCuisine.Save();
      Cuisine databaseCuisine = Cuisine.FindById(localCuisine.Id);

      bool result = localCuisine.HasSamePropertiesAs(databaseCuisine);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void Update_UpdateCuisineInDatabase_CuisineWithNewInfo()
    {
      Cuisine initialCuisine = new Cuisine("Italian");
      initialCuisine.Save();
      Cuisine newCuisine = new Cuisine("Mexican", initialCuisine.Id);
      initialCuisine.Update(newCuisine);
      Cuisine updatedCuisine = Cuisine.FindById(initialCuisine.Id);

      bool result = updatedCuisine.HasSamePropertiesAs(newCuisine);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void DestroyById_RemovesEntryFromDatabase_EntryRemoved()
    {
      Cuisine cuisine1 = new Cuisine("Italian");
      cuisine1.Save();
      Cuisine cuisine2 = new Cuisine("Chinese");
      cuisine2.Save();
      Cuisine cuisine3 = new Cuisine("Mexican");
      cuisine3.Save();
      Cuisine.DestroyById(cuisine2.Id);
      List<Cuisine> remainingCuisines = Cuisine.GetAll();

      bool result = (
        cuisine1.HasSamePropertiesAs(remainingCuisines[0]) &&
        cuisine3.HasSamePropertiesAs(remainingCuisines[1])
      );

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void GetRestaurants_RestaurantsAreStoredUnderCorrectCuisineType_Restaurants()
    {
      Cuisine cuisine1 = new Cuisine("Italian");
      cuisine1.Save();
      Cuisine cuisine2 = new Cuisine("Mexican");
      cuisine2.Save();
      Restaurant localRestaurant1 = new Restaurant("Gugino's", cuisine1.Id);
      localRestaurant1.Save();
      Restaurant localRestaurant2 = new Restaurant("Pepino's", cuisine1.Id);
      localRestaurant2.Save();
      Restaurant localRestaurant3 = new Restaurant("Not Gugino's", cuisine2.Id);
      localRestaurant3.Save();
      Restaurant localRestaurant4 = new Restaurant("Not Pepino's", cuisine2.Id);
      localRestaurant4.Save();
      List<Restaurant> restaurantsInCuisine = cuisine1.GetRestaurants();

      bool result = (
        restaurantsInCuisine[0].HasSamePropertiesAs(localRestaurant1) &&
        restaurantsInCuisine[1].HasSamePropertiesAs(localRestaurant2)
      );

      Assert.AreEqual(true, result);
    }
  }
}
