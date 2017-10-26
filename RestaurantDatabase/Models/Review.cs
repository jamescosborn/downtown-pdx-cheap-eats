using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RestaurantDatabase.Models
{
  public class Review
  {
    public int Id {get; private set;}
    public string Comment {get; private set;}
    public string Author {get; private set;}
    public int RestaurantId {get; private set;}

    public Review(string comment, string author, int restaurantId, int id = 0)
    {
      Comment = comment;
      Author = author;
      RestaurantId = restaurantId;
      Id = id;
    }

    public static List<Review> GetAll()
    {
      List<Review> output = new List<Review> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM reviews;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string comment = rdr.GetString(1);
        string author = rdr.GetString(2);
        int restaurantId = rdr.GetInt32(3);
        Review newReview = new Review(comment, author, restaurantId, id);
        output.Add(newReview);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return output;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM reviews;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Review FindById(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM reviews WHERE id = @ReviewId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@ReviewId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      int reviewId = 0;
      string reviewComment = "";
      string reviewAuthor = "";
      int restaurantId = 0;

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        reviewId = rdr.GetInt32(0);
        reviewComment = rdr.GetString(1);
        reviewAuthor = rdr.GetString(2);
        restaurantId = rdr.GetInt32(3);
      }
      Review output = new Review(reviewComment, reviewAuthor, restaurantId, reviewId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return output;
    }

    public static void DestroyById(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM reviews WHERE id = @ReviewId;";

      MySqlParameter reviewId = new MySqlParameter();
      reviewId.ParameterName = "@ReviewId";
      reviewId.Value = id;
      cmd.Parameters.Add(reviewId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO reviews (comment, author, restaurant_id) VALUES (@Comment, @Author, @RestaurantId);";

      MySqlParameter comment = new MySqlParameter();
      comment.ParameterName = "@Comment";
      comment.Value = this.Comment;
      cmd.Parameters.Add(comment);

      MySqlParameter author = new MySqlParameter();
      author.ParameterName = "@Author";
      author.Value = this.Author;
      cmd.Parameters.Add(author);

      MySqlParameter restaurantId = new MySqlParameter();
      restaurantId.ParameterName = "@RestaurantId";
      restaurantId.Value = this.RestaurantId;
      cmd.Parameters.Add(restaurantId);

      cmd.ExecuteNonQuery();
      this.Id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Update(Review newReview)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE reviews SET comment = @NewComment, author = @NewAuthor, restaurant_id = @NewRestaurantId WHERE id = @ReviewId;";

      MySqlParameter newComment = new MySqlParameter();
      newComment.ParameterName = "@NewComment";
      newComment.Value = newReview.Comment;
      cmd.Parameters.Add(newComment);

      MySqlParameter newAuthor = new MySqlParameter();
      newAuthor.ParameterName = "@NewAuthor";
      newAuthor.Value = newReview.Author;
      cmd.Parameters.Add(newAuthor);

      MySqlParameter newRestaurantId = new MySqlParameter();
      newRestaurantId.ParameterName = "@NewRestaurantId";
      newRestaurantId.Value = newReview.RestaurantId;
      cmd.Parameters.Add(newRestaurantId);

      MySqlParameter reviewId = new MySqlParameter();
      reviewId.ParameterName = "@ReviewId";
      reviewId.Value = this.Id;
      cmd.Parameters.Add(reviewId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public bool HasSamePropertiesAs(Review other)
    {
      return (
        this.Id == other.Id &&
        this.RestaurantId == other.RestaurantId &&
        this.Author == other.Author &&
        this.Comment == other.Comment);
    }
  }
}
