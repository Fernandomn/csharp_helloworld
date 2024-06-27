using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Text.Json;
using AutoMapper;
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
            string computersJson = File.ReadAllText("ComputersSnake.json");

            Mapper mapper = new Mapper(new MapperConfiguration((cfg) =>
            {
                cfg.CreateMap<ComputerSnake, Computer>()
                    .ForMember(destination => destination.ComputerId,
                        options => options.MapFrom(source => source.computer_id))
                    .ForMember(destination => destination.CPUCores,
                        options => options.MapFrom(source => source.cpu_cores))
                    .ForMember(destination => destination.HasLTE,
                        options => options.MapFrom(source => source.has_lte))
                    .ForMember(destination => destination.HasWifi,
                        options => options.MapFrom(source => source.has_wifi))
                    .ForMember(destination => destination.Motherboard,
                        options => options.MapFrom(source => source.motherboard))
                    .ForMember(destination => destination.Price,
                        options => options.MapFrom(source => source.price))
                    .ForMember(destination => destination.ReleaseDate,
                        options => options.MapFrom(source => source.release_date))
                    .ForMember(destination => destination.VideoCard,
                        options => options.MapFrom(source => source.video_card));
            }));

            IEnumerable<Computer>? computersSystemJSONPropertyMapping = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);


            if (computersSystemJSONPropertyMapping != null)
            {

                foreach (Computer computer in computersSystemJSONPropertyMapping)
                {
                    Console.WriteLine(computer.Motherboard);
                }
            }


            // IEnumerable<ComputerSnake>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);

            // if (computersSystem != null)
            // {
            //     IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computersSystem);

            //     foreach (Computer computer in computerResult)
            //     {
            //         Console.WriteLine(computer.Motherboard);
            //     }
            // }

            // Console.WriteLine(computersJson);

            // JsonSerializerOptions options = new JsonSerializerOptions()
            // {
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // };

            // // IEnumerable<Computer>? computers = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

            // IEnumerable<Computer>? computers = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

            // if (computers != null)
            // {
            //     foreach (Computer computer in computers)
            //     {
            //         string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //                     Motherboard,
            //                     HasWifi,
            //                     HasLTE,
            //                     ReleaseDate,
            //                     Price,
            //                     VideoCard
            //                 ) VALUES ('" + computer.Motherboard
            //                     + "','" + computer.HasWifi
            //                     + "','" + computer.HasLTE
            //                     + "','" + computer.ReleaseDate?.ToString("yyyy-MM-dd")
            //                     + "','" + computer.Price.ToString("0.00", CultureInfo.InvariantCulture)
            //                     + "','" + EscapeSingleQuote(computer.VideoCard)
            //                 + "')";
            //         // Console.WriteLine(sql);
            //         dapper.ExecuteSql(sql);
            //     }
            // }

            // JsonSerializerSettings settings = new JsonSerializerSettings()
            // {
            //     ContractResolver = new CamelCasePropertyNamesContractResolver()
            // };

            // string computersCopyNewtonsoft = JsonConvert.SerializeObject(computers, settings);
            // File.WriteAllText("computersCopyNewtonsoft.txt", computersCopyNewtonsoft);

            // string computersCopySystem = System.Text.Json.JsonSerializer.Serialize(computers, options);
            // File.WriteAllText("computersCopySystem.txt", computersCopySystem);
        }

        static string EscapeSingleQuote(string input)
        {
            return input.Replace("'", "''");
        }
    }
}