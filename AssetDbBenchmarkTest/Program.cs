using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDbBenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {

            QueryCount();

            Console.WriteLine("Done! [Enter] to end program");
            Console.ReadLine();
      
        }

        private static void GenerateData()
        {
            for (int i = 0; i < 99; i++)
            {
                AssetHelper.AddItemAsValue("STA",i);
                AssetHelper.AddItemAsData("STA", "RANDOM STRING DATA TYPE");
            }
        }

        private static void QueryCount()
        {
            Database db = new Database();

            var assetQueryCount = db.Items.Count(i => i.Value > 90);
            Console.WriteLine(assetQueryCount);
        }
    }
}
