using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AssetDbBenchmarkTest
{
    class Program
    {
        const string FilePath = @"C:\Users\Jerry\Downloads\7-8 mar.csv";
        static void Main(string[] args)
        {
           // var data = GetCsvData(FilePath, 999999999);

            //BenchmarkInsertV1(); TOO SLOW
           // BenchmarkInsertV2();
            //BenchmarkInsertV3(); TOO SLOW
            BenchmarkInsertV4();
            BenchmarkCount();
            BenchmarkCountWithCondition();
           // BenchmarkSelectWithCondition();
            //BenchmarkSelectWithCondition2();
            BenchmarkMax();
            PrintInstruction("Done! [Enter] to end program");
            Console.ReadKey();
      
        }
        private static void BenchmarkInsertV1()
        {
            const int row = 10;
            var stopwatch = Stopwatch.StartNew();
            var assets = GetCsvData(FilePath, row);

            foreach (var item in assets)
            {
                foreach (var singleItem in item.Value)
                {
                    SqlAssetHelper.AddItemV1(item.Key, singleItem);
                }
                Console.WriteLine($"Added {item.Value.Count} data to {item.Key}");
            }

            PrintLine();
            PrintMsg($"[Task] Insert CSV data to Datbase - V1");
            PrintMsg($"[Desc] Adding {row} * 24 sensor data into database using Entity framework Add Function");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds / (row * 24)}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }
        private static void BenchmarkInsertV2()
        {
            const int row = 100;
            var stopwatch = Stopwatch.StartNew();
            var assets = GetCsvData(FilePath, row);

            foreach (var item in assets)
            {
                SqlAssetHelper.AddItemV2(item.Key, item.Value);
                Console.WriteLine($"Added {item.Value.Count} data to {item.Key}");
            }
            PrintLine();
            PrintMsg($"[Task] Insert CSV data to Datbase - V2");
            PrintMsg($"[Desc] Adding {row} * 24 sensor data into database using SQL Query");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds / (row * 24)}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }
        private static void BenchmarkInsertV3()
        {
            const int row = 10;
            var stopwatch = Stopwatch.StartNew();
            var assets = GetCsvData(FilePath, row);

            foreach (var item in assets)
            {
                SqlAssetHelper.AddItemV3(item.Key, item.Value);
                Console.WriteLine($"Added {item.Value.Count} data to {item.Key}");
            }
            PrintLine();
            PrintMsg($"[Task] Insert CSV data to Datbase - V3");
            PrintMsg($"[Desc] Adding {row} * 24 sensor data into database using Entity framework AddRange Function");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds / (row * 24)}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }

        private static void BenchmarkInsertV4()
        {
             int row = 9999999;
            var stopwatch = Stopwatch.StartNew();
            var assets = GetCsvData(FilePath, row);
            row = assets.Values.First().Count;
            Console.WriteLine(row);
            foreach (var item in assets)
            {
                SqlAssetHelper.AddItemV4(item.Key,item.Value);
                Console.WriteLine($"Added {item.Value.Count} data to {item.Key}");
            }
            PrintLine();
            PrintMsg($"[Task] Insert CSV data to Datbase - V4");
            PrintMsg($"[Desc] Adding {row} * 24 sensor data into database using Entity Framework Extension Bulk Insert Function");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds / (row*24)}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }

        private static void BenchmarkCount()
        {

            var stopwatch = Stopwatch.StartNew();
            var db = new Database();
            var queryAsset = "\"Q1_Act ValueY\"";
            var queryResult = db.Items.Count(i => i.Asset.AssetId.Equals(queryAsset));
            Console.WriteLine($"Total number of data in {queryAsset}: {queryResult}");
            PrintLine();
            PrintMsg($"[Task] Count");
            PrintMsg($"[Desc] Calculate the total amount of data under Asset: {queryAsset} ");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }

        private static void BenchmarkCountWithCondition()
        {
            var stopwatch = Stopwatch.StartNew();
            var db = new Database();
            var queryAsset = "\"Q1_Act ValueY\"";
            var queryResult = db.Items.Count(i => i.Asset.AssetId.Equals(queryAsset) && i.Value > 5);
            Console.WriteLine($"Total number of data in {queryAsset}: {queryResult}");
            PrintLine();
            PrintMsg($"[Task] Count With Condition ");
            PrintMsg($"[Desc] Calculate the total amount of data under Asset: {queryAsset} where its value is greater than 5");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }

        private static void BenchmarkSelectWithCondition()
        {
            const int row = 10;
            var stopwatch = Stopwatch.StartNew();
            var db = new Database();
            var queryAsset = "\"Q1_Act ValueY\"";
            var queryResult = db.Items.Where(i => i.Asset.AssetId.Equals(queryAsset) && i.Value > 5).Take(row);
            foreach (var item in queryResult)//
            {
                Console.WriteLine($"{queryAsset}: {item.GetData} at {item.DateTime:T}");
            }
            
            PrintLine();
            PrintMsg($"[Task] Extract Data with condition");
            PrintMsg($"[Desc] Display the latest {row} Asset: {queryAsset} where its value is greater than 5");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds/row}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }

        private static void BenchmarkSelectWithCondition2()
        {
            const int row = 10;
            var stopwatch = Stopwatch.StartNew();
            var db = new Database();
            var queryAsset = "\"Q1_Act ValueY\"";
            var queryResult = db.Items.OrderByDescending(p => p.DateTime).Where(i => i.Asset.AssetId.Equals(queryAsset) && i.Value > 5).Take(row);
            foreach (var item in queryResult)//
            {
                Console.WriteLine($"{queryAsset}: {item.GetData} at {item.DateTime:T}");
            }

            PrintLine();
            PrintMsg($"[Task] Extract Data with condition");
            PrintMsg($"[Desc] Display the latest {row} Asset: {queryAsset} where its value is greater than 5");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds / row}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }

        private static void BenchmarkMax()
        {
            var stopwatch = Stopwatch.StartNew();
            var db = new Database();
            var queryAsset = "\"Q1_Act ValueY\"";
            var queryResult = db.Items.Where(i => i.Asset.AssetId.Equals(queryAsset)).Max(i => i.Value);
            Console.WriteLine($"Max value: {queryResult}");
            PrintLine();
            PrintMsg($"[Task] Find Highest Sensor Data");
            PrintMsg($"[Desc] Display the Max value for Asset: {queryAsset} ");
            PrintMsg($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {(double)stopwatch.ElapsedMilliseconds}ms per data");
            PrintInstruction("Hit Enter to continue next test");
            PrintLine();
            Console.ReadKey();
        }

        public static Dictionary<string, List<Item>> GetCsvData(string filePath,int row)
        {
            using (var reader = new StreamReader(filePath))
            {
                var names = new List<string>();
                var items = new Dictionary<string, List<Item>>();

                for (var i = 0; i < row && !reader.EndOfStream; i++)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('\t');
                    if (i == 0) names.AddRange(values);
                    else
                    {
                        for (var col = 0; col < values.Length; col++)
                        {
                            var stringValue = values[col];
                            var name = names[col];
                            if(name.Contains("Time")) continue;
                            if (!items.ContainsKey(name)) items[name] = new List<Item>();

                            var date = DateTime.Parse(values[0]);
                            items[name].Add(double.TryParse(stringValue, out var number) ? new Item(number, date) : new Item(stringValue, date));
                        }
                    }
                }
                return items;
            }


        }

        public static void PrintLine()
        {
            Print(ConsoleColor.DarkYellow, "--------------------------------------------------------");
        }

        public static void PrintMsg(object text)
        {
            Print(ConsoleColor.Cyan, text.ToString());
        }

        public static void PrintInstruction(object text)
        {
            Print(ConsoleColor.Magenta, text.ToString());
        }

        public static void Print(ConsoleColor color, string text)
        {
            var ori = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ori;
        }
    }
}
