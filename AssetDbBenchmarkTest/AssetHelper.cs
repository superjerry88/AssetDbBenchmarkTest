using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDbBenchmarkTest
{
    class AssetHelper
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

        public static void AddItem(string asset, Item item)
        {
            CreateAssetIfNotExist(asset);

            var db = new Database();
            var dbAsset = db.Assets.First(a => a.AssetId.Equals(asset));
            dbAsset.Items.Add(item);
            db.SaveChanges();
        }

        public static void AddItem(string asset, List<Item> items)
        {
            CreateAssetIfNotExist(asset);

            var db = new Database();
            db.Configuration.AutoDetectChangesEnabled = false; //Magic that speed up large transaction
            var dbAsset = db.Assets.First(a => a.AssetId.Equals(asset));
            dbAsset.Items.AddRange(items);
            db.SaveChanges();
        }
    }
}
