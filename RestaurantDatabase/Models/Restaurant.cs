using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RestaurantDatabase.Models
{
  public class Restaurant
  {
    public int Id {get; private set;}
    public string Name {get; private set;}
    public string Address {get; private set;}
    public string Phone {get; private set;}
    public string Website {get; private set;}
    public string Cost {get; private set;}
    public string Rating {get; private set;}
    public int CuisineId {get; private set;}

    public Restaurant(string name, int cuisineId = 0, int id = 0)
    {
      Name = name;
      Address = "";
      Phone = "";
      Website = "";
      Cost = "";
      Rating = "";
      Id = id;
      CuisineId = cuisineId;
    }

    public Restaurant(string name, string address, string phone, string website, string cost, string rating, int cuisineId = 0, int id = 0)
    {
      Name = name;
      Address = address;
      Phone = phone;
      Website = website;
      Cost = cost;
      Rating = rating;
      Id = id;
      CuisineId = cuisineId;
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> output = new List<Restaurant> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string address = rdr.GetString(2);
        string phone = rdr.GetString(3);
        string website = rdr.GetString(4);
        string cost = rdr.GetString(5);
        string rating = rdr.GetString(6);
        int cuisineId = rdr.GetInt32(7);
        Restaurant newRestaurant = new Restaurant(name, address, phone, website, cost, rating, cuisineId, id);
        output.Add(newRestaurant);
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
      cmd.CommandText = @"DELETE FROM restaurants;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      Review.ClearAll();
    }

    public static Restaurant FindById(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants WHERE id = @RestaurantId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@RestaurantId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      int restaurantId = 0;
      string restaurantName = "";
      string restaurantAddress = "";
      string restaurantPhone = "";
      string restaurantWebsite = "";
      string restaurantCost = "";
      string restaurantRating = "";
      int cuisineId = 0;

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        restaurantId = rdr.GetInt32(0);
        restaurantName = rdr.GetString(1);
        restaurantAddress = rdr.GetString(2);
        restaurantPhone = rdr.GetString(3);
        restaurantWebsite = rdr.GetString(4);
        restaurantCost = rdr.GetString(5);
        restaurantRating = rdr.GetString(6);
        cuisineId = rdr.GetInt32(7);
      }
      Restaurant output = new Restaurant(restaurantName, restaurantAddress, restaurantPhone, restaurantWebsite, restaurantCost, restaurantRating, cuisineId, restaurantId);

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
      cmd.CommandText = @"DELETE FROM restaurants WHERE id = @RestaurantId;";

      MySqlParameter restaurantId = new MySqlParameter();
      restaurantId.ParameterName = "@RestaurantId";
      restaurantId.Value = id;
      cmd.Parameters.Add(restaurantId);

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
      cmd.CommandText = @"INSERT INTO restaurants (name, address, phone, website, cost, rating, cuisine_id) VALUES (@Name, @Address, @Phone, @Website, @Cost, @Rating, @CuisineId);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@Name";
      name.Value = this.Name;
      cmd.Parameters.Add(name);

      MySqlParameter address = new MySqlParameter();
      address.ParameterName = "@Address";
      address.Value = this.Address;
      cmd.Parameters.Add(address);

      MySqlParameter phone = new MySqlParameter();
      phone.ParameterName = "@Phone";
      phone.Value = this.Phone;
      cmd.Parameters.Add(phone);

      MySqlParameter website = new MySqlParameter();
      website.ParameterName = "@Website";
      website.Value = this.Website;
      cmd.Parameters.Add(website);

      MySqlParameter cost = new MySqlParameter();
      cost.ParameterName = "@Cost";
      cost.Value = this.Cost;
      cmd.Parameters.Add(cost);

      MySqlParameter rating = new MySqlParameter();
      rating.ParameterName = "@Rating";
      rating.Value = this.Rating;
      cmd.Parameters.Add(rating);

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@CuisineId";
      cuisineId.Value = this.CuisineId;
      cmd.Parameters.Add(cuisineId);

      cmd.ExecuteNonQuery();
      this.Id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Update(Restaurant newRestaurant)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE restaurants SET name = @NewName, address = @NewAddress, phone = @NewPhone, website = @NewWebsite, cost = @NewCost, rating = @NewRating, cuisine_id = @NewCuisineId WHERE id = @RestaurantId;";

      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@NewName";
      newName.Value = newRestaurant.Name;
      cmd.Parameters.Add(newName);

      MySqlParameter newAddress = new MySqlParameter();
      newAddress.ParameterName = "@NewAddress";
      newAddress.Value = newRestaurant.Address;
      cmd.Parameters.Add(newAddress);

      MySqlParameter newPhone = new MySqlParameter();
      newPhone.ParameterName = "@NewPhone";
      newPhone.Value = newRestaurant.Phone;
      cmd.Parameters.Add(newPhone);

      MySqlParameter newWebsite = new MySqlParameter();
      newWebsite.ParameterName = "@NewWebsite";
      newWebsite.Value = newRestaurant.Website;
      cmd.Parameters.Add(newWebsite);

      MySqlParameter newCost = new MySqlParameter();
      newCost.ParameterName = "@NewCost";
      newCost.Value = newRestaurant.Cost;
      cmd.Parameters.Add(newCost);

      MySqlParameter newRating = new MySqlParameter();
      newRating.ParameterName = "@NewRating";
      newRating.Value = newRestaurant.Rating;
      cmd.Parameters.Add(newRating);

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@NewCuisineId";
      cuisineId.Value = newRestaurant.CuisineId;
      cmd.Parameters.Add(cuisineId);

      MySqlParameter restaurantId = new MySqlParameter();
      restaurantId.ParameterName = "@RestaurantId";
      restaurantId.Value = newRestaurant.Id;
      cmd.Parameters.Add(restaurantId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Review> GetReviews()
    {
      List<Review> output = new List<Review> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM reviews WHERE restaurant_id = @RestaurantId;";

      MySqlParameter restaurantIdParameter = new MySqlParameter();
      restaurantIdParameter.ParameterName = "@RestaurantId";
      restaurantIdParameter.Value = this.Id;
      cmd.Parameters.Add(restaurantIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
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

    public bool HasSamePropertiesAs(Restaurant other)
    {
      return (
        this.Id == other.Id &&
        this.Name == other.Name &&
        this.Address == other.Address &&
        this.Phone == other.Phone &&
        this.Website == other.Website &&
        this.Cost == other.Cost &&
        this.Rating == other.Rating &&
        this.CuisineId == other.CuisineId);
    }
  }
}
