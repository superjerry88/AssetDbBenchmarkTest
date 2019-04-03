using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AssetDbBenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ClearDatabase();
            //GenerateDataSingle();
            GenerateDataGroup();

            Console.WriteLine("Done! [Enter] to end program");
            Console.ReadLine();
      
        }

        private static void GenerateDataSingle()
        {
            var stopwatch = Stopwatch.StartNew();
            var range = 10;
            for (var i = 0; i < range; i++)
            {
                AssetHelper.AddItem("STA",new Item(i));
                AssetHelper.AddItem("STA", new Item("RANDOM STRING ABC"));
            }

            Console.WriteLine($"[Task] Inserting Data to DB");
            Console.WriteLine($"[Desc] Adding 20 mix data using single transaction");
            Console.WriteLine($"[Time] Total: {stopwatch.ElapsedMilliseconds}ms | Average: {stopwatch.ElapsedMilliseconds/ range*2}ms");
            Console.WriteLine($"------------------------------------------------------\n");
        }

        private static void GenerateDataGroup()
        {
            var stopwatch = Stopwatch.StartNew();
            var range = 1000;
            var items = new List<Item>();
            for (var i = 0; i < 1000; i++)
            {
                items.Add( new Item(i));
                items.Add(new Item("RANDOM STRING ABC"));
            }
            AssetHelper.AddItem("STA",items);
            Console.WriteLine($"[Task] Inserting Data to DB");
            Console.WriteLine($"[Desc] Adding 2000 mix data using range transaction");
            Console.WriteLine($"[Time] {stopwatch.ElapsedMilliseconds}ms | Average: {stopwatch.ElapsedMilliseconds / range*2}ms"); 
            Console.WriteLine($"------------------------------------------------------\n");
        }

        private static void QueryCount()
        {
            var db = new Database();
            var assetQueryCount = db.Items.Count(i => i.Value > 90);
            Console.WriteLine(assetQueryCount);
        }

        private static void ClearDatabase()
        {
            Database db = new Database();
            db.Assets.RemoveRange(db.Assets);
            db.Items.RemoveRange(db.Items);
            db.SaveChanges();
        }
    }
}
