using Newtonsoft.Json;
using SeedScript.Models;
using System;
using System.Collections.Generic;
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
            var seedModels = seedConfigs.Select(sc =>
            {
                var json = File.ReadAllText(sc);
                return JsonConvert.DeserializeObject<SqlSeedModel>(json);
            }).ToList();

            var totalRowsToInsert = TotalRowsInListOfSeedModels(seedModels);
            var progressBar = new ProgressBar(totalRowsToInsert);
            var seedReport = new SeedReport();

            foreach (var seedModel in seedModels)
            {
                var seeder = new SqlSeeder(seedModel.TableName, seedModel.ColumnNames.Split(";").ToList());
                seeder.Execute(seedModel, progressBar, seedReport);
            }

            Console.WriteLine("\nSeed finished...");
            Console.WriteLine(seedReport);
        }

        private static int TotalRowsInListOfSeedModels(List<SqlSeedModel> seedModel)
        {
            int sum = 0;
            seedModel.ForEach(sm => sum += sm.TotalRowsCount);

            return sum;
        }
    }

    public class ProgressBar
    {
        private int _barLength = 40;

        private int _progress = 0;
        private int _operationsCount;

        public ProgressBar(int operationsCount)
        {
            _operationsCount = operationsCount;
        }

        public bool JobFinished => _progress >= _operationsCount;

        public void UpdateProgressBar()
        {
            Console.SetCursorPosition(0, Console.CursorTop);

            _progress++;
            var precentDone = (double) _progress / _operationsCount * 100;
            var filledBarsCount = (double)_barLength / 100 * precentDone;

            var progressBar = $"[{new string('#', (int)filledBarsCount)}{new string('-', _barLength - (int)filledBarsCount)}]";
            Console.Write(progressBar);
        }

        public void DecreaseOperationsCount(int operationsCount)
        {
            if (operationsCount <= _operationsCount)
            {
                _operationsCount -= operationsCount;
            }
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
