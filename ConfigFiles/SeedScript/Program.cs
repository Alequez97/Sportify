using Newtonsoft.Json;
using SeedScript.Models;
using System;
using System.IO;
using System.Linq;

namespace SeedScript
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Seed started...");
            var seedConfigs = Directory.GetFiles("SeedConfigs").Where(file => Path.GetExtension(file) == ".json").ToList();
            var seedModels = seedConfigs.Select(sc => {
                var json = File.ReadAllText(sc);
                return JsonConvert.DeserializeObject<SqlSeedModel>(json);
            }).ToList();

            foreach (var seedModel in seedModels)
            {
                var seeder = new SqlSeeder(seedModel.TableName, seedModel.ColumnNames.Split(";").ToList());
                seeder.Execute(seedModel);
            }

            Console.WriteLine("Seed finished...");
        }
    }
}
