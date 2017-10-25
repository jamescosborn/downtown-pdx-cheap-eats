using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantDatabase.Models;

namespace RestaurantDatabase.Models.Tests
{
  [TestClass]
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_test;";
    }
    public void Dispose()
    {
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
  }
}
