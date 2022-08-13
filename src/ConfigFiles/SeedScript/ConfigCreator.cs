using Newtonsoft.Json;
using SeedScript.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeedScript
{
    public class ConfigCreator
    {
        public static void CreateConfig()
        {
            Console.WriteLine("Operation started...");
            string outputFileName = "zgeolocations.json";
            var numberGenerator = new SequenceNumbers(1, 8);

            var seedModel = new SqlSeedModel()
            {
                TableName = "Geolocations",
                ColumnNames = "Latitude;Longitude",
                ListOfRows = new List<RowModel>(),
                InsertInnerData = true,
                InsertOnlyIfEmpty = false
            };


            double minLat = 56, maxLat = 58, minLng = 23, maxLng = 26;
            double delta = 0.01;
            double currentLat = minLat, currentLng = minLng;

            while (currentLng < maxLng)
            {
                currentLng += delta;
                currentLat = minLat;

                var rowModel1 = new RowModel() { Row = $"{currentLat};{currentLng}" };

                var innerSeedModel1 = new SqlSeedModel()
                {
                    TableName = "SportsGroundLocations",
                    ColumnNames = "Country;City;Description;TypeId;CreatorId;GeolocationId",
                    ListOfRows = new List<RowModel>() { new RowModel() { Row = $"Latvia;Riga;Very good sport location;{numberGenerator.GetNextNumber()};1" } }
                };
                rowModel1.InnerInsertModel = innerSeedModel1;

                seedModel.ListOfRows.Add(rowModel1);

                while (currentLat < maxLat)
                {
                    currentLat += delta;
                    var rowModel2 = new RowModel() { Row = $"{currentLat};{currentLng}" };

                    var innerSeedModel2 = new SqlSeedModel()
                    {
                        TableName = "SportsGroundLocations",
                        ColumnNames = "Country;City;Description;TypeId;CreatorId;GeolocationId",
                        InsertOnlyIfEmpty = false,
                        InsertInnerData = true,
                        ListOfRows = new List<RowModel>() { new RowModel() { Row = $"Latvia;Riga;Very good sport location;{numberGenerator.GetNextNumber()};1" } }
                    };
                    rowModel2.InnerInsertModel = innerSeedModel2;

                    seedModel.ListOfRows.Add(rowModel2);
                }
            }

            var jsonString = JsonConvert.SerializeObject(seedModel);
            File.WriteAllText(@"C:\Users\aleks\OneDrive\Desktop\MyProjects\developing\Sportify\ConfigFiles\SeedScript\SeedConfigs\" + outputFileName, jsonString);

            Console.WriteLine("Operation finished...");
        }
    }

    public class SequenceNumbers
    {
        private int _from;
        private int _to;
        private int _current;

        public SequenceNumbers(int from, int to)
        {
            _from = _current = from;
            _to = to;
        }

        public int GetNextNumber()
        {
            if (_current == _to)
            {
                return _current = _from;
            }

            return _current++;
        }
    }
}
