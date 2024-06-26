using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Text.Json;
using Dapper;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelloWorld
{

    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("./appsettings.json").Build();
            DataContextDapper dapper = new DataContextDapper(config);

            // Console.WriteLine(sql);

            // // File.WriteAllText("log.txt", sql);

            // using StreamWriter openFile = new("log.txt", append: true);
            // openFile.WriteLine(sql);

            // openFile.Close();
            string computersJson = File.ReadAllText("Computers.json");
            // Console.WriteLine(computersJson);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // IEnumerable<Computer>? computers = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

            IEnumerable<Computer>? computers = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

            if (computers != null)
            {
                foreach (Computer computer in computers)
                {
                    string sql = @"INSERT INTO TutorialAppSchema.Computer (
                                Motherboard,
                                HasWifi,
                                HasLTE,
                                ReleaseDate,
                                Price,
                                VideoCard
                            ) VALUES ('" + computer.Motherboard
                                + "','" + computer.HasWifi
                                + "','" + computer.HasLTE
                                + "','" + computer.ReleaseDate?.ToString("yyyy-MM-dd")
                                + "','" + computer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                                + "','" + EscapeSingleQuote(computer.VideoCard)
                            + "')";
                    // Console.WriteLine(sql);
                    dapper.ExecuteSql(sql);
                }
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string computersCopyNewtonsoft = JsonConvert.SerializeObject(computers, settings);
            File.WriteAllText("computersCopyNewtonsoft.txt", computersCopyNewtonsoft);

            string computersCopySystem = System.Text.Json.JsonSerializer.Serialize(computers, options);
            File.WriteAllText("computersCopySystem.txt", computersCopySystem);
        }

        static string EscapeSingleQuote(string input)
        {
            return input.Replace("'", "''");
        }
    }
}