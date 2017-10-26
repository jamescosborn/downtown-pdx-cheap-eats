using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RestaurantDatabase.Models
{
  public class Cuisine
  {
    public int Id {get; private set;}
    public string Name {get; private set;}

    public Cuisine(string name, int id = 0)
    {
      Name = name;
      Id = id;
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> output = new List<Cuisine> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Cuisine newCuisine = new Cuisine(name, id);
        output.Add(newCuisine);
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
      cmd.CommandText = @"DELETE FROM cuisines;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      Restaurant.ClearAll();
    }

    public static Cuisine FindById(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines WHERE id = @CuisineId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@CuisineId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      int cuisineId = 0;
      string cuisineName = "";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        cuisineId = rdr.GetInt32(0);
        cuisineName = rdr.GetString(1);
      }
      Cuisine output = new Cuisine(cuisineName, cuisineId);

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
      cmd.CommandText = @"DELETE FROM cuisines WHERE id = @CuisineId;";

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@CuisineId";
      cuisineId.Value = id;
      cmd.Parameters.Add(cuisineId);

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
      cmd.CommandText = @"INSERT INTO cuisines (name) VALUES (@Name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@Name";
      name.Value = this.Name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      this.Id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Update(Cuisine newCuisine)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE cuisines SET name = @NewName WHERE id = @CuisineId;";

      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@NewName";
      newName.Value = newCuisine.Name;
      cmd.Parameters.Add(newName);

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@CuisineId";
      cuisineId.Value = this.Id;
      cmd.Parameters.Add(cuisineId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Restaurant> GetRestaurants()
    {
      List<Restaurant> output = new List<Restaurant> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants WHERE cuisine_id = @CuisineId;";

      MySqlParameter cuisineIdParameter = new MySqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = this.Id;
      cmd.Parameters.Add(cuisineIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
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

    public bool HasSamePropertiesAs(Cuisine other)
    {
      return (
        this.Id == other.Id &&
        this.Name == other.Name);
    }
  }
}
