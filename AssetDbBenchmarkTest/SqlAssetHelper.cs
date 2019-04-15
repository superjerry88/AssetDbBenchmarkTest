using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace AssetDbBenchmarkTest
{
    class SqlAssetHelper
    {
        public static void CreateAssetIfNotExist(string assetId)
        {
            var db = new Database();
            if(db.Assets.FirstOrDefault(a => a.AssetId.Equals(assetId))== null)
            {
                db.Assets.Add(new Asset { AssetId = assetId });
                db.SaveChanges();
            }
        }

        public static void AddItemV1(string asset, Item item)
        {
            CreateAssetIfNotExist(asset);

            var db = new Database();
            var dbAsset = db.Assets.First(a => a.AssetId.Equals(asset));
            dbAsset.Items.Add(item);
            db.SaveChanges();
        }

        public static void AddItemV2(string asset, List<Item> items)
        {
            CreateAssetIfNotExist(asset);

            var db = new Database();
            var assetId = db.Assets.First(a => a.AssetId.Equals(asset)).Id;

            foreach (var item in items)
            {
                db.Database.ExecuteSqlCommand(
                    "Insert into Items Values(@Data, @Value, @DateTime, @IsValue, @Asset_Id)",
                    new SqlParameter("Data", item.Data ?? "NULL"),
                    new SqlParameter("Value", item.Value),
                    new SqlParameter("DateTime", item.DateTime),
                    new SqlParameter("IsValue", item.IsValue),
                    new SqlParameter("Asset_Id", assetId)
                );
            }
            db.SaveChanges();
        }

        public static void AddItemV3(string asset, List<Item> items)
        {
            CreateAssetIfNotExist(asset);

            var db = new Database();
            db.Configuration.AutoDetectChangesEnabled = false; //Magic that speed up large transaction

            db.Assets.First(a => a.AssetId.Equals(asset)).Items.AddRange(items);

            db.SaveChanges();
        }

        public static void AddItemV4(string asset, List<Item> items)
        {
            CreateAssetIfNotExist(asset);

            var db = new Database();
            var assetId = db.Assets.First(a => a.AssetId.Equals(asset));

            foreach (var item in items)
            {
                item.Asset = assetId;
            }

            db.BulkInsert(items);

        }

    }
}
