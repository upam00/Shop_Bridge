using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Shop_Bridge.Models
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string ItemName { get; set; }

        public decimal? Price { get; set; }

        public string About { get; set; }

        public string ImageBase64 { get; set; }

        //public string Author { get; set; }

    }
}
