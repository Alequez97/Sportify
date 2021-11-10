﻿using Microsoft.Data.SqlClient;
using SeedScript.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedScript
{
    public class SqlSeeder
    {
        private const string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=sportify_db;Trusted_Connection=True";
        private string queryTemplate;

        public SqlSeeder(string tableName, List<string> columnsNames)
        {
            queryTemplate = string.Format("INSERT INTO {0} ({1}) VALUES()", tableName, string.Join(',', columnsNames));
            queryTemplate = queryTemplate.Substring(0, queryTemplate.Length - 1) + "{0}); SELECT SCOPE_IDENTITY();";
        }

        public void Execute(SqlSeedModel seedModel, int insertId = 0)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                if (seedModel.InsertOnlyIfEmpty)
                {
                    var rowCountQuery = $"SELECT COUNT(*) FROM {seedModel.TableName}";
                    using (SqlCommand command = new SqlCommand(rowCountQuery, sqlConnection))
                    {
                        var rowCount = Convert.ToInt32(command.ExecuteScalar());
                        if (rowCount > 0)
                        {
                            Console.WriteLine($"Skipping {seedModel.TableName} seed because table is not empty");
                            return;
                        }
                    }
                }

                Console.WriteLine($"Seeding {seedModel.TableName} table...");
                foreach (var row in seedModel.ListOfRows)
                {
                    var rowValues = row.Row.Split(";").Select(v => $"'{v.Replace("'", "''")}'").ToList();
                    if (insertId > 0)
                    {
                        rowValues.Add($"'{insertId}'");
                    }
                    var insertQuery = string.Format(queryTemplate, string.Join(',', rowValues));

                    using (SqlCommand command = new SqlCommand(insertQuery, sqlConnection))
                    {
                        try
                        {
                            var insertedId = Convert.ToInt32(command.ExecuteScalar());
                            //command.ExecuteNonQuery();
                            if (row.InnerInsertModel != null && seedModel.InsertInnerData == true)
                            {
                                var innerSqlSeeder = new SqlSeeder(row.InnerInsertModel.TableName, row.InnerInsertModel.ColumnNames.Split(";").ToList());
                                innerSqlSeeder.Execute(row.InnerInsertModel, insertedId);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}
