using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace AssetDbBenchmarkTest
{
    public class Database : DbContext
    {
        public Database()
            : base("name=Database")
        {
        }

        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Item> Items { get; set; }
    }

    public class Asset
    {
        public int Id { get; set; }
        public string AssetId { get; set; }
        public virtual List<Item> Items { get; set; } = new List<Item>();
    }

    public class Item
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsValue { get; set; }

        public string GetData => IsValue ? Value.ToString() : Data;

        public Item(string data, DateTime? dt = null)
        {
            var datetime = DateTime.Now;
            if (dt != null) datetime = (DateTime)dt;
            Data = data;
            DateTime = datetime;
            IsValue = false;
        }

        public Item(double data, DateTime? dt = null)
        {
            var datetime = DateTime.Now;
            if (dt != null) datetime = (DateTime)dt;
            Value = data;
            DateTime = datetime;
            IsValue = true;
        }

        public Item(){}
    }
}