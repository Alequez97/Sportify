using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace CountriesCitiesJsonModifier
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Country and city seed start...");

            var json = File.ReadAllText("countries_cities.json");
            var countries = JsonConvert.DeserializeObject<List<Country>>(json);

            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=sportify_db;Trusted_Connection=True";
            var countrySqlTemplate = "INSERT INTO Countries (Name, Iso2, Iso3) VALUES ('{0}', '{1}', '{2}'); SELECT SCOPE_IDENTITY();";
            var citySqlTemplate = "INSERT INTO Cities (Name, CountryId) VALUES ('{0}', {1})";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                foreach (var country in countries)
                {
                    if (country.Name == "Latvia")
                    {
                        var countrySql = string.Format(countrySqlTemplate, country.Name.Replace("'", "`"), country.Iso2, country.Iso3);
                        using (SqlCommand countryCommand = new SqlCommand(countrySql, sqlConnection))
                        {
                            try
                            {
                                var countryId = Convert.ToInt32(countryCommand.ExecuteScalar());
                                foreach (var city in country.Cities)
                                {
                                    var citySql = string.Format(citySqlTemplate, city.Name.Replace("'", "`"), countryId);
                                    using (SqlCommand cityCommand = new SqlCommand(citySql, sqlConnection))
                                    {
                                        cityCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
            }

            System.Console.WriteLine("Country and city seed finished...");
        }
    }
}
