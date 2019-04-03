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

        public static void AddItemAsData (string asset, string data, DateTime? dt = null)
        {
            var datetime = DateTime.Now;
            if(dt != null) datetime = (DateTime) dt; 

            CreateAssetIfNotExist(asset);

            var db = new Database();
            var dbAsset = db.Assets.First(a => a.AssetId.Equals(asset));
            dbAsset.Items.Add(new Item
            {
                Data = data,
                DateTime = datetime,
                IsValue = false
            });
            db.SaveChanges();
        }

        public static void AddItemAsValue(string asset, double data, DateTime? dt = null)
        {
            var datetime = DateTime.Now;
            if (dt != null) datetime = (DateTime)dt;

            CreateAssetIfNotExist(asset);

            var db = new Database();
            var dbAsset = db.Assets.First(a => a.AssetId.Equals(asset));
            dbAsset.Items.Add(new Item
            {
                Value = data,
                DateTime = datetime,
                IsValue = true
            });
            db.SaveChanges();
        }
    }
}
