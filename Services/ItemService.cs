using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Shop_Bridge.Models;

namespace Shop_Bridge.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> _items;

        public ItemService(IItemstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _items = database.GetCollection<Item>(settings.ItemsCollectionName);
        }

        public List<Item> Get() =>
            _items.Find(item => true).SortByDescending(item => item.Id).ToList();
            //_items.Find().sort( { 'timestamp': -1 } ).limit(10);
            //_items.Find(item=>true).Sort({'_id': -1}).ToList();

        public Item Get(string id) =>
            _items.Find<Item>(item => item.Id == id).FirstOrDefault();

        public Item Create(Item item)
        {
            _items.InsertOne(item);
            //_items.InsertOne.
            return item;
        }

        public void Update(string id, Item itemIn) =>
            _items.ReplaceOne(item => item.Id == id, itemIn);

        public void Remove(Item itemIn) =>
            _items.DeleteOne(item => item.Id == itemIn.Id);

        public void Remove(string id) =>
            _items.DeleteOne(item => item.Id == id);

        /// Search Sort and Pagination
        public Object Query(string s, string sort, int? queryPage)
        {
            var filter = Builders<Item>.Filter.Empty;

            if (!string.IsNullOrEmpty(s))
            {
                filter = Builders<Item>.Filter.Regex("Title", new BsonRegularExpression(s, "i")) |
                         Builders<Item>.Filter.Regex("Description", new BsonRegularExpression(s, "i"));
            }

            var find = _items.Find(filter);

            if (sort == "asc")
            {
                find = find.SortBy(p => p.Price);
            }
            else if (sort == "desc")
            {
                find = find.SortByDescending(p => p.Price);
            }

            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int perPage = 9;
            var total = find.CountDocuments();

            return new
            {
                data = find.Skip((page - 1) * perPage).Limit(perPage).ToList(),
                total,
                page,
                last_page = total / perPage
            };
        }



    }
}

