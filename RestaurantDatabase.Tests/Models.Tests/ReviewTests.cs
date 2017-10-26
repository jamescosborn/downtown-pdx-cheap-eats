using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantDatabase.Models;

namespace RestaurantDatabase.Models.Tests
{
  [TestClass]
  public class ReviewTests : IDisposable
  {
    public ReviewTests()
    {
      DBConfiguration.ConnectionString = "server=localhost; user id=root; password=root; port=8889; database=best_restaurants_test;";
    }
    public void Dispose()
    {
      Review.ClearAll();
    }

    [TestMethod]
    public void GetAll_DatabaseIsEmptyAtFirst_0()
    {
      int result = Review.GetAll().Count;

      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void HasSamePropertiesAs_BothHaveSameProperties_True()
    {
      Review review1 = new Review("This is very Italian", "ItalianGuy2453", 0);
      Review review2 = new Review("This is very Italian", "ItalianGuy2453", 0);

      bool result = review1.HasSamePropertiesAs(review2);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void HasSamePropertiesAs_BothDontHaveSameProperties_False()
    {
      Review review1 = new Review("This is very Italian", "ItalianGuy2453", 0);
      Review review2 = new Review("This is very French", "NotItalianGuy2453", 0);

      bool result = review1.HasSamePropertiesAs(review2);

      Assert.AreEqual(false, result);
    }
    [TestMethod]
    public void Save_SavesReviewToDatabase_ReviewSaved()
    {
      Review localReview = new Review("This is very Italian", "ItalianGuy2453", 0);
      localReview.Save();
      Review databaseReview = Review.GetAll()[0];

      bool result = localReview.HasSamePropertiesAs(databaseReview);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void Save_SavesMultipleReviewsToDatabase_ReviewsSaved()
    {
      Review localReview1 = new Review("This is very Italian", "ItalianGuy2453", 0);
      localReview1.Save();
      Review localReview2 = new Review("This is very French", "NotItalianGuy2453", 0);
      localReview2.Save();
      Review databaseReview1 = Review.GetAll()[0];
      Review databaseReview2 = Review.GetAll()[1];

      bool result =
        localReview1.HasSamePropertiesAs(databaseReview1) &&
        localReview2.HasSamePropertiesAs(databaseReview2);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void FindById_GetsSpecificReviewFromDatabase_Review()
    {
      Review localReview = new Review("This is very French", "NotItalianGuy2453", 0);
      localReview.Save();
      Review databaseReview = Review.FindById(localReview.Id);

      bool result = localReview.HasSamePropertiesAs(databaseReview);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void Update_UpdateReviewInDatabase_ReviewWithNewInfo()
    {
      Review initialReview = new Review("This is very French", "NotItalianGuy2453", 0);
      initialReview.Save();
      Review newReview = new Review("This is very Italian", "ItalianGuy2453", 0, initialReview.Id);
      initialReview.Update(newReview);
      Review updatedReview = Review.FindById(initialReview.Id);

      bool result = updatedReview.HasSamePropertiesAs(newReview);

      Assert.AreEqual(true, result);
    }
    [TestMethod]
    public void DestroyById_RemovesEntryFromDatabase_EntryRemoved()
    {
      Review review1 = new Review("This is very Italian", "ItalianGuy2453", 0);
      review1.Save();
      Review review2 = new Review("This is very French", "NotItalianGuy2453", 0);
      review2.Save();
      Review review3 = new Review("This is not very Mexican", "TheRealPepino", 0);
      review3.Save();
      Review.DestroyById(review2.Id);
      List<Review> remainingReviews = Review.GetAll();

      bool result = (
        review1.HasSamePropertiesAs(remainingReviews[0]) &&
        review3.HasSamePropertiesAs(remainingReviews[1])
      );

      Assert.AreEqual(true, result);
    }
  }
}
